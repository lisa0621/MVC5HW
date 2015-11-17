using System;
using System.Linq;
using System.Collections.Generic;
	
namespace MVC5HW.Models
{   
	public  class 客戶相關資訊數量Repository : EFRepository<客戶相關資訊數量>, I客戶相關資訊數量Repository
	{

    }

	public  interface I客戶相關資訊數量Repository : IRepository<客戶相關資訊數量>
	{

	}
}