﻿using System.Linq;
using System.Web.Http;

namespace FakeNewsAPI.Controllers
{
    public class LegitController : ApiController
    {
        public IQueryable<NewsResponse> GetLegitNews()
        {
            return null;
        }
    }
}
