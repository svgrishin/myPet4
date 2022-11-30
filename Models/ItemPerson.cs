using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace myPet.Data
{
    public partial class ItemPerson
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ItemPerson()
        {
            this.Transactions = new HashSet<Transactions>();
        }

        public int id { get; set; }
        public string nameOf { get; set; }
        
        
        public int person { get; set; }
        public decimal summ { get; set; }
        public decimal savings { get; set; }

        [ForeignKey("person")]
        public virtual Persons Persons { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}
