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
        public IActionResult userCreateForm(string login, DateOnly? DateBegin, string? step)
        {
            var d = DateBegin;
            var d2 = step;




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
            //var d1 = 


            List<Persons> p3 = _context.Persons.Where(p => p.id == id).Include(p => p.ItemPerson).ToList();
            foreach (ItemPerson item in p3.First().ItemPerson)
            {
                item.Transactions = _context.Transactions.Where(p => p.summ > 1000).Where(p => p.item == item.id).ToList();
            }
            return View("Index");
        }
        
        
        
        public IActionResult ItemsCreate()
        {
            return View();
        }
    }
}