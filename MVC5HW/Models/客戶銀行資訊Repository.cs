using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5HW.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        public 客戶銀行資訊 GetByID(int? id)
        {
            return this.All().FirstOrDefault(p => p.Id == id.Value);
        }
    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}