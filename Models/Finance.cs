using MessagePack;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace myPet4.Models
{
    [Owned]
    public partial class Finance
    {
        public int ID { get; set; }
        [DisplayName("Текущий баланс:")]
        public int cash { get; set; }
        [DisplayName("Кредитный долг")]
        public int credit { get; set; }
        [DisplayName("Сумма сбережений")]
        public int toSave { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [DisplayName("Свободные деньги")]
        public int? freeMoney { get; private set; }
        [DisplayName("Стабильный доход")]
        public int salary { get; set; }
        [DisplayName("Начало расчётного периода")]
        [Required(ErrorMessage = "Необходимо указать дату начала расчётного периода")]
        public DateTime dateBegin { get; set; }
        [DisplayName("Конец расчётного периода")]
        public DateTime dateEnd { get; set; }
        [DisplayName("Период")]
        [Required(ErrorMessage = "Необходимо указать периодичность")]
        public char step { get; set; }
        [ForeignKey("ID")]
        public virtual Persons Person { get; set; }

        public Finance(int ID, int cash, int credit, int toSave, int salary, DateTime dateBegin, DateTime dateEnd, char step)
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
