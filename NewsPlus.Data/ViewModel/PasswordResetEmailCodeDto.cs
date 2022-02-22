namespace NewsPlus.Data.ViewModel
{
    public class PasswordResetEmailCodeDto
    {
        public string Code { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
