using Microsoft.Extensions.Logging;
using NewsPlus.Data.EF;
using NewsPlus.Data.Entities;
using NewsPlus.Data.ViewModel;
using NewsPlus.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsPlus.Business.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        private readonly DataDbContext _context;
        private readonly IEmailSender _emailSender;
        public UserService(ILogger<UserService> logger, DataDbContext context, IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            _emailSender = emailSender;
        }

        public void CreateUser(RegisterRequest request)
        {
            int salt = 12;
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password, salt);

            SysAppUser user = new SysAppUser()
            {
                Username = request.UserName,
                Password = passwordHash,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                FullName = request.FirstName + ' ' + request.LastName,
                JoinedDate = DateTime.Now,
                Birthday = DateTime.Now,
                CreatedDate = DateTime.Now,
                Status = 1,
                IsDeleted = false
            };

            _context.SysAppUsers.Add(user);
            _context.SaveChanges();
        }

        public bool SendPasswordResetCodeEmail(string username, string code)
        {
            var user = _context.SysAppUsers.FirstOrDefault(x => x.Username == username);

            if (user != null)
            {
                if (user.Email == null)
                {
                    return false;
                }
                else
                {
                    string subject = "Reset Password";
                    string message = "Hi there,<br/><br/>We got a request for resetting your account password. Please copy this code to reset your password."
                                      + "<br/><br/>" + "<pre style='font-size: 17px'>" + code + "</pre>";

                    var mail = new MessageDto(
                        new string[] { user.Email },
                        subject,
                        message,
                        null);
                    _emailSender.SendEmail(mail);

                    user.ResetPasswordCode = code;
                    _context.SysAppUsers.Update(user);
                    _context.SaveChanges();

                    return true;
                }
            }

            return false;
        }

        public void SendPasswordResetLinkEmail(string username, string link, string code)
        {
            var user = _context.SysAppUsers.Where(user => user.Username == username).FirstOrDefault();
            if (user != null)
            {
                string subject = "Reset Password";
                string message = "Hi there,<br/><br/>We got a request for resetting your account password. Please click the link below to reset your password."
                                  + "<br/><br/>" + "<a href=" + link + ">Reset Password Link</a>"
                                  + "<br/><br/>" + link;

                var mail = new MessageDto(
                    new string[] { user.Email },
                    subject,
                    message,
                    null);
                _emailSender.SendEmail(mail);

                user.EmailConfirmationCode = code;
                _context.SysAppUsers.Update(user);
                _context.SaveChanges();
            }
        }

        public string FindEmailByCode(string code)
        {
            var user = _context.SysAppUsers.Where(user => user.EmailConfirmationCode == code).FirstOrDefault();

            if (user != null)
                return user.Email;

            return string.Empty;
        }

        public void VerifyEmail(string email)
        {
            var user = _context.SysAppUsers.Where(user => user.Email == email).FirstOrDefault();

            user.EmailConfirmation = true;
            _context.SysAppUsers.Update(user);
            _context.SaveChanges();
        }


        public bool ResetUserPasswordEmailCode(string username, PasswordResetEmailCodeDto request)
        {
            var user = _context.SysAppUsers.FirstOrDefault(x => x.Username == username);

            if (request.Code == null)
                return false;

            if (user != null)
            {
                if (user.Email == null && user.ResetPasswordCode == null)
                {
                    return false;
                }
                else
                {
                    if (user.ResetPasswordCode == request.Code)
                    {
                        int salt = 12;
                        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword, salt);

                        user.Password = passwordHash;
                        _context.SysAppUsers.Update(user);
                        _context.SaveChanges();

                        return true;
                    }
                }
            }
            return false;
        }

        public SysAppUser FindUser(string email)
        {
            if (email == null)
                return null;

            var user = _context.SysAppUsers.FirstOrDefault(x => x.Email == email);

            if (user != null)
                return user;

            return null;
        }

        public bool IsValidUserCredentials(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var user = _context.SysAppUsers.FirstOrDefault(x => x.Username == userName);

            if (user != null)
            {
                var check = BCrypt.Net.BCrypt.Verify(password, user.Password);

                if (check)
                    return true;
                else
                    return false;
            }

            return false;
        }

        public bool IsAnExistingUser(string userName)
        {
            var existingUser = _context.SysAppUsers.FirstOrDefault(x => x.Username == userName);

            if (existingUser != null)
            {
                return true;
            }

            return false;
        }

        public string GetUserRole(string userName)
        {
            var userRole = from user in _context.SysAppUsers
                           join role in _context.SysAppRoles on user.RoleId equals role.Id
                           where user.Username == userName
                           select new { user, role };

            if (!IsAnExistingUser(userName))
            {
                return string.Empty;
            }

            if (userRole != null)
            {
                var roleName = userRole.Select(u => u.role.Name).DefaultIfEmpty().FirstOrDefault();

                if (roleName == null)
                    return string.Empty;
                else
                    return roleName.ToString();
            }

            return string.Empty;
        }
    }
}
