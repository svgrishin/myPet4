using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace myPet.Data
{
    public class Transaction
    {
        public int id { get; set; }
        public int item { get; set; }
        public DateTime dateOf { get; set; }
        public decimal summ { get; set; }
        public bool credit { get; set; }
        public string? description { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}