using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5HW.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
		public 客戶聯絡人 GetByID(int? id)
		{
			return this.All().FirstOrDefault(p => p.Id == id.Value);
		}
	}

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}