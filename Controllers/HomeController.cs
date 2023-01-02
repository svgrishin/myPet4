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
                currentPerson.Finance = new Finance(currentPerson.id, cash, credit, toSave, salary, dateBegin, d2, s);
                _context.Finance.Add(currentPerson.Finance);
                _context.SaveChanges();
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
            var a = new List<Persons>(_context.Persons.ToList());

            List<Persons> persons = new List<Persons>(_context.Persons.OrderBy(p => p.login).ToList());
            ViewBag.personList = persons;

            return View();
        }

        [HttpPost]
        public IActionResult LogonForm(int id)
        {
            currentPerson = _context.Persons.Where(p => p.id == id).First();
            currentPerson.Finance=_context.Finance.Where(p=>p.ID== id).First();
            currentPerson.ItemPerson = _context.Item.Where(p => p.person == currentPerson.id).ToList();
            return View("UserForm");
        }

        public IActionResult UserForm()
        {
            //if (currentPerson==null) currentPerson = _context.Persons.Where(p => p.id == id) as Persons;
            Data d = new Data(currentPerson);
            return View("userForm",d);
        }

        [HttpGet]
        public IActionResult CreateItems()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateItems(string nameof, decimal summ)
        {
            currentPerson.ItemPerson.Add(new ItemPerson(currentPerson.id, nameof, summ));
            _context.Item.Add(currentPerson.ItemPerson.Last());
            _context.SaveChanges();
            
            return RedirectToAction();
        }
    }
}