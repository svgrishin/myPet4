using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using myPet;
using myPet.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using myPet.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Immutable;

namespace myPet4.Controllers
{
    public class HomeController : Controller
    {
        private readonly myPetContext _context;
        public HomeController(myPetContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult HelloForm()
        {
            return View();
        }

        public IActionResult userCreateForm(string login)
        {
            if (ModelState.IsValid)
            { 
                var persons = _context.Persons.Where(p => p.login == login);
                if (persons.IsNullOrEmpty())
                {
                    Persons person = new Persons(login);
                    _context.Add<Persons>(person);
                    _context.SaveChanges();
                    return View("ItemsCreate");
                }
                else
                {
                    ModelState.AddModelError("UserCreatingErr", "Такой пользователь уже существует");   
                    return View("userCreateForm");
                }
             }
             else return View("userCreateForm");       
        }

        public IActionResult LogonForm()
        {
            ViewData["PersonsList"] = new SelectList(_context.Persons, "id","login",0);
            return View();
        }
        
        [HttpPost]
        public IActionResult UserForm(int id)
        {
            try
            {
                //_context.user = new Data(_context.Persons.Find(id));
                _context.SetCurrentPerson(_context.Persons.Find(id));
            } catch (Exception ex) { }

            //_context.currentPerson.income = new List<income>(_context.Income.Where(p => p.person == id));
            //_context.currentPerson.ItemPerson = new List<ItemPerson>(_context.Item.Where(p => p.person == id));
            //_context.currentPerson.Finance = new Finance(_context.Finance.Where(p => p.ID == id));
            //foreach(var userItem in _context.currentPerson.ItemPerson)
            //{
            //    userItem.Transactions = new List<Transaction>(_context.Transactions.Where(p => p.item == userItem.Id));
            //}

            List<Persons> p = new List<Persons>(_context.Persons.Where(p => p.id == id));
            //List<Persons> p2 = _context.Persons.Where(p => p.id == id).Include(p=>p.items).ToList();
            List<Persons> p3 = _context.Persons.Where(p => p.id == id).Include(p => p.ItemPerson).ToList();
            List<Persons> p3 = _context.Persons.Where(p => p.id == id).(p => p.ItemPerson).ToList();
            //_context.user.Items = new List<Data.personItem>();
            //var items = new List<Item>(_context.Items.Where(p => p.person == id));
            //foreach (var item in items)
            //{
            //    List<Transaction> itemTransactions = new List<Transaction>(_context.Transactions.Where(p => p.item == item.Id));
            //    Data.personItem personItem = new Data.personItem(item, itemTransactions);
            //    _context.user.Items.Add(personItem);
            //    //_context.user.Items.Add(new Data.personItem(item, new List<Transaction>(_context.Transactions.Where(p => p.item == item.Id))));
            //}

            //_context.user.Incomes = new List<Income>(_context.Incomes.Where(p=>p.person == id));


            return View("Index");
        }


        
        
        
        public IActionResult ItemsCreate()
        {
            return View();
        }
    }
}