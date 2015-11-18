﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5HW.Models;
using System.Data.Entity.Validation;
using MVC5HW.ActionFilters;
using PagedList;

namespace MVC5HW.Controllers
{
    public class 客戶資料Controller : Controller
    {
        private 客戶資料Entities db = new 客戶資料Entities();
        客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();
        客戶聯絡人Repository repoContact = RepositoryHelper.Get客戶聯絡人Repository();

        // GET: 客戶資料
        [TimeFilterAttribute]
        public ActionResult Index(string search, string customType, int? page, string sortOrder)
        {
           
            //var data = db.客戶資料.AsQueryable();
            var data = repo.All();
            data = data.Where(p => p.是否已刪除 == false);

            //var customTypes = repo.All().Where(p => p.是否已刪除 == false).GroupBy(x => x.客戶分類).Select(g => g.FirstOrDefault());
            //ViewBag.客戶分類 = new SelectList(customTypes, "客戶分類", "客戶分類");

            ViewBag.客戶分類 = repo.Get客戶分類資料篩選();

            if (!String.IsNullOrEmpty(search))
            {
                data = data.Where(p => p.客戶名稱.Contains(search));
            }

            if (!String.IsNullOrEmpty(customType))
            {
                data = data.Where(p => p.客戶分類==customType);
            }

            ViewBag.客戶名稱SortParm = String.IsNullOrEmpty(sortOrder) ? "客戶名稱_desc" : "";
            ViewBag.客戶分類SortParm = sortOrder == "客戶分類" ? "客戶分類_desc" : "客戶分類";
            ViewBag.統一編號SortParm = sortOrder == "統一編號" ? "統一編號_desc" : "統一編號";
            ViewBag.電話SortParm = sortOrder == "電話" ? "電話_desc" : "電話";
            ViewBag.傳真SortParm = sortOrder == "傳真" ? "傳真_desc" : "傳真";
            ViewBag.地址SortParm = sortOrder == "地址" ? "地址_desc" : "地址";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email_desc" : "Email";

            switch (sortOrder)
            {
                case "客戶名稱_desc":
                    data = data.OrderByDescending(s => s.客戶名稱);
                    break;
                case "客戶分類":
                    data = data.OrderBy(s => s.客戶分類);
                    break;
                case "客戶分類_desc":
                    data = data.OrderByDescending(s => s.客戶分類);
                    break;
                case "統一編號":
                    data = data.OrderBy(s => s.統一編號);
                    break;
                case "統一編號_desc":
                    data = data.OrderByDescending(s => s.統一編號);
                    break;
                case "電話":
                    data = data.OrderBy(s => s.電話);
                    break;
                case "電話_desc":
                    data = data.OrderByDescending(s => s.電話);
                    break;
                case "傳真":
                    data = data.OrderBy(s => s.傳真);
                    break;
                case "傳真_desc":
                    data = data.OrderByDescending(s => s.傳真);
                    break;
                case "地址":
                    data = data.OrderBy(s => s.地址);
                    break;
                case "地址_desc":
                    data = data.OrderByDescending(s => s.地址);
                    break;
                case "Email":
                    data = data.OrderBy(s => s.Email);
                    break;
                case "Email_desc":
                    data = data.OrderByDescending(s => s.Email);
                    break;
                default:
                    data = data.OrderBy(s => s.客戶名稱);
                    break;
            }


            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            IPagedList<客戶資料> onePageOfdata = data.ToPagedList(pageNumber, 5);
            return View(onePageOfdata);
        }

        // GET: 客戶相關資料數量
        public ActionResult Counts()
        {
            //return View(db.客戶相關資訊數量.ToList());
            return View(RepositoryHelper.Get客戶相關資訊數量Repository().All().ToList());
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo.GetByID(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            //return View(客戶資料);

            var contactsData = repoContact.All().Where(p => p.客戶Id == id);

            CustomDetailWithContact data = new CustomDetailWithContact
            {
                custom = 客戶資料,
                contacts = contactsData
            };

            return View(data);
        }

        [HttpPost]
        public ActionResult Details(FormCollection form)
        {
            IList<客戶聯絡人> data = new List<客戶聯絡人>();
            if (TryUpdateModel<IList<客戶聯絡人>>(data, "data"))
            {
            }
                return View();
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: 客戶資料/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,客戶分類,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                //db.客戶資料.Add(客戶資料);
                //db.SaveChanges();

                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo.GetByID(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HandleError(ExceptionType = typeof(DbEntityValidationException), View = "Error_DbEntityValidationException")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,客戶名稱,客戶分類,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        public ActionResult Edit(int? id, FormCollection form)
        {
            throw new DbEntityValidationException();

            客戶資料 客戶資料 = repo.GetByID(id);
            var includeProperties = "Id,客戶名稱,客戶分類,統一編號,電話,傳真,地址,Email".Split(',');
            if (TryUpdateModel<客戶資料>(客戶資料, includeProperties))
            {
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            //if (ModelState.IsValid)
            //{
            //    db.Entry(客戶資料).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo.GetByID(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //客戶資料 客戶資料 = db.客戶資料.Find(id);
            客戶資料 客戶資料 = repo.GetByID(id);
            客戶資料.是否已刪除 = true;
            //db.客戶資料.Remove(客戶資料);
            //db.SaveChanges();
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult JsonIndex()
        {
            return View();
        }

        [HttpPost]
        public JsonResult JsonTry()
        {
            db.Configuration.LazyLoadingEnabled = false;
            var data = db.客戶資料.Take(2).ToList();

            return Json(data);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
