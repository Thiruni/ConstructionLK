using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConstructionLK.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Data.Entity.Validation;

namespace ConstructionLK.Controllers
{
    public class ServiceProvidersIndividualController : Controller
    {
        private ConstructionLKContext db = new ConstructionLKContext();
        public ActionResult MyProfile(string user)
        {
            //using (ConstructionLKContext db = new ConstructionLKContext())
            //{
            if (user == null)
            {
                //return RedirectToAction("MyProfile", "Customers", new { id = customer.Id });
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Customer customer = db.Customers.Find(id);
            ServiceProvider serviceProvider = db.ServiceProviders.SingleOrDefault(sp => sp.ApplicationUserId == user);
            if (serviceProvider == null)
            {
                return HttpNotFound();
            }
            //if ((!User.IsInRole(RoleName.Customer) || !User.IsInRole(RoleName.CanManageAll) || !User.IsInRole(RoleName.ServiceProvider)))
            //{
            //    //no role
            //    RedirectToAction("Index", "UserSelector");
            //}
            //var principal = (RolePrincipal)User;
            //if (!principal.GetRoles().Any())
            //{
            //    //no role
            //    RedirectToAction("Index", "UserSelector");
            //}

            return View("MyProfile", serviceProvider);
            //return View();
            //}
        }

        // GET: ServiceProvidersIndividual
        public ActionResult Index()
        {
            //using (ConstructionLKContext db = new ConstructionLKContext())
            //{
            var serviceProviders = db.ServiceProviders.Include(s => s.MembershipType).Include(s => s.ServiceProviderType);
            return View(serviceProviders.ToList());
            //}
        }

        // GET: ServiceProvidersIndividual/Details/5
        public ActionResult Details(int? id)
        {
            //using (ConstructionLKContext db = new ConstructionLKContext())
            //{

            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceProvider serviceProvider = db.ServiceProviders.Find(id);
            if (serviceProvider == null)
            {
                return HttpNotFound();
            }
            return View(serviceProvider);
            //}
        }

        //GET: ServiceProvidersIndividual/Create
        public ActionResult Create()
        {
            //using (ConstructionLKContext db = new ConstructionLKContext())
            //{
            ViewBag.MembershipTypeId = new SelectList(db.MembershipTypes, "Id", "Name");
            ViewBag.TypeId = new SelectList(db.ServiceProviderTypes, "Id", "Type");
            return View();
            //}
        }

        // POST: ServiceProvidersIndividual/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Username,MembershipTypeId,BasedCity,MailingAddress,Bio,FirstName,LastName,DateOfBirth,Telephone,CompanyName,CompanyRegNo,StartedDate,Avatar,StatusId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,TypeId,ApplicationUserId")] ServiceProvider serviceProvider)
        public ActionResult Create(ServiceProvider serviceProvider, HttpPostedFileBase file)
        {
            //string path = Path.Combine(Server.MapPath("~/Resources/ProfilePicturesSP/"), Path.GetFileName(file.FileName));
            //using (ConstructionLKContext db = new ConstructionLKContext())
            //{
            if (!(file == null))
            {
                if (ModelState.IsValid)
                {

                    file.SaveAs(Path.Combine(Server.MapPath("~/Resources/ProfilePicturesSP/"), Path.GetFileName(file.FileName)));
                    db.ServiceProviders.Add(new ServiceProvider
                    {
                        Id = serviceProvider.Id,
                        Username = serviceProvider.Username,
                        MembershipTypeId = serviceProvider.MembershipTypeId,
                        BasedCity = serviceProvider.BasedCity,
                        MailingAddress = serviceProvider.MailingAddress,
                        Bio = serviceProvider.Bio,
                        FirstName = serviceProvider.FirstName,
                        LastName = serviceProvider.LastName,
                        DateOfBirth = serviceProvider.DateOfBirth,
                        Telephone = serviceProvider.Telephone,
                        StartedDate = serviceProvider.StartedDate,
                        Avatar = "~/Resources/ProfilePicturesSP/" + Path.GetFileName(file.FileName),
                        StatusId = serviceProvider.StatusId,
                        CreatedBy = serviceProvider.CreatedBy,
                        CreatedDate = serviceProvider.CreatedDate,
                        ModifiedBy = serviceProvider.ModifiedBy,
                        ModifiedDate = serviceProvider.ModifiedDate,
                        TypeId = serviceProvider.TypeId,
                        //CompanyName = serviceProvider.CompanyName,
                        //CompanyRegNo = serviceProvider.CompanyRegNo,
                        ApplicationUserId = serviceProvider.ApplicationUserId
                    }/*serviceProvider*/);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine(">>>>>>Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw e;

                    }
                    
                    //return RedirectToAction("Index");
                    return RedirectToAction("MyProfile", "ServiceProvidersIndividual", new { user = User.Identity.GetUserId() });
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    
                    db.ServiceProviders.Add(serviceProvider);
                    try
                    {
                        
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    return RedirectToAction("MyProfile", "ServiceProvidersIndividual", new { user = User.Identity.GetUserId() });
                    //return RedirectToAction("Index");
                }
            }



            ViewBag.MembershipTypeId = new SelectList(db.MembershipTypes, "Id", "Name", serviceProvider.MembershipTypeId);
            //ViewBag.TypeId = new SelectList(db.ServiceProviderTypes, "Id", "Type", serviceProvider.TypeId);
            return View(serviceProvider);
            //}
        }

        // GET: ServiceProvidersIndividual/Edit/5
        public ActionResult Edit(int? id)
        {
            //using (ConstructionLKContext db = new ConstructionLKContext())
            //{
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceProvider serviceProvider = db.ServiceProviders.Find(id);
            if (serviceProvider == null)
            {
                return HttpNotFound();
            }
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", serviceProvider.StatusId);

            ViewBag.MembershipTypeId = new SelectList(db.MembershipTypes, "Id", "Name", serviceProvider.MembershipTypeId);
            ViewBag.TypeId = new SelectList(db.ServiceProviderTypes, "Id", "Type", serviceProvider.TypeId);
            return View(serviceProvider);
            //}
        }

        // POST: ServiceProvidersIndividual/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,MembershipTypeId,Password,BasedCity,MailingAddress,Bio,FirstName,LastName,DateOfBirth,Telephone,CompanyName,CompanyRegNo,StartedDate,Avatar,StatusId,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,TypeId")] ServiceProvider serviceProvider)
        {
            //using (ConstructionLKContext db = new ConstructionLKContext())
            //{
            if (ModelState.IsValid)
            {
                db.Entry(serviceProvider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MyProfile", "ServiceProvidersIndividual", new { user = User.Identity.GetUserId() });
            }
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", serviceProvider.StatusId);

            ViewBag.MembershipTypeId = new SelectList(db.MembershipTypes, "Id", "Name", serviceProvider.MembershipTypeId);
            ViewBag.TypeId = new SelectList(db.ServiceProviderTypes, "Id", "Type", serviceProvider.TypeId);
            return View(serviceProvider);
            //}
        }

        //public ActionResult Edit( ServiceProvider serviceProvider, HttpPostedFileBase file)
        //{
        //    if (!(file == null))
        //    {
        //        if (ModelState.IsValid)
        //        {

        //            file.SaveAs(Path.Combine(Server.MapPath("~/Resources/ProfilePicturesSP/"), Path.GetFileName(file.FileName)));
        //            db.ServiceProviders.Add(new ServiceProvider
        //            {
        //                //Id = serviceProvider.Id,
        //                Username = serviceProvider.Username,
        //                MembershipTypeId = serviceProvider.MembershipTypeId,
        //                BasedCity = serviceProvider.BasedCity,
        //                MailingAddress = serviceProvider.MailingAddress,
        //                Bio = serviceProvider.Bio,
        //                FirstName = serviceProvider.FirstName,
        //                LastName = serviceProvider.LastName,
        //                DateOfBirth = serviceProvider.DateOfBirth,
        //                Telephone = serviceProvider.Telephone,
        //                StartedDate = serviceProvider.StartedDate,
        //                Avatar = "~/Resources/ProfilePicturesSP/" + Path.GetFileName(file.FileName),
        //                StatusId = serviceProvider.StatusId,
        //                CreatedBy = serviceProvider.CreatedBy,
        //                CreatedDate = serviceProvider.CreatedDate,
        //                ModifiedBy = serviceProvider.ModifiedBy,
        //                ModifiedDate = serviceProvider.ModifiedDate,
        //                TypeId = serviceProvider.TypeId,
        //                //ApplicationUserId = serviceProvider.ApplicationUserId
        //            }/*serviceProvider*/);
        //            db.SaveChanges();
        //            //return RedirectToAction("Index");
        //            return RedirectToAction("MyProfile", "ServiceProvidersIndividual", new { id = User.Identity.GetUserId() });
        //        }
        //    }
        //    else
        //    {
        //        if (ModelState.IsValid)
        //        {

        //            db.ServiceProviders.Add(serviceProvider);
        //            try
        //            {

        //                db.SaveChanges();
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e);
        //                throw;
        //            }
        //            return RedirectToAction("MyProfile", "ServiceProvidersIndividual", new { id = User.Identity.GetUserId() });
        //            //return RedirectToAction("Index");
        //        }
        //    }



        //    ViewBag.MembershipTypeId = new SelectList(db.MembershipTypes, "Id", "Name", serviceProvider.MembershipTypeId);
        //    //ViewBag.TypeId = new SelectList(db.ServiceProviderTypes, "Id", "Type", serviceProvider.TypeId);
        //    return View(serviceProvider);
        //}

        // GET: ServiceProvidersIndividual/Delete/5
        public ActionResult Delete(int? id)
        {
            //using (ConstructionLKContext db = new ConstructionLKContext())
            //{
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceProvider serviceProvider = db.ServiceProviders.Find(id);
            if (serviceProvider == null)
            {
                return HttpNotFound();
            }
            return View(serviceProvider);
            //}
        }

        // POST: ServiceProvidersIndividual/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //using (ConstructionLKContext db = new ConstructionLKContext())
            //{
            ServiceProvider serviceProvider = db.ServiceProviders.Find(id);
            db.ServiceProviders.Remove(serviceProvider);
            db.SaveChanges();
            return RedirectToAction("Index");

            //}
        }

        protected override void Dispose(bool disposing)
        {
            //using (ConstructionLKContext db = new ConstructionLKContext())
            //{
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
            //}
        }
    }
}
