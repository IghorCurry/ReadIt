using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ReadIt.BLL.Models.UserModels;
using ReadIt.DAL.Entities;
using ReadIt.DAL.Persistance.Services;
using System.Web;

namespace ReadIt.BLL.Managers.AuthManager
{
    public class IdentityManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EmailSenderService _emailSender;

        public IdentityManager(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, EmailSenderService emailSender)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
        }

        public virtual async Task<UserDetailedViewModel> WhoAmI()
        {
            string id = _httpContextAccessor.HttpContext.User.Identity.Name;
            User user = await _userManager.FindByIdAsync(id);
            var model = user.Adapt<UserDetailedViewModel>();
            var roleNames = await _userManager.GetRolesAsync(user);
            model.RoleNames = roleNames;
            return model;
        }

        public virtual async Task<bool> ChangeEmail(User user, string newEmail)
        {
            var encodedToken = HttpUtility.UrlEncode(await _userManager.GenerateChangeEmailTokenAsync(user, newEmail));
            var encodedOldEmail = HttpUtility.UrlEncode(user.Email);
            var encodedNewEmail = HttpUtility.UrlEncode(newEmail);

            string host = _httpContextAccessor.HttpContext.Request.Host.Value;

            var confirmationlink = "https://" + host + "/api/read-it/User/UpdateEmailConfirm?oldEmail=" + encodedOldEmail + "&newEmail=" + encodedNewEmail + "&token=" + encodedToken;

            await _emailSender.SendEmailAsync(newEmail, "Email changing", "Click on the link to change email: " + confirmationlink);

            return true;
        }
    }

}
