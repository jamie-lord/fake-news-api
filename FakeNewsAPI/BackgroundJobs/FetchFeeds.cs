using FakeNewsAPI.Models;
using System;
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

                foreach (var syndicationItem in rssFormatter.Feed.Items)
                {
                    //Console.WriteLine("Article: {0}",
                    //   syndicationItem.Title.Text);
                    //Console.WriteLine("URL: {0}",
                    //   syndicationItem.Links[0].Uri);
                    //Console.WriteLine("Summary: {0}",
                    //   syndicationItem.Summary.Text);
                    //Console.WriteLine();
                }
            }
        }
    }
}