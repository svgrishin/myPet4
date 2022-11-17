using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using myPet;
using myPet.Data;

namespace myPet4.Controllers
{
    public class HomeController : Controller
    {
        private readonly myPetContext _context;
        public HomeController(myPetContext context)
        {
            _context=context; 
        }

        
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Transactions.ToListAsync());
            ViewData["count"] = _context.Transactions.Count();
            return View();
        }

        public IActionResult login()
        {
            return View();
        }

        public IActionResult userCreateForm(string login)
        {
            if(ModelState.IsValid)
            {
                return Redirect("/");
            }

            //    if (login != null)
            //    {
            //        Person person = new Person(login);
            //        _context.Persons.Add(person);
            //        _context.SaveChanges();
            //        return View();
            //    }
            //    return View();
            return View(userCreateForm);
        }
    }
}