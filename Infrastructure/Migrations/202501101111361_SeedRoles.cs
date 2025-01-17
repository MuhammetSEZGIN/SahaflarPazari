namespace Infrastructure.Migrations
{
    using Infrastructure.Data;
    using Infrastructure.Identity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    
public partial class SeedRoles : DbMigration
    {
        public override void Up()
        {
            using (var context = new SahaflarPazari())
            {
                var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));

                if (!roleManager.RoleExists("Admin"))
                {
                    roleManager.Create(new ApplicationRole { Name = "Admin" });
                }

                if (!roleManager.RoleExists("User"))
                {
                    roleManager.Create(new ApplicationRole { Name = "User" });
                }
               
            }
        }

        public override void Down()
        {
            using (var context = new SahaflarPazari())
            {
                var roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(context));

                var adminRole = roleManager.FindByName("Admin");
                if (adminRole != null)
                {
                    roleManager.Delete(adminRole);
                }

                var userRole = roleManager.FindByName("User");
                if (userRole != null)
                {
                    roleManager.Delete(userRole);
                }
            }
        }
    }
}
