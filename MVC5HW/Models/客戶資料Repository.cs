using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MVC5HW.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public 客戶資料 GetByID(int? id)
        {
            return this.All().FirstOrDefault(p => p.Id == id.Value);
        }

        public SelectList Get客戶分類資料篩選()
        {
            var customTypes = this.All().Where(p => p.是否已刪除 == false).GroupBy(x => x.客戶分類).Select(g => g.FirstOrDefault());
            var result = new SelectList(customTypes, "客戶分類", "客戶分類");
            return result;
        }

    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}