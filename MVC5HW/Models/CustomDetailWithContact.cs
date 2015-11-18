using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC5HW.Models
{
    public class CustomDetailWithContact
    {
        public 客戶資料 custom { get; set; }

        public IEnumerable<客戶聯絡人> contacts { get; set; }
    }
}