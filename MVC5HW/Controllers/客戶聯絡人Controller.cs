using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5HW.Models;

namespace MVC5HW.Controllers
{
    public class 客戶聯絡人Controller : Controller
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        客戶聯絡人Repository repo = RepositoryHelper.Get客戶聯絡人Repository();
        // GET: 客戶聯絡人

        public ActionResult Index(string search, string jobType)
        {
            //var data = db.客戶聯絡人.Include(客 => 客.客戶資料).AsQueryable();
            var data = repo.All ().Include(客 => 客.客戶資料).AsQueryable();

            data = data.Where(p => p.是否已刪除 == false);

            var jobTypes = repo.All().Where(p => p.是否已刪除 == false).GroupBy(x => x.職稱).Select(g => g.FirstOrDefault());
            ViewBag.職稱 = new SelectList(jobTypes, "職稱", "職稱");

            if (!String.IsNullOrEmpty(search))
            {
                data = data.Where(p => p.姓名.Contains(search));
            }

            if (!String.IsNullOrEmpty(jobType))
            {
                data = data.Where(p => p.職稱 == jobType);
            }

            return View(data);
        }

        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            客戶聯絡人 客戶聯絡人 = repo.GetByID(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            //ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");
            ViewBag.客戶Id = new SelectList(RepositoryHelper.Get客戶資料Repository().All(), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {

            //var isRepeat = db.客戶聯絡人.Where(p => p.客戶Id == 客戶聯絡人.客戶Id & p.Email == 客戶聯絡人.Email).Any();
            var isRepeat = repo.All().Where(p => p.客戶Id == 客戶聯絡人.客戶Id & p.Email == 客戶聯絡人.Email).Any();

            if (isRepeat)
            {
                ModelState.AddModelError("Email", "Email 不能重複");
            }

            if (ModelState.IsValid)
            {
                //db.客戶聯絡人.Add(客戶聯絡人);
                //db.SaveChanges();
                repo.Add(客戶聯絡人);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            //ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            ViewBag.客戶Id = new SelectList(RepositoryHelper.Get客戶資料Repository().All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);

            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            客戶聯絡人 客戶聯絡人 = repo.GetByID(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }

            //ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            ViewBag.客戶Id = new SelectList(RepositoryHelper.Get客戶資料Repository().All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);

            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        public ActionResult Edit(int? id, FormCollection form)
        {
            客戶聯絡人 客戶聯絡人 = repo.GetByID(id);
            var includeProperties = "Id,客戶Id,職稱,姓名,Email,手機,電話".Split(',');
            if (TryUpdateModel<客戶聯絡人>(客戶聯絡人, includeProperties))
            {
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            //if (ModelState.IsValid)
            //{
            //    db.Entry(客戶聯絡人).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            ViewBag.客戶Id = new SelectList(RepositoryHelper.Get客戶資料Repository().All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);

            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            客戶聯絡人 客戶聯絡人 = repo.GetByID(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            客戶聯絡人 客戶聯絡人 = repo.GetByID(id);
            客戶聯絡人.是否已刪除 = true;

            //db.客戶聯絡人.Remove(客戶聯絡人);
            //db.SaveChanges();
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
