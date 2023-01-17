using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace myPet4.Models
{
    public partial class income
    {
        public int id { get; set; }
        public int person { get; set; }
        [DisplayName("Сумма")]
        [Required(ErrorMessage = "Доход без суммы не засчитывается")]
        public decimal summ { get; set; }
        [DisplayName("Дата")]
        [Required(ErrorMessage = "Доход без даты не засчитывается")]
        public DateTime dateOf { get; set; }

        [ForeignKey("person")]
        public virtual Persons Persons { get; set; }
    }
}
