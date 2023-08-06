using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebBirthdayMVC.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = ""; // имя пользователя
        [Column(TypeName = "date")]
        public DateTime Birthday { get; set; } // День рождения пользователя
        public bool Type { get; set; } // Важное?
        public string? Photo { get; set; } // Фото пользователя // Convert.FromBase64String (string s);
        public int DayOfYear
        {
            get
            {
                int temp = DayOfYear;
                int n = DateTime.Now.DayOfYear;
                if (temp < n)
                {
                    temp += 366;
                }
                return temp;
            }
            set
            {
                DayOfYear = Birthday.DayOfYear;
            }                
        }
        //public decimal DayOfYear => Birthday.DayOfYear;
        /*[NotMapped]*/
    }
}
