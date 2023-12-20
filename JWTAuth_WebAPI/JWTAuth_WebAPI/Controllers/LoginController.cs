using JWTAuth_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Jwt.Filters;

namespace JWTAuth_WebAPI.Controllers
{
    public class LoginController : ApiController
    {
        WebAPIAuthEntities db = new WebAPIAuthEntities();

        [JwtAuthentication]
        [Route("Login")]
        [HttpPost]
        public IHttpActionResult Login(LoginModel loginModel)
        {
            var controller = new TokenController();
            bool IsValidated = controller.CheckUser(loginModel.UserName, loginModel.Password);

            if (!IsValidated)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}