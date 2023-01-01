using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing.Matching;
using myPet.Data;

namespace myPet.Models
{
    public class Data
    {
        public Persons Person { get; set; }
        public DateTime dateBegin { get; set; }
        public DateTime dateEnd { get; set; }
        public decimal cash { get; set; }
        public decimal freeMoney { get; set; }
        public decimal credit { get; set; }
        //public decimal savedMony { get; set; }
        public decimal toSave { get; set; }
        public List<Item> items { get; set; }
        public class Item
        {
            public ItemPerson item { get; set; }
            public decimal currentSumm { get; set; }
            public decimal daySumm { get; set; }
            public decimal spentByDay { get; set; }
            public int percentOf { get; set; }
            public int loaded { get; set; }
            public Item(ItemPerson item, DateTime dateBegin, DateTime dateEnd)
            {
                this.item = item;
                currentSumm = item.summ / dateEnd.CompareTo(DateTime.Today);

                decimal d = 0;

                foreach (Transactions t in item.Transactions.Where(p => p.dateOf == DateTime.Today))
                {
                    d += t.summ;
                }
                spentByDay = currentSumm - d;

                decimal p = 0;
                foreach (Transactions t in item.Transactions.Where(p => p.dateOf >= dateBegin))
                {
                    p += t.summ;
                }
                percentOf = (int)(p / item.summ);
            }            
        }
        public Data(Persons person, DateTime dateBegin, DateTime dateEnd, decimal cash, decimal credit, decimal toSave, ICollection<ItemPerson> items)
        {
            Person = person;
            this.dateBegin = dateBegin;
            this.dateEnd = dateEnd;
            this.cash = cash;
            //this.freeMoney = freeMoney;
            this.credit = credit;
            //this.savedMony = savedMony;
            this.toSave = toSave;
            
            foreach(ItemPerson i in items)
            {
                this.items.Add(new Item(i, dateBegin, dateEnd));
            }
        }
        public Data()
        {

        }
    }

}