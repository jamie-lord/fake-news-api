namespace FakeNewsAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Keywords",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Url = c.String(nullable: false, maxLength: 450),
                        Title = c.String(nullable: false),
                        Published = c.DateTime(),
                        Updated = c.DateTime(),
                        ImageUrl = c.String(),
                        AuthorsString = c.String(),
                        Summary = c.String(),
                        CategoriesString = c.String(),
                        Score = c.Double(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Source_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sources", t => t.Source_Id, cascadeDelete: true)
                .Index(t => t.Url, unique: true)
                .Index(t => t.Source_Id);
            
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RSSurl = c.String(nullable: false),
                        Title = c.String(),
                        LastScrape = c.DateTime(),
                        Score = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NewsKeywords",
                c => new
                    {
                        News_Id = c.Int(nullable: false),
                        Keyword_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.News_Id, t.Keyword_Name })
                .ForeignKey("dbo.News", t => t.News_Id, cascadeDelete: true)
                .ForeignKey("dbo.Keywords", t => t.Keyword_Name, cascadeDelete: true)
                .Index(t => t.News_Id)
                .Index(t => t.Keyword_Name);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.News", "Source_Id", "dbo.Sources");
            DropForeignKey("dbo.NewsKeywords", "Keyword_Name", "dbo.Keywords");
            DropForeignKey("dbo.NewsKeywords", "News_Id", "dbo.News");
            DropIndex("dbo.NewsKeywords", new[] { "Keyword_Name" });
            DropIndex("dbo.NewsKeywords", new[] { "News_Id" });
            DropIndex("dbo.News", new[] { "Source_Id" });
            DropIndex("dbo.News", new[] { "Url" });
            DropTable("dbo.NewsKeywords");
            DropTable("dbo.Sources");
            DropTable("dbo.News");
            DropTable("dbo.Keywords");
        }
    }
}
