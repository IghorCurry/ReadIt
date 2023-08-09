using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReadIt.BLL.Models.AuthModels;
using ReadIt.BLL.Models.UserModels;
using ReadIt.DAL.Entities;
using ReadIt.DAL.Persistance.Services;
using ReadIt.DAL.Persistance.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace ReadIt.BLL.Managers.AuthManager
{
    public class AuthManager
    {
        private readonly UserManager<User> _userManager;
        private readonly AccessTokenSettings _authSettings;
        private readonly EmailSenderService _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public AuthManager(UserManager<User> userManager, IOptions<AccessTokenSettings> authSettings, EmailSenderService emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _authSettings = authSettings.Value;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual async Task<bool> Register(UserCreateModel model)
        {
            User user = model.Adapt<User>();
            IdentityResult res = await _userManager.CreateAsync(user, model.Password);
            if (!res.Succeeded)
            {
                throw new Exception(res.Errors.FirstOrDefault().Description);
            }

            var encodedToken = HttpUtility.UrlEncode(await _userManager.GenerateEmailConfirmationTokenAsync(user));
            var encodedEmail = HttpUtility.UrlEncode(user.Email);

            string host = _httpContextAccessor.HttpContext.Request.Host.Value;

            var confirmationlink = "https://" + host + "/api/Auth/ConfirmEmail?email=" + encodedEmail + "&token=" + encodedToken;

            await _emailSender.SendEmailAsync(user.Email, "Confirm Email", "Click on the link to confirm email: " + confirmationlink);
            IdentityResult resAdd = await _userManager.AddToRoleAsync(user, "User");
            if (!res.Succeeded)
            {
                throw new Exception("Unable to assign user to role 'User'");
            }
            return true;
        }

        public virtual async Task<bool> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new Exception("The User with such email doesn't exist");
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                throw new Exception("Unable to confirm email");
            }
            return true;
        }

        public virtual async Task<TokenPairModel> Login(LoginModel loginModel)
        {
            TokenPairModel tokenPair = new TokenPairModel();
            User user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                throw new Exception("The User with such email doesn't exist");
            }
            if (!await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                throw new Exception("Wrong password");
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                throw new Exception("Email is not confirmed");
            }

            string accessToken = GenerateJwtToken(user);

            var refreshToken = await _userManager.GenerateUserTokenAsync(user, "ReadIt", "RefreshToken");
            await _userManager.SetAuthenticationTokenAsync(user, "ReadIt", "RefreshToken", refreshToken);
            tokenPair.RefreshToken = refreshToken;
            tokenPair.AccessToken = accessToken;

            return tokenPair;
        }

        private ClaimsIdentity GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString())
            };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "AccessToken", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        public async Task<TokenPairModel> Refresh(TokenPairModel tokenPair)
        {
            string accessToken = tokenPair.AccessToken;
            string refreshToken = tokenPair.RefreshToken;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.SigningKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            if (principal == null)
            {
                throw new Exception("Not existing access token.");
            }
            string id = principal.Identity.Name;

            var user = await _userManager.FindByIdAsync(id);
            var userToken = await _userManager.GetAuthenticationTokenAsync(user, "ReadIt", "RefreshToken");
            var isValid = await _userManager.VerifyUserTokenAsync(user, "ReadIt", "RefreshToken", refreshToken);

            if (user == null || userToken != refreshToken || !isValid)
            {
                return null;
            }

            var newAccessToken = GenerateJwtToken(user);

            await _userManager.RemoveAuthenticationTokenAsync(user, "ReadIt", "RefreshToken");

            var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, "ReadIt", "RefreshToken");
            await _userManager.SetAuthenticationTokenAsync(user, "ReadIt", "RefreshToken", newRefreshToken);

            tokenPair.AccessToken = newAccessToken;
            tokenPair.RefreshToken = newRefreshToken;

            return tokenPair;
        }

        public string GenerateJwtToken(User user)
        {
            var identity = GetClaims(user);
            if (identity == null)
            {
                return null;
            }

            var now = DateTime.UtcNow;
            var key = Encoding.ASCII.GetBytes(_authSettings.SigningKey);

            var jwt = new JwtSecurityToken(
                        issuer: _authSettings.Issuer,
                        audience: _authSettings.Audience,
                        notBefore: now,
                        claims: identity.Claims,
                        expires: now.Add(TimeSpan.FromMinutes(15)),
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;

        }
    }
}
