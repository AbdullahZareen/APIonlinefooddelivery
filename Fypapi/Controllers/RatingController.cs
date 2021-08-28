using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fypapi.Models;
namespace Fypapi.Controllers
{
   
    public class RatingController : ApiController
    {
        FYPEntities fd = new FYPEntities();

        [HttpPost]
        public HttpResponseMessage Resturantrate(returantrating rating)
        {
            fd.returantratings.Add(rating);
            fd.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpPost]
        public HttpResponseMessage Dboyrate(Dboyrating rating)
        {
            fd.Dboyratings.Add(rating);
            fd.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpGet]
        public HttpResponseMessage updateresturantrate(int resratid, int stars)
        {
            var row = fd.returantratings.Where(w => w.resratid.Equals(resratid));
            foreach (var item in row)
            {
                item.rating = stars;
                item.status = "Done";

            }
            fd.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpGet]
        public HttpResponseMessage updatedboyrate(int ratid, int stars)
        {
            var row = fd.Dboyratings.Where(w => w.ratid.Equals(ratid));
            foreach (var item in row)
            {
                item.rating = stars;
                item.status = "Done";

            }
            fd.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Submitted");
        }
        public HttpResponseMessage GetRequestForDboy(int cid)
        {
            try
            {
                //var list = db.DriverRatings.Where(w => w.C_id.Equals(Cid) & w.Status.Equals("Pending")).ToList();
                var list = fd.Dboyratings.Where(x => x.cid == cid & x.status == "pending");
                return Request.CreateResponse(HttpStatusCode.OK, list);

            }
            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
