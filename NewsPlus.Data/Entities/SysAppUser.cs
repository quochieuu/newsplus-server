namespace NewsPlus.Data.Entities
{
    public class SysAppUser : ModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Bio { get; set; }
        public DateTime Birthday { get; set; }
        public int Gender { get; set; }
        public string? Address { get; set; }
        public string? Introduction { get; set; }
        public string? AvatarUrl { get; set; }
        public string? EmailConfirmationCode { get; set; }
        public bool? EmailConfirmation { get; set; }
        public string? ResetPasswordCode { get; set; }
        public bool? EmailNotification { get; set; }
        public string? PasswordResetCode { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? ShouldChangePassword { get; set; }
        public DateTimeOffset? JoinedDate { get; set; }
        public Guid RoleId { get; set; }
        public virtual SysAppRole? SysAppRole { get; set; }
    }
}
