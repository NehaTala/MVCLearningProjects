using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UserManagement_MVC;

namespace UserManagement_MVC.Controllers
{
    public class UserController : Controller
    {
        private WebAPIAuthEntities db = new WebAPIAuthEntities();

        // GET: tbl_User
        public ActionResult Index()
        {
            var tbl_User = db.tbl_User.Include(t => t.tbl_Roles);
            return View(tbl_User.ToList());
        }

        // GET: tbl_User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_User tbl_User = db.tbl_User.Find(id);
            if (tbl_User == null)
            {
                return HttpNotFound();
            }
            return View(tbl_User);
        }

        // GET: tbl_User/Create
        public ActionResult Register()
        {
            ViewBag.RoleID = new SelectList(db.tbl_Roles, "PK_RoleID", "RoleName");
            return View();
        }

        // POST: tbl_User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "PK_UserID,UserName,Password,ConfirmPassword,RoleID,EmailID")] tbl_User tbl_User)
        {
            if (ModelState.IsValid)
            {
                var userObject = new tbl_User
                {
                    UserName = tbl_User.UserName,
                    Password = tbl_User.Password,
                    ConfirmPassword = tbl_User.ConfirmPassword,
                    RoleID = tbl_User.RoleID,
                    EmailID = tbl_User.EmailID
                };

                var objAsJson = JsonConvert.SerializeObject(userObject);
                var content = new StringContent(objAsJson, Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44312/");
                    HttpResponseMessage response = client.PutAsync(client.BaseAddress + "Register", content).Result;

                    if (response != null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            ViewBag.RoleID = new SelectList(db.tbl_Roles, "PK_RoleID", "RoleName", tbl_User.RoleID);
            return View(tbl_User);
        }

        // GET: tbl_User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_User tbl_User = db.tbl_User.Find(id);
            if (tbl_User == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.tbl_Roles, "PK_RoleID", "RoleName", tbl_User.RoleID);
            return View(tbl_User);
        }

        // POST: tbl_User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PK_UserID,UserName,Password,RoleID,EmailID")] tbl_User tbl_User)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_User).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.tbl_Roles, "PK_RoleID", "RoleName", tbl_User.RoleID);
            return View(tbl_User);
        }

        // GET: tbl_User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_User tbl_User = db.tbl_User.Find(id);
            if (tbl_User == null)
            {
                return HttpNotFound();
            }
            return View(tbl_User);
        }

        // POST: tbl_User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_User tbl_User = db.tbl_User.Find(id);
            db.tbl_User.Remove(tbl_User);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
