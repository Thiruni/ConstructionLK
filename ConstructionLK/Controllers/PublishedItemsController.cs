using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConstructionLK.Models;
using Microsoft.AspNet.Identity;

namespace ConstructionLK.Controllers
{
    public class PublishedItemsController : Controller
    {
        private ConstructionLKContext db = new ConstructionLKContext();

        // GET: PublishedItems
        public ActionResult Index()
        {
            var publishedItems = db.PublishedItems.Include(p => p.Item).Include(p => p.ServiceProvider);
            return View(publishedItems.ToList());
        }

        // GET: PublishedItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublishedItem publishedItem = db.PublishedItems.Find(id);
            if (publishedItem == null)
            {
                return HttpNotFound();
            }
            return View(publishedItem);
        }

        // GET: PublishedItems/Create
        public ActionResult Create(int? id,string user)
        {
            ViewBag.ItemId = id;
            ViewBag.UserId = user;
            //ViewBag.ItemId = new SelectList(db.Items, "Id", "ItemName");
            //ViewBag.UserId = new SelectList(db.ServiceProviders, "Id", "Username");
           
            return View("MyProducts", "Items", new { user = User.Identity.GetUserId() });
        }

        // POST: PublishedItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PublishedItem publishedItem)
        {
            if (ModelState.IsValid)
            {
                db.PublishedItems.Add(publishedItem);
                try
                {
                    db.SaveChanges();
                }
                catch (SqlException e)
                {

                    throw e;
                }

                return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(db.Items, "Id", "ItemName", publishedItem.ItemId);
            ViewBag.UserId = new SelectList(db.ServiceProviders, "Id", "Username", publishedItem.UserId);
            return View(publishedItem);
        }

        // GET: PublishedItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublishedItem publishedItem = db.PublishedItems.Find(id);
            if (publishedItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "ItemName", publishedItem.ItemId);
            ViewBag.UserId = new SelectList(db.ServiceProviders, "Id", "Username", publishedItem.UserId);
            return View(publishedItem);
        }

        // POST: PublishedItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,ItemId,Price,Discount")] PublishedItem publishedItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publishedItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "ItemName", publishedItem.ItemId);
            ViewBag.UserId = new SelectList(db.ServiceProviders, "Id", "Username", publishedItem.UserId);
            return View(publishedItem);
        }

        // GET: PublishedItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PublishedItem publishedItem = db.PublishedItems.Find(id);
            if (publishedItem == null)
            {
                return HttpNotFound();
            }
            return View(publishedItem);
        }

        // POST: PublishedItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PublishedItem publishedItem = db.PublishedItems.Find(id);
            db.PublishedItems.Remove(publishedItem);
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
