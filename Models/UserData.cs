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
            /// % исчерпания статьи расхода
            /// </summary>
            public int loaded { get; set; }
            public UserItem(ItemPerson item, DateTime dateBegin, DateTime dateEnd)
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
        /// <summary>
        /// Статьи расходов для приложения. Содержит дополнительные параметры к EF ItemPerson
        /// </summary>
        public List<UserItem> userItems;

        public void Add(ItemPerson item, Transactions transaction)
        {
            item.Transactions.Add(transaction);
        }

        public UserData(Persons person)
        {
            Person = person;
            toSaveByPeriod = 0;
            savedMoney = 0;

            userItems = new List<UserItem>();
            foreach(ItemPerson item in person.ItemPerson)
            {
                userItems.Add(new UserItem(item, person.Finance.dateBegin, person.Finance.dateEnd));
            }

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
        public UserData(){}
    }

}