using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        public virtual Persons personItem { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transactions>? transactions { get; set; }

        public ItemPerson(Persons person, string nameOf, int summ)
        {
            this.nameOf = nameOf;
            this.summ = summ;
            this.person = person.id;
            personItem = person;
            if (transactions.IsNullOrEmpty()) transactions = new HashSet<Transactions>();
        }

        public ItemPerson()
        {
            transactions = new HashSet<Transactions>();
        }

        public ItemPerson(ItemPerson item)
        {
            this.person = item.person;
            this.nameOf = item.nameOf;
            this.summ = item.summ;
            this.savings = item.savings;
            this.id= item.id;
            this.transactions = item.transactions;
            this.personItem = item.personItem;
        }
    }
}