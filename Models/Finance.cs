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
        public DateTime dateBegin { get; set; }
        public DateTime dateEnd { get; set; }
        public char step { get; set; }

        [ForeignKey("id")]
        public virtual Persons Person { get; set; }


        //public Finance(Persons person, int ID, decimal cash, decimal credit, decimal toSave, decimal salary, DateTime dateBegin, DateTime dateEnd, char step)
        public Finance(decimal cash, decimal credit, decimal toSave, decimal salary, DateTime dateBegin, DateTime dateEnd, char step)
        {
            //this.Person= person;
            this.cash = cash; 
            this.credit = credit;
            this.toSave = toSave;
            this.salary = salary;
            this.dateBegin = dateBegin;
            this.dateEnd = dateEnd;
            this.step = step;
        }
    }
}
