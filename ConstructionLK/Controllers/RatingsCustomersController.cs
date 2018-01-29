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
    public class RatingsCustomersController : Controller
    {
        private ConstructionLKContext db = new ConstructionLKContext();

        // GET: RatingsCustomers
        public ActionResult Index()
        {
            var ratingsCustomers = db.RatingsCustomers.Include(r => r.Customer).Include(r => r.ItemRequest).Include(r => r.ServiceProvider);
            return View(ratingsCustomers.ToList());
        }

        // GET: RatingsCustomers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingsCustomer ratingsCustomer = db.RatingsCustomers.Find(id);
            if (ratingsCustomer == null)
            {
                return HttpNotFound();
            }
            return View(ratingsCustomer);
        }

        // GET: RatingsCustomers/Create
        public ActionResult Create(int? id, int? receivedUser, string postUser)
        {
            //ViewBag.ReceivedUser = new SelectList(db.Customers, "Id", "Username");
            //ViewBag.RequestId = new SelectList(db.ItemRequests, "Id", "Message");
            //ViewBag.PostUser = new SelectList(db.ServiceProviders, "Id", "Username");
            //return View();
           // var PostUser = db.Customers.SingleOrDefault(c => c.ApplicationUserId == postUser);
            var PostUser = db.ServiceProviders.SingleOrDefault(c => c.ApplicationUserId == postUser);
            ViewBag.Postuser = PostUser.Id;
            ViewBag.RequestId = id;
            ViewBag.ReceivedUser = receivedUser;
            return View();
        }

        // POST: RatingsCustomers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PostUser,ReceivedUser,Rate,RequestId")] RatingsCustomer ratingsCustomer)
        {
            if (ModelState.IsValid)
            {
                db.RatingsCustomers.Add(ratingsCustomer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReceivedUser = new SelectList(db.Customers, "Id", "Username", ratingsCustomer.ReceivedUser);
            ViewBag.RequestId = new SelectList(db.ItemRequests, "Id", "Message", ratingsCustomer.RequestId);
            ViewBag.PostUser = new SelectList(db.ServiceProviders, "Id", "Username", ratingsCustomer.PostUser);
            return View(ratingsCustomer);
        }

        // GET: RatingsCustomers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingsCustomer ratingsCustomer = db.RatingsCustomers.Find(id);
            if (ratingsCustomer == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReceivedUser = new SelectList(db.Customers, "Id", "Username", ratingsCustomer.ReceivedUser);
            ViewBag.RequestId = new SelectList(db.ItemRequests, "Id", "Message", ratingsCustomer.RequestId);
            ViewBag.PostUser = new SelectList(db.ServiceProviders, "Id", "Username", ratingsCustomer.PostUser);
            return View(ratingsCustomer);
        }

        // POST: RatingsCustomers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PostUser,ReceivedUser,Rate,RequestId")] RatingsCustomer ratingsCustomer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ratingsCustomer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReceivedUser = new SelectList(db.Customers, "Id", "Username", ratingsCustomer.ReceivedUser);
            ViewBag.RequestId = new SelectList(db.ItemRequests, "Id", "Message", ratingsCustomer.RequestId);
            ViewBag.PostUser = new SelectList(db.ServiceProviders, "Id", "Username", ratingsCustomer.PostUser);
            return View(ratingsCustomer);
        }

        // GET: RatingsCustomers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RatingsCustomer ratingsCustomer = db.RatingsCustomers.Find(id);
            if (ratingsCustomer == null)
            {
                return HttpNotFound();
            }
            return View(ratingsCustomer);
        }

        // POST: RatingsCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RatingsCustomer ratingsCustomer = db.RatingsCustomers.Find(id);
            db.RatingsCustomers.Remove(ratingsCustomer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // reading ratings
        public ActionResult showRatings(int? id)
        {
            var avgRatings = db.RatingsServiceProviders.Where(c => c.Id == id).Average(c => c.Rate);
            ViewBag.AvgRatings = avgRatings;
            return View("showRatingsForCustomer");
        }

        public ActionResult showRatingsForCustomer()
        {

            return View("showRatingsForCustomer");
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
