namespace NewsPlus.Data.ViewModel
{
    public class PasswordResetDto
    {
        public string? Token { get; set; }
        public string? Code { get; set; }
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
