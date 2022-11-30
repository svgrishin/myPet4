using Microsoft.EntityFrameworkCore;
using myPet.Data;
using myPet.Models;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Configuration;

namespace myPet
{
    public class myPetContext : DbContext
    {
        public myPetContext(DbContextOptions<myPetContext> options) : base(options)
        { }

        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<Persons>Persons { get; set; }
        public DbSet<ItemPerson> Item { get; set; }
        public DbSet<income> Income { get; set; }
        public DbSet<Finance>Finance { get; set; }
        public Persons currentPerson { get; set; }
        public void SetCurrentPerson(Persons person)
        {
            currentPerson = person;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<income>().ToTable("income");
            modelBuilder.Entity<Transactions>().ToTable("transactions");
            modelBuilder.Entity<ItemPerson>().ToTable("ItemPerson");
            modelBuilder.Entity<Persons>(b =>
            {
                b.HasKey(e => e.id);
                b.OwnsOne(e => e.Finance, md =>
                {
                    md.ToTable("Finance");
                });
                b.ToTable("Persons");
            });
        }
    }
}