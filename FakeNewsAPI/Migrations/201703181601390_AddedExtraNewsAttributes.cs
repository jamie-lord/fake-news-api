namespace FakeNewsAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedExtraNewsAttributes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "Updated", c => c.DateTime(nullable: false));
            AddColumn("dbo.News", "Summary", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "Summary");
            DropColumn("dbo.News", "Updated");
        }
    }
}
