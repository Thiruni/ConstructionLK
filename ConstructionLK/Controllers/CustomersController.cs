using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ConstructionLK.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.Owin.Security;
namespace ConstructionLK.Controllers
{
    public class CustomersController : Controller
    {
        private ConstructionLKContext db = new ConstructionLKContext();

        public ActionResult MyProfile(string id)
        {
            if (id == null)
            {
                //return RedirectToAction("MyProfile", "Customers", new { id = customer.Id });
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Customer customer = db.Customers.Find(id);
            Customer customer = db.Customers.SingleOrDefault(c => c.ApplicationUserId == id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            if ((!User.IsInRole(RoleName.Customer) || !User.IsInRole(RoleName.CanManageAll) || !User.IsInRole(RoleName.ServiceProvider)))
            {
                //no role
                RedirectToAction("Index", "UserSelector");
            }
            //var principal = (RolePrincipal)User;
            //if (!principal.GetRoles().Any())
            //{
            //    //no role
            //    RedirectToAction("Index", "UserSelector");
            //}

            return View("MyProfile", customer);
            //return View();
        }


        // customer profile for service provide
        public ActionResult profileOut(string id, int? requestid)
        {
            ViewBag.requestid = requestid;
            Customer customer = db.Customers.SingleOrDefault(i => i.ApplicationUserId == id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            
            return View(customer);
        }

        // GET: Customers
        public ActionResult Index()
        {
            var customers = db.Customers.Include(c => c.Status);

            if (User.IsInRole(RoleName.CanManageAll))
                return View(customers.ToList());

            return View("ReadOnlyIndex", customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Customer customer = db.Customers.Find(id);
            Customer customer = db.Customers.Include(c=>c.Status).SingleOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole(RoleName.CanManageAll))
                return View(customer);

            return View("MyProfile", customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");

            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,FirstName,LastName,DateOfBirth,Telephone,Gender,Address,StatusId,CreatedDate,ModifiedDate,IsSubscribedToNewsletter,ApplicationUserId")]Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                //return RedirectToAction("Index");
                //return RedirectToAction("MyProfile","Customers", new { id = customer.Id });
                return RedirectToAction("MyProfile", "Customers", new { id = User.Identity.GetUserId() });
            }
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", customer.StatusId);

            //ViewBag.ApplicationUserId = new SelectList(db.AspNetUsers, "Id", "Username");
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", customer.StatusId);

            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,FirstName,LastName,DateOfBirth,Telephone,Gender,Address,StatusId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,ApplicationUserId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", customer.StatusId);

            return View(customer);
        }

       

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
        //public ViewResult IndexTest()
        //{
        //    var customers = GetCustomers();

        //    return View(customers);
        //}

        //public ActionResult DetailsTest(int id)
        //{
        //    var customer = GetCustomers().SingleOrDefault(c => c.Id == id);

        //    if (customer == null)
        //        return HttpNotFound();

        //    return View(customer);
        //}

        //private IEnumerable<Customer> GetCustomers()
        //{
        //    return new List<Customer>
        //    {
        //        new Customer { Id = 1, FirstName = "John Smith" },
        //        new Customer { Id = 2, FirstName = "Mary Williams" }
        //    };
        //}
        [AllowAnonymous]
        public ActionResult Blacklisted()
        {
            //return RedirectToAction("LogOutBlacklist", "Account", new { id = 1 });
            return View();
        }
    }
}
