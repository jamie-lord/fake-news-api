using FakeNewsAPI.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace FakeNewsAPI.BackgroundJobs
{
    public class GenerateScores
    {
        private readonly FakeNewsAPIContext db = new FakeNewsAPIContext();

        private static readonly Lazy<GenerateScores> lazy = new Lazy<GenerateScores>(() => new GenerateScores());

        public static GenerateScores Instance { get { return lazy.Value; } }

        private GenerateScores()
        {
        }

        public void UpdateAll()
        {
            foreach (News news in db.News.Include(n => n.Keywords)
                .Include(n => n.Source))
            {
                double newScore = 0;
                foreach (Keyword keyword in news.Keywords)
                {
                    var relatedCount = keyword.News.Count;
                    newScore += relatedCount * 0.1;
                }
                if (newScore != 0)
                {
                    news.Score = newScore;
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }
        }
    }
}