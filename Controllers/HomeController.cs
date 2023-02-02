using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Http.Features;
using myPet4.Models;
using myPet4.Data;


namespace myPet4.Controllers
{
    public class HomeController : Controller
    {
        //public static Persons currentPerson;
        public static UserData currentUser = new UserData();
        private readonly myPetContext _context;
        public HomeController(myPetContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

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
                    //currentPerson = person;
                    currentUser.Person = person;
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
            currentUser.Person.Finance = new Finance();
            return View();
        }

        [HttpPost]
        public IActionResult createFinance(int ID, int cash, int credit, int toSave, int salary, DateTime dateBegin, DateTime? dateEnd, string step)
        //public IActionResult createFinance(string step)
        {
            DateTime d2 = dateBegin;
            char s = 'c';

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
                ////currentPerson.Finance = new Finance(currentPerson.id, cash, credit, toSave, salary, dateBegin, d2, s);
                currentUser.Person.Finance = new Finance(currentUser.Person.id, cash, credit, toSave, salary, dateBegin, d2, s);

                return View("CreateItems");
            }
            else
            {
                ModelState.AddModelError("UserCreatingErr", "Ошибка заполнения полей");
                return View("createFinance");
            }
        }

        [HttpGet]
        public IActionResult LogonForm()
        {
            List<Persons> persons = new List<Persons>(_context.Persons.OrderBy(p => p.login).ToList());
            ViewBag.personList = persons;
            ViewData["firstPersonName"] = persons.First().login;
            return View();
        }

        [HttpPost]
        public IActionResult LogonForm(int id)
        {
            Persons person = _context.Persons.Where(p => p.id == id).Include(f => f.Finance).First();
            person.ItemPerson = _context.Item.Where(i => i.person == id).Include(t => t.Transactions).ToList();
            person.income = _context.Income.Where(i => i.person == id).ToList();

            //currentPerson = _context.Persons.Where(p => p.id == id).Include(f => f.Finance).First();
            //currentPerson.ItemPerson = _context.Item.Where(i => i.person == currentPerson.id).Include(t => t.Transactions).ToList();
            //currentPerson.income = _context.Income.Where(i => i.person == currentPerson.id).ToList();

            

            currentUser = new UserData(person);
            
            

            return RedirectToAction("UserForm");
        }

        public IActionResult UserForm()
        {
            //UserData d = new UserData(currentPerson);
            currentUser = new UserData(currentUser.Person);
            return View("UserForm", currentUser);
        }

        [HttpGet]
        public IActionResult CreateItems()
        {
            //if (currentUser.Person.ItemPerson.IsNullOrEmpty()) currentUser.Person.ItemPerson = new List<ItemPerson>();
            //currentUser.Person.ItemPerson.Add(new ItemPerson());
            return View("CreateItems");
        }

        [HttpPost]
        public IActionResult CreateItems(string nameof, int summ)
        {
            ////currentPerson.ItemPerson.Add(new ItemPerson(currentPerson.id, nameof, summ));
            //currentUser.Person.ItemPerson.Add(new ItemPerson(currentUser.Person.id, nameof, summ));
            currentUser.Person.ItemPerson.Add(new ItemPerson(currentUser.Person.id, nameof, summ));
            return RedirectToAction();
        }

        public IActionResult saveProfile()
        {
            //_context.Add(currentPerson);
            //_context.Finance.Add(currentPerson.Finance);
            //_context.Item.Add(currentPerson.ItemPerson.Last());

            _context.Add(currentUser.Person);
            _context.Finance.Add(currentUser.Person.Finance);
            _context.Item.Add(currentUser.Person.ItemPerson.Last());
            
            _context.SaveChanges();

            return RedirectToAction("UserForm");
        }

        public IActionResult AddTransaction(ItemPerson item, Transactions transaction)
        {
            return View("UserForm");
        }
    }
}