using System.Web.Mvc;
using System.Web.Security;

using BI.Web.Models;
using Contec.Data.Services;

namespace BI.Web.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IFormsAuthenticationService formsAuthenticationService, IAccountService accountService)
        {
            _formsService = formsAuthenticationService;
            _accountService = accountService;
        }

        // Account/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_accountService.ValidateUser(model.Username, model.Password))
                {
                    _formsService.SignIn(model.Username, model.RememberMe);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // Account/Logout
        public ActionResult Logout()
        {
            _formsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // Account/Register
        public ActionResult Register()
        {
            ViewData["PasswordLength"] = _accountService.MinPasswordLength;

            return View();
        }

        [HttpPost]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                var createStatus = _accountService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    _formsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "User");
                }

                ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = _accountService.MinPasswordLength;

            return View(model);
        }

        // Account/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = _accountService.MinPasswordLength;

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                if (_accountService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }

                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = _accountService.MinPasswordLength;

            return View(model);
        }

        // Account/ChangePasswordSuccess
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        private readonly IFormsAuthenticationService _formsService;
        private readonly IAccountService _accountService;
    }
}