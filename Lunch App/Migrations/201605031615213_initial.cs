namespace Lunch_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Handle = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Resturants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Location = c.String(),
                        HoursOfOperation = c.String(),
                        PriceRange = c.Int(nullable: false),
                        Website = c.String(),
                        CuisineType = c.Int(nullable: false),
                        DietaryOptions = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lunches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MeetingDateTime = c.DateTime(nullable: false),
                        Creator_Id = c.String(maxLength: 128),
                        Resturant_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Creator_Id)
                .ForeignKey("dbo.Resturants", t => t.Resturant_Id)
                .Index(t => t.Creator_Id)
                .Index(t => t.Resturant_Id);
            
            CreateTable(
                "dbo.LunchMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvitedTime = c.DateTime(nullable: false),
                        Lunch_Id = c.Int(),
                        Member_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lunches", t => t.Lunch_Id)
                .ForeignKey("dbo.Users", t => t.Member_Id)
                .Index(t => t.Lunch_Id)
                .Index(t => t.Member_Id);
            
            CreateTable(
                "dbo.ResturantOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rank = c.Int(nullable: false),
                        Lunch_Id = c.Int(),
                        Resturant_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lunches", t => t.Lunch_Id)
                .ForeignKey("dbo.Resturants", t => t.Resturant_Id)
                .Index(t => t.Lunch_Id)
                .Index(t => t.Resturant_Id);
            
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsFinished = c.Boolean(nullable: false),
                        TimeAvailable = c.DateTime(nullable: false),
                        MinutesAvailiable = c.Int(nullable: false),
                        ZipCode = c.Int(nullable: false),
                        ZipCodeRadius = c.Int(nullable: false),
                        CuisineWanted = c.Int(nullable: false),
                        CuisineNotWanted = c.Int(nullable: false),
                        DiataryIssues = c.Int(nullable: false),
                        Lunch_Id = c.Int(),
                        SuggestedResturant_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lunches", t => t.Lunch_Id)
                .ForeignKey("dbo.Resturants", t => t.SuggestedResturant_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Lunch_Id)
                .Index(t => t.SuggestedResturant_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ResturantFans",
                c => new
                    {
                        Resturant_Id = c.Int(nullable: false),
                        LunchUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Resturant_Id, t.LunchUser_Id })
                .ForeignKey("dbo.Resturants", t => t.Resturant_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.LunchUser_Id, cascadeDelete: true)
                .Index(t => t.Resturant_Id)
                .Index(t => t.LunchUser_Id);
            
            CreateTable(
                "dbo.LunchBuddies",
                c => new
                    {
                        LunchUserId = c.String(nullable: false, maxLength: 128),
                        BuddyId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LunchUserId, t.BuddyId })
                .ForeignKey("dbo.Users", t => t.LunchUserId)
                .ForeignKey("dbo.Users", t => t.BuddyId)
                .Index(t => t.LunchUserId)
                .Index(t => t.BuddyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.LunchBuddies", "BuddyId", "dbo.Users");
            DropForeignKey("dbo.LunchBuddies", "LunchUserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.Surveys", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Surveys", "SuggestedResturant_Id", "dbo.Resturants");
            DropForeignKey("dbo.Surveys", "Lunch_Id", "dbo.Lunches");
            DropForeignKey("dbo.Lunches", "Resturant_Id", "dbo.Resturants");
            DropForeignKey("dbo.ResturantOptions", "Resturant_Id", "dbo.Resturants");
            DropForeignKey("dbo.ResturantOptions", "Lunch_Id", "dbo.Lunches");
            DropForeignKey("dbo.LunchMembers", "Member_Id", "dbo.Users");
            DropForeignKey("dbo.LunchMembers", "Lunch_Id", "dbo.Lunches");
            DropForeignKey("dbo.Lunches", "Creator_Id", "dbo.Users");
            DropForeignKey("dbo.ResturantFans", "LunchUser_Id", "dbo.Users");
            DropForeignKey("dbo.ResturantFans", "Resturant_Id", "dbo.Resturants");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropIndex("dbo.LunchBuddies", new[] { "BuddyId" });
            DropIndex("dbo.LunchBuddies", new[] { "LunchUserId" });
            DropIndex("dbo.ResturantFans", new[] { "LunchUser_Id" });
            DropIndex("dbo.ResturantFans", new[] { "Resturant_Id" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.Surveys", new[] { "User_Id" });
            DropIndex("dbo.Surveys", new[] { "SuggestedResturant_Id" });
            DropIndex("dbo.Surveys", new[] { "Lunch_Id" });
            DropIndex("dbo.ResturantOptions", new[] { "Resturant_Id" });
            DropIndex("dbo.ResturantOptions", new[] { "Lunch_Id" });
            DropIndex("dbo.LunchMembers", new[] { "Member_Id" });
            DropIndex("dbo.LunchMembers", new[] { "Lunch_Id" });
            DropIndex("dbo.Lunches", new[] { "Resturant_Id" });
            DropIndex("dbo.Lunches", new[] { "Creator_Id" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropTable("dbo.LunchBuddies");
            DropTable("dbo.ResturantFans");
            DropTable("dbo.UserLogins");
            DropTable("dbo.Surveys");
            DropTable("dbo.ResturantOptions");
            DropTable("dbo.LunchMembers");
            DropTable("dbo.Lunches");
            DropTable("dbo.Resturants");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
        }
    }
}
