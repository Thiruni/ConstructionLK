﻿using System;
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
    public class BlogPostsController : Controller
    {
        private ConstructionLKContext db = new ConstructionLKContext();

        // GET: BlogPosts
        public ActionResult Index()
        {

            var blogPosts = db.BlogPosts.Include(b => b.ServiceProvider).Include(b => b.BlogCategory).Where(b => b.Status == 1);
            return View("Index", blogPosts.ToList());
        }

        // GET: BlogPosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Create
        public ActionResult Create(string user)
        {
            ViewBag.userid = db.ServiceProviders.SingleOrDefault(s => s.ApplicationUserId == user).Id;
            ViewBag.CategoryId = new SelectList(db.BlogCategories, "Id", "Category");
            ViewBag.providerid = user;
            return View();
        }
        public ActionResult Posted()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                db.BlogPosts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Posted");
            }

            ViewBag.CategoryId = new SelectList(db.BlogCategories, "Id", "Category", blogPost.CategoryId);
            ViewBag.UserId = new SelectList(db.ServiceProviders, "Id", "Username", blogPost.UserId);

            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.BlogCategories, "Id", "Category", blogPost.CategoryId);
            //ViewBag.UserId = new SelectList(db.ServiceProviders, "Id", "Username", blogPost.UserId);
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PostContent,Status,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,UserId,CategoryId")] BlogPost blogPost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.BlogCategories, "Id", "Category", blogPost.CategoryId);
            ViewBag.UserId = new SelectList(db.ServiceProviders, "Id", "Username", blogPost.UserId);
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = db.BlogPosts.Find(id);
            db.BlogPosts.Remove(blogPost);
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
