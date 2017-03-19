using FakeNewsAPI.BackgroundJobs;
using FakeNewsAPI.BackgroundTasks;
using System.Web.Http;

namespace FakeNewsAPI.Controllers
{
    public class FetchFeedsController : ApiController
    {
        public string Get()
        {
            FetchFeeds.Instance.FetchAll();
            GenerateScores.Instance.UpdateAll();
            return "Working on it kiddo.";
        }
    }
}
