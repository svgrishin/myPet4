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
        public static Persons currentPerson;
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

        //[HttpGet]
        //public IActionResult userCreateForm()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult userCreateForm(string login, DateOnly DateBegin, DateOnly? DateEnd, string step)
        //{
        //    DateOnly d2 = DateBegin;
        //    char s;

        //    switch (step)
        //    {
        //        case "Неделя":
        //            s = 'd';
        //            d2 = d2.AddDays(7);
         
        //            break;

        //        case "Месяц":
        //            s = 'm';
        //            d2 = d2.AddMonths(1);
        //            break;
                        
        //        case "Год":
        //            s = 'y';
        //            d2 = d2.AddYears(1);
        //            break;

        //        case "Настраиваемый":
        //            s = 'c';
        //            d2 = new DateOnly(DateEnd.Value.Year, DateEnd.Value.Month, DateEnd.Value.Day);
        //            break;
        //    }
            

        //    if (ModelState.IsValid)
        //    { 
        //        var persons = _context.Persons.Where(p => p.login == login);
        //        if (persons.IsNullOrEmpty())
        //        {
        //            Persons person = new Persons(login);
        //            _context.Add<Persons>(person);
        //            _context.SaveChanges();
        //            return View("ItemsCreate");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("UserCreatingErr", "Такой пользователь уже существует");   
        //            return View("userCreateForm");
        //        }
        //     }
        //     else return View("userCreateForm");       
        //}

        [HttpGet]
        public IActionResult createUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult createUser(string login)
        {
            if (ModelState.IsValid)
            {
                var persons = _context.Persons.Where(p => p.login == login);
                if (persons.IsNullOrEmpty())
                {
                    Persons person = new Persons(login);
                    _context.Add<Persons>(person);
                    _context.SaveChanges();
                    currentPerson = person;
                    return View("createFinance");
                }
                else
                {
                    ModelState.AddModelError("UserCreatingErr", "Такой пользователь уже существует");
                    return View("createUser");
                }
            }
            else return View("createUser");
        }

        [HttpGet]
        public IActionResult createFinance()
        {
            return View();
        }

        [HttpPost]
        public IActionResult createFinance(int ID, decimal cash, decimal credit, decimal toSave, decimal salary, DateTime dateBegin, DateTime? dateEnd, string step)
        {
            DateTime d2 = dateBegin;
            char s='c';

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
                    d2 = new DateTime(dateEnd.Value.Year, dateEnd.Value.Month, dateEnd.Value.Day);
                    break;
            }

            if (ModelState.IsValid)
            {
                https://translated.turbopages.org/proxy_u/en-ru.ru.45538a6c-63a8445d-095a8d9d-74722d776562/https/stackoverflow.com/questions/10608488/how-do-insert-row-into-child-table


                //currentPerson.Finance.ID = currentPerson.id;
                //currentPerson.Finance.dateEnd = d2;
                //currentPerson.Finance.dateBegin = dateBegin;
                //currentPerson.Finance.cash = cash;
                //currentPerson.Finance.credit = credit;
                //currentPerson.Finance.salary = salary;

                currentPerson.Finance = new Finance(cash, credit,toSave, salary, dateBegin, d2, s);

                _context.Update(currentPerson);
                _context.SaveChanges();

                //_context.Add<Finance>(currentPerson.Finance);
                _context.SaveChanges();
                return View("Index");//следующая форма
            }
            else return View("createFinance");
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
            //personIndex = p3.First().id;
            //foreach (ItemPerson item in p3.First().ItemPerson)
            //foreach (ItemPerson item in p3.First().ItemPerson)
            foreach (ItemPerson item in currentPerson.ItemPerson)
            {
                item.Transactions = _context.Transactions.Where(p => p.summ > 1000).Where(p => p.item == item.id).ToList();
            }
            //ViewBag.personIndex = personIndex;
            ViewBag.personIndex = currentPerson.id;
            return View("userForm");
        }
        
        
        public IActionResult ItemsCreate()
        {
            return View();
        }
    }
}