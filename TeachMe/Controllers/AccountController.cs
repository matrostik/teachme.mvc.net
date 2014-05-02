using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using TeachMe.Models;
using TeachMe.Helpers;
using System.Data.Entity;

namespace TeachMe.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            Db = new ApplicationDbContext();
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public ApplicationDbContext Db { get; private set; }


        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(model.UserName, model.Password);
                if (user != null && user.IsConfirmed)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "שם משתמש או סיסמא שגויים.");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Remove leading and trailing spaces 
                model.Email = model.Email.Trim();
                // Check email
                ApplicationUser user = Db.Users.SingleOrDefault(u => u.Email == model.Email);
                if (user != null)
                    ModelState.AddModelError("Email", "דוא\"ל כבר קיים");
                // Check username
                user = Db.Users.SingleOrDefault(u => u.UserName == model.UserName);
                if (user != null)
                    ModelState.AddModelError("UserName", "שם משתמש כבר קיים");
                // Display errors if has
                if (!ModelState.IsValid)
                    return View(model);

                // Attempt to register the user
                string confirmationToken = CreateConfirmationToken();
                user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    ConfirmationToken = confirmationToken,
                    IsConfirmed = false
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // User created send confirmation mail
                    Email.Send(model.Email, model.UserName, confirmationToken,EmailTemplate.Registration);
                    return RedirectToAction("Index","Result", new { Message = ResultMessageId.RegisterStepTwo });
                }
                else
                {
                    AddErrors(result);
                }

                #region default registration


                //}
                #endregion
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/RegisterConfirmation
        [AllowAnonymous]
        public async Task<ActionResult> RegisterConfirmation(string Id)
        {
            if (await ConfirmAccount(Id))
            {
                return RedirectToAction("Index", "Result", new { Message = ResultMessageId.ConfirmationSuccess });
            }
            return RedirectToAction("Index", "Result", new { Message = ResultMessageId.ConfirmationFailure });
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "הסיסמא שונתה בהצלחה."
                : message == ManageMessageId.SetPasswordSuccess ? "הסיסמא עודכנה."
                : message == ManageMessageId.RemoveLoginSuccess ? "ההתחברות החיצונית הוסרה בהצלחה."
                : message == ManageMessageId.Error ? "שגיאה."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            ApplicationUser user = context.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user == null)// no such email
            {
                return View(model);//error
            }
            else// user found send reset password email
            {
                // Update token
                string confirmationToken = CreateConfirmationToken();
                user.ConfirmationToken = confirmationToken;
                DbSet<ApplicationUser> dbSet = context.Set<ApplicationUser>();
                dbSet.Attach(user);
                context.Entry(user).State = EntityState.Modified;
                await context.SaveChangesAsync();
                // Send reset password email
                Email.Send(user.Email, user.UserName, confirmationToken, EmailTemplate.ResetPassword);
                return RedirectToAction("Index", "Result", new { Message = ResultMessageId.ResetPasswordEmail, userName = user.Email });
            }
        }

        //
        // GET: /Account/ResetPasswordStepTwo
        [AllowAnonymous]
        public ActionResult ResetPasswordStepTwo(string Id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            ApplicationUser user = context.Users.SingleOrDefault(u => u.ConfirmationToken == Id);
            if (user != null)
            {
                // Correct token 
                ResetPasswordStepTwoViewModel model = new ResetPasswordStepTwoViewModel();
                model.UserId = user.Id;
                return View(model);
            }
            // Wrong token dispaly error
            return RedirectToAction("Index", "Result", new { Message = ResultMessageId.ResetPasswordTokenError });
        }

        //
        // POST: /Account/ResetPasswordStepTwo
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPasswordStepTwo(ResetPasswordStepTwoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var res1 = UserManager.RemovePassword(model.UserId);
                var res2 = UserManager.AddPassword(model.UserId, model.NewPassword);
                if (res2.Succeeded)
                {
                    // Password changed
                    return RedirectToAction("Index","Result", new { Message = ResultMessageId.ResetPasswordCompleted });
                }
                else
                {
                    // Error
                    return RedirectToAction("Index","Result", new { Message = ResultMessageId.Error });
                }
            }
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }
            //Get user email
            var externalIdentity = await AuthenticationManager
           .GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);

            var emailClaim = externalIdentity.Claims.FirstOrDefault(x => x.Type.Equals(
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",
                    StringComparison.OrdinalIgnoreCase));

            var email = emailClaim != null ? emailClaim.Value : null;

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // Check if user already registered
                ApplicationUser appuser = Db.Users.SingleOrDefault(u => u.Email == email);
                if (appuser != null)
                {
                    if (!appuser.IsConfirmed)
                    {
                        // User account not confirmed
                        appuser.IsConfirmed = true;
                        DbSet<ApplicationUser> dbSet = Db.Set<ApplicationUser>();
                        dbSet.Attach(appuser);
                        Db.Entry(appuser).State = EntityState.Modified;
                        Db.SaveChanges();
                    }
                    // Add External login
                    var result = await UserManager.AddLoginAsync(appuser.Id, loginInfo.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(appuser, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                else
                {
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName, Email = email });
                }
            }
            return RedirectToAction("Index","Result", new { Message = ResultMessageId.Error });
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            // !!!! must check if exist
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Manage");

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                    return View("ExternalLoginFailure");

                var user = new ApplicationUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    ConfirmationToken = "0",
                    IsConfirmed = true
                };
                try
                {
                    // Create new user
                    var result = await UserManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        // create user external login
                        result = await UserManager.AddLoginAsync(user.Id, info.Login);
                        if (result.Succeeded)
                        {
                            await SignInAsync(user, isPersistent: false);
                            return RedirectToLocal(returnUrl);
                        }
                    }
                    AddErrors(result);
                }
                catch (Exception)
                {
                }

            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }



        /// <summary>
        /// Create token 
        /// </summary>
        /// <returns>string token</returns>
        private string CreateConfirmationToken()
        {
            return ShortGuid.NewGuid();
        }

        /// <summary>
        /// Add roles and add user to role
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task AddUserToRole(ApplicationUser user)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(Db));
            if (!rm.RoleExists("Admin"))
                rm.Create(new IdentityRole("Admin"));
            if (!rm.RoleExists("Moder"))
                rm.Create(new IdentityRole("Moder"));
            if (!rm.RoleExists("Tutor"))
                rm.Create(new IdentityRole("Tutor"));
            if (!rm.RoleExists("User"))
                rm.Create(new IdentityRole("User"));

            if (user.UserName.Equals("Ros"))
            {
                await UserManager.AddToRoleAsync(user.Id, "Admin");
                await UserManager.AddToRoleAsync(user.Id, "Moder");
                await UserManager.AddToRoleAsync(user.Id, "Tutor");
            }
        }

        /// <summary>
        /// Confirm account
        /// </summary>
        /// <param name="confirmationToken">token</param>
        /// <returns>true or false</returns>
        private async Task<bool> ConfirmAccount(string confirmationToken)
        {
            ApplicationUser user = Db.Users.SingleOrDefault(u => u.ConfirmationToken == confirmationToken);
            if (user != null && !user.IsConfirmed)
            {
                user.IsConfirmed = true;
                DbSet<ApplicationUser> dbSet = Db.Set<ApplicationUser>();
                dbSet.Attach(user);
                Db.Entry(user).State = EntityState.Modified;
                Db.SaveChanges();
                // Add roles
                await AddUserToRole(user);

                await UserManager.AddToRoleAsync(user.Id, "User");
                await SignInAsync(user, isPersistent: false);
                return true;
            }
            return false;
        }

        public enum ResultMessageId
        {
            RegisterStepTwo,
            ConfirmationSuccess,
            ConfirmationFailure,
            ResetPasswordEmail,
            ResetPasswordCompleted,
            ResetPasswordTokenError,
            Error
        }


        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        public string UserName { get; set; }
    }
}