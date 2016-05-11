namespace Lunch_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddZipCache : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ZipCaches",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Zip = c.String(),
                        Radius = c.Int(nullable: false),
                        ZipsInRadius = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ZipCaches");
        }
    }
}
