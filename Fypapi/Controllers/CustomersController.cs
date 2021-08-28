using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fypapi.Models;
                 
namespace Fypapi.Controllers
{
    public class CustomersController : ApiController
    {
        FYPEntities fd = new FYPEntities();
        [HttpGet]
        public HttpResponseMessage CustomerAlerts(int cid)
        {
            var food = (from os in fd.ordersstatus
                        join r in fd.Resturants on os.rid equals r.rid
                        where os.cid == cid & os.acceptstatus == "cancel" & os.avalibilitystatus =="unchecked"
                        select new
                        {
                            os.osid,
                            r.rcname
                        }
                        ).Distinct().ToList();
            return Request.CreateResponse(HttpStatusCode.OK, food);
        }
        [HttpGet]
        public List<Customer> Allcustomers()
        {
                var cus = fd.Customers.OrderBy(b => b.cname).ToList();
              //  return Request.CreateResponse(HttpStatusCode.OK,cus );
                return cus;
            
        }
        [HttpGet]
        public List<Customer> customerprofile(int id )
        {
            var cus = fd.Customers.Where(c=>c.cid==id).ToList();
            //  return Request.CreateResponse(HttpStatusCode.OK,cus );
            return cus;

        }
        public HttpResponseMessage Addcustomers(Customer c)
        {
            try
            {
                fd.Customers.Add(c);
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, c.cid);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpPost]
        public HttpResponseMessage modifycusto(Customer c)
        {
            try
            {
                var cus = fd.Customers.Find(c.cid);
                if (cus == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }
                fd.Entry(cus).CurrentValues.SetValues(c);
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "UPDATED");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage showschedule(int id)
        {
            try
            {
                // var sch = fd.scheduledata2().ToList();
                //var fooditem = fd.FoodItems;
                //var sch = fd.Menus.Join(fd.FoodItems, m => m.mid, f => f.fid, (m, f) => new
                //{
                //    fname = f.fName,
                //    day=m.Deleiveryday,
                //}) ;
                var sch = (from  m in fd.Menus 
                           join f in fd.FoodItems on m.fid equals f.fid
                           join r in fd.Resturants on f.rid equals r.rid
                           where m.cid == id
                           select new
                           {
                               f.fName,
                               m.quantity,
                               m.Routinetype,
                               m.dtime,
                               f.FImage,
                               f.fprice,
                               r.rcname,
                               m.Deleiveryday,
                               m.activatestatus,
                               m.mid,
                               m.oid
                           }
                ).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, sch);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}