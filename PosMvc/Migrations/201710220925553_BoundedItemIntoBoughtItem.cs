namespace PosMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BoundedItemIntoBoughtItem : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.BoughtItems", "ItemId");
            AddForeignKey("dbo.BoughtItems", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BoughtItems", "ItemId", "dbo.Items");
            DropIndex("dbo.BoughtItems", new[] { "ItemId" });
        }
    }
}
