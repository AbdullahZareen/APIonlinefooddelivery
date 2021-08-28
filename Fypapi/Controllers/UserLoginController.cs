using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Fypapi.Models;
namespace Fypapi.Controllers
{
    public class UserLoginController : ApiController
    {
        FYPEntities f = new FYPEntities();
        [HttpGet]
        public HttpResponseMessage allusers()
        {
            var user = f.UserLogins.Select(s => new {
                email = s.email,
               role = s.roles
            });
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }
        [HttpGet]
        public HttpResponseMessage Login(string useremail,string  password)
        {
            var user = f.UserLogins.FirstOrDefault(d => d.email == useremail && d.passward == password);
            if (user == null)
                return Request.CreateResponse(HttpStatusCode.NotFound,false);
            return Request.CreateResponse(HttpStatusCode.OK, user);

        }
    }
}
