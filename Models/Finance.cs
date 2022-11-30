using MessagePack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace myPet.Data
{
    public partial class Finance
    {
        //private IQueryable<Finance> finance;

        //public Finance(IQueryable<Finance> finance)
        //{
        //    this.finance = finance;
        //}

        //[ForeignKey("id")]
        public int ID { get; set; }
        public decimal cash { get; set; }
        public decimal credit { get; set; }
        public decimal toSave { get; set; }
        public Nullable<decimal> freeMoney { get; set; }
        public decimal salary { get; set; }

        [ForeignKey("id")]
        public virtual Persons Person { get; set; }
    }
}
