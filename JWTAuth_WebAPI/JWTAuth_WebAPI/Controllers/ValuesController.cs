using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Jwt.Filters;

namespace JWTAuth_WebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        [JwtAuthentication]
        public string Get()
        {
            return "value";
        }
    }
        
}
