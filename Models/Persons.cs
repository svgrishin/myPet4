using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace myPet.Data
{
    public partial class Persons
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Persons(string login)
        {
            this.income = new HashSet<income>();
            this.ItemPerson = new HashSet<ItemPerson>();
            this.login= login;
        }

        public Persons(Persons p)
        {
            this.income = p.income;
            this.ItemPerson = p.ItemPerson;
            this.login = p.login;
        }

        public Persons()
        {
            
        }

        public int id { get; set; }
        public string login { get; set; }
        public virtual Finance? Finance { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<income>? income { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemPerson>? ItemPerson { get; set; }
    }
}