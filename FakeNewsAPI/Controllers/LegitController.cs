using FakeNewsAPI.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FakeNewsAPI.Controllers
{
    [EnableCors(origins: "*", headers: "Access-Control-Allow-Origin: *", methods: "*")]
    public class LegitController : ApiController
    {
        private readonly FakeNewsAPIContext db = new FakeNewsAPIContext();
        public IQueryable<News> GetLegitNews()
        {
            return db.News.Where(n => n.Score > 0).Include(n => n.Source).Include(n => n.Keywords).OrderByDescending(n => n.Score).Take(30);
        }
    }
}
