namespace FakeNewsAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPublisherSourceToNewsModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "Source_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.News", "Source_Id");
            AddForeignKey("dbo.News", "Source_Id", "dbo.Sources", "Id", cascadeDelete: true);
            DropColumn("dbo.News", "Publisher");
        }
        
        public override void Down()
        {
            AddColumn("dbo.News", "Publisher", c => c.String(nullable: false));
            DropForeignKey("dbo.News", "Source_Id", "dbo.Sources");
            DropIndex("dbo.News", new[] { "Source_Id" });
            DropColumn("dbo.News", "Source_Id");
        }
    }
}
