using FakeNewsAPI.Helpers;
using FakeNewsAPI.Models;
using Newtonsoft.Json;
using OpenScraping;
using OpenScraping.Config;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
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
                WebClient client = new WebClient();
                using (XmlReader reader = new SyndicationFeedXmlReader(client.OpenRead(source.RSSurl)))
                {
                    SyndicationFeed feed = SyndicationFeed.Load(reader);

                    foreach (SyndicationItem syndicationItem in feed.Items)
                    {
                        AddNews(syndicationItem, source);
                    }
                }
                source.LastScrape = DateTime.Now;
                db.Sources.AddOrUpdate(source);
            }
            db.SaveChanges();
        }

        private void AddNews(SyndicationItem syndicationItem, Source source)
        {
            string uri = syndicationItem.Links[0].Uri.ToString();
            if (db.News.Any(n => n.Url == uri))
            {
                return;
            }
            News news = new News();
            news.Url = uri;
            List<string> authors = new List<string>();
            foreach (var author in syndicationItem.Authors)
            {
                if (author.Name != null)
                {
                    authors.Add(author.Name.ToString());
                }
            }
            news.Authors = authors;
            news.Published = syndicationItem.PublishDate.DateTime.ToNullIfTooEarlyForDb();
            news.Updated = syndicationItem.LastUpdatedTime.DateTime.ToNullIfTooEarlyForDb();
            news.Summary = syndicationItem.Summary.Text;
            news.Source = source;
            news.Title = syndicationItem.Title.Text;

            ScrapeMainContent(news.Url);

            db.News.AddOrUpdate(news);
        }

        private void ScrapeMainContent(string uri)
        {
            var configJson = @"
            {
                'body': '//div[@itemprop=\'articleBody\']'
            }
            ";

            var config = StructuredDataConfig.ParseJsonString(configJson);

            using (WebClient client = new WebClient())
            {
                string html = client.DownloadString(uri);

                var openScraping = new StructuredDataExtractor(config);
                var scrapingResults = openScraping.Extract(html);

                var r = JsonConvert.SerializeObject(scrapingResults, Newtonsoft.Json.Formatting.Indented);
            }


        }
    }
}