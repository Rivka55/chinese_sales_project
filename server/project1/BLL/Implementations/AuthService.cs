using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using project1.BLL.Helpers;
using project1.BLL.Interfaces;
using project1.DAL.Interfaces;
using project1.DTOs;
using project1.DTOs.Auth;
using project1.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;

namespace project1.BLL.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserDAL _userDal;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserDAL userDal, IConfiguration config, IMapper mapper, ILogger<AuthService> logger)
        {
            _userDal = userDal;
            _config = config;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task RegisterAsync(RegisterDTO dto)
        {
            _logger.LogInformation("Validating registration for user: {Email}", dto.Email);

            if (await _userDal.GetByNameAsync(dto.Name) != null)
            {
                _logger.LogWarning("Registration rejected: Username '{Name}' already exists", dto.Name);
                throw new InvalidOperationException("User Name already exists");
            }

            if (await _userDal.GetByEmailAsync(dto.Email) != null)
            {
                _logger.LogWarning("Registration rejected: Email '{Email}' already exists", dto.Email);
                throw new InvalidOperationException("User Email already exists");
            }

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = PasswordHasher.Hash(dto.Password);

            await _userDal.AddAsync(user);
            _logger.LogInformation("User {Email} successfully persisted to database.", dto.Email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userDal.GetByEmailAsync(email) != null;
        }

        public async Task<bool> NameExistsAsync(string name)
        {
            return await _userDal.GetByNameAsync(name) != null;
        }

        public async Task<string> LoginAsync(LoginDTO dto)
        {
            _logger.LogInformation("Validating credentials for: {Email}", dto.Email);

            var user = await _userDal.GetByEmailAsync(dto.Email);

            if (user == null)
            {
                _logger.LogWarning("Login failed: User with email {Email} not found", dto.Email);
                throw new UnauthorizedAccessException("אימייל או סיסמה אינם נכונים");
            }

            if (!PasswordHasher.Verify(dto.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed: Incorrect password for user {Email}", dto.Email);
                throw new UnauthorizedAccessException("אימייל או סיסמה אינם נכונים");
            }

            _logger.LogInformation("Credentials verified for {Email}. Generating JWT token...", dto.Email);
            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(2),
                    signingCredentials: creds
                );

                _logger.LogDebug("JWT token successfully created for User ID: {Id}", user.Id);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Failed to generate JWT token for User ID: {Id}", user.Id);
                throw;
            }
        }
    }
}















//using AutoMapper;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using project1.BLL.Helpers;
//using project1.BLL.Interfaces;
//using project1.DAL.Interfaces;
//using project1.DTOs;
//using project1.DTOs.Auth;
//using project1.Models;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;


//namespace project1.BLL.Implementations
//{
//    public class AuthService : IAuthService
//    {
//        private readonly IUserDAL _userDal;
//        private readonly IConfiguration _config;
//        private readonly IMapper _mapper;

//        public AuthService(IUserDAL userDal, IConfiguration config, IMapper mapper)
//        {
//            _userDal = userDal;
//            _config = config;
//            _mapper = mapper;
//        }


//        public async Task RegisterAsync(RegisterDTO dto)
//        {
//            if (await _userDal.GetByNameAsync(dto.Name) != null)
//                throw new Exception("User Name already exists");

//            var user = _mapper.Map<User>(dto);

//            user.PasswordHash = PasswordHasher.Hash(dto.Password);

//            await _userDal.AddAsync(user);
//        }

//        public async Task<string> LoginAsync(LoginDTO dto)
//        {
//            var user = await _userDal.GetByEmailAsync(dto.Email);

//            if (user == null || !PasswordHasher.Verify(dto.Password, user.PasswordHash))
//                throw new UnauthorizedAccessException("אימייל או סיסמה אינם נכונים");

//            return GenerateJwtToken(user);
//        }

//        private string GenerateJwtToken(User user)
//        {
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                new Claim(ClaimTypes.Name, user.Name),
//                new Claim(ClaimTypes.Email, user.Email),
//                new Claim(ClaimTypes.Role, user.Role)
//            };

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                issuer: _config["Jwt:Issuer"],
//                audience: _config["Jwt:Audience"],
//                claims: claims,
//                expires: DateTime.Now.AddHours(2),
//                signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}