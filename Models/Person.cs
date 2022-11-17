using System.ComponentModel.DataAnnotations;

namespace myPet.Data
{
    public class Person
    {
        public int? id { get; set; }
        public string login { get; set; }
        public virtual ICollection<Finance>? Finances { get; set; }
        public virtual ICollection<Income>? Incomes { get; set; }
        public virtual ICollection<Item>? Items { get; set; }

        public Person(string login)
        {
            this.login = login;
        }
    }
}