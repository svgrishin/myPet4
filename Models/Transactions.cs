using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace myPet4.Models
{
    public partial class Transactions
    {
        public int id { get; set; }
        [DisplayName("Статья расхода")]
        public int item { get; set; }
        [DisplayName("Дата")]
        [Required(ErrorMessage = "Затраты без даты не учитываются")]
        public DateTime dateOf { get; set; }
        [DisplayName("Сумма")]
        [Required(ErrorMessage = "Затраты без суммы не учитываются")]
        public int summ { get; set; }
        [DisplayName("Оплачено кредитной картой")]
        public bool credit { get; set; }
        [DisplayName("Комментарий")]
        public string? description { get; set; }

        [ForeignKey("item")]
        public virtual ItemPerson ItemPerson { get; set; }

        public Transactions(ItemPerson item, DateTime dateOf, int summ, bool credit, string? description)
        {
            this.item = item.id;
            this.dateOf = dateOf;
            this.summ = summ;
            this.credit = credit;
            this.description = description;
            ItemPerson= item;
        }
        public Transactions() { }
    }
}