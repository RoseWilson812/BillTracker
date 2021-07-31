using BillTracker.Data;
using BillTracker.Models;
using BillTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
           
            List<DisplayBill> allBills = new List<DisplayBill>();
   //         List<DisplayBill> billsAndCategoryName = new List<DisplayBill>();
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Member> saveMember = context.Members
                .Where(m => m.UserId == currentUserId).ToList();

            

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
//                saveMember = context.Members
//                    .Where(m => m.UserId == currentUserId).ToList();

            }
                                 
            
            AddBillViewModel addBillViewModel = new AddBillViewModel();
           

            addBillViewModel.UserId = currentUserId;
            
            addBillViewModel.SaveBills = allBills.OrderBy(displayBill => displayBill.DueDate).ToList(); 
      
            return View(addBillViewModel);
        }

        [HttpGet] 
        public IActionResult AddBill()
        {

      //          List<Category> categories = context.Categorys.OrderBy(category => category.CategoryName).ToList();
      //      List<Category> rawCategories = new List<Category>();
            List<Category> categories = new List<Category>();
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Member> saveMember = context.Members
                .Where(m => m.UserId == currentUserId).ToList();
            List<Category> rawCategories = context.Categorys
                .Where(c => c.UserId == currentUserId)
                .ToList();
//            foreach (MemberCategory rec in memberCategories)
//            {
//                rawCategories.Add(rec.Category);
//            }

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
   //         Member holdMember = context.Members.Find(AddBillViewModel.Member.Id);
            List<Member> saveMember = context.Members
               .Where(m => m.UserId == addBillViewModel.UserId).ToList();
            if (! (addBillViewModel.PaidDate is null))
            {
                if (addBillViewModel.PaymentType is null)
                {
                    ModelState.AddModelError("PaymentType", "Payment Type must be a check #, 'Card' or 'Cash'.");                    
                }
            }
            if (ModelState.IsValid)
            {
                Category holdCategory = context.Categorys.Find(addBillViewModel.CategoryId);

                bill.TaxDeductible = Char.ToUpper(addBillViewModel.TaxDeductible);
                bill.Amount = decimal.Round(bill.Amount, 2);
                context.Bills.Add(bill);
               
//                var holdMemberBills = new MemberBill
//                {
//                    Member = saveMember[0],
//                    Bill = bill
//                };
//                context.MemberBills.Add(holdMemberBills);

/*                 var holdCategoryBill = new MemberCategoryBill
                 {
                        Member = saveMember[0],
                        Category = holdCategory,
                        Bill = bill
                    };
                context.MemberCategoryBills.Add(holdCategoryBill);
*/

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
            //            ClaimsPrincipal currentUser = this.User;
            //            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Bill> editBill = context.Bills
         .Where(b => b.Id == id)
         .ToList();

            List<Category> categories = context.Categorys
                .Where(c => c.UserId == editBill[0].UserId)
                .ToList();
        

            EditBillViewModel editBillViewModel = new EditBillViewModel(
                editBill[0].Id,
                editBill[0].PaymentType,
                editBill[0].DueDate,
                editBill[0].PaidDate,
                editBill[0].Payee,
                editBill[0].Amount,
                editBill[0].CategoryId,
                editBill[0].Memo,
                editBill[0].TaxDeductible,
                editBill[0].UserId,
                categories
                );
            
            ViewBag.message = "";
            editBillViewModel.CreateDropdown();
           
            return View(editBillViewModel);
        }
        [HttpPost]
        public IActionResult EditBill(EditBillViewModel editBillViewModel)
        {
//            bool categoryChangedSw = false;
            Bill oldBill = context.Bills.Find(editBillViewModel.Id);
/*            List<MemberCategoryBill> oldMemberCategoryBills = context.MemberCategoryBills
                   .Where(cb => cb.BillId == editBillViewModel.Id)
                   .Include(cb => cb.Category)
                   .Include(cb => cb.Member)
                   .ToList();
*/
            Category oldCategory = context.Categorys.Find(editBillViewModel.CategoryId);
            //            Member oldMember = oldMemberCategoryBills[0].Member;
            if (!(editBillViewModel.PaidDate is null))
            {
                if (editBillViewModel.PaymentType is null)
                {
                    ModelState.AddModelError("PaymentType", "Payment Type must be a check #, 'Card' or 'Cash'.");
                }
            }

            if (ModelState.IsValid)
            {
                oldBill.PaymentType = editBillViewModel.PaymentType;
                oldBill.DueDate = editBillViewModel.DueDate;
                oldBill.PaidDate = editBillViewModel.PaidDate;
                oldBill.Payee = editBillViewModel.Payee;
                oldBill.Memo = editBillViewModel.Memo;
/*                if (oldBill.CategoryId == editBillViewModel.CategoryId)
                {
                    categoryChangedSw = false;
                }
                else
                {
                    categoryChangedSw = true;
                }
*/
                oldBill.CategoryId = editBillViewModel.CategoryId;
                oldBill.Amount = editBillViewModel.Amount;
                oldBill.TaxDeductible = Char.ToUpper(editBillViewModel.TaxDeductible);
                oldBill.UserId = editBillViewModel.UserId;
                context.Bills.Update(oldBill);

 /*               if (categoryChangedSw == true)
                {
                    context.MemberCategoryBills.Remove(oldMemberCategoryBills[0]);
                    MemberCategoryBill newMemberCategoryBill = new MemberCategoryBill
                    {
                        Member = oldMemberCategoryBills[0].Member,
                        Category = oldCategory,
                        Bill = oldBill
                    };

                    context.MemberCategoryBills.Add(newMemberCategoryBill);
                }
 */
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
         

//            ClaimsPrincipal currentUser = this.User;
//            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Bill> editBill = context.Bills
                    .Where(b => b.Id == id)
                    .ToList();

               List<Category> categories = context.Categorys
                .Where(c => c.UserId == editBill[0].UserId)
                .ToList();

            List<Member> saveMember = context.Members
              .Where(m => m.UserId == editBill[0].UserId)
              .ToList();

            DeleteBillViewModel deleteBillViewModel = new DeleteBillViewModel(
                editBill[0].Id,
                editBill[0].PaymentType,
                editBill[0].DueDate,
                editBill[0].PaidDate,
                editBill[0].Payee,
                editBill[0].Amount,
                editBill[0].CategoryId,
                editBill[0].Memo,
                editBill[0].TaxDeductible,
                editBill[0].UserId,
                saveMember[0],
                categories
               
                );

            ViewBag.message = "";
            deleteBillViewModel.CreateDropdown();

            return View(deleteBillViewModel);
        }

        [HttpPost]
        [Route("/Home/DeleteBill/{id}")]
        public IActionResult DeleteBill(int id, DeleteBillViewModel deleteBillViewModel)
        {
            Bill oldBill = context.Bills.Find(id);
/*            List<MemberCategoryBill> oldMemberCategoryBills = context.MemberCategoryBills
                  .Where(cb => cb.MemberId == DeleteBillViewModel.Member.Id && cb.BillId == id)
                  .Include(cb => cb.Category)
                  .Include(cb => cb.Member)
                  .ToList();
*/
       //     Category oldCategory = context.Categorys.Find(deleteBillViewModel.CategoryId);
       //     Member oldMember = oldCategoryBills[0].Member;

//            List<MemberBill> saveMemberBill = context.MemberBills
//               .Where(mb => mb.MemberId == DeleteBillViewModel.Member.Id && mb.BillId == id).ToList();

//            context.MemberBills.Remove(saveMemberBill[0]);
//           context.MemberCategoryBills.Remove(oldMemberCategoryBills[0]);
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
