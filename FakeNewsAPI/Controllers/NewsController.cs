using FakeNewsAPI.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace FakeNewsAPI.Controllers
{
    public class NewsController : ApiController
    {
        private FakeNewsAPIContext db = new FakeNewsAPIContext();

        // GET: api/News
        public IQueryable<News> GetNews()
        {
            return db.News;
        }

        // GET: api/News/5
        [ResponseType(typeof(News))]
        public async Task<IHttpActionResult> GetNews(int id)
        {
            News news = await db.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            return Ok(news);
        }

        // PUT: api/News/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNews(int id, News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != news.Id)
            {
                return BadRequest();
            }

            db.Entry(news).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/News
        [ResponseType(typeof(News))]
        public async Task<IHttpActionResult> PostNews(News news)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.News.Add(news);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = news.Id }, news);
        }

        // DELETE: api/News/5
        [ResponseType(typeof(News))]
        public async Task<IHttpActionResult> DeleteNews(int id)
        {
            News news = await db.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            db.News.Remove(news);
            await db.SaveChangesAsync();

            return Ok(news);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NewsExists(int id)
        {
            return db.News.Count(e => e.Id == id) > 0;
        }
    }
}