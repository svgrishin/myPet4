using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace myPet.Data
{
    public partial class Transactions
    {
        public int id { get; set; }
        public int item { get; set; }
        public System.DateTime dateOf { get; set; }
        public decimal summ { get; set; }
        public bool credit { get; set; }
        public string description { get; set; }

        public virtual ItemPerson ItemPerson { get; set; }
    }
}