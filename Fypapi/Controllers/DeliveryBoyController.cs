using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fypapi.Models;
using Newtonsoft.Json;

namespace Fypapi.Controllers
{
    public class DeliveryBoyController : ApiController
    {
        FYPEntities fd = new FYPEntities();
        [HttpGet]
        public List<DeliverBoy> AlldeliveryBoy()
        {
            var del = fd.DeliverBoys.OrderBy(b => b.dname).ToList();
            //  return Request.CreateResponse(HttpStatusCode.OK,cus );
            return del;
        }
        [HttpGet]
        public HttpResponseMessage Dboyprofile(int id)
        {
            var res = fd.DeliverBoys.Where(c => c.did == id).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, res);
        }
        public HttpResponseMessage AdddeliveryBoy(DeliverBoy d)
        {
            try
            {
                fd.DeliverBoys.Add(d);
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, d.did);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }

        }
        [HttpPost]
        public HttpResponseMessage modifyDboy(DeliverBoy d)
        {
            try
            {
                var del = fd.DeliverBoys.Find(d.did);

                if (del== null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }
                fd.Entry(del).CurrentValues.SetValues(d);
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "UPDATED");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpPost]
        public HttpResponseMessage statsupdate(DeliverBoy d)
        {
            try
            {
                var del = fd.DeliverBoys.Find(d.did);

                if (del == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }
                del.WorkingStatus = d.WorkingStatus;
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "UPDATED");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        //[HttpGet]
        //public HttpResponseMessage dboyorder()
        //{
        //    var res = (from c in fd.Customers 
        //               join o in fd.Orders on c.cid equals o.cid
        //               join r in fd.Resturants on o.rid equals r.rid
        //               where  o.status=="pending" 
        //               select new
        //               {
        //                   c.cAddrees,
        //                   c.cname,
        //                   c.clattitude,
        //                   c.clongitude,
        //                   o.oid,
        //                   o.Totalbill,
        //                   o.acceptstatus,
        //                   r.rcname,
        //                   r.rcaddress,
        //                   r.rclattitude,
        //                   r.rclongitude,
        //               }
        //        ).ToList();
        //    return Request.CreateResponse(HttpStatusCode.OK, res);

        //}
        [HttpGet]
        public HttpResponseMessage dboyordersshow()
        {
            var res = (from c in fd.Customers
                       join o in fd.Orders on c.cid equals o.cid 
                       join od in fd.OrderDetails on o.oid equals od.oid
                       join f in fd.FoodItems on od.fid equals f.fid
                       join r in fd.Resturants on f.rid equals r.rid
                       where o.acceptstatus == true & o.status == "pending"
                       select new
                       {
                           c.cAddrees,
                           c.cname,
                           c.clattitude,
                           c.clongitude,
                           o.oid,
                           o.acceptstatus,
                           r.rcname,
                           r.rcaddress,
                           r.rclattitude,
                           r.rclongitude,
                       }
                ).Distinct().ToList();
            return Request.CreateResponse(HttpStatusCode.OK, res);

        }
        [HttpGet]
        public HttpResponseMessage dboyacceptordershow(int id)
        {
            var res = (from c in fd.Customers
                       join o in fd.Orders on c.cid equals o.cid
                       join od in fd.OrderDetails on o.oid equals od.oid
                       join f in fd.FoodItems on od.fid equals f.fid
                       join r in fd.Resturants on f.rid equals r.rid
                       where o.acceptstatus == true & (o.status == "accepted" | o.status=="picked") & o.did==id
                       select new
                       {
                           c.cAddrees,
                           c.cname,
                           c.clattitude,
                           c.clongitude,
                           o.oid,
                           o.acceptstatus,
                           r.rcname,
                           r.rcaddress,
                           r.rclattitude,
                           r.rclongitude,
                           o.status,
                       }
                ).Distinct().ToList();
            return Request.CreateResponse(HttpStatusCode.OK, res);

        }

        [HttpPost]
        public HttpResponseMessage updateorderstatus(Order o)
        {
            try
            {
                var del = fd.Orders.Find(o.oid);

                if (del == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }
                del.status = o.status;
                del.did = o.did;
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "UPDATED");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }

        }
        [HttpPost]
        public HttpResponseMessage updatepickupstatus(Order o)
        {
            try
            {
                var del = fd.Orders.Find(o.oid);

                if (del == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }
                del.status = o.status;
                del.opickuptime = o.opickuptime;
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "UPDATED");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }

        }
        [HttpPost]
        public HttpResponseMessage updatedeliveredstatus(Order o)
        {
            try
            {
                var del = fd.Orders.Find(o.oid);

                if (del == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "not found");
                }
                del.status = o.status;
                del.odeliverytime = o.odeliverytime;
                del.Delivereddate = o.Delivereddate;

                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "UPDATED");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }

        }
    }
}
