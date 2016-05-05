namespace Lunch_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adaptModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resturants", "LocationZip", c => c.String());
            AddColumn("dbo.Surveys", "IsComing", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Surveys", "ZipCode", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Surveys", "ZipCode", c => c.Int(nullable: false));
            DropColumn("dbo.Surveys", "IsComing");
            DropColumn("dbo.Resturants", "LocationZip");
        }
    }
}
