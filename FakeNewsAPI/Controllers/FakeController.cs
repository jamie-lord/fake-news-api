using System.Linq;
using System.Web.Http;

namespace FakeNewsAPI.Controllers
{
    public class FakeController : ApiController
    {
        public IQueryable<NewsResponse> GetFakeNews()
        {
            return null;
        }
    }
}
