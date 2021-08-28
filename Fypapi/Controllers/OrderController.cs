using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fypapi.Models;

namespace Fypapi.Controllers
{
    public class OrderController : ApiController
    {
        FYPEntities fd = new FYPEntities();
        [HttpGet]
        public HttpResponseMessage allorders()
        {
            var order = fd.Orders.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, order);
        }
        [HttpPost]
        public HttpResponseMessage Addorders(Order o)
        {

            fd.Orders.Add(o);
            fd.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, o.oid);
        }
        [HttpPost]
        public HttpResponseMessage Addorderdetail(OrderDetail od)
        {

            fd.OrderDetails.Add(od);
            fd.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
        [HttpGet]
        public HttpResponseMessage customerorder(int id)
        {
            var od = fd.Orders.Where(c => c.cid == id).Select(o=> new { o.oid, o.odate, o.odeliverytime, o.Totalbill });

            return Request.CreateResponse(HttpStatusCode.OK, od);
        }
        [HttpGet]
        public HttpResponseMessage orderfood(int oid)
        {
            var o= fd.OrderDetails.Where(od => od.oid == oid).Join(fd.FoodItems,
                od => od.fid,
                f => f.fid,
                (od, f) => new { f.fName,od.foodQantity,f.FImage,f.fprice });

            return Request.CreateResponse(HttpStatusCode.OK, o);
        }

    }
}
