using MessagePack;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace myPet.Data
{
    [Owned]
    public partial class Finance
    {
        public int ID { get; set; }
        public decimal cash { get; set; }
        public decimal credit { get; set; }
        public decimal toSave { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Nullable<decimal> freeMoney { get; private set; }
        public decimal salary { get; set; }
        public DateTime dateBegin { get; set; }
        public DateTime dateEnd { get; set; }
        public char step { get; set; }

        [ForeignKey("ID")]
        public virtual Persons Person { get; set; }


        //public Finance(Persons person, int ID, decimal cash, decimal credit, decimal toSave, decimal salary, DateTime dateBegin, DateTime dateEnd, char step)
        public Finance(int ID, decimal cash, decimal credit, decimal toSave, decimal salary, DateTime dateBegin, DateTime dateEnd, char step)
        {
            this.ID = ID;
            this.cash = cash; 
            this.credit = credit;
            this.toSave = toSave;
            this.salary = salary;
            this.dateBegin = dateBegin;
            this.dateEnd = dateEnd;
            this.step = step;
        }

        public Finance() { }
    }
}
