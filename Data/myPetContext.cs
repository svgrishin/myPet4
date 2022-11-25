using Microsoft.EntityFrameworkCore;
using myPet.Data;
using myPet.Models;
using System.Collections.Generic;
using System.Configuration;

namespace myPet
{
    public class myPetContext : DbContext
    {
        public myPetContext(DbContextOptions<myPetContext> options) : base(options)
        { }
        
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet <Person>Persons { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet <Finance>Finances { get; set; }
        
        public Models.Data user { get; set; } = new Models.Data();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Income>().ToTable("income");
            modelBuilder.Entity<Transaction>().ToTable("transactions");
            modelBuilder.Entity<Item>().ToTable("items");
            modelBuilder.Entity<Person>().ToTable("Persons");
            modelBuilder.Entity<Finance>().ToTable("Finance");
            //user = new Person();
        }
    }
}