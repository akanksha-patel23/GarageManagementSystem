using GarageManagement.Api.DTOs;
using GarageManagement.Api.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GarageManagement.Api.Services
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }

    /// <summary>
    /// Verifies credentials against the same tblUsers row UserBAL.cs (Web Forms)
    /// checks — same algorithm (PBKDF2-HMAC-SHA256, 100,000 iterations, 32-byte
    /// key), so a password works identically from either layer. This is an
    /// independent verification path, not a shared secret between layers,
    /// consistent with every other table in this project being read
    /// independently by both the Web Forms DAL and the API's repositories.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        private const int Iterations = 100000;
        private const int HashSize = 32;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            var user = await _userRepository.GetUserByUsernameAsync(request.Username);
            if (user == null || !user.IsActive)
                return null;

            byte[] salt = Convert.FromBase64String(user.PasswordSalt);
            byte[] storedHash = Convert.FromBase64String(user.PasswordHash);
            byte[] computedHash = Rfc2898DeriveBytes.Pbkdf2(request.Password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);

            if (!CryptographicOperations.FixedTimeEquals(storedHash, computedHash)) // time-constant method to prevent timing attacks and compare the arrays safely
                return null;

            string token = GenerateJwtToken(user.Username, user.Role);

            return new LoginResponseDto
            {
                Token = token,
                Role = user.Role,
                FullName = user.FullName
            };
        }

        private string GenerateJwtToken(string username, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"])),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
