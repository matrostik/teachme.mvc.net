using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using TeachMe.Models;
using TeachMe.Helpers;
using System.Security.Claims;
using System.Data.Entity;

namespace TeachMe.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new TeachMeDBContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
            // Allow email address as username
            UserManager.UserValidator = new UserValidator<ApplicationUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };
            Db = new TeachMeDBContext();
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }
        public TeachMeDBContext Db { get; private set; }


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
                if (user != null && user.EmailConfirmed)
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
                model.UserName = model.UserName.Trim();
                // Check email
                ApplicationUser user = await UserManager.FindByNameAsync(model.UserName); ;
                if (user != null)
                    ModelState.AddModelError("UserName", "דוא\"ל כבר קיים");
                // Display errors if has
                if (!ModelState.IsValid)
                    return View(model);

                // Create token and user
                string confirmationToken = CreateConfirmationToken();
                user = new ApplicationUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    ConfirmationToken = confirmationToken,
                    EmailConfirmed = false,
                    RegistrationDate = DateTime.Today
                };
                // Attempt to register the user
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Add user to Tutor role
                    await AddUserToRole(user, "Tutor");
                    // User created send confirmation mail
                    Email.Send(model.UserName, model.FirstName, confirmationToken, EmailTemplate.Registration);
                    // Redirect to ResultController (show message)
                    return RedirectToAction("Index", "Result", new { Message = ResultMessage.RegisterStepTwo });
                }
                else
                {
                    // Add error messages to model state
                    AddErrors(result);
                }
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
                return RedirectToAction("Index", "Result", new { Message = ResultMessage.ConfirmationSuccess });
            }
            return RedirectToAction("Index", "Result", new { Message = ResultMessage.ConfirmationFailure });
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
            return RedirectToAction("Manage", new { Message = message, tabId = "tab3" });
        }

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message, string tabId = "tab1")
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "הסיסמא שונתה בהצלחה."
                : message == ManageMessageId.SetPasswordSuccess ? "הסיסמא עודכנה."
                : message == ManageMessageId.RemoveLoginSuccess ? "ההתחברות החיצונית הוסרה בהצלחה."
                : message == ManageMessageId.Error ? "שגיאה."
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");

            var id1 = User.Identity.GetUserId();
            Teacher t = Db.Teachers.FirstOrDefault(x => x.ApplicationUserId == id1);
            ViewBag.HasProfile = t != null;
            ViewBag.ActiveTab = tabId;
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
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess, tabId = "tab2" });
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
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess, tabId = "tab2" });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            var id1 = User.Identity.GetUserId();
            Teacher t = Db.Teachers.FirstOrDefault(x => x.ApplicationUserId == id1);
            ViewBag.HasProfile = t != null;
            ViewBag.ActiveTab = "tab2";
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
            //ApplicationDbContext context = new ApplicationDbContext();
            ApplicationUser user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)// no such email
            {
                return View(model);//error
            }
            else// user found send reset password email
            {
                // Update token
                string confirmationToken = CreateConfirmationToken();
                user.ConfirmationToken = confirmationToken;
                var result = await UserManager.UpdateAsync(user);

                // Send reset password email
                Email.Send(user.UserName, "", confirmationToken, EmailTemplate.ResetPassword);
                return RedirectToAction("Index", "Result", new { Message = ResultMessage.ResetPasswordEmail, userName = user.UserName });
            }
        }

        //
        // GET: /Account/ResetPasswordStepTwo
        [AllowAnonymous]
        public ActionResult ResetPasswordStepTwo(string Id)
        {
            ApplicationUser user = Db.Users.SingleOrDefault(u => u.ConfirmationToken == Id);
            if (user != null)
            {
                // Correct token 
                ResetPasswordStepTwoViewModel model = new ResetPasswordStepTwoViewModel();
                model.UserId = user.Id;
                return View(model);
            }
            // Wrong token dispaly error
            return RedirectToAction("Index", "Result", new { Message = ResultMessage.ResetPasswordTokenError });
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
                    return RedirectToAction("Index", "Result", new { Message = ResultMessage.ResetPasswordCompleted });
                }
                else
                {
                    // Error
                    return RedirectToAction("Index", "Result", new { Message = ResultMessage.Error });
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

            // facebook fullname
            var nameClaim = externalIdentity.Claims.FirstOrDefault(c => c.Type == "urn:facebook:name");
            var fName = nameClaim != null ? nameClaim.Value : null;

            // google name
            nameClaim = externalIdentity.Claims.FirstOrDefault(c => c.Type.Equals(
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
                    StringComparison.OrdinalIgnoreCase));
            var gName = nameClaim != null ? nameClaim.Value : null;
            //split name
            var name = string.IsNullOrEmpty(fName) ? gName : fName;
            var st = name.Split(new char[] { ' ' }, 2);

            string firstName = st.Count() > 0 ? st[0] : string.Empty;
            string lastName = st.Count() > 1 ? st[1] : string.Empty;

            // get email, same for fb and google
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
                ApplicationUser appuser = await UserManager.FindByNameAsync(email);
                if (appuser != null)
                {
                    if (!appuser.EmailConfirmed)
                    {
                        // User account not confirmed
                        appuser.EmailConfirmed = true;
                        var res = UserManager.UpdateAsync(appuser);
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
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = email, FirstName = firstName, LastName = lastName });
                }
            }
            return RedirectToAction("Index", "Result", new { Message = ResultMessage.Error });
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
            ViewBag.ActiveTab = "tab1";
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error, tabId = "tab3" });
            }
            // !!!! must check if exist
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage", new { tabId = "tab3" });
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error, tabId = "tab3" });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Manage", new { tabId = "tab3" });

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                    return View("ExternalLoginFailure");

                var user = new ApplicationUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    ConfirmationToken = "0",
                    EmailConfirmed = true,
                    RegistrationDate = DateTime.Today
                };
                try
                {
                    // Create new user
                    var result = await UserManager.CreateAsync(user);
                    // Create roles and set specials users to roles
                    await AddUserToRole(user, "User");
                    await AddUserToRole(user, "Tutor");

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
        private async Task AddUserToRole(ApplicationUser user, string roleName = "User")
        {
            // Create roles must be executed only once
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(Db));
            if (!rm.RoleExists("Admin"))
                rm.Create(new IdentityRole("Admin"));
            if (!rm.RoleExists("Moder"))
                rm.Create(new IdentityRole("Moder"));
            if (!rm.RoleExists("Tutor"))
                rm.Create(new IdentityRole("Tutor"));
            if (!rm.RoleExists("User"))
                rm.Create(new IdentityRole("User"));
            // end create roles

            // Add users to special roles
            if (user.UserName.Equals("matrostik@gmail.com"))
            {
                await UserManager.AddToRoleAsync(user.Id, "Admin");
                await UserManager.AddToRoleAsync(user.Id, "Moder");
                await UserManager.AddToRoleAsync(user.Id, "Tutor");
            }
            // Set user to specified mail
            else
                await UserManager.AddToRoleAsync(user.Id, roleName);
        }

        /// <summary>
        /// Confirm account
        /// </summary>
        /// <param name="confirmationToken">token</param>
        /// <returns>true or false</returns>
        private async Task<bool> ConfirmAccount(string confirmationToken)
        {
            // Get user by token
            ApplicationUser user = Db.Users.SingleOrDefault(u => u.ConfirmationToken == confirmationToken);
            if (user != null && !user.EmailConfirmed)
            {
                // Activate user account 
                user.EmailConfirmed = true;

                // Save changes to db
                Db.Entry(user).State = EntityState.Modified;
                int t = Db.SaveChanges();

                // Add user to Tutor role
                await AddUserToRole(user, "Tutor");

                // Signin user
                await SignInAsync(user, isPersistent: false);
                return true;
            }
            return false;
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
            // Add user claims
            identity.AddClaim(new Claim("FirstName", user.FirstName != null ? user.FirstName : ""));
            identity.AddClaim(new Claim("LastName", user.LastName != null ? user.LastName : ""));
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                if (error.Equals("Incorrect password."))
                    ModelState.AddModelError("", "סיסמא שגויה.");
                else
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
    }
}