using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace myPet.Data
{
    public partial class income
    {
        public int id { get; set; }
        public int person { get; set; }
        public decimal summ { get; set; }
        public System.DateTime dateOf { get; set; }

        [ForeignKey("person")]
        public virtual Persons Persons { get; set; }
    }
}
