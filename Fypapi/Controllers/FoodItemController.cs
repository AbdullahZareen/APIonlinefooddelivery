using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fypapi.Models;


namespace Fypapi.Controllers
{
    public class FoodItemController : ApiController
    {
        FYPEntities fd = new FYPEntities();
        [HttpGet]
        public HttpResponseMessage AllFood()
        {
            var food= (from f in fd.FoodItems
                      join r in fd.Resturants on f.rid equals r.rid
                      select new
                      {
                          fid = f.fid,
                          name = f.fName,
                          type = f.ftype,
                          price = f.fprice,
                          Imagepath = f.FImage,
                          f.fdiscount,
                          f.fcooktime,
                          r.rcname
                          

                      }
                        );
            return Request.CreateResponse(HttpStatusCode.OK, food);
        }
        [HttpPost]
        public HttpResponseMessage addFood(FoodItem f)
        {
            try
            {
                var food = fd.FoodItems.Add(f);
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, f.fid);
            }

            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        public HttpResponseMessage getfood(int id)
        {
            try
            {
                   var food = fd.FoodItems.Where(x => x.rid == id).Select(s => new {
                       fid=s.fid,
                       fname = s.fName,
                       ftype = s.ftype,
                       fprice = s.fprice,
                       fImagepath = s.FImage
                   }); 
                return Request.CreateResponse(HttpStatusCode.OK, food);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPost]
        public HttpResponseMessage modifyfood(FoodItem f)
        {
            try
            {
                var food = fd.FoodItems.Find(f.fid);
                if (food == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Food_item not Found");
                }
                fd.Entry(food).CurrentValues.SetValues(f);
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Food is updated");
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage Deletefood(int fid)
        {
            try
            {
                var food = fd.FoodItems.Find(fid);
                if (food == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Food_item not Found");
                }
                fd.Entry(food).State = System.Data.Entity.EntityState.Deleted;
                fd.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Food is Delete");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        public HttpResponseMessage Searchfood(int fid)
        {
            try
            {
             var food = fd.FoodItems.Where(x => x.fid == fid).Select(s => new {
                       fid=s.fid,
                       fname = s.fName,
                       ftype = s.ftype,
                       fprice = s.fprice,
                       fImagepath = s.FImage,
                       fcooktime=s.fcooktime,
                       fdiscount=s.fdiscount,
                       

                   }); 
                return Request.CreateResponse(HttpStatusCode.OK,food);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public HttpResponseMessage ratingupdate(int odid,int r)
        {

            //var food = fd.FoodItems.Find(fid);
            //if (food == null)
            //{
            //    return Request.CreateResponse(HttpStatusCode.NotFound, "Food_item not Found");
            //}
            //fd.Entry(food).CurrentValues.SetValues(f);
            //fd.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, "Food is updated");

        }
    }
}