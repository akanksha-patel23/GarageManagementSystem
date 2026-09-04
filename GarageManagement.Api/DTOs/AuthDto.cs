namespace GarageManagement.Api.DTOs
{
    public class LoginRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
    }
}
