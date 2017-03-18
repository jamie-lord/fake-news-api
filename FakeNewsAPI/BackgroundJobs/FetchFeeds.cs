using FakeNewsAPI.Helpers;
using FakeNewsAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.ServiceModel.Syndication;
using System.Xml;

namespace FakeNewsAPI.BackgroundTasks
{
    public sealed class FetchFeeds
    {
        private readonly FakeNewsAPIContext db = new FakeNewsAPIContext();

        private static readonly Lazy<FetchFeeds> lazy =
        new Lazy<FetchFeeds>(() => new FetchFeeds());

        public static FetchFeeds Instance { get { return lazy.Value; } }

        private FetchFeeds()
        {
        }

        public void FetchAll()
        {
            foreach (Source source in db.Sources)
            {
                Rss20FeedFormatter rssFormatter;

                using (var xmlReader = XmlReader.Create
                   (source.RSSurl))
                {
                    rssFormatter = new Rss20FeedFormatter();
                    rssFormatter.ReadFrom(xmlReader);

                }

                var title = rssFormatter.Feed.Title.Text;

                foreach (SyndicationItem syndicationItem in rssFormatter.Feed.Items)
                {
                    AddNews(syndicationItem, source);
                }
                source.LastScrape = DateTime.Now;
                db.Sources.AddOrUpdate(source);
            }
            db.SaveChanges();
        }

        private void AddNews(SyndicationItem syndicationItem, Source source)
        {
            News news = new News();
            news.Url = syndicationItem.Links[0].Uri.ToString();
            List<string> authors = new List<string>();
            foreach (var author in syndicationItem.Authors)
            {
                authors.Add(author.Name.ToString());
            }
            news.Authors = authors;
            news.Published = syndicationItem.PublishDate.DateTime.ToNullIfTooEarlyForDb();
            news.Updated = syndicationItem.LastUpdatedTime.DateTime.ToNullIfTooEarlyForDb();
            news.Summary = syndicationItem.Summary.Text;
            news.Source = source;
            news.Title = syndicationItem.Title.Text;

            db.News.AddOrUpdate(news);
        }
    }
}