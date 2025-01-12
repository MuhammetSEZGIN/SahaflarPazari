using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using Infrastructure.Data;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<SahaflarPazari>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };



            // Configure validation logic for passwords
            // I want to control it before coming here in viewModel so we are not gonna use it
            //manager.PasswordValidator = new PasswordValidator
            //{
            //    RequiredLength = 6,
            //    RequireNonLetterOrDigit = true,
            //    RequireDigit = true,
            //    RequireLowercase = true,
            //    RequireUppercase = true,
            //};

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            return manager;
        }
        public async Task <IdentityResult> SetFirstName(string userId, string firstName)
        {
            var user = await FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed("User not found");
            }
            user.FirstName = firstName;
            return await UpdateAsync(user);
            
        }  public async Task <IdentityResult> SetLastName(string userId, string lastName)
        {
            var user = await FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed("User not found");
            }
            user.LastName = lastName;
            return await UpdateAsync(user);
        }


    }
}
