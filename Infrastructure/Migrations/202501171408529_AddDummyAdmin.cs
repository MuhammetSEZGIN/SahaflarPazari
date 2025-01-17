namespace Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddDummyAdmin : DbMigration
    {
        public override void Up()
        {
            // Add the admin user and role using SQL commands
            Sql(@"
                IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Admin')
                BEGIN
                    INSERT INTO AspNetRoles (Id, Name) VALUES (NEWID(), 'Admin')
                END

                IF NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE UserName = 'admin')
                BEGIN
                    INSERT INTO AspNetUsers (Id, UserName, Email, EmailConfirmed, PhoneNumber, PhoneNumberConfirmed, PasswordHash, SecurityStamp, FirstName, LastName, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
                    VALUES (NEWID(), 'admin', 'admin@outlook.com', 0, '55555555555', 0, 'admin', NEWID(), 'admin', 'admin', 0, 0, 0)
                END

                DECLARE @UserId NVARCHAR(128)
                SELECT @UserId = Id FROM AspNetUsers WHERE UserName = 'admin'

                IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = @UserId AND RoleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Admin'))
                BEGIN
                    INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@UserId, (SELECT Id FROM AspNetRoles WHERE Name = 'Admin'))
                END
            ");
        }

        public override void Down()
        {
            // Remove the admin user and role using SQL commands
            Sql(@"
                DECLARE @UserId NVARCHAR(128)
                SELECT @UserId = Id FROM AspNetUsers WHERE UserName = 'admin'

                IF @UserId IS NOT NULL
                BEGIN
                    DELETE FROM AspNetUserRoles WHERE UserId = @UserId
                    DELETE FROM AspNetUsers WHERE Id = @UserId
                END

                IF EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Admin')
                BEGIN
                    DELETE FROM AspNetRoles WHERE Name = 'Admin'
                END
            ");
        }
    }
}
