using FakeNewsAPI.BackgroundTasks;
using System.Web.Http;

namespace FakeNewsAPI.Controllers
{
    public class FetchFeedsController : ApiController
    {
        public string Get()
        {
            FetchFeeds.Instance.FetchAll();
            return "Working on it kiddo.";
        }
    }
}
