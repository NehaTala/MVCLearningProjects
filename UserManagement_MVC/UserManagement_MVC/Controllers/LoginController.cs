using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UserManagement_MVC.Models;

namespace UserManagement_MVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "UserName,Password")] LoginModel login)
        {
            try
            {
                string Token = GetToken(login.UserName, login.Password);
                if(Token == null)
                {
                    ModelState.AddModelError("", "Invalid UserName or Password.");
                    return RedirectToAction("Login");
                }

                var userObject = new LoginModel
                {
                    UserName = login.UserName,
                    Password = login.Password
                };

                var objAsJson = JsonConvert.SerializeObject(userObject);
                var content = new StringContent(objAsJson, Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44312/");
                    client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", Token));
                    HttpResponseMessage response = client.PostAsync(client.BaseAddress + "Login", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Session["LoginedIn"] = login.UserName;
                        return Redirect("/User/Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid UserName or Password.");
                    }

                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Error Occured from Login - " + ex.Message + "" + ex.StackTrace);
            }
            return RedirectToAction("Login");
        }

        private string GetToken(string username, string password)
        {
            string Token = null;
            try
            {
                var queryParameters = new Dictionary<string, string>
                {
                    { "username", username },
                    { "password", password }
                };
                var dictFormUrlEncoded = new FormUrlEncodedContent(queryParameters);
                var queryString = dictFormUrlEncoded.ReadAsStringAsync();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44312/");
                    HttpResponseMessage response = client.GetAsync(client.BaseAddress + "api/Token?" + queryString.Result).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        Token = response.Content.ReadAsStringAsync().Result.ToString();
                    }
                    else
                    {
                        ModelState.AddModelError("", response.StatusCode.ToString());
                    }
                }
                
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Error Occured from GetToken - " + ex.Message + "" + ex.StackTrace);
            }
            return Token;
        }
    }
}