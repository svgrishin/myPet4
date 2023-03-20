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
        /// <summary>
        /// Сбережения текущего расчётного периода
        /// </summary>
        [DisplayName("Текущие сбережения")]
        public int toSaveByPeriod { get; set; }
        /// <summary>
        /// Прибыль в текущем периоде
        /// </summary>
        [DisplayName("Расчётная прибыль")]
        public int profit { get; set; }
        /// <summary>
        /// Весь доход в текущем периоде
        /// </summary>
        [DisplayName("Доход")]
        public int currentIncome { get; set; }
        /// <summary>
        /// Общий расход
        /// </summary>
        private int currentTransactionsSumm { get; set; }
        /// <summary>
        /// Общие сбережения
        /// </summary>
        [DisplayName("Общие сбережения")]
        public int savedMoney { get; set; }
        public class UserItem
        {
            public ItemPerson item { get; set; }
            /// <summary>
            /// Текущая норма в день
            /// </summary>
            public decimal currentSumm { get; set; }
            /// <summary>
            /// Дневной остаток по статье расхода
            /// </summary>
            public decimal dailyBalance { get; set; }
            /// <summary>
            /// сумма расхода по статье за период
            /// </summary>
            public int loaded { get; set; }
            public UserItem(ItemPerson item, DateTime dateBegin, DateTime dateEnd)
            {
                DateTime date1 = new DateTime(2023, 3, 12);

                this.item = item;

                //currentSumm = item.summ / dateEnd.Subtract(DateTime.Today).Days;
                currentSumm = (item.summ) / dateEnd.Subtract(date1).Days + 1;

                int d = 0;
                try
                {
                    //foreach (Transactions t in item.transactions.Where(p => p.dateOf == DateTime.Today))
                    foreach (Transactions t in item.transactions.Where(transaction=>transaction.dateOf == date1))
                    {
                        d += t.summ;
                    }
                }
                catch { }
                dailyBalance = currentSumm - d;

                //int p = 0;
                try
                {
                    foreach (Transactions t in item.transactions.Where(transaction=>transaction.dateOf >= dateBegin))
                    {
                        loaded += t.summ;
                    }
                    //loaded = (int)(p / item.summ);
                    //loaded = p;
                }
                catch
                {
                    loaded = 0;
                }

                
            }
        }
        /// <summary>
        /// Статьи расходов для приложения. Содержит дополнительные параметры к EF ItemPerson
        /// </summary>
        public List<UserItem> userItems;

        public void Add(ItemPerson item, Transactions transaction)
        {
            item.transactions.Add(transaction);
        }

        public UserData(Persons person)
        {
            Person = person;
            toSaveByPeriod = 0;
            savedMoney = 0;

            userItems = new List<UserItem>();
            foreach (ItemPerson item in person.itemPerson)
            {
                userItems.Add(new UserItem(item, person.Finance.dateBegin, person.Finance.dateEnd));
            }

            currentIncome = 0;
            List<Income> currentIncomes = person.income.Where(i => i.dateOf >= person.Finance.dateBegin && i.dateOf <= person.Finance.dateEnd).ToList();
            if (currentIncomes.Count > 0)
            {
                foreach (Income i in currentIncomes)
                {
                    currentIncome += i.summ;
                }
                toSaveByPeriod = currentIncome * (person.Finance.toSave / person.Finance.salary);
            }

            currentTransactionsSumm = 0;
            List<Transactions> currentTransactions = new List<Transactions>();
            foreach (ItemPerson i in person.itemPerson)
            {
                if (i.transactions != null)
                {
                    currentTransactions.AddRange(i.transactions.Where(i => i.dateOf >= person.Finance.dateBegin && i.dateOf <= person.Finance.dateEnd));
                    foreach (Transactions t in currentTransactions)
                    {
                        currentTransactionsSumm += t.summ;
                    }
                }
            }

            profit = 0;
            if (person.Finance.salary > currentIncome)
            {
                profit = person.Finance.salary - currentTransactionsSumm;
            }
            else
            {
                profit = currentIncome - currentTransactionsSumm;
            }
        }
        public UserData() { }
        
        public void UpdateUser()
        {
            List<Income> currentIncomes = Person.income.Where(i => i.dateOf >= Person.Finance.dateBegin && i.dateOf <= Person.Finance.dateEnd).ToList();
            if (currentIncomes.Count > 0)
            {
                foreach (Income i in currentIncomes)
                {
                    currentIncome += i.summ;
                }
                toSaveByPeriod = currentIncome * (Person.Finance.toSave / Person.Finance.salary);
            }

            currentTransactionsSumm = 0;
            List<Transactions> currentTransactions = new List<Transactions>();
            foreach (ItemPerson i in Person.itemPerson)
            {
                if (i.transactions != null)
                {
                    currentTransactions.AddRange(i.transactions.Where(i => i.dateOf >= Person.Finance.dateBegin && i.dateOf <= Person.Finance.dateEnd));
                    foreach (Transactions t in currentTransactions)
                    {
                        currentTransactionsSumm += t.summ;
                    }
                }
            }
        }
    }


}