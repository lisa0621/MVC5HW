using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MVC5HW.Models
{
    public class LoginVM : IValidatableObject
    {
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required]
        public string 帳號 { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required]
        public string 密碼 { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            
            string ExamPassword = sha256_hash(密碼);

            客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();
            if(!repo.Where(x=> x.帳號 == 帳號 && x.密碼 == ExamPassword).Any())
            { 
                yield return new ValidationResult("無此帳號或密碼", new string[] { "帳號" });
            }
        }

        public static String sha256_hash(String value)
        {
            using (SHA256 hash = SHA256Managed.Create())
            {
                return String.Join("", hash
                  .ComputeHash(Encoding.UTF8.GetBytes(value))
                  .Select(item => item.ToString("x2")));
            }
        }
    }
}