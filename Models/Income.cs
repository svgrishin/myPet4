using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace myPet.Data
{
    public class Income
    {
        public int id { get; set; }
        public int person { get; set; }
        public decimal summ { get; set; }
        public DateTime dateOf { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
    }
}
