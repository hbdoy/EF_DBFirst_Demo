using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DemoLibraryMVC.Models;

namespace DemoLibraryMVC.Controllers
{
    public class NewBooksController : Controller
    {
        private GSSWEBEntities db = new GSSWEBEntities();

        // GET: NewBooks
        public ActionResult Index()
        {
            return View(db.BOOK_DATA.ToList());
        }

        // GET: NewBooks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOK_DATA bOOK_DATA = db.BOOK_DATA.Find(id);
            if (bOOK_DATA == null)
            {
                return HttpNotFound();
            }
            return View(bOOK_DATA);
        }

        // GET: NewBooks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NewBooks/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BOOK_ID,BOOK_NAME,BOOK_CLASS_ID,BOOK_AUTHOR,BOOK_BOUGHT_DATE,BOOK_PUBLISHER,BOOK_NOTE,BOOK_STATUS,BOOK_KEEPER,BOOK_AMOUNT,CREATE_DATE,CREATE_USER,MODIFY_DATE,MODIFY_USER")] BOOK_DATA bOOK_DATA)
        {
            if (ModelState.IsValid)
            {
                db.BOOK_DATA.Add(bOOK_DATA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bOOK_DATA);
        }

        // GET: NewBooks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOK_DATA bOOK_DATA = db.BOOK_DATA.Find(id);
            if (bOOK_DATA == null)
            {
                return HttpNotFound();
            }
            return View(bOOK_DATA);
        }

        // POST: NewBooks/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BOOK_ID,BOOK_NAME,BOOK_CLASS_ID,BOOK_AUTHOR,BOOK_BOUGHT_DATE,BOOK_PUBLISHER,BOOK_NOTE,BOOK_STATUS,BOOK_KEEPER,BOOK_AMOUNT,CREATE_DATE,CREATE_USER,MODIFY_DATE,MODIFY_USER")] BOOK_DATA bOOK_DATA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bOOK_DATA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bOOK_DATA);
        }

        // GET: NewBooks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BOOK_DATA bOOK_DATA = db.BOOK_DATA.Find(id);
            if (bOOK_DATA == null)
            {
                return HttpNotFound();
            }
            return View(bOOK_DATA);
        }

        // POST: NewBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BOOK_DATA bOOK_DATA = db.BOOK_DATA.Find(id);
            db.BOOK_DATA.Remove(bOOK_DATA);
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
