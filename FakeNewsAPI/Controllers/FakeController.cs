using FakeNewsAPI.Models;
using System.Data;
using System.Linq;
using System.Web.Http;

namespace FakeNewsAPI.Controllers
{
    public class FakeController : ApiController
    {
        private readonly FakeNewsAPIContext db = new FakeNewsAPIContext();
        public IQueryable<News> GetFakeNews()
        {
            return db.News.Where(n => n.Score > 0).OrderBy(n => n.Score).Take(30);
        }
    }
}
