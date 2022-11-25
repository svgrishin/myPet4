﻿using Microsoft.AspNetCore.Mvc;
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
            ViewData["count"] = _context.Transactions.Count();
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
                    Person person = new Person(login);
                    _context.Add<Person>(person);
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

        public IActionResult LogonFormShow()
        {
            ViewData["PersonsList"] = new SelectList(_context.Persons, "id","login",0);
            return View();
        }
        
        [HttpPost]
        public IActionResult UserForm(int id)
        {
            _context.user.Person = _context.Persons.Find(id);
            return View("Index");
        }

        public IActionResult ItemsCreate()
        {
            return View();
        }
    }
}