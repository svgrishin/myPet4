using myPet.Data;

namespace myPet.Models
{
    public interface IData
    {
        List<Transaction> Transactions { get; set; }
        List<Transaction> InicializeTransactions(int ID, DateOnly dateBegin, DateOnly dateEnd);
        Person Persons { get; set; }
        Person GetCurrentPersonByID(int ID);
        List<Item> Items { get; set; }
        List<Item> InicializeItems(int ID);
        List<Income> Incomes { get; set; }
        List<Income> InicializeIncomes(int ID, DateOnly dateBegin, DateOnly dateEnd);
        Finance Finance { get; set; }
        Finance InicializeFinance(int ID);
    }
}
