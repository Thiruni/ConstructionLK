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
    public class ItemRequestsController : Controller
    {
        private ConstructionLKContext db = new ConstructionLKContext();

        // GET: ItemRequests
        public ActionResult Index()
        {
            var itemRequests = db.ItemRequests.Include(i => i.Customer).Include(i => i.Item).Include(i => i.ServiceProvider).Include(i => i.ItemRequestStatus);
            return View(itemRequests.ToList());
        }

        // index for customer
        public ActionResult UserIndex(int? id)
        {
            var itemRequests = db.ItemRequests.Include(i => i.Customer).Include(i => i.Item).Include(i => i.ServiceProvider).Include(i => i.ItemRequestStatus);
            var citemrequest = db.ItemRequests.Where(i => i.CustomerId == id);
            return View(citemrequest.ToList());
        }


        // GET: ItemRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemRequest itemRequest = db.ItemRequests.Find(id);
            if (itemRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.latitude = itemRequest.Latitude;
            ViewBag.longitude = itemRequest.Longitude;
            try
            {
                var result = db.RequestProgreses.GroupBy(x => x.ReqId)
                          .Select(x => x.OrderByDescending(y => y.Id).FirstOrDefault());
                var p = result.FirstOrDefault(i => i.ReqId == id).value;
                if (p != 0)
                    ViewBag.P = p;

            }
            catch (Exception)
            {
                ViewBag.P = 0;
            }
            return View(itemRequest);
        }
        // GET: MyItemRequests/Index
        public ActionResult MyRequests(string user)
        {
            var appuser = db.ServiceProviders.SingleOrDefault(s => s.ApplicationUserId == user).Id;
            // var customer=db.Customers.SingleOrDefault(c => c.ApplicationUserId == user).Username;
            var itemsRequests = db.ItemRequests.Include(i => i.Customer).Include(i => i.Item).Include(i => i.Location).Where(i => i.ServiceProviderId == appuser).Where(i => i.StatusId == 1);

            if (User.IsInRole(RoleName.CanManageAll))
                return View(itemsRequests.ToList());
            if (itemsRequests == null)
            {
                return View("NothingToShow");
            }
            return View("MyRequests", itemsRequests.ToList());
        }

        // GET: RespondMyRequests/Respond
        //public ActionResult Respond(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ItemRequest itemRequest = db.ItemRequests.Find(id);
        //    if (itemRequest == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    ViewBag.StatusId = new SelectList(db.ItemRequestStatuses, "Id", "Name", itemRequest.StatusId);

        //    //ViewBag.CustomerId = itemRequest.CustomerId;
        //    //ViewBag.CustomerId = itemRequest.ItemId;
        //    //ViewBag.LocationId = itemRequest.LocationId;
        //    //ViewBag.ServiceProviderId = itemRequest.ServiceProviderId;
        //    //ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Username", itemRequest.CustomerId);
        //    //ViewBag.ItemId = new SelectList(db.Items, "Id", "ItemName", itemRequest.ItemId);
        //    //ViewBag.LocationId = new SelectList(db.Locations, "Id", "Type", itemRequest.LocationId);
        //    //ViewBag.ServiceProviderId = new SelectList(db.ServiceProviders, "Id", "Username", itemRequest.ServiceProviderId);
        //    return View(itemRequest);
        //}

        // POST: RespondMyRequests/Respond
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Respond([Bind(Include = "AcceptedDate,StatusId")] ItemRequest itemRequest)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(itemRequest).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.StatusId = new SelectList(db.ItemRequestStatuses, "Id", "Name", itemRequest.StatusId);

        //    //ViewBag.CustomerId = itemRequest.CustomerId;
        //    //ViewBag.CustomerId = itemRequest.ItemId;
        //    //ViewBag.LocationId = itemRequest.LocationId;
        //    //ViewBag.ServiceProviderId = itemRequest.ServiceProviderId;
        //    //ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Username", itemRequest.CustomerId);
        //    //ViewBag.ItemId = new SelectList(db.Items, "Id", "ItemName", itemRequest.ItemId);
        //    //ViewBag.LocationId = new SelectList(db.Locations, "Id", "Type", itemRequest.LocationId);
        //    //ViewBag.ServiceProviderId = new SelectList(db.ServiceProviders, "Id", "Username", itemRequest.ServiceProviderId);
        //    return View(itemRequest);
        //}

        // GET: ItemRequests/Create
        public ActionResult Create(int? id,string user,int? provider)
        {
            ViewBag.StatusId = new SelectList(db.ItemRequestStatuses, "Id", "Name");

            //ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Username");
            var Customer = db.Customers.SingleOrDefault(c => c.ApplicationUserId == user);
            ViewBag.cid = Customer.Id;
            ViewBag.ItemId = id;
            ViewBag.pid = provider;
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Type");
            ViewBag.ServiceProviderId = new SelectList(db.ServiceProviders, "Id", "Username");
            return View();
        }

        // POST: ItemRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Message,CreatedDate,AcceptedDate,ExpDate,CompletedDate,StatusId,LocationId,CustomerId,ServiceProviderId,ItemId,Latitude,Longitude")] ItemRequest itemRequest)
        {
            if (ModelState.IsValid)
            {
                db.ItemRequests.Add(itemRequest);
                db.SaveChanges();
                //return RedirectToAction("Index");
                if (itemRequest.Message != null)
                {
                    //QxF+O9qg6fk-a9dYLek4plJk8JARdZ2081qRlIWKtz
                    String message = HttpUtility.UrlEncode(itemRequest.Message);
                    var sp = db.ServiceProviders.SingleOrDefault(i => i.Id == itemRequest.ServiceProviderId);
                    String number = sp.Telephone;
                    using (var wb = new WebClient())
                    {
                        byte[] response = wb.UploadValues("https://api.txtlocal.com/send/", new NameValueCollection()
                        {
                            {"apikey" , "EXfapgBZ+4M-qSpQCEdOLhG6ZEc0y7esVVeTi6rL41"},
                            {"numbers" , "+94"+number},
                            {"message" , message},
                            {"sender" , "CLK Alert"}
                        });
                        string result = System.Text.Encoding.UTF8.GetString(response);
                        //return Content(result);
                    }
                }

                return RedirectToAction("Create", "ItemPayments", new { id = itemRequest.Id });
                // return View("Views/ItemPayments/Create.cshtml");

                // return RedirectToAction("Confirming");
            }
            ViewBag.StatusId = new SelectList(db.ItemRequestStatuses, "Id", "Name", itemRequest.StatusId);
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Username", itemRequest.CustomerId);
            ViewBag.ItemId = new SelectList(db.Items, "Id", "ItemName", itemRequest.ItemId);
            ViewBag.ServiceProviderId = new SelectList(db.ServiceProviders, "Id", "Username", itemRequest.ServiceProviderId);
            return View(itemRequest);
        }

        // GET: ItemRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemRequest itemRequest = db.ItemRequests.Find(id);
            if (itemRequest == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusId = new SelectList(db.ItemRequestStatuses, "Id", "Name", itemRequest.StatusId);

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Username", itemRequest.CustomerId);
            ViewBag.ItemId = new SelectList(db.Items, "Id", "ItemName", itemRequest.ItemId);
            //ViewBag.LocationId = new SelectList(db.Locations, "Id", "Type", itemRequest.LocationId);
            ViewBag.ServiceProviderId = new SelectList(db.ServiceProviders, "Id", "Username", itemRequest.ServiceProviderId);
            return View(itemRequest);
        }

        // POST: ItemRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Message,CreatedDate,AcceptedDate,ExpDate,CompletedDate,StatusId,LocationId,CustomerId,ServiceProviderId,ItemId,Latitude,Longitude")] ItemRequest itemRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyRequests", new { user=User.Identity.GetUserId()});
            }
            ViewBag.StatusId = new SelectList(db.ItemRequestStatuses, "Id", "Name", itemRequest.StatusId);

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Username", itemRequest.CustomerId);
            ViewBag.ItemId = new SelectList(db.Items, "Id", "ItemName", itemRequest.ItemId);
            ViewBag.ServiceProviderId = new SelectList(db.ServiceProviders, "Id", "Username", itemRequest.ServiceProviderId);
            return View(itemRequest);
        }

        // GET: ItemRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemRequest itemRequest = db.ItemRequests.Find(id);
            if (itemRequest == null)
            {
                return HttpNotFound();
            }

            return View(itemRequest);
        }

        // POST: ItemRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemRequest itemRequest = db.ItemRequests.Find(id);
            db.ItemRequests.Remove(itemRequest);
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
        public JsonResult GetLocation(int? id)
        {
            var data = db.ItemRequests.Where(i=>i.Id==id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
