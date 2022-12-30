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
        public decimal savedMony { get; set; }
        public decimal toSave { get; set; }
        public class Item
        {
            public ItemPerson item { get; set; }
            public decimal currentSumm { get; set; }
            public decimal perDay { get; set; }
            public int percentOf { get; set; }
            public int loaded { get; set; }

            public Item(ItemPerson item, DateTime dateEnd) {
                this.item = item;
                currentSumm = item.summ / dateEnd.CompareTo(DateTime.Today);

            }
        public Item[] items { get; set; }


    }
}