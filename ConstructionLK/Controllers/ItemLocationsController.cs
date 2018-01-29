using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConstructionLK.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace ConstructionLK.Controllers
{
    public class ItemLocationsController : Controller
    {
        private ConstructionLKContext db = new ConstructionLKContext();

        // GET: ItemLocations
        public ActionResult Index(int? iid)
        {
            var id = iid;
            string markers = "[";
            string conString = ConfigurationManager.ConnectionStrings["Azure"].ConnectionString;
            SqlCommand cmd = new SqlCommand("SELECT * FROM ItemLocations WHERE ItemId="+id+"");
            using (SqlConnection con = new SqlConnection(conString))
            {
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        markers += "{";
                        //markers += string.Format("'title': '{0}',", sdr["Name"]);
                        markers += string.Format("'lat': '{0}',", sdr["Latitude"]);
                        markers += string.Format("'lng': '{0}',", sdr["Longitude"]);
                        markers += string.Format("'description': '{0}'", sdr["ItemId"]);
                        markers += "},";
                    }
                }
                con.Close();
            }

            markers += "];";
            ViewBag.Markers = markers;
            return View();
            //ViewBag.ItemId = iid;
            //var itemLocations = db.ItemLocations.Include(i => i.Item);
            //return View(itemLocations.ToList());
        }
        //public JsonResult GetLocation()
        //{
        //    //var data = db.ItemLocations.Where(i => i.ItemId == id).ToList();
        //    var data = db.ItemLocations.ToList();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        // GET: ItemLocations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemLocations itemLocations = db.ItemLocations.Find(id);
            if (itemLocations == null)
            {
                return HttpNotFound();
            }
            return View(itemLocations);
        }

        // GET: ItemLocations/Create
        public ActionResult Create(int? iid)
        {
            ViewBag.ItemId = iid;
            return View();
        }

        // POST: ItemLocations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemLocations itemLocations)
        {
            var item = db.Items.SingleOrDefault(i => i.Id == itemLocations.ItemId);
            if (ModelState.IsValid)
            {
                db.ItemLocations.Add(itemLocations);
                db.SaveChanges();
                return RedirectToAction("Index", "ItemLocations", new { iid = item.Id });
                //return RedirectToAction("Index");
            }

            ViewBag.ItemId = new SelectList(db.Items, "Id", "ItemName", itemLocations.ItemId);
            return View(itemLocations);
        }

        // GET: ItemLocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemLocations itemLocations = db.ItemLocations.Find(id);
            if (itemLocations == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "ItemName", itemLocations.ItemId);
            return View(itemLocations);
        }

        // POST: ItemLocations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Latitude,Longitude,ItemId")] ItemLocations itemLocations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemLocations).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemId = new SelectList(db.Items, "Id", "ItemName", itemLocations.ItemId);
            return View(itemLocations);
        }

        // GET: ItemLocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemLocations itemLocations = db.ItemLocations.Find(id);
            if (itemLocations == null)
            {
                return HttpNotFound();
            }
            return View(itemLocations);
        }

        // POST: ItemLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemLocations itemLocations = db.ItemLocations.Find(id);
            db.ItemLocations.Remove(itemLocations);
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
