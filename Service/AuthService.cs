using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Diary.Data;
using Diary.DTOs;
using Diary.Helpers;
using Diary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Diary.Service
{
    public class AuthService : IAuthService
    {
        private AppDataContext _context;
        private IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AuthService(AppDataContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }
       
        public async Task<ServiceResponse<RegisterResponseDTO>>Register(RegisterRequestDTO registerRequestDTO)
        {
            ServiceResponse<RegisterResponseDTO> response = new ServiceResponse<RegisterResponseDTO>();
            
            if (_context.AuthEntities.Any(x => x.Email == registerRequestDTO.Email))
            {
                response.Success = false;
                response.Message = "the email is already used";
                return response;
            }

            if (await IsUserExistByUserName(registerRequestDTO.UserName))
            {
                response.Success = false;
                response.Message = "the username is already used";
                return response;
            }

            CreatePasswordHash(registerRequestDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var authentity = _mapper.Map<AuthEntity>(registerRequestDTO);
            authentity.PasswordHash = passwordHash;
            authentity.PasswordSalt = passwordSalt;
            
            _context.AuthEntities.Add(authentity);
            await _context.SaveChangesAsync();
            response.Data = _mapper.Map<RegisterResponseDTO>(authentity);
            
            return response;
        }

        public async Task<bool> IsUserExistByUserName(string userName)
        {
            if (await _context.AuthEntities.AnyAsync(x => x.UserName.ToLower() == userName.ToLower()))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<ServiceResponse<string>> Login(LoginRequestDTO loginRequestDTO)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var authentity = await _context.AuthEntities
                .FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(loginRequestDTO.UserName.ToLower()));

            if (authentity == null)
            {
                response.Success = false;
                response.Message = "User Not Found";
            }
            else if (!VerifyPasswordHash(loginRequestDTO.Password, authentity.PasswordHash, authentity.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong Password";
            }
            else
            {
                response.Data = CreateToken(authentity);
            }
            return response;
        }

        private string CreateToken(AuthEntity authentity)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, authentity.Id.ToString()),
                new Claim(ClaimTypes.Name, authentity.UserName)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSetting:Token").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);

        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}