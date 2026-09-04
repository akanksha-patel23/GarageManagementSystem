namespace GarageManagement.Api.Repositories
{
    public class UserRecord
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
    public interface IUserRepository
    {
        Task<UserRecord> GetUserByUsernameAsync(string username);
    }
}
