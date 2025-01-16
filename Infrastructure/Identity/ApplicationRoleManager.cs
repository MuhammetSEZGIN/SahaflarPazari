using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin; 
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Infrastructure.Data;
namespace Infrastructure.Identity
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var roleStore = new RoleStore<ApplicationRole>(context.Get<SahaflarPazari>());
            return new ApplicationRoleManager(roleStore);
        }
    }
}
