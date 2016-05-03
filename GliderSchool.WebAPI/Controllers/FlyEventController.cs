using GliderSchool.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GliderSchool.WebAPI.Controllers
{
    [RoutePrefix("api/FlyEvent")]
    public class FlyEventController : ApiController
    {
        [Route("")]
        public IHttpActionResult GetAllFlyEvents()
        {
            var flyEventList = new List<FlyEvent>();

            flyEventList.Add(new FlyEvent { Title = "Engstlegenalp Überflug", Description = "Wir gehen auf die Engstlegenalp und machen einen tollen Tag", Remarks = "Warme kleider mitnehmen" });
            flyEventList.Add(new FlyEvent { Title = "Hummel Abendflug", Description = "Wir gehen auf den Hummel und machen einen tollen Tag", Remarks = "Testmaterial dabei" });
            flyEventList.Add(new FlyEvent { Title = "Rigi Soaring", Description = "Wir gehen auf die Rigi und soaren", Remarks = "Warme kleider mitnehmen" });

            return Ok(flyEventList);
        }
    }
}
