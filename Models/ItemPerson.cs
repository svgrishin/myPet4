using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace myPet4.Models
{
    public partial class ItemPerson
    {
        public int id { get; set; }
        [DisplayName("Название")]
        [Required(ErrorMessage = "У статьи расходов должно быть название")]
        public string nameOf { get; set; }
        public int person { get; set; }
        [DisplayName("Сумма")]
        [Required(ErrorMessage = "У статьи расходов должно быть сумма")]
        public int summ { get; set; }
        [DisplayName("Сбережения")]
        public int savings { get; set; }

        [ForeignKey("person")]
        public virtual Persons Persons { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transactions>? Transactions { get; set; }



        public ItemPerson(int person, string nameOf, int summ)
        {
            this.nameOf = nameOf;
            this.summ = summ;
            this.person = person;
        }

        public ItemPerson()
        {
            Transactions = new HashSet<Transactions>();
        }

        
    }
}