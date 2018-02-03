namespace PosMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BoughtItems", "ItemId", "dbo.Items");
            DropIndex("dbo.BoughtItems", new[] { "ItemId" });
            DropColumn("dbo.BoughtItems", "ItemId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BoughtItems", "ItemId", c => c.Int(nullable: false));
            CreateIndex("dbo.BoughtItems", "ItemId");
            AddForeignKey("dbo.BoughtItems", "ItemId", "dbo.Items", "Id", cascadeDelete: true);
        }
    }
}
