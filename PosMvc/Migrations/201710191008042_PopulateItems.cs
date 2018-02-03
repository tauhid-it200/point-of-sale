namespace PosMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateItems : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Items (Name, Price, Stock) VALUES ('Pen', 5, 50)");
            Sql("INSERT INTO Items (Name, Price, Stock) VALUES ('Shirt', 100, 20)");
            Sql("INSERT INTO Items (Name, Price, Stock) VALUES ('Cap', 50, 30)");
        }
        
        public override void Down()
        {
        }
    }
}
