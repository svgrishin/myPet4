using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace myPet.Data
{
    public class Person
    {
        public int? id { get; set; }
        [Required(ErrorMessage = "Заполните имя пользователя")]
        [StringLength(10, ErrorMessage = "Имя не должно превышать 10 символов")]
        [DisplayName("Имя пользователя")]
        public string login { get; set; }
        public virtual ICollection<Finance>? Finances { get; set; }
        public virtual ICollection<Income>? Incomes { get; set; }
        public virtual ICollection<Item>? Items { get; set; }

        public Person(string login)
        {
            this.login = login;
        }

        public Person()
        {

        }
    }
}