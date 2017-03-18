namespace FakeNewsAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDateTimeFixAgain : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "Published", c => c.DateTime());
            AlterColumn("dbo.News", "Updated", c => c.DateTime());
            AlterColumn("dbo.Sources", "LastScrape", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sources", "LastScrape", c => c.DateTime(nullable: false));
            AlterColumn("dbo.News", "Updated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.News", "Published", c => c.DateTime(nullable: false));
        }
    }
}
