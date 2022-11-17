using System.Data.SqlTypes;

namespace myPet.Data
{
    public class Item
    {
        public int Id { get; set; }
        public string nameOf { get; set; }
        public int person { get; set; }
        public decimal summ { get; set; }
        public decimal savings { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
    }
}
