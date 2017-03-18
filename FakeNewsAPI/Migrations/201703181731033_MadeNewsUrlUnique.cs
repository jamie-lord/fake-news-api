namespace FakeNewsAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MadeNewsUrlUnique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "Url", c => c.String(nullable: false, maxLength: 450));
            CreateIndex("dbo.News", "Url", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.News", new[] { "Url" });
            AlterColumn("dbo.News", "Url", c => c.String(nullable: false));
        }
    }
}
