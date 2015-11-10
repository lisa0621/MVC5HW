using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MVC5HW.Models
{
    public class 手機電話格式 : DataTypeAttribute
    {
        public const string RegExString = @"\d{4}-\d{6}";

        public 手機電話格式() : base(DataType.Text)
        {
            this.ErrorMessage = "手機格式錯誤";
        }

        public override bool IsValid(object value)
        {
            Regex regEx = new Regex(RegExString);
            return regEx.IsMatch(value.ToString());
        }
    }
}