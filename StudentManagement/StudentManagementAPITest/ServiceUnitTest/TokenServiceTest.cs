using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StudentManagementAPI.Interfaces;
using StudentManagementAPI.Models.DBModels;
using StudentManagementAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentManagementAPITest.ServiceUnitTest
{
    public class TokenServiceTests
    {
        private ITokenService _tokenService;
        private IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"TokenKey:JWT", "08adeaf6148022445c28b37a1a8bf67806a73bcd6a30fd3ad41136f37fcab65d"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _tokenService = new TokenService(_configuration);
        }

        [Test]
        public void GenerateToken_ValidUser_ReturnsToken()
        {
            // Arrange
            var user = new User
            {
                UserId = 1,
                Role = UserRole.Student
            };

            // Act
            var token = _tokenService.GenerateToken(user);

            // Assert
            Assert.NotNull(token);
            Assert.That(token, Is.Not.Empty);

            // Verify token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("08adeaf6148022445c28b37a1a8bf67806a73bcd6a30fd3ad41136f37fcab65d");
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userIdClaim = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            var roleClaim = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value;

            Assert.That(userIdClaim, Is.EqualTo("1"));
            Assert.That(roleClaim, Is.EqualTo("Student"));
        }
    }
}
