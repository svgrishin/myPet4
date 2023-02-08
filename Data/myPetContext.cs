using Microsoft.EntityFrameworkCore;
using myPet4.Models;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Configuration;

namespace myPet4.Data
{
    public class myPetContext : DbContext
    {
        public myPetContext(DbContextOptions<myPetContext> options) : base(options)
        { }

        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Persons> Persons { get; set; }
        public DbSet<ItemPerson> Item { get; set; }
        public DbSet<Income> Income { get; set; }
        public DbSet<Finance> Finance { get; set; }
        //public Persons currentPerson { get; set; }
        //public void SetCurrentPerson(Persons person)
        //{
        //    currentPerson = person;
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Income>().ToTable("income");
            modelBuilder.Entity<Transactions>().ToTable("Transactions");
            modelBuilder.Entity<ItemPerson>().ToTable("ItemPerson");
            modelBuilder.Entity<Finance>().ToTable("Finance");

            modelBuilder.Entity<Persons>(b =>
            {
                b.HasKey(e => e.id);

                b.ToTable("Persons");
            });
        }
    }
}