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
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics.CodeAnalysis;
using NuGet.Configuration;
using NuGet.Versioning;

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

        public IActionResult AddTransaction(int id)
        {
            Transactions newTransaction = new Transactions();
            newTransaction.item = id;
            newTransaction.dateOf = DateTime.Now.Date;
            return View("EditTransaction", newTransaction);
        }

        public IActionResult AddIncome(int id)
        {
            Income newIncome = new Income();
            newIncome.person = id;
            newIncome.dateOf = DateTime.Now.Date;
            return View("EditIncome", newIncome);
        }

        public IActionResult TransactionsForm(int id)
        {
            ItemPerson item = new ItemPerson(_context.Item.Where(m => m.id == id).Include(t => t.transactions.Where(d => d.dateOf >= currentUser.Person.Finance.dateBegin)).First());

            UserItem userItem = currentUser.userItems.Find(userItem => userItem.item.id == item.id);
            userItem.item = item;

            userItem.UpdateItem(currentUser.Person.Finance.dateBegin, currentUser.Person.Finance.dateEnd);

            return View(userItem);
        }

        public IActionResult IncomeForm()
        {
            return View(currentUser);
        }

        public IActionResult EditTransaction(int transactionId)
        {
            Transactions transaction = _context.Transactions.Find(transactionId);
            return View(transaction);
        }
        public IActionResult EditIncome(int incomeId)
        {
            Income income = currentUser.Person.income.Where(income => income.id == incomeId).First();
            return View(income);
        }

        [HttpPost]
        public IActionResult SaveTransaction(Transactions newTransaction)
        {
            UserItem userItem = currentUser.userItems.Where(m => m.item.id == newTransaction.item).First();

            Finance finance = _context.Finance.Find(currentUser.Person.id);

            Transactions oldTransaction = null;
            try
            {
                oldTransaction = userItem.item.transactions.Where(m => m.id == newTransaction.id).First();
            }
            catch
            { }


            if (oldTransaction != null)
            {
                //Finance f = currentUser.DeleteExpence(oldTransaction);
                //finance.cash = f.cash;
                //finance.credit = f.credit;

                oldTransaction = _context.Transactions.Find(oldTransaction.id);
                _context.Transactions.Remove(oldTransaction);
            }

            Finance f = currentUser.AddExpence(newTransaction);
            finance.cash = f.cash;
            finance.credit = f.credit;

            _context.Finance.Update(finance);
            _context.Transactions.Add(newTransaction);


            //if (oldTransaction != null)
            //{
            //    _context.Transactions.Update(newTransaction);

            //    userItem.item.transactions.Remove(oldTransaction);

            //    switch (oldTransaction.credit)
            //    {
            //        case true:
            //            currentUser.Person.Finance.credit -= oldTransaction.summ;
            //            finance.credit = currentUser.Person.Finance.credit;



            //            break;
            //        case false:
            //            currentUser.Person.Finance.cash += oldTransaction.summ;
            //            finance.cash = currentUser.Person.Finance.cash;

            //            break;
            //    }
            //    oldTransaction.summ *= (-1);
            //    //currentUser.UpdateProfit(oldTransaction);
            //}
            //userItem.item.transactions.Add(newTransaction);


            //userItem.UpdateItem(currentUser.Person.Finance.dateBegin, currentUser.Person.Finance.dateEnd);

            //switch (newTransaction.credit)
            //{
            //    case true:
            //        currentUser.Person.Finance.credit += newTransaction.summ;
            //        finance.credit = currentUser.Person.Finance.credit;

            //        break;
            //    case false:
            //        currentUser.Person.Finance.cash -= newTransaction.summ;
            //        finance.cash = currentUser.Person.Finance.cash;

            //        break;
            //}

            ////currentUser.UpdateProfit(newTransaction);

            _context.SaveChanges();

            currentUser.Person.Finance = _context.Finance.Find(currentUser.Person.id);

            return View("TransactionsForm", userItem);

            //_context.Transactions.Update(newTransaction);

            //UserItem userItem = currentUser.userItems.Where(m => m.item.id == newTransaction.item).First();

            //Transactions oldTransaction = null;
            //try
            //{
            //    oldTransaction = userItem.item.transactions.Where(m => m.id == newTransaction.id).First();
            //}
            //catch
            //{ }

            //Finance finance = _context.Finance.Find(currentUser.Person.id);

            //if (oldTransaction != null)
            //{
            //    userItem.item.transactions.Remove(oldTransaction);

            //    switch (oldTransaction.credit)
            //    {
            //        case true:
            //            currentUser.Person.Finance.credit -= oldTransaction.summ;
            //            finance.credit = currentUser.Person.Finance.credit;



            //            break;
            //        case false:
            //            currentUser.Person.Finance.cash += oldTransaction.summ;
            //            finance.cash = currentUser.Person.Finance.cash;

            //            break;
            //    }
            //    oldTransaction.summ *= (-1);
            //    //currentUser.UpdateProfit(oldTransaction);
            //}
            //userItem.item.transactions.Add(newTransaction);


            //userItem.UpdateItem(currentUser.Person.Finance.dateBegin, currentUser.Person.Finance.dateEnd);

            //switch (newTransaction.credit)
            //{
            //    case true:
            //        currentUser.Person.Finance.credit += newTransaction.summ;
            //        finance.credit = currentUser.Person.Finance.credit;

            //        break;
            //    case false:
            //        currentUser.Person.Finance.cash -= newTransaction.summ;
            //        finance.cash = currentUser.Person.Finance.cash;

            //        break;
            //}

            ////currentUser.UpdateProfit(newTransaction);

            //_context.SaveChanges();

            //return View("TransactionsForm", userItem);
        }

        [HttpPost]
        public IActionResult SaveIncome(Income newIncome)
        {
            newIncome.person = currentUser.Person.id;

            Finance finance = _context.Finance.Find(currentUser.Person.id);

            Income oldIncome = null;
            try
            {
                oldIncome = currentUser.Person.income.Where(m => m.id == newIncome.id).First();
            }
            catch
            { }

            if (oldIncome == null)
            {
                finance.cash = currentUser.AddIncome(newIncome).cash;
                _context.Income.Add(newIncome);
                currentUser.Person.income.Add(newIncome);


                //finance.cash += newIncome.summ;
                //_context.Finance.Update(finance);

                //currentUser.Person.income.Add(newIncome);
                //currentUser.Person.Finance.cash += newIncome.summ;

                //currentUser.UpdateProfit(newIncome);
            }
            else
            {
                finance.cash = currentUser.DeleteIncome(oldIncome).cash;
                finance.cash = currentUser.AddIncome(newIncome).cash;
                _context.Income.Update(newIncome);
                currentUser.Person.income.Remove(oldIncome);
                currentUser.Person.income.Add(newIncome);


                //int summ = newIncome.summ - oldIncome.summ;
                //currentUser.Person.Finance.cash += summ;
                //_context.Income.Update(newIncome);

                //finance.cash += summ;
                //_context.Finance.Update(finance);

                //currentUser.Person.income.Remove(oldIncome);
                //currentUser.Person.income.Add(newIncome);

                //newIncome.summ = summ;
                //currentUser.UpdateProfit(newIncome);

                //currentUser.Person.income = new List<Income>(currentUser.Person.income.OrderBy(incomeList => incomeList.dateOf));

            }


            _context.Finance.Update(finance);

            _context.SaveChanges();

            currentUser.Person.Finance = _context.Finance.Find(currentUser.Person.id);

            //return RedirectToAction("IncomeForm");
            return View("IncomeForm", currentUser);
        }

        public IActionResult DeleteTransaction(int transactionId)
        {
            Transactions transaction = _context.Transactions.Find(transactionId);
            _context.Transactions.Remove(transaction);
            
            UserItem userItem = currentUser.userItems.Where(m => m.item.id == transaction.item).First();

            Finance finance = _context.Finance.Find(currentUser.Person.id);

            transaction = userItem.item.transactions.Where(t => t.id == transaction.id).First();

            //if (transaction.credit)
            //{
            //    currentUser.Person.Finance.credit -= transaction.summ;
            //    finance.credit = currentUser.Person.Finance.credit;
            //}
            //else
            //{
            //    currentUser.Person.Finance.cash += transaction.summ;
            //    finance.cash = currentUser.Person.Finance.cash;
            //}
            
            userItem.item.transactions.Remove(transaction);

            Finance f = currentUser.DeleteExpence(transaction);
            finance.cash = f.cash;
            finance.credit = f.credit;

            _context.Finance.Update(finance);

            _context.SaveChanges();
            currentUser.Person.Finance = _context.Finance.Find(currentUser.Person.id);

            //currentUser.currentProfit -= transaction.summ;
            //currentUser.UpdateProfit(transaction);

            return RedirectToAction("TransactionsForm", new { id = transaction.item });

            //Transactions transaction = _context.Transactions.Find(transactionId);

            //_context.Transactions.Remove(transaction);

            //Finance finance = _context.Finance.Find(currentUser.Person.id);
            //switch (transaction.credit)
            //{
            //    case true:
            //        currentUser.Person.Finance.credit -= transaction.summ;
            //        finance.credit = currentUser.Person.Finance.credit;
            //        break;
            //    case false:
            //        currentUser.Person.Finance.cash += transaction.summ;
            //        finance.cash = currentUser.Person.Finance.cash;
            //        break;
            //}



            //_context.Finance.Update(finance);

            //_context.SaveChanges();
            //currentUser.Person.Finance = _context.Finance.Find(currentUser.Person.id);

            ////currentUser.currentProfit -= transaction.summ;
            ////currentUser.UpdateProfit(transaction);

            //return RedirectToAction("TransactionsForm", new { id = transaction.item });
        }

        public IActionResult DeleteIncome(int incomeId)
        {
            Income income = currentUser.Person.income.Where(i => i.id == incomeId).FirstOrDefault();

            Finance finance = _context.Finance.Find(currentUser.Person.id);

            finance.cash = currentUser.DeleteIncome(income).cash;

            //_context.Finance.Find(currentUser.Person.id).cash-=income.summ;

            //income.summ *= (-1);

            //if (currentUser.currentIncome > currentUser.Person.Finance.salary)
            //{
            //    currentUser.currentIncome -= income.summ;
            //}


            //_context.SaveChanges();

            //currentUser.Person.Finance = _context.Finance.Find(currentUser.Person.id);

            currentUser.Person.income.Remove(income);

            _context.Income.Remove(_context.Income.Find(incomeId));
            _context.Finance.Update(finance);

            _context.SaveChanges();

            currentUser.Person.Finance = _context.Finance.Find(currentUser.Person.id);


            return View("IncomeForm", currentUser);
        }

        [HttpGet]
        public IActionResult TopView()
        {
            return View(currentUser);
        }
    }
}