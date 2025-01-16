using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SahaflarPazari.Models;
using Infrastructure.Identity;
using System.Collections.Generic;
using System.Linq;

namespace SahaflarPazari.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // Identity managers from OWIN context
        private ApplicationUserManager _userManager =>
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private ApplicationSignInManager _signInManager =>
            HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

        private IAuthenticationManager _authManager =>
            HttpContext.GetOwinContext().Authentication;

        /// <summary>
        /// Logs out user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Logout()
        {
            _authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// GET: Login
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// POST: Login
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                  return Json(new
                    {
                        success = false,
                        message = "Validation failed",
                    });
                
            }

            var result = await _signInManager.PasswordSignInAsync(
                loginModel.userName,
                loginModel.password,
                isPersistent: false,   
                shouldLockout: false
            );

            switch (result)
            {
                case SignInStatus.Success:
                    {
                        
                        return Json(new { 
                            success = true,
                            redirectUrl = Url.Action("Index", "Home") 
                        });
                        
                    }
                case SignInStatus.Failure:
                default:
                    {
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return Json(new
                        {
                            success = false,
                            message = "Invalid login attempt. Please check your username/password."
                        });
                    }
            }
        }

        /// <summary>
        /// GET: Register
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// POST: Register
        /// Creates a new Identity user, signs them in, or shows errors
        /// </summary>

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel registerModel)
        {
            if (!ModelState.IsValid)
            {
                var fieldErrors = GetFieldErrors(ModelState);
                    return Json(new
                    {
                        success = false,
                        fieldErrors,
                        message = "Validation failed"
                    });
            }
            var newUser = new ApplicationUser
            {
                UserName = registerModel.UserName,
                Email = registerModel.Email,
                PhoneNumber = registerModel.Phone,
                FirstName= registerModel.FirstName,
                LastName= registerModel.LastName
            };

            var result = await _userManager.CreateAsync(newUser, registerModel.Password);
            if (result.Succeeded)
            {

                //System.Diagnostics.Debug.WriteLine(newUser.Id);
                //System.Diagnostics.Debug.WriteLine("\n"+ newUser);

                var roleResult = await _userManager.AddToRoleAsync(newUser.Id, "User");
                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    var roleErrors = GetFieldErrors(ModelState);
                    return Json(new
                    {
                        success = false,
                        fieldErrors= roleErrors,
                        message = "Failed to add user to role."
                    });
                }
                await _signInManager.SignInAsync(newUser, isPersistent: false, rememberBrowser: false);

                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("Index", "Home")
                });
                
            }
       
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError(string.Empty, err);
            }
            var identityErrors = GetFieldErrors(ModelState);
          
            return Json(new
            {
                success = false,
                fieldErrors= identityErrors,
                message = "Failed to register user."
            });
           
        }


        /// <summary>
        /// Example helper to build a dictionary of field -> list of error messages
        /// so the client can display them next to each field
        /// </summary>
        private Dictionary<string, List<string>> GetFieldErrors(ModelStateDictionary modelState)
        {
            var errorsDict = new Dictionary<string, List<string>>();
            
            foreach (var states in modelState)
            {
                // key usually matches the property, e.g. "UserName", "Email", etc.
                var state = states.Value;
                var stateKey= states.Key;
                var fieldErrors = state.Errors.Select(e => e.ErrorMessage).
                    Where(msg => !string.IsNullOrEmpty(msg)).
                    ToList();

                if (fieldErrors.Any())
                    errorsDict[states.Key] = fieldErrors;
            }
            return errorsDict;
        }
    }
}
