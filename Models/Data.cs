using myPet.Data;

namespace myPet.Models
{
    public class Data
    {
        public List<Transaction> Transactions { get; set; }
        public Person Persons { get; set; }
        public List<Item> Items { get; set; }
        public List<Income> Incomes { get; set; }
        public Finance Finance { get; set; }
    }
}