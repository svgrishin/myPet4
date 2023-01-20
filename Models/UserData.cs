using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing.Matching;
using NuGet.Packaging;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace myPet4.Models
{
    public class UserData
    {
        public Persons Person { get; set; }
        public decimal toSaveByPeriod { get; set; }
        [DisplayName("Расчётная прибыль")]
        public decimal profit { get; set; }
        public decimal currentIncome { get; set; }
        private decimal currentTransactionsSumm { get; set; }
        [DisplayName("Сбережения")]
        public decimal savedMoney { get; set; }
        //public List<Item> items = new List<Item>();
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

        //public void getCurrentIncome(Persons person)
        //{
        //    List<income> currentIncomes = person.income.Where(i => i.dateOf >= person.Finance.dateBegin & i.dateOf <= person.Finance.dateEnd).ToList();
        //    if (currentIncomes.Count > 0)
        //    {
        //        foreach (income i in currentIncomes)
        //        {
        //            currentIncome += i.summ;
        //        }
        //    }
        //    else
        //    {
        //        currentIncome = 0;
        //    }
        //}
        public UserData(Persons person)
        {
            Person = person;
            toSaveByPeriod = 0;
            savedMoney = 0;

            currentIncome = 0;
            List<income> currentIncomes = person.income.Where(i => i.dateOf >= person.Finance.dateBegin && i.dateOf <= person.Finance.dateEnd).ToList();
            if (currentIncomes.Count > 0)
            {
                foreach (income i in currentIncomes)
                {
                    currentIncome += i.summ;
                }
                toSaveByPeriod = currentIncome * (person.Finance.toSave / person.Finance.salary);
            }

            currentTransactionsSumm = 0;
            List<Transactions> currentTransactions = new List<Transactions>();
            foreach (ItemPerson i in person.ItemPerson)
            {
                if (i.Transactions != null)
                {
                    currentTransactions.AddRange(i.Transactions.Where(i => i.dateOf >= person.Finance.dateBegin && i.dateOf <= person.Finance.dateEnd));
                    foreach (Transactions t in currentTransactions)
                    {
                        currentTransactionsSumm += t.summ;
                    }
                }
            }

            if (person.Finance.salary > currentIncome)
            {
                profit = person.Finance.salary - currentTransactionsSumm;
            }
            else
            {
                profit = currentIncome - currentTransactionsSumm;
            }

            //items = new List<Item>();

            //foreach (ItemPerson i in person.ItemPerson)
            //{
            //    items.Add(new Item(i, person.Finance.dateBegin, person.Finance.dateEnd));
            //}
        }
    }

}