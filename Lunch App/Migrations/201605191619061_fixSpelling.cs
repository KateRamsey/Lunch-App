namespace Lunch_App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixSpelling : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Resturants", newName: "Restaurants");
            RenameTable(name: "dbo.ResturantOptions", newName: "RestaurantOptions");
            RenameTable(name: "dbo.ResturantFans", newName: "RestaurantFans");
            RenameColumn(table: "dbo.RestaurantFans", name: "Resturant_Id", newName: "Restaurant_Id");
            RenameColumn(table: "dbo.Lunches", name: "Resturant_Id", newName: "Restaurant_Id");
            RenameColumn(table: "dbo.Surveys", name: "SuggestedResturant_Id", newName: "SuggestedRestaurant_Id");
            RenameColumn(table: "dbo.RestaurantOptions", name: "Resturant_Id", newName: "Restaurant_Id");
            RenameIndex(table: "dbo.Lunches", name: "IX_Resturant_Id", newName: "IX_Restaurant_Id");
            RenameIndex(table: "dbo.Surveys", name: "IX_SuggestedResturant_Id", newName: "IX_SuggestedRestaurant_Id");
            RenameIndex(table: "dbo.RestaurantOptions", name: "IX_Resturant_Id", newName: "IX_Restaurant_Id");
            RenameIndex(table: "dbo.RestaurantFans", name: "IX_Resturant_Id", newName: "IX_Restaurant_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.RestaurantFans", name: "IX_Restaurant_Id", newName: "IX_Resturant_Id");
            RenameIndex(table: "dbo.RestaurantOptions", name: "IX_Restaurant_Id", newName: "IX_Resturant_Id");
            RenameIndex(table: "dbo.Surveys", name: "IX_SuggestedRestaurant_Id", newName: "IX_SuggestedResturant_Id");
            RenameIndex(table: "dbo.Lunches", name: "IX_Restaurant_Id", newName: "IX_Resturant_Id");
            RenameColumn(table: "dbo.RestaurantOptions", name: "Restaurant_Id", newName: "Resturant_Id");
            RenameColumn(table: "dbo.Surveys", name: "SuggestedRestaurant_Id", newName: "SuggestedResturant_Id");
            RenameColumn(table: "dbo.Lunches", name: "Restaurant_Id", newName: "Resturant_Id");
            RenameColumn(table: "dbo.RestaurantFans", name: "Restaurant_Id", newName: "Resturant_Id");
            RenameTable(name: "dbo.RestaurantFans", newName: "ResturantFans");
            RenameTable(name: "dbo.RestaurantOptions", newName: "ResturantOptions");
            RenameTable(name: "dbo.Restaurants", newName: "Resturants");
        }
    }
}
