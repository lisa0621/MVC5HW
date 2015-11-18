using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5HW.Models;
using NPOI.HSSF.UserModel;
using System.IO;

namespace MVC5HW.Controllers
{
    public class 客戶聯絡人Controller : Controller
    {
        //private 客戶資料Entities db = new 客戶資料Entities();
        客戶聯絡人Repository repo = RepositoryHelper.Get客戶聯絡人Repository();
        // GET: 客戶聯絡人

        public ActionResult Index(string search, string jobType, string sortOrder)
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

            ViewBag.職稱SortParm = String.IsNullOrEmpty(sortOrder) ? "職稱_desc" : "";
            ViewBag.姓名SortParm = sortOrder == "姓名" ? "姓名_desc" : "姓名";
            ViewBag.EmailSortParm = sortOrder == "Email" ? "Email_desc" : "Email";
            ViewBag.手機SortParm = sortOrder == "手機" ? "手機_desc" : "手機";
            ViewBag.電話SortParm = sortOrder == "電話" ? "電話_desc" : "電話";
            ViewBag.客戶名稱SortParm = sortOrder == "客戶名稱" ? "客戶名稱_desc" : "客戶名稱";

            switch (sortOrder)
            {
                case "職稱_desc":
                    data = data.OrderByDescending(s => s.職稱);
                    break;
                case "姓名":
                    data = data.OrderBy(s => s.姓名);
                    break;
                case "姓名_desc":
                    data = data.OrderByDescending(s => s.姓名);
                    break;
                case "Email":
                    data = data.OrderBy(s => s.Email);
                    break;
                case "Email_desc":
                    data = data.OrderByDescending(s => s.Email);
                    break;
                case "手機":
                    data = data.OrderBy(s => s.手機);
                    break;
                case "手機_desc":
                    data = data.OrderByDescending(s => s.手機);
                    break;
                case "電話":
                    data = data.OrderBy(s => s.電話);
                    break;
                case "電話_desc":
                    data = data.OrderByDescending(s => s.電話);
                    break;
                case "客戶名稱":
                    data = data.OrderBy(s => s.客戶資料.客戶名稱);
                    break;
                case "客戶名稱_desc":
                    data = data.OrderByDescending(s => s.客戶資料.客戶名稱);
                    break;
                default:
                    data = data.OrderBy(s => s.職稱);
                    break;
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

        public FileResult NPOIdownload()
        {
            var data = repo.All().AsQueryable();
            data = data.Where(p => p.是否已刪除 == false);

            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet("客戶聯絡人");
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Id");
            headerRow.CreateCell(1).SetCellValue("客戶Id");
            headerRow.CreateCell(2).SetCellValue("職稱");
            headerRow.CreateCell(3).SetCellValue("姓名");
            headerRow.CreateCell(4).SetCellValue("Email");
            headerRow.CreateCell(5).SetCellValue("手機");
            headerRow.CreateCell(6).SetCellValue("電話");
            sheet.CreateFreezePane(0, 1, 0, 1);

            int rowNumber = 1;
            foreach (var datarow in data)
            {
                var row = sheet.CreateRow(rowNumber++);
                row.CreateCell(0).SetCellValue(datarow.Id);
                row.CreateCell(1).SetCellValue(datarow.客戶Id);
                row.CreateCell(2).SetCellValue(datarow.職稱);
                row.CreateCell(3).SetCellValue(datarow.姓名);
                row.CreateCell(4).SetCellValue(datarow.Email);
                row.CreateCell(5).SetCellValue(datarow.手機);
                row.CreateCell(6).SetCellValue(datarow.電話);
            }

            MemoryStream output = new MemoryStream();
            workbook.Write(output);
            return File(output.ToArray(), "application/vnd.ms-excel", "客戶聯絡人.xls");
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
