using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fypapi.Models;
namespace Fypapi.Controllers
{
    public class ResturantController : ApiController
    {
        FYPEntities fd = new FYPEntities();
        [HttpGet]
        public HttpResponseMessage Resturantprofile(int id)
        {
            var res = fd.Resturants.Where(c => c.rid == id).Select(r => new { r.rccity, r.rcnumber, r.Category, r.rid, r.rcname, r.rcaddress, r.rcemail, r.rcImage, r.ownername, r.rpassword }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }
        [HttpPost]
        public HttpResponseMessage modifyResturant(Resturant r)
        {
            try
            {
                var res = fd.Resturants.Find(r.rid);

                if (res == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }
                fd.Entry(res).CurrentValues.SetValues(r);
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "UPDATED");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage AllResturant()
        {
            var res = fd.Resturants.Select(r => new { rid = r.rid, rcname = r.rcname, rcaddress = r.rcaddress, rccity = r.rccity, rcemail = r.rcemail, rpassword = r.rpassword, rcImage = r.rcImage, ownername = r.ownername, Category = r.Category, r.rclattitude, r.rclongitude }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, res);


        }
        [HttpGet]
        public HttpResponseMessage closeResturant()
        {
            var res = fd.Resturants.Select(r => new { r.rccity, r.rcnumber, r.Category, r.rid, r.rcname, r.rcaddress, r.rcemail, r.ownername, r.rpassword, r.rclattitude, r.rclongitude }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, res);


        }

        [HttpPost]
        public HttpResponseMessage AddResturant(Resturant r)
        {

            try
            {
                fd.Resturants.Add(r);
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage Resturantfood(int id)
        {
            try
            {
                var food = fd.FoodItems.Where(x => x.rid == id).Select(s => new
                {
                    fid = s.fid,
                    fname = s.fName,
                    ftype = s.ftype,
                    fprice = s.fprice,
                    fImagepath = s.FImage.ToString()
                });
                return Request.CreateResponse(HttpStatusCode.OK, food);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage resturantsimpleorder(int id)
        {
            // 2 / 12 / 2021 "2021-02-12T00:00:00"== o.deliverydate
            DateTime now = DateTime.Now;
            string date = now.ToString("yyyy-MM-dd");
            date = "" + date + "T00:00:00";
            var res = (from c in fd.Customers
                       join o in fd.Orders on c.cid equals o.cid
                       join od in fd.OrderDetails on o.oid equals od.oid
                       join f in fd.FoodItems on od.fid equals f.fid
                       where f.rid == id & o.status != "pickup" & o.schday == "tuesday"
                       select new
                       {
                           o.oid,
                           c.cname,
                           o.Totalbill,
                           o.acceptstatus,
                           o.otime,
                           o.odate,
                           o.odeliverytime,
                           o.did,
                           o.deliverydate,
                           f.fName,
                           od.foodQantity,
                           o.cid,



                       }

                ).Distinct().ToList();
            return Request.CreateResponse(HttpStatusCode.OK, res);

        }
        [HttpGet]
        public HttpResponseMessage currentorderdetail(int id)
        {
            var res = (from od in fd.OrderDetails
                       join f in fd.FoodItems on od.fid equals f.fid
                       where od.oid == id
                       select new
                       {
                           f.fName,
                           od.foodQantity,

                       }
                ).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, res);

        }
        [HttpGet]
        public HttpResponseMessage resturantschduleorder(int id)
        {
            var res = (from c in fd.Customers
                       join m in fd.Menus on c.cid equals m.cid
                       join f in fd.FoodItems on m.fid equals f.fid
                       where f.rid == id
                       select new
                       {
                           c.cname,
                           m.quantity,
                           f.fName,
                       }
                ).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, res);

        }
        [HttpGet]
        public HttpResponseMessage SearchResturant(string name)
        {
            var results = fd.Resturants.Where(x => x.rcname.Contains(name)).Select(s => new { s.rid, s.rcname, s.rcaddress, s.rccity, s.rcemail, s.rpassword, s.rcImage, s.ownername, s.Category }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, results);


        }

        [HttpPost]
        public HttpResponseMessage acceptupdate(Order o)
        {
            try
            {
                var ord = fd.Orders.Find(o.oid);
                if (ord == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }
                ord.acceptstatus = o.acceptstatus;
                ord.rid = o.rid;
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "UPDATED");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpPost]
        public HttpResponseMessage acceptorderupdate(ordersstatu o)
        {
            try
            {
                fd.ordersstatus.Add(o);
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, o.osid);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage updatealert(int  osid)
        {
            var ord = fd.ordersstatus.Where(w=>w.osid.Equals(osid));
            foreach (var i in ord)
            {
                i.avalibilitystatus = "checked";
            }
                fd.SaveChanges();
          
            return Request.CreateResponse(HttpStatusCode.OK,"updated");
        }
    }
}