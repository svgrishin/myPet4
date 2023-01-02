using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing.Matching;
using myPet.Data;

namespace myPet.Models
{
    public class Data
    {
        public Persons Person { get; set; }

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
                catch {}
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
            items = new List<Item>();
            
            foreach(ItemPerson i in person.ItemPerson)
            {
                this.items.Add(new Item(i, person.Finance.dateBegin, person.Finance.dateEnd));
            }
        }
    }

}