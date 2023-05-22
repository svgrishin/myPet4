﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing.Matching;
using NuGet.Packaging;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using static myPet4.Models.UserData;

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
        /// Прибыль текущая за период
        /// </summary>
        [DisplayName("Текущая прибыль")]
        public int currentProfit { get; set; }
        /// <summary>
        /// Прибыль теоритическая за период
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
        public int currentTransactionsSumm { get; set; }
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
                loaded = 0;

                //DateTime date1 = new DateTime(2023, 3, 12);
                DateTime date1 = new DateTime(2023, 5, 5);

                this.item = item;
            }

            public void UpdateItem(DateTime dateBegin, DateTime dateEnd)
            {
                //DateTime date1 = new DateTime(2023, 3, 12);
                DateTime date1 = new DateTime(2023, 5, 5);

                loaded = 0;
                try
                {
                    foreach (Transactions t in item.transactions.Where(transaction => transaction.dateOf >= dateBegin))
                    {
                        loaded += t.summ;
                    }
                }
                catch
                {
                    loaded = 0;
                }

                int d = 0;
                try
                {
                    //foreach (Transactions t in item.transactions.Where(p => p.dateOf == DateTime.Today))
                    foreach (Transactions t in item.transactions.Where(transaction => transaction.dateOf == date1))
                    {
                        d += t.summ;
                    }
                }
                catch { }

                //currentSumm = item.summ / dateEnd.Subtract(DateTime.Today).Days;


                int balanceIncToday = item.summ - loaded;
                int balanceExcToday = balanceIncToday + d;
                currentSumm = (balanceIncToday) / (dateEnd.Subtract(date1).Days + 1);
                dailyBalance = (balanceExcToday) / (dateEnd.Subtract(date1).Days + 1) - d;
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
                //userItems.Add(new UserItem(item, person.Finance.dateBegin, person.Finance.dateEnd));
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
            int itemsSumm=0;
            foreach (ItemPerson i in person.itemPerson)
            {
                itemsSumm+= i.summ;
                if (i.transactions != null)
                {
                    currentTransactions.AddRange(i.transactions.Where(i => i.dateOf >= person.Finance.dateBegin && i.dateOf <= person.Finance.dateEnd));
                }
            }
            foreach (Transactions t in currentTransactions)
            {
                currentTransactionsSumm += t.summ;
            }

            currentProfit = 0;
            if (person.Finance.salary > currentIncome)
            {
                currentProfit = person.Finance.salary - currentTransactionsSumm;
                profit = person.Finance.salary - itemsSumm;
            }
            else
            {
                currentProfit = currentIncome - currentTransactionsSumm;
                profit = currentIncome - itemsSumm;
            }
        }

        public void UpdateIncome(int income)
        {
            currentIncome += income;
            currentProfit = currentIncome - currentTransactionsSumm;
        }

        public UserData() { }
    }


}