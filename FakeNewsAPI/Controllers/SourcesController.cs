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
    public class SourcesController : ApiController
    {
        private FakeNewsAPIContext db = new FakeNewsAPIContext();

        // GET: api/Sources
        public IQueryable<Source> GetSources()
        {
            return db.Sources;
        }

        // GET: api/Sources/5
        [ResponseType(typeof(Source))]
        public async Task<IHttpActionResult> GetSource(int id)
        {
            Source source = await db.Sources.FindAsync(id);
            if (source == null)
            {
                return NotFound();
            }

            return Ok(source);
        }

        // PUT: api/Sources/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSource(int id, Source source)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != source.Id)
            {
                return BadRequest();
            }

            db.Entry(source).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SourceExists(id))
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

        // POST: api/Sources
        [ResponseType(typeof(Source))]
        public async Task<IHttpActionResult> PostSource(Source source)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sources.Add(source);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = source.Id }, source);
        }

        // DELETE: api/Sources/5
        [ResponseType(typeof(Source))]
        public async Task<IHttpActionResult> DeleteSource(int id)
        {
            Source source = await db.Sources.FindAsync(id);
            if (source == null)
            {
                return NotFound();
            }

            db.Sources.Remove(source);
            await db.SaveChangesAsync();

            return Ok(source);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SourceExists(int id)
        {
            return db.Sources.Count(e => e.Id == id) > 0;
        }
    }
}