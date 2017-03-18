using FakeNewsAPI.Helpers;
using FakeNewsAPI.Models;
using Newtonsoft.Json.Linq;
using OpenScraping;
using OpenScraping.Config;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
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
            ScrapeMainContent(uri);
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



            db.News.AddOrUpdate(news);
        }

        private void ScrapeMainContent(string uri)
        {
            using (WebClient client = new WebClient())
            {
                var configJson = @"{'body': '//div[@itemprop=\'articleBody\']'}";
                var config = StructuredDataConfig.ParseJsonString(configJson);
                string html = Regex.Replace(client.DownloadString(uri), @"\t|\n|\r", " ");
                var openScraping = new StructuredDataExtractor(config);
                var scrapingResults = openScraping.Extract(html);

                try
                {
                    var article = scrapingResults.FindTokens("body").First().ToString();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }

    public static class JsonExtensions
    {
        public static List<JToken> FindTokens(this JToken containerToken, string name)
        {
            List<JToken> matches = new List<JToken>();
            FindTokens(containerToken, name, matches);
            return matches;
        }

        private static void FindTokens(JToken containerToken, string name, List<JToken> matches)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (JProperty child in containerToken.Children<JProperty>())
                {
                    if (child.Name == name)
                    {
                        matches.Add(child.Value);
                    }
                    FindTokens(child.Value, name, matches);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    FindTokens(child, name, matches);
                }
            }
        }
    }
}