namespace FakeNewsAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedNewsKeywords : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Keywords", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Keywords", "Id", c => c.Int(nullable: false));
        }
    }
}
