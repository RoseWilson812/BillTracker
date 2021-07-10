using BillTracker.Data;
using BillTracker.Models;
using BillTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
            List<Bill> allBills = new List<Bill>();
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Member> saveMember = context.Members
                .Where(m => m.UserId == currentUserId).ToList();
           
            if (saveMember.Count > 0)
            {
                List<MemberBill> allMemberBills = context.MemberBills
                    .Where(mb => mb.MemberId == saveMember[0].Id)
                    .Include(mb => mb.Bill)
                    .ToList();

                foreach (MemberBill rec in allMemberBills)
                {
                    rec.Bill.Amount = decimal.Round(rec.Bill.Amount, 2);
                    allBills.Add(rec.Bill);
                }
            }
            else
            {
                Member  newMember = new Member(currentUserId);
                context.Members.Add(newMember);
                context.SaveChanges();
                saveMember = context.Members
                    .Where(m => m.UserId == currentUserId).ToList();

            }
            /*            List<Bill> bills = context.Bills
                            .Include(e => e.Category)
                            .ToList();
            */
            AddBillViewModel addBillViewModel = new AddBillViewModel();
 //           AddBillViewModel.SaveMemberId = saveMember[0].Id;
            AddBillViewModel.Member = saveMember[0];
            
            addBillViewModel.SaveBills = allBills.OrderBy(bill => bill.DueDate).ToList(); 
      //      addBillViewModel.SaveBills = allBills.GetRange(0, allBills.Count);
            return View(addBillViewModel);
        }

        [HttpGet] 
        public IActionResult AddBill()
        {

            List<BillCategory> categories = context.Categorys.OrderBy(category => category.CategoryName).ToList();
             ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Member> saveMember = context.Members
                .Where(m => m.UserId == currentUserId).ToList();

            if (categories.Count == 0)
            {
                ViewBag.message = "At least 1 Category must be created before adding any bills";
                return View();
            }
            ViewBag.message = "";
            AddBillViewModel addBillViewModel = new AddBillViewModel(categories, saveMember[0]);
           
            return View(addBillViewModel);
          
        }

        [HttpPost]
        public IActionResult AddBill(Bill bill, AddBillViewModel addBillViewModel)
        {
            List<Member> saveMember = context.Members
             .Where(m => m.UserId == AddBillViewModel.Member.UserId).ToList();
            if (ModelState.IsValid)
            {

                bill.TaxDeductible = Char.ToUpper(addBillViewModel.TaxDeductible);
                bill.Amount = decimal.Round(bill.Amount, 2);
                context.Bills.Add(bill);
               
                var holdMemberBills = new MemberBill
                {
                    Member = saveMember[0],
                    Bill = bill
                };
                context.MemberBills.Add(holdMemberBills);

                context.SaveChanges();
                ViewBag.message = "Bill Successfully Added";
                return RedirectToAction("AddBill", "Home");
            }
            else
            {

 //    var errors = ModelState.Select(x => x.Value.Errors)
 //                                          .Where(y => y.Count > 0)
 //                                          .ToList();
  // displays model.state errors               
                ViewBag.message = "";
                addBillViewModel.CreateDropdown();
                return View(addBillViewModel);
            }
        }

        [HttpGet]
        [Route("/Home/EditBill/{id}")]
        public IActionResult EditBill(int id)
        {
            List<BillCategory> categories = context.Categorys.ToList();
            
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                      
            List<Bill> editBill = context.Bills
                    .Where(b => b.Id == id)
                    .ToList();
            EditBillViewModel editBillViewModel = new EditBillViewModel(
                editBill[0].Id,
                editBill[0].DueDate,
                editBill[0].PaidDate,
                editBill[0].Payee,
                editBill[0].Amount,
                editBill[0].CategoryId,
                editBill[0].Memo,
                editBill[0].TaxDeductible,                
                categories
                );

            ViewBag.message = "";
            editBillViewModel.CreateDropdown();
           
            return View(editBillViewModel);
        }
        [HttpPost]
        public IActionResult UpdateBill(EditBillViewModel editBillViewModel)
        {
            Bill oldBill = context.Bills.Find(editBillViewModel.Id);
            
            if (ModelState.IsValid)
            {
                oldBill.DueDate = editBillViewModel.DueDate;
                oldBill.PaidDate = editBillViewModel.PaidDate;
                oldBill.Payee = editBillViewModel.Payee;
                oldBill.Memo = editBillViewModel.Memo;
                oldBill.CategoryId = editBillViewModel.CategoryId;
                oldBill.Amount = editBillViewModel.Amount;
                oldBill.TaxDeductible = Char.ToUpper(editBillViewModel.TaxDeductible);
                context.Bills.Update(oldBill);
                context.SaveChanges();
                ViewBag.message = "";
                editBillViewModel.CreateDropdown();
               
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
            List<BillCategory> categories = context.Categorys.ToList();

            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Member> saveMember = context.Members
                .Where(m => m.UserId == currentUserId).ToList();

            List<Bill> editBill = context.Bills
                    .Where(b => b.Id == id)
                    .ToList();
            DeleteBillViewModel deleteBillViewModel = new DeleteBillViewModel(
                editBill[0].Id,
                editBill[0].DueDate,
                editBill[0].PaidDate,
                editBill[0].Payee,
                editBill[0].Amount,
                editBill[0].CategoryId,
                editBill[0].Memo,
                editBill[0].TaxDeductible,
                categories,
                saveMember[0]
                );

            ViewBag.message = "";
            deleteBillViewModel.CreateDropdown();

            return View(deleteBillViewModel);
        }

        [HttpPost]
        [Route("/Home/DeleteBill/{id}")]
        public IActionResult DeleteRecipe(int id, DeleteBillViewModel deleteBillViewModel)
        {
            Bill oldBill = context.Bills.Find(id);

            List<MemberBill> saveMemberBill = context.MemberBills
                    .Where(mb => mb.MemberId == DeleteBillViewModel.Member.Id && mb.BillId == id).ToList();
            context.Bills.Remove(oldBill);
            context.MemberBills.Remove(saveMemberBill[0]);
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
