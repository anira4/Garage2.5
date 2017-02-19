using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage2._5.DAL;
using Garage2._5.Models;

namespace Garage2._5.Controllers
{
    public class MembersController : Controller
    {
        private GarageContext db = new GarageContext();

        // GET: Members
        public ActionResult Index(string orderBy, string currentFilter, string searchString, int page = 1)
        {
            IQueryable<Member> members = db.Members;

            if (searchString != null)
            {
                page = 1; // If the search string is changed during paging, the page has to be reset to 1
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentMemberFilter = searchString;
            if (!String.IsNullOrEmpty(searchString))
            {
                members = members.Where(m => m.Name.Contains(searchString) ||
                                            m.Username.Contains(searchString) ||
                                            m.Phone.Contains(searchString));
            }
            switch (orderBy)
            {
                case "username":
                    members = members.OrderBy(m => m.Username);
                    break;
                case "username_dec":
                    members = members.OrderByDescending(m => m.Username);
                    break;
                case "name_dec":
                    members = members.OrderByDescending(m => m.Name);
                    break;
                case "phone":
                    members = members.OrderBy(m => m.Phone);
                    break;
                case "phone_dec":
                    members = members.OrderByDescending(m => m.Phone);
                    break;
                default:
                    members = members.OrderBy(m => m.Name);
                    break;
            }
            ViewBag.CurrentSort = orderBy;
            return View(members.ToList());
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,Name,Phone")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,Name,Phone")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
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
