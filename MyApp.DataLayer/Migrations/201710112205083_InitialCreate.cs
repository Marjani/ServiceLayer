namespace MyApp.DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemEntry = c.Boolean(nullable: false),
                        CreationDateTime = c.DateTimeOffset(precision: 7),
                        LastModificationDateTime = c.DateTimeOffset(precision: 7),
                        CreatorIp = c.String(maxLength: 255),
                        LastModifierIp = c.String(maxLength: 255),
                        CreatorBrowserName = c.String(maxLength: 1024),
                        LastModifierBrowserName = c.String(maxLength: 1024),
                        LastModifierUserId = c.Long(),
                        CreatorUserId = c.Long(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .Index(t => t.Name, unique: true, name: "UIX_Role_Name")
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.RoleClaims",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ClaimType = c.String(nullable: false, maxLength: 256),
                        ClaimValue = c.String(nullable: false, maxLength: 256),
                        RoleId = c.Long(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        UserName = c.String(nullable: false, maxLength: 256),
                        NormalizedUserName = c.String(nullable: false, maxLength: 256),
                        Email = c.String(nullable: false, maxLength: 256),
                        NormalizedEmail = c.String(nullable: false, maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(nullable: false, maxLength: 256),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(maxLength: 20),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateTime = c.DateTimeOffset(precision: 7),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsSystemEntry = c.Boolean(nullable: false),
                        LastVisitDateTime = c.DateTimeOffset(precision: 7),
                        PhotoFileName = c.String(),
                        BirthDate = c.DateTimeOffset(precision: 7),
                        RegisterationDateTime = c.DateTimeOffset(nullable: false, precision: 7),
                        LastLoggedInDateTime = c.DateTimeOffset(nullable: false, precision: 7),
                        CreationDateTime = c.DateTimeOffset(precision: 7),
                        LastModificationDateTime = c.DateTimeOffset(precision: 7),
                        CreatorIp = c.String(maxLength: 255),
                        LastModifierIp = c.String(maxLength: 255),
                        CreatorBrowserName = c.String(maxLength: 1024),
                        LastModifierBrowserName = c.String(maxLength: 1024),
                        LastModifierUserId = c.Long(),
                        CreatorUserId = c.Long(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatorUserId)
                .ForeignKey("dbo.Users", t => t.LastModifierUserId)
                .Index(t => t.NormalizedUserName, unique: true, name: "UIX_User_NormalizedUserName")
                .Index(t => t.NormalizedEmail, unique: true, name: "UIX_User_NormalizedEmail")
                .Index(t => t.LastModifierUserId)
                .Index(t => t.CreatorUserId);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ClaimType = c.String(nullable: false, maxLength: 256),
                        ClaimValue = c.String(nullable: false, maxLength: 256),
                        UserId = c.Long(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LoginProvider = c.String(nullable: false, maxLength: 256),
                        ProviderKey = c.String(nullable: false, maxLength: 256),
                        UserId = c.Long(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => new { t.LoginProvider, t.ProviderKey, t.UserId }, unique: true, name: "UIX_User_LoginProvider_ProviderKey_UserId");
            
            CreateTable(
                "dbo.UserTokens",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserUsedPasswords",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HashedPassword = c.String(nullable: false, maxLength: 256),
                        UserId = c.Long(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        RoleId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.Users", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Roles", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.Roles", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.UserUsedPasswords", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserTokens", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRole", "UserId", "dbo.Roles");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "LastModifierUserId", "dbo.Users");
            DropForeignKey("dbo.Users", "CreatorUserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.RoleClaims", "RoleId", "dbo.Roles");
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserUsedPasswords", new[] { "UserId" });
            DropIndex("dbo.UserTokens", new[] { "UserId" });
            DropIndex("dbo.UserLogins", "UIX_User_LoginProvider_ProviderKey_UserId");
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "CreatorUserId" });
            DropIndex("dbo.Users", new[] { "LastModifierUserId" });
            DropIndex("dbo.Users", "UIX_User_NormalizedEmail");
            DropIndex("dbo.Users", "UIX_User_NormalizedUserName");
            DropIndex("dbo.RoleClaims", new[] { "RoleId" });
            DropIndex("dbo.Roles", new[] { "CreatorUserId" });
            DropIndex("dbo.Roles", new[] { "LastModifierUserId" });
            DropIndex("dbo.Roles", "UIX_Role_Name");
            DropTable("dbo.UserRole");
            DropTable("dbo.UserUsedPasswords");
            DropTable("dbo.UserTokens");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.RoleClaims");
            DropTable("dbo.Roles");
        }
    }
}
