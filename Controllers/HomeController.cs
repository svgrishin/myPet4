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
using static myPet4.Models.UserData;


namespace myPet4.Controllers
{
    public class HomeController : Controller
    {
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
            currentUser.Person.Finance.person = currentUser.Person;
            return View();
        }

        [HttpPost]
        public IActionResult createFinance(int cash, int credit, int toSave, int salary, DateTime dateBegin, DateTime? dateEnd, string step)
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
                currentUser.Person.Finance = new Finance(currentUser.Person, cash, credit, toSave, salary, dateBegin, d2, s);
                return View("CreateItems");
            }
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
            person.itemPerson = _context.Item.Where(i => i.person == id).Include(t => t.transactions).ToList();
            person.income = _context.Income.Where(i => i.person == id).ToList();          

            currentUser = new UserData(person);
            
            

            return RedirectToAction("UserForm");
        }

        public IActionResult UserForm()
        {
            currentUser = new UserData(currentUser.Person);
            return View("UserForm", currentUser);
        }

        [HttpGet]
        public IActionResult CreateItems()
        {
            return View("CreateItems");
        }

        [HttpPost]
        public IActionResult CreateItems(string nameof, int summ)
        {
            currentUser.Person.itemPerson.Add(new ItemPerson(currentUser.Person, nameof, summ));
            return RedirectToAction();
        }

        public IActionResult saveProfile()
        {
            _context.Add(currentUser.Person);
            _context.Finance.Add(currentUser.Person.Finance);
            _context.Item.Add(currentUser.Person.itemPerson.Last());
            
            _context.SaveChanges();

            return RedirectToAction("UserForm");
        }

        public IActionResult AddTransaction(ItemPerson item, Transactions transaction)
        {
            return View("UserForm");
        }

        public IActionResult DeleteTransaction(int id)
        {
            var t = _context.Transactions.Find(id);
            _context.Transactions.Remove(t);
            var item = _context.Item.Find(t.item);

            _context.SaveChanges();
            return RedirectToAction("TransactionsForm", new { id = item.id });
        }

        public IActionResult TransactionsForm(int id)
        {
            ItemPerson item = _context.Item.Where(m => m.id == id).Include(t => t.transactions).First();
            List<Transactions> transactions = new List<Transactions>(item.transactions);
            ViewBag.Transactions = transactions;
            return View("TransactionsForm");
        }

        [HttpGet]
        public IActionResult EditTransaction(int id)
        {
            Transactions transaction = _context.Transactions.Find(id);
            return View(transaction);
        }

        [HttpPost]
        public IActionResult SaveTransaction(Transactions transaction)
        {
            
            return RedirectToAction("TransactionForm", transaction.item);
        }
    }
}