using myPet.Data;

namespace myPet.Models
{
    public class Data
    {
        public Persons Person { get; set; }
        public DateOnly[] Period { get; set; }
        public Data(Persons person)
        {
            Person = person;
        }
        //public class personItem
        //{
        //    public List<Transaction> Transactions { get; set; }
        //    public Item item { get; set; }
        //    public personItem(Item itemOf, List<Transaction> transactions)
        //    {
        //        this.item = itemOf;
        //        Transactions = transactions;
        //    }
        //}
    }
}