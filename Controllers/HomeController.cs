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
using Microsoft.AspNetCore.Http.Features;

namespace myPet4.Controllers
{
    public class HomeController : Controller
    {
        public static int personIndex;
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

        [HttpGet]
        public IActionResult userCreateForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult userCreateForm(string login, DateOnly DateBegin, DateOnly? DateEnd, string step)
        {
            DateOnly d2 = DateBegin;
            char s;

            switch (step)
            {
                case "Неделя":
                    s = 'd';
                    d2 = d2.AddDays(7);
         
                    break;

                case "Месяц":
                    s = 'm';
                    d2 = d2.AddMonths(1);
                    break;
                        
                case "Год":
                    s = 'y';
                    d2 = d2.AddYears(1);
                    break;

                case "Настраиваемый":
                    s = 'c';
                    d2 = new DateOnly(DateEnd.Value.Year, DateEnd.Value.Month, DateEnd.Value.Day);
                    break;
            }
            

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
            var a = new List<Persons>(_context.Persons.ToList());

            List<Persons> persons = new List<Persons>(_context.Persons.OrderBy(p => p.login).ToList());
            ViewBag.personList = persons;

            return View();
        }
        
        [HttpPost]
        public IActionResult UserForm(int id)
        {
            List<Persons> p3 = _context.Persons.Where(p => p.id == id).Include(p => p.ItemPerson).ToList();
            personIndex = p3.First().id;
            foreach (ItemPerson item in p3.First().ItemPerson)
            {
                item.Transactions = _context.Transactions.Where(p => p.summ > 1000).Where(p => p.item == item.id).ToList();
            }
            ViewBag.personIndex = personIndex;
            return View("userForm");
        }
        
        
        public IActionResult ItemsCreate()
        {
            return View();
        }
    }
}