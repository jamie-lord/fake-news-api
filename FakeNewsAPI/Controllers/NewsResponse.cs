using FakeNewsAPI.Models;

namespace FakeNewsAPI.Controllers
{
    public class NewsResponse : News
    {
        public News[] Related { get; set; }
    }
}