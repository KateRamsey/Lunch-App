namespace Lunch_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Friday : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Surveys", "DietaryIssues", c => c.Int(nullable: false));
            DropColumn("dbo.Surveys", "DiataryIssues");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Surveys", "DiataryIssues", c => c.Int(nullable: false));
            DropColumn("dbo.Surveys", "DietaryIssues");
        }
    }
}
