using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Models
{
    public class Book
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public string author { get; set; }
    }
}
