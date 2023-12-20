using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using JWTAuth_WebAPI.Models;

namespace JWTAuth_WebAPI.Controllers
{
    public class RegisterController : ApiController
    {
        WebAPIAuthEntities db = new WebAPIAuthEntities();

       [AllowAnonymous]
       [Route("Register")]
       [HttpPut]
       public IHttpActionResult Register(RegisterModel registerModel)
       {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var controller = new TokenController();
            //string token = controller.ValidateAndGetToken(registerModel.UserName, registerModel.Password);

            tbl_User user = new tbl_User();
            user.UserName = registerModel.UserName;
            user.Password = registerModel.Password;
            user.RoleID = registerModel.RoleId;
            user.EmailID = registerModel.EmailID;

            db.tbl_User.Add(user);
            db.SaveChanges();

            return Ok();
        }

        
    }
}