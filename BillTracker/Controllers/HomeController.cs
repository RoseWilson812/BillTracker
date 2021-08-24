using BillTracker.Data;
using BillTracker.Models;
using BillTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;


namespace BillTracker.Controllers
{
    public class HomeController : Controller
    {
        private BillDbContext context;

        public HomeController(BillDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        [Authorize]
        public IActionResult Index()
        {
 // Get current logged in userId and see if it's in the Member table           
            List<DisplayBill> allBills = new List<DisplayBill>();
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Member> saveMember = context.Members
                .Where(m => m.UserId == currentUserId).ToList();

            
// If the current logged in userId is not in the Member table(newly registered user), add it.
// Otherwise, display all the bills for the current userId.
            if (saveMember.Count > 0)
            {
                List<Bill> allMemberBills = context.Bills
                    .Where(b => b.UserId == currentUserId)
                    .ToList();
                
                foreach (Bill rec in allMemberBills)
                { 
                    Category findCategory = context.Categorys.Find(rec.CategoryId);
                    rec.Amount = decimal.Round(rec.Amount, 2);
                    DisplayBill newDisplayBill = new DisplayBill(rec.Id, rec.PaymentType, rec.DueDate,
                        rec.PaidDate, rec.Payee, rec.Amount, rec.CategoryId,
                        rec.Memo, rec.TaxDeductible, rec.UserId, findCategory.CategoryName);
                        
                    allBills.Add(newDisplayBill);
                   
                }
            }
            else
            {
                Member  newMember = new Member(currentUserId);
                context.Members.Add(newMember);
                context.SaveChanges();

            }
                                 
            
            AddBillViewModel addBillViewModel = new AddBillViewModel();
           

            addBillViewModel.UserId = currentUserId;
            
            addBillViewModel.SaveBills = allBills.OrderBy(displayBill => displayBill.DueDate).ToList(); 
      
            return View(addBillViewModel);
        }

        [HttpGet] 
        public IActionResult AddBill()
        {

// Get the current userId, all the categories for that user and display a blank Add Bill page. 
            List<Category> categories = new List<Category>();
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Category> rawCategories = context.Categorys
                .Where(c => c.UserId == currentUserId)
                .ToList();


            if (rawCategories.Count == 0)
            {
                ViewBag.message = "At least 1 Category must be created before adding any bills";
                return View();
            }
            ViewBag.message = "";
            categories = rawCategories.OrderBy(c => c.CategoryName).ToList();
            AddBillViewModel addBillViewModel = new AddBillViewModel(categories, currentUserId);
           
            return View(addBillViewModel);
          
        }

        [HttpPost]
        public IActionResult AddBill(Bill bill, AddBillViewModel addBillViewModel)
        {
 // If a PaidDate is entered, the PaymentType must also be entered.
 // If PaidDate is not entered, PaymentType must be blank. 
            if (! (addBillViewModel.PaidDate is null))
            {
                if (addBillViewModel.PaymentType is null)
                {
                    ModelState.AddModelError("PaymentType", "Payment Type must be a check #, 'Card' or 'Cash'.");                    
                }
                else
                {
                    string enteredPaymentType = addBillViewModel.PaymentType.Trim().ToLower();
                    if (enteredPaymentType == "card" || 
                        enteredPaymentType == "cash" ||
                        enteredPaymentType.All(char.IsDigit))
                    {
                        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
// Capitalized the first letter in payment type if it's == card or cash.
                        string upperPaymentType = textInfo.ToTitleCase(enteredPaymentType);
                        bill.PaymentType = upperPaymentType;
                        addBillViewModel.PaymentType = upperPaymentType;
                    }
                    else
                    {
                        ModelState.AddModelError("PaymentType", "Payment Type must be a check #, 'Card' or 'Cash'.");
                    }
                }
            }
            else
            {
                if (!(addBillViewModel.PaymentType is null))
                {
                    ModelState.AddModelError("PaymentType", "Payment Type must be blank if Paid Date is blank.");
                }
            }
            if (ModelState.IsValid)
            {
                if (addBillViewModel.PaymentType == " ")
                {
                    bill.PaymentType = null;
                }
               
                bill.TaxDeductible = Char.ToUpper(addBillViewModel.TaxDeductible);
                bill.Amount = decimal.Round(bill.Amount, 2);
                context.Bills.Add(bill);

                context.SaveChanges();
             
                return RedirectToAction("AddBill", "Home");
            }
            else
            {
// displays model.state errors:
//    var errors = ModelState.Select(x => x.Value.Errors)
//                                          .Where(y => y.Count > 0)
//                                          .ToList();

                ViewBag.message = "";
                addBillViewModel.CreateDropdown();
                return View(addBillViewModel);
            }
        }

        [HttpGet]
        [Route("/Home/EditBill/{id}")]
        public IActionResult EditBill(int id)
        {
           
            Bill editBill = context.Bills.Find(id);

            List<Category> categories = context.Categorys
                .Where(c => c.UserId == editBill.UserId)
                .ToList();
        

            EditBillViewModel editBillViewModel = new EditBillViewModel(
                editBill.Id,
                editBill.PaymentType,
                editBill.DueDate,
                editBill.PaidDate,
                editBill.Payee,
                editBill.Amount,
                editBill.CategoryId,
                editBill.Memo,
                editBill.TaxDeductible,
                editBill.UserId,
                categories
                );
            
            ViewBag.message = "";
            editBillViewModel.CreateDropdown();
           
            return View(editBillViewModel);
        }
        [HttpPost]
        public IActionResult EditBill(EditBillViewModel editBillViewModel)
        {

            Bill oldBill = context.Bills.Find(editBillViewModel.Id);
            Category oldCategory = context.Categorys.Find(editBillViewModel.CategoryId);
            if (!(editBillViewModel.PaymentType is null))
            {
                string upperPaymentType = "";
                upperPaymentType = editBillViewModel.PaymentType.Trim().ToLower();
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

                upperPaymentType = textInfo.ToTitleCase(upperPaymentType);

                editBillViewModel.PaymentType = upperPaymentType;
            }

// If a PaidDate is entered, the PaymentType must also be entered.
// If PaidDate is not entered, PaymentType must be blank.
            if (!(editBillViewModel.PaidDate is null))
            {
                if (editBillViewModel.PaymentType is null)
                {
                    ModelState.AddModelError("PaymentType", "Payment Type must be a check #, 'Card' or 'Cash'.");
                }
                else
                {
                    string enteredPaymentType = editBillViewModel.PaymentType.ToLower();
                    if (enteredPaymentType == "card" ||
                        enteredPaymentType == "cash" ||
                        enteredPaymentType.All(char.IsDigit))
                    {
                        ;
                    }
                    else
                    {
                        ModelState.AddModelError("PaymentType", "Payment Type must be a check #, 'Card' or 'Cash'.");
                    }
                }
            }
            else
            {
                if (!(editBillViewModel.PaymentType is null))
                {
                    ModelState.AddModelError("PaymentType", "Payment Type must be blank if Paid Date is blank.");
                }
            }

            if (ModelState.IsValid)
            {
                oldBill.PaymentType = editBillViewModel.PaymentType;
                oldBill.DueDate = editBillViewModel.DueDate;
                oldBill.PaidDate = editBillViewModel.PaidDate;
                oldBill.Payee = editBillViewModel.Payee;
                oldBill.Memo = editBillViewModel.Memo;
                oldBill.CategoryId = editBillViewModel.CategoryId;
                oldBill.Amount = editBillViewModel.Amount;
                oldBill.TaxDeductible = Char.ToUpper(editBillViewModel.TaxDeductible);
                oldBill.UserId = editBillViewModel.UserId;
                context.Bills.Update(oldBill);
                context.SaveChanges();
                ViewBag.message = "";
               
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.message = "";
                editBillViewModel.CreateDropdown();
                return View(editBillViewModel);
            }
        }

        [HttpGet]
        [Route("/Home/DeleteBill/{id}")]
        public IActionResult DeleteBill(int id)
        {
            Bill editBill = context.Bills.Find(id);
// prevents error when using back arrow
            if (editBill == null )
            {
                return RedirectToAction("Index", "Home");
            }
              List<Category> categories = context.Categorys
                .Where(c => c.UserId == editBill.UserId)
                .ToList();

            DeleteBillViewModel deleteBillViewModel = new DeleteBillViewModel(
                editBill.Id,
                editBill.PaymentType,
                editBill.DueDate,
                editBill.PaidDate,
                editBill.Payee,
                editBill.Amount,
                editBill.CategoryId,
                editBill.Memo,
                editBill.TaxDeductible,
                editBill.UserId,
                categories
                );

            return View(deleteBillViewModel);
        }

        [HttpPost]
        [Route("/Home/DeleteBill/{id}")]
        public IActionResult DeleteBill(int id, DeleteBillViewModel deleteBillViewModel)
        {
            Bill oldBill = context.Bills.Find(id);

            context.Bills.Remove(oldBill);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
