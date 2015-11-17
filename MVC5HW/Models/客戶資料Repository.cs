using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5HW.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public 客戶資料 GetByID(int? id)
        {
            return this.All().FirstOrDefault(p => p.Id == id.Value);
        }

    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}