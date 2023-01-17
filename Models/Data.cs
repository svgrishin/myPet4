using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing.Matching;
using System.Linq.Expressions;

namespace myPet4.Models
{
    public class Data
    {
        public Persons Person { get; set; }
        decimal toSaveByPeriod { get; set; }
        decimal profit { get; set; }
        public List<Item> items { get; set; }
        public class Item
        {
            public ItemPerson item { get; set; }
            /// <summary>
            /// Текущая норма в день
            /// </summary>
            public decimal currentSumm { get; set; }
            /// <summary>
            /// Потрачено за день
            /// </summary>
            public decimal dailyBalance { get; set; }
            /// <summary>
            /// % исчерпания статьи расхода
            /// </summary>
            public int loaded { get; set; }
            public Item(ItemPerson item, DateTime dateBegin, DateTime dateEnd)
            {
                this.item = item;
                currentSumm = item.summ / dateEnd.Subtract(DateTime.Today).Days;

                decimal d = 0;
                try
                {
                    foreach (Transactions t in item.Transactions.Where(p => p.dateOf == DateTime.Today))
                    {
                        d += t.summ;
                    }
                }
                catch { }
                dailyBalance = currentSumm - d;

                decimal p = 0;
                try
                {
                    foreach (Transactions t in item.Transactions.Where(p => p.dateOf >= dateBegin))
                    {
                        p += t.summ;
                    }
                    loaded = (int)(p / item.summ);
                }
                catch
                {
                    loaded = 0;
                }
            }
        }
        public Data(Persons person)
        {
            Person = person;

            toSaveByPeriod = 0;

            decimal currentIncomeSumm = 0;
            List<income> currentIncomes = person.income.Where(i => i.dateOf >= person.Finance.dateBegin && i.dateOf <= person.Finance.dateEnd).ToList();
            if (currentIncomes.Count > 0)
            {
                foreach (income i in currentIncomes)
                {
                    currentIncomeSumm += i.summ;
                }
                toSaveByPeriod = currentIncomeSumm * (person.Finance.toSave / person.Finance.salary);
            }

            decimal currentTransactionsSumm = 0;
            foreach (ItemPerson i in person.ItemPerson)
            {
                if (i.Transactions != null)
                {
                    foreach (Transactions t in i.Transactions)
                    {
                        currentTransactionsSumm += t.summ;
                    }
                }
            }

            if (person.Finance.salary > currentIncomeSumm)
            {
                profit = person.Finance.salary - currentTransactionsSumm;
            }
            else
            {
                profit = currentIncomeSumm - currentTransactionsSumm;
            }

            items = new List<Item>();

            foreach (ItemPerson i in person.ItemPerson)
            {
                items.Add(new Item(i, person.Finance.dateBegin, person.Finance.dateEnd));
            }
        }
    }

}