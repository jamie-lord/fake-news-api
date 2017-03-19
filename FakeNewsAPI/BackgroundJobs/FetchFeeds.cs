using FakeNewsAPI.Helpers;
using FakeNewsAPI.Models;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using X.Text;

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
            var sources = db.Sources.ToList();
            foreach (Source source in sources)
            {
                WebClient client = new WebClient();
                try
                {
                    using (XmlReader reader = new SyndicationFeedXmlReader(client.OpenRead(source.RSSurl)))
                    {
                        SyndicationFeed feed = SyndicationFeed.Load(reader);

                        foreach (SyndicationItem syndicationItem in feed.Items)
                        {
                            AddNews(syndicationItem, source);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                source.LastScrape = DateTime.Now;
                db.Sources.AddOrUpdate(source);
                db.SaveChanges();
            }
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

            var keywords = ScrapeMainContent(news.Url);
            if (keywords != null && keywords.Count > 0)
            {
                foreach (var key in keywords)
                {
                    Keyword keyword = new Keyword { Name = key };
                    Keyword keywordInDb = db.Keywords.Where(k => k.Name == keyword.Name).SingleOrDefault();
                    if (keywordInDb == null)
                    {
                        db.Keywords.Add(keyword);
                    }
                    else
                    {
                        news.Keywords.Add(keywordInDb);
                    }
                }

            }
            else
            {
                // Ensure we've got some keywords or don't insert news item into database.
                return;
            }

            List<string> authors = new List<string>();
            foreach (var author in syndicationItem.Authors)
            {
                if (author.Name != null)
                {
                    authors.Add(author.Name);
                }
            }
            news.Authors = authors;
            List<string> categories = new List<string>();
            foreach (var category in syndicationItem.Categories)
            {
                if (category.Name != null)
                {
                    categories.Add(category.Name);
                }
            }
            news.Categories = categories;
            news.Published = syndicationItem.PublishDate.DateTime.ToNullIfTooEarlyForDb();
            news.Updated = syndicationItem.LastUpdatedTime.DateTime.ToNullIfTooEarlyForDb();
            news.Summary = syndicationItem.Summary.Text;
            news.Source = source;
            news.Title = syndicationItem.Title.Text;

            db.News.AddOrUpdate(news);
            db.SaveChanges();
        }

        private List<string> ScrapeMainContent(string uri)
        {
            var keywords = new List<string>();
            try
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument document = web.Load(uri);
                var article = "";
                foreach (HtmlNode paragraph in document.DocumentNode.SelectNodes("//p"))
                {
                    if (paragraph.InnerText == null || paragraph.InnerText.Length < 60)
                    {
                        continue;
                    }
                    var t = Regex.Replace(paragraph.InnerText, @"\t|\n|\r", " ");
                    article += " " + t;
                }
                if (article.Length > 0)
                {
                    keywords = TextHelper.GetKeywords(article.ToLower(), 30).Split(new string[] { ", " }, StringSplitOptions.None).Except(_excludeWords).ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return keywords;
        }

        private string[] _excludeWords = new[] { "the", "be", "to", "of", "and", "a", "in", "that", "have", "I", "it", "for", "not", "on", "with", "he", "as", "you", "do", "at", "this", "but", "his", "by", "from", "they", "we", "say", "her", "she", "or", "an", "will", "my", "one", "all", "would", "there", "their", "what", "so", "up", "out", "if", "about", "who", "get", "which", "go", "me", "when", "make", "can", "like", "time", "no", "just", "him", "know", "take", "person", "into", "year", "your", "good", "some", "could", "them", "see", "other", "than", "then", "now", "look", "only", "come", "its", "over", "think", "also", "back", "after", "use", "two", "how", "our", "work", "first", "well", "way", "even", "new", "want", "because", "any", "these", "give", "day", "most", "us" };
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