namespace BhoomiGlobalAPI.DTOs
{
    public class UserRegistrationDTO
    {
        public string? UserId { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? Address { get; set; }

        public List<string>? RoleNames { get; set; }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserDetailsUpdateDTO
    {
        public long Id { get; set; }
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public String? PhoneNumber { get; set; }
    }

    public class SignInDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsMobile { get; set; }
        public int? LoginType { get; set; }
    }
}
