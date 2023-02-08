using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace myPet4.Models
{
    public partial class Income
    {
        public int id { get; set; }
        public int person { get; set; }
        [DisplayName("Сумма")]
        [Required(ErrorMessage = "Доход без суммы не засчитывается")]
        public int summ { get; set; }
        [DisplayName("Дата")]
        [Required(ErrorMessage = "Доход без даты не засчитывается")]
        public DateTime dateOf { get; set; }

        [ForeignKey("person")]
        public virtual Persons incomePerson { get; set; }

        public Income(DateTime dateOf, Persons person)
        {
            this.person = person.id;
            this.dateOf = dateOf;
            this.incomePerson = person;
        }
        public Income() { }
    }
}