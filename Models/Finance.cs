using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace myPet.Data
{
    public class Finance
    {
        public int Id { get; set; }
        public decimal cash { get; set; }
        public decimal credit { get; set; }
        public decimal toSave { get; set; }
        public decimal freeMoney  { get; set; }
        public decimal salary { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
    }
}
