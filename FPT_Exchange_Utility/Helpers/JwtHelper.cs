using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FPT_Exchange_Utility.Helpers
{
    public class JwtHelper
    {
        public static string GenerateJwtToken(string id, string name, string role, string key)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(key);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, name),
                    new Claim("Role", role),
                    new Claim("userId", id),

                }),

                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);

        }

        public static ClaimsPrincipal DecodeJwtToken(string jwtToken, string signingKey)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Giải mã JWT token và xác thực token
                var principal = jwtTokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out var validatedToken);

                // Kiểm tra xem token có hợp lệ không
                if (validatedToken is JwtSecurityToken validJwtToken)
                {
                    // Truy xuất các claims từ token
                    var claims = validJwtToken.Claims;
                    // Ví dụ lấy thông tin name, role, userId từ claims
                    var name = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                    var role = claims.FirstOrDefault(x => x.Type == "Role")?.Value;
                    var userId = claims.FirstOrDefault(x => x.Type == "userId")?.Value;

                    // Tiếp tục xử lý với thông tin đã trích xuất

                    return principal;
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi giải mã token (nếu có)
                Console.WriteLine($"Error decoding JWT: {ex.Message}");
            }

            return null; // Trường hợp token không hợp lệ
        }
    }
}