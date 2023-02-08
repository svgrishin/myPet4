﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace myPet4.Models
{
    public partial class Persons
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Persons(string login)
        {
            income = new HashSet<Income>();
            itemPerson = new HashSet<ItemPerson>();
            this.login = login;
        }

        //public Persons(Persons p)
        //{
        //    income = p.income;
        //    itemPerson = p.itemPerson;
        //    login = p.login;
            
        //}

        public Persons()
        {
            
        }

        public int id { get; set; }
        [DisplayName("Имя пользователя")]
        public string login { get; set; }
        public virtual Finance? Finance { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Income?> income { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [DisplayName("Статьи расходов")]
        public virtual ICollection<ItemPerson>? itemPerson { get; set; }
    }
}