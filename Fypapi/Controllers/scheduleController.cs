using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fypapi.Models;
namespace Fypapi.Controllers
{
    public class scheduleController : ApiController
    {
        FYPEntities fd = new FYPEntities();
        public HttpResponseMessage addschedule(Menu m)
        {
            try
            {
                var sch = fd.Menus.Add(m);
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK,m.mid);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
       
        [HttpGet]
        public HttpResponseMessage showschedule(string d)
        {
            try
            {
                var sch = fd.scheduledataday1(d).ToList();
                //var fooditem = fd.FoodItems;
                //var sch = fd.Menus.Join(fd.FoodItems, m => m.mid, f => f.fid, (m, f) => new
                //{
                //    fname = f.fName,
                //    day=m.Deleiveryday,
                //}) ;

                return Request.CreateResponse(HttpStatusCode.OK, sch);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage DeleteSchedule(int mid)
        {
            try
            {
                var sch = fd.Menus.Find(mid);
                if (sch == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Food_item not Found");
                }
                fd.Entry(sch).State = System.Data.Entity.EntityState.Deleted;
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Food is Delete");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage updateactivestatus(int mid,bool status)
        {
            try
            {
                var men = fd.Menus.Find(mid);
                if (men == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }

                men.activatestatus=status;
                fd.SaveChanges();
             
                 return Request.CreateResponse(HttpStatusCode.OK, men);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        public HttpResponseMessage GetFood(int oid) {

            var ord = new orderdata();
            var ot ="12/2/2021";
            ord.oid = oid;
            ord.otime = ot;
            var fd = new List<orderfood>();
            //
            ord.fooddata = fd;

            return Request.CreateResponse(HttpStatusCode.OK, "UPDATED");
        }
    }
    public class orderdata {
        public int oid { get; set; }
        public string otime { get; set; }
        public List<orderfood> fooddata { get; set; }

    }
    public class orderfood {
        public string fName { get; set; }
        public int foodqantity { get; set; }
    }
}
