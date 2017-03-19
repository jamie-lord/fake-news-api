using FakeNewsAPI.Models;
using System.Linq;
using System.Web.Http;

namespace FakeNewsAPI.Controllers
{
    public class LegitController : ApiController
    {
        private readonly FakeNewsAPIContext db = new FakeNewsAPIContext();
        public IQueryable<News> GetLegitNews()
        {
            return db.News.Where(n => n.Score > 0).OrderByDescending(n => n.Score).Take(30);
        }
    }
}
