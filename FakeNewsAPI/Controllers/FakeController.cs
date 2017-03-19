using FakeNewsAPI.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace FakeNewsAPI.Controllers
{
    public class FakeController : ApiController
    {
        private readonly FakeNewsAPIContext db = new FakeNewsAPIContext();
        public IQueryable<News> GetFakeNews()
        {
            return db.News.Where(n => n.Score > 0).Include(n => n.Source).Include(n => n.Keywords).OrderBy(n => n.Score).Take(30);
        }
    }
}
