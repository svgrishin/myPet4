namespace myPet4.Models
{
    public interface IData
    {
        List<Transactions> Transactions { get; set; }
        List<Transactions> InicializeTransactions(int ID, DateOnly dateBegin, DateOnly dateEnd);
        Persons Persons { get; set; }
        Persons GetCurrentPersonByID(int ID);
        List<ItemPerson> Items { get; set; }
        List<ItemPerson> InicializeItems(int ID);
        List<Income> Incomes { get; set; }
        List<Income> InicializeIncomes(int ID, DateOnly dateBegin, DateOnly dateEnd);
        Finance Finance { get; set; }
        Finance InicializeFinance(int ID);
    }
}
