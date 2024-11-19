using DoctorSchedule.Domain.Configuration;
using DoctorSchedule.Domain.Entities;
using DoctorSchedule.Domain.Notifications;
using DoctorSchedule.Domain.RepositoriesInterface;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DoctorSchedule.Infrastructure.RepositoriesImplementation
{
    /// <summary>
    /// This class implements the IJwtUtils interface.
    /// </summary>
    public class JwtUtils : IJwtUtils
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<JwtUtils> _logger;

        public JwtUtils(IOptions<AppSettings> appSettings,ILogger<JwtUtils> logger)
        {
            _appSettings = appSettings.Value;

            if (string.IsNullOrEmpty(_appSettings.Secret))
                throw new Exception(Notification.JWTNoSecretMessage);

            _logger = logger;
        }

        /// <summary>
        /// This method generates a JWT token.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GenerateJwtToken(User user)
        {
            try
            {
                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(JwtUtils)} - {nameof(GenerateJwtToken)}: attempting to generate token for user {user.FirstName}.");
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddMinutes(_appSettings.MinutesExpiry??0),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(JwtUtils)} - {nameof(GenerateJwtToken)}: successfully generated token  {token}");
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(JwtUtils)} - {nameof(GenerateJwtToken)}: failed to generate token for user {user.FirstName}. {ex.Message}.");
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }
        /// <summary>
        /// This method is responsible for validating a token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int? ValidateJwtToken(string? token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
            try
            {
                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(JwtUtils)} - {nameof(ValidateJwtToken)}: attempting to validate token {token}.");

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                _logger.LogInformation($"{DateTime.UtcNow} - {nameof(JwtUtils)} - {nameof(ValidateJwtToken)}: token validated successfully.");

                return userId;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow} - {nameof(JwtUtils)} - {nameof(ValidateJwtToken)}: failed to validate token.");
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }

    }
}
