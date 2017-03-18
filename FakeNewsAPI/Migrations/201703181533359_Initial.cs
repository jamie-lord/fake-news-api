namespace FakeNewsAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false),
                        Title = c.String(nullable: false),
                        Published = c.DateTime(nullable: false),
                        ImageUrl = c.String(),
                        Score = c.Double(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Source_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sources", t => t.Source_Id, cascadeDelete: true)
                .Index(t => t.Source_Id);
            
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RSSurl = c.String(nullable: false),
                        Title = c.String(),
                        LastScrape = c.DateTime(nullable: false),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "Source_Id", "dbo.Sources");
            DropIndex("dbo.News", new[] { "Source_Id" });
            DropTable("dbo.Sources");
            DropTable("dbo.News");
        }
    }
}
