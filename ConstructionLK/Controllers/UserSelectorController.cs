using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConstructionLK.Models;
using Microsoft.AspNet.Identity;

namespace ConstructionLK.Controllers
{   [AllowAnonymous]
    public class UserSelectorController : Controller
    {
        private ConstructionLKContext db = new ConstructionLKContext();
        // GET: UserSelector
        public ActionResult Index()
        {
            if (!(User.IsInRole(RoleName.Customer) || User.IsInRole(RoleName.ServiceProvider)))
            {
                
                return View("IndexToNew");//register
            }

            return View("Index");//create
        }

        public ActionResult UserProfile()
        {
            var userId = User.Identity.GetUserId();
            if (User.IsInRole(RoleName.CanManageAll)|| User.IsInRole(RoleName.AdministrativeStaff))
            {
                return RedirectToAction("AdminPanel", "AdministrativeStaff", new { area = "Admin" , id = userId });
            }
            ////new
            if (User.IsInRole(RoleName.Customer))
            {
                var customerStatus = db.Customers.SingleOrDefault(c => c.ApplicationUserId == userId).StatusId;
                if (customerStatus == 3)
                {
                    return RedirectToAction("LogOutBlacklist", "Account", new { id = 1 });

                }
                return RedirectToAction("MyProfile", "Customers", new { user = userId });
            }
            ServiceProvider type = db.ServiceProviders.SingleOrDefault(user => user.ApplicationUserId == userId);

            if (type != null && (User.IsInRole(RoleName.ServiceProvider) && type.TypeId == ServiceProviderTypeName.SpIndividual))
            {
                var spStatus = db.ServiceProviders.SingleOrDefault(c => c.ApplicationUserId == userId).StatusId;
                if (spStatus == 3)
                {
                    return RedirectToAction("LogOutBlacklist", "Account", new { id = 1 });

                }
                return RedirectToAction("MyProfile", "ServiceProvidersIndividual", new { user = userId });
            }
            if (type != null && (User.IsInRole(RoleName.ServiceProvider) && type.TypeId == ServiceProviderTypeName.SpCooperate))
            {
                var spStatus = db.ServiceProviders.SingleOrDefault(c => c.ApplicationUserId == userId).StatusId;
                if (spStatus == 3)
                {
                    return RedirectToAction("LogOutBlacklist", "Account", new { id = 1 });

                }
                return RedirectToAction("MyProfile", "ServiceProvidersCooperate", new { user = userId });
            }
            //end

            //AspNetUser email = db.AspNetUsers.SingleOrDefault(a => a.Id == userId);
            //if(email != null && !email.EmailConfirmed)
            //{
            //    return RedirectToAction("Index", "UserSelector");
            //}
            if (!(User.IsInRole(RoleName.Customer) || User.IsInRole(RoleName.ServiceProvider)))
            {
                var user = db.AspNetUsers.Find(userId);

                if (user != null && user.UserSelection == ServiceProviderTypeName.SpIndividual)
                {
                    return RedirectToAction("Create", "ServiceProvidersIndividual");
                }
                if (user != null && user.UserSelection == ServiceProviderTypeName.SpCooperate)
                {
                    return RedirectToAction("Create", "ServiceProvidersCooperate");
                }
                if (user != null && user.UserSelection == 3)
                {
                    return RedirectToAction("Create", "Customers");
                }
                if (user != null && user.UserSelection == 4)
                {
                    return RedirectToAction("Create", "AdministrativeStaff", new { area = "Admin" });
                    
                }
                return View("Index");//create
            }
            //var userId = User.Identity.GetUserId();
            //ServiceProvider type = db.ServiceProviders.SingleOrDefault(user => user.ApplicationUserId == userId);
            //var type = db.ServiceProviders.Find(User.Identity.GetUserId());
            if (User.IsInRole(RoleName.Customer))
            {
                var customerStatus = db.Customers.SingleOrDefault(c => c.ApplicationUserId == userId).StatusId;
                if (customerStatus == 3)
                {
                    return RedirectToAction("LogOutBlacklist", "Account", new { id = 1 });

                }
                return RedirectToAction("MyProfile", "Customers", new { user = userId });
            }

            if (type != null && (User.IsInRole(RoleName.ServiceProvider) && type.TypeId == ServiceProviderTypeName.SpIndividual))
            {
                var spStatus = db.ServiceProviders.SingleOrDefault(c => c.ApplicationUserId == userId).StatusId;
                if (spStatus == 3)
                {
                    return RedirectToAction("LogOutBlacklist", "Account", new { id = 1 });

                }
                return RedirectToAction("MyProfile", "ServiceProvidersIndividual", new { user = userId });
            }
            if (type != null && (User.IsInRole(RoleName.ServiceProvider) && type.TypeId == ServiceProviderTypeName.SpCooperate))
            {
                var spStatus = db.ServiceProviders.SingleOrDefault(c => c.ApplicationUserId == userId).StatusId;
                if (spStatus == 3)
                {
                    return RedirectToAction("LogOutBlacklist", "Account", new { id = 1 });

                }
                return RedirectToAction("MyProfile", "ServiceProvidersCooperate", new { user = userId });
            }

            return HttpNotFound();
        }

        public ActionResult Edit(string user)
        {

            if (User.IsInRole(RoleName.CanManageAll) || User.IsInRole(RoleName.AdministrativeStaff))
            {
                var admin = db.AdministrativeStaffs.SingleOrDefault(a => a.ApplicationUserId == user);
                return RedirectToAction("Edit", "AdministrativeStaff", new { area = "Admin", id = admin.Id });
            }
            if (User.IsInRole(RoleName.ServiceProvider))
            {
                var newuser = db.AspNetUsers.Find(user);
                if (newuser != null && newuser.UserSelection == ServiceProviderTypeName.SpIndividual)
                {
                    var spi = db.ServiceProviders.SingleOrDefault(s => s.ApplicationUserId == user);
                    return RedirectToAction("Edit", "ServiceProvidersIndividual", new { area = "", id = spi.Id });
                }
                if (newuser != null && newuser.UserSelection == ServiceProviderTypeName.SpCooperate)
                {
                    var spc = db.ServiceProviders.SingleOrDefault(s => s.ApplicationUserId == user);
                    return RedirectToAction("Edit", "ServiceProvidersCooperate", new { area = "", id = spc.Id });
                }
            }
            if ((User.IsInRole(RoleName.Customer)))
            {

                    var spc = db.Customers.SingleOrDefault(s => s.ApplicationUserId == user);
                    return RedirectToAction("Edit", "Customers", new { area = "", id = spc.Id });
            }
            return HttpNotFound();

        }

    }
}