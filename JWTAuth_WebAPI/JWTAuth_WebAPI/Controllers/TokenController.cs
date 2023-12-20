using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;

namespace JWTAuth_WebAPI.Controllers
{
    public class TokenController : ApiController
    {
        private WebAPIAuthEntities db = new WebAPIAuthEntities();

        // GET api/<controller>/ValidateAndGetToken/username/password
        [AllowAnonymous]
        [HttpGet]
        public ResponseMessageResult ValidateAndGetToken(string username, string password)
        {
            //HttpResponseMessage httpResponseMessage = JwtManager.GenerateToken(username);
            try
            {
                var content = "\r";
                content = JwtManager.GenerateToken(username);
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.Accepted)
                {
                    RequestMessage = Request,
                    Content = new StringContent(content)
                };
                return ResponseMessage(httpResponseMessage);
            }
            catch(Exception)
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            //if (CheckUser(username, password))
            //{
            //    var Token = JwtManager.GenerateToken(username);
            //    return Token;
            //}
        }

        public bool CheckUser(string username, string password)
        {
            bool result = false;
            var user = db.tbl_User.Where(u => u.UserName == username && u.Password == password).FirstOrDefault().UserName;

            if (user != null)
            {
                result = true;
            }

            return result;
        }
    }
}