using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConstructionLK.Models;
using System.Collections.Specialized;
using System.Configuration;
using Microsoft.AspNet.Identity;

namespace ConstructionLK.Controllers
{
    public class ItemsController : Controller
    {
        private ConstructionLKContext db = new ConstructionLKContext();
        [AllowAnonymous]
        // GET: Items
        public ActionResult Index(int? id)
        {
            //var items = db.Items.Include(i => i.ItemStatus).Include(i => i.ItemSubCategory).Include(i => i.ItemType).Include(i => i.ServiceProvider);
            if (!(id == null))
            {
                var items = db.Items.Include(i => i.ItemSubCategory).Include(i => i.ItemType).Include(i => i.ServiceProvider).Include(i => i.ItemStatus).Include(i => i.PublishedItems).Where(i => i.TypeId == id);
                if (User.IsInRole(RoleName.CanManageAll))
                    return View(items.ToList());

                return View("ReadOnlyIndex", items.ToList());
            }
            else
            {
                var items = db.Items.Include(i => i.ItemSubCategory).Include(i => i.ItemType).Include(i => i.ServiceProvider).Include(i => i.ItemStatus).Include(i => i.PublishedItems);
                if (User.IsInRole(RoleName.CanManageAll))
                    return View(items.ToList());

                return View("ReadOnlyIndex", items.ToList());
            }

            //var items = db.Items.Include(i => i.ItemSubCategory).Include(i => i.ItemType).Include(i => i.ServiceProvider);


        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(RoleName.CanManageAll)|| User.IsInRole(RoleName.ServiceProvider))
                return View(item);

            return View("ReadOnlyDetails", item);

        }

        // GET: Items/Create
        public ActionResult Create(string user)
        {
            ViewBag.puserid = db.ServiceProviders.SingleOrDefault(s => s.ApplicationUserId == user).Id;

            ViewBag.StatusId = new SelectList(db.ItemStatus, "Id", "Name");
            ViewBag.SubCategoryId = new SelectList(db.ItemSubCategories, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Type");
            ViewBag.UserId = new SelectList(db.ServiceProviders, "Id", "Username");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                
                db.SaveChanges();

                return RedirectToAction("Create", "PublishedItems",new { id = item.Id,user = item.UserId});
            }
            ViewBag.StatusId = new SelectList(db.ItemStatus, "Id", "Name",item.StatusId);
            ViewBag.SubCategoryId = new SelectList(db.ItemSubCategories, "Id", "Name", item.SubCategoryId);
            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Type", item.TypeId);
            ViewBag.UserId = new SelectList(db.ServiceProviders, "Id", "Username", item.UserId);

            return View(item);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusId = new SelectList(db.ItemStatus, "Id", "Name", item.StatusId);
            ViewBag.SubCategoryId = new SelectList(db.ItemSubCategories, "Id", "Name", item.SubCategoryId);
            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Type", item.TypeId);
            ViewBag.UserId = id;
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemName,ItemCode,Description,StatusId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,SubCategoryId,TypeId,UserId,Price,Tax")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyProducts", new { user = User.Identity.GetUserId() });
            }
            ViewBag.StatusId = new SelectList(db.ItemStatus, "Id", "Name", item.StatusId);
            ViewBag.SubCategoryId = new SelectList(db.ItemSubCategories, "Id", "Name", item.SubCategoryId);
            ViewBag.TypeId = new SelectList(db.ItemTypes, "Id", "Type", item.TypeId);
            ViewBag.UserId = new SelectList(db.ServiceProviders, "Id", "Username", item.UserId);
            return View(item);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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
        
        public ActionResult MyProducts(string user)
        {
            //var items = db.Items.Include(i => i.ItemStatus).Include(i => i.ItemSubCategory).Include(i => i.ItemType).Include(i => i.ServiceProvider);
            var appuser = db.ServiceProviders.SingleOrDefault(u => u.ApplicationUserId == user);
            var items = db.Items.Include(i => i.ItemSubCategory).Include(i => i.ItemType).Include(i => i.ServiceProvider).Include(i => i.ItemStatus).Include(i => i.PublishedItems).Where(i => i.UserId == appuser.Id);
            //var items = db.Items.Include(i => i.ItemSubCategory).Include(i => i.ItemType).Include(i => i.ServiceProvider);
            //ViewBag.apuser= db.ServiceProviders.SingleOrDefault(u => u.ApplicationUserId == user).Id;
            ViewBag.user = user;
            if (User.IsInRole(RoleName.CanManageAll))
                return View(items.ToList());

            return View("Index", items.ToList());
        }
        public ActionResult Buy(int id)
        {
            var price = db.PublishedItems.SingleOrDefault(p => p.ItemId == id);
            var itemprice = price.Price;
            Item item = db.Items.Find(id);
            ViewBag.data = itemprice;
            ViewBag.idata = item;
            return View("BuyProduct");// sent to add the quntity 
        }
    }
}
