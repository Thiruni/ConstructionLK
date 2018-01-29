using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConstructionLK.Models;

namespace ConstructionLK.Controllers
{
    public class RatingsServiceProvidersController : Controller
    {
        private ConstructionLKContext db = new ConstructionLKContext();

        // GET: RatingsServiceProviders
        public ActionResult Index()
        {
            var ratingsServiceProviders = db.RatingsServiceProviders.Include(r => r.Customer).Include(r => r.ItemRequest).Include(r => r.ServiceProvider);
            return View(ratingsServiceProviders.ToList());
        }

        // GET: RatingsServiceProviders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingsServiceProvider ratingsServiceProvider = db.RatingsServiceProviders.Find(id);
            if (ratingsServiceProvider == null)
            {
                return HttpNotFound();
            }
            return View(ratingsServiceProvider);
        }

        // GET: RatingsServiceProviders/Create
        public ActionResult Create(int? id, int? receivedUser, string postUser )
        {
            //ViewBag.PostUser = new SelectList(db.Customers, "Id", "Username");
            //ViewBag.RequestId = new SelectList(db.ItemRequests, "Id", "Id");
            //ViewBag.ReceivedUser = new SelectList(db.ServiceProviders, "Id", "Username");
            var PostUser = db.Customers.SingleOrDefault(c => c.ApplicationUserId == postUser);
            ViewBag.Postuser = PostUser.Id;
            ViewBag.RequestId = id;
            ViewBag.ReceivedUser = receivedUser;
            return View();
        }

        // POST: RatingsServiceProviders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PostUser,ReceivedUser,Rate,RequestId")] RatingsServiceProvider ratingsServiceProvider)
        {
            if (ModelState.IsValid)
            {
                db.RatingsServiceProviders.Add(ratingsServiceProvider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostUser = new SelectList(db.Customers, "Id", "Username", ratingsServiceProvider.PostUser);
            ViewBag.RequestId = new SelectList(db.ItemRequests, "Id", "Message", ratingsServiceProvider.RequestId);
            ViewBag.ReceivedUser = new SelectList(db.ServiceProviders, "Id", "Username", ratingsServiceProvider.ReceivedUser);
            return View(ratingsServiceProvider);
        }

        // GET: RatingsServiceProviders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingsServiceProvider ratingsServiceProvider = db.RatingsServiceProviders.Find(id);
            if (ratingsServiceProvider == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostUser = new SelectList(db.Customers, "Id", "Username", ratingsServiceProvider.PostUser);
            ViewBag.RequestId = new SelectList(db.ItemRequests, "Id", "Message", ratingsServiceProvider.RequestId);
            ViewBag.ReceivedUser = new SelectList(db.ServiceProviders, "Id", "Username", ratingsServiceProvider.ReceivedUser);
            return View(ratingsServiceProvider);
        }

        // POST: RatingsServiceProviders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PostUser,ReceivedUser,Rate,RequestId")] RatingsServiceProvider ratingsServiceProvider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ratingsServiceProvider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PostUser = new SelectList(db.Customers, "Id", "Username", ratingsServiceProvider.PostUser);
            ViewBag.RequestId = new SelectList(db.ItemRequests, "Id", "Message", ratingsServiceProvider.RequestId);
            ViewBag.ReceivedUser = new SelectList(db.ServiceProviders, "Id", "Username", ratingsServiceProvider.ReceivedUser);
            return View(ratingsServiceProvider);
        }

        // GET: RatingsServiceProviders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingsServiceProvider ratingsServiceProvider = db.RatingsServiceProviders.Find(id);
            if (ratingsServiceProvider == null)
            {
                return HttpNotFound();
            }
            return View(ratingsServiceProvider);
        }

        // POST: RatingsServiceProviders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RatingsServiceProvider ratingsServiceProvider = db.RatingsServiceProviders.Find(id);
            db.RatingsServiceProviders.Remove(ratingsServiceProvider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //showing ratings in profile
        public ActionResult showRatingsForServiceProvider(int? id)
        {
            var avgRatings = db.RatingsServiceProviders.Where(c => c.Id == id).Average(c => c.Rate);
            ViewBag.AvgRatings = avgRatings;
            return View("showRatingsForServiceProvider");
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
