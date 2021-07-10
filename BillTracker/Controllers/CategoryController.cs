using BillTracker.Data;
using BillTracker.Models;
using BillTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BillTracker.Controllers
{
    public class CategoryController : Controller
    {
        private BillDbContext context;

        public CategoryController(BillDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<BillCategory> allBillCategorys = new List<BillCategory>();
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Member> saveMember = context.Members
                .Where(m => m.UserId == currentUserId).ToList();

            if (saveMember.Count > 0)
            {
                List<MemberCategory> allMemberCategorys = context.MemberCategorys
                    .Where(mb => mb.MemberId == saveMember[0].Id)
                    .Include(mb => mb.Category)
                    .ToList();

                foreach (MemberCategory rec in allMemberCategorys)
                {
                    allBillCategorys.Add(rec.Category);
                }
            }
            else
            {
                Member newMember = new Member(currentUserId);
                context.Members.Add(newMember);
                context.SaveChanges();
                saveMember = context.Members
                  .Where(m => m.UserId == currentUserId).ToList();
            }
                AddCategoryViewModel addCategoryViewModel = new AddCategoryViewModel();
                AddCategoryViewModel.Member = saveMember[0];
                AddCategoryViewModel.SaveCategorys = new List<BillCategory>();

                AddCategoryViewModel.SaveCategorys = allBillCategorys.OrderBy(billCategory => billCategory.CategoryName).ToList();
                addCategoryViewModel.CategoryList = AddCategoryViewModel.SaveCategorys.GetRange(0, AddCategoryViewModel.SaveCategorys.Count);
                ViewBag.edit = "";
                return View("AddCategory", addCategoryViewModel);


        }
        [HttpPost]
        public IActionResult AddCategory(BillCategory category, AddCategoryViewModel addCategoryViewModel)
        {
            addCategoryViewModel.CategoryList = AddCategoryViewModel.SaveCategorys.GetRange(0, AddCategoryViewModel.SaveCategorys.Count);
            List<Member> saveMember = context.Members
             .Where(m => m.UserId == AddCategoryViewModel.Member.UserId).ToList();
            if (ModelState.IsValid)
            {
                context.Categorys.Add(category);

                var holdMemberCategory = new MemberCategory
                {
                    Member = saveMember[0],
                    Category = category
                };
                context.MemberCategorys.Add(holdMemberCategory);
                context.SaveChanges();
                ViewBag.edit = "";
                return Redirect("/Category");
            }
            ViewBag.edit = "";
            return View(addCategoryViewModel);
        }


        [HttpGet]
        [Route("/Home/EditCategory/{id}")]
        public IActionResult EditCategory(int id)
        {
            List<BillCategory> allBillCategorys = new List<BillCategory>();
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Member> saveMember = context.Members
                .Where(m => m.UserId == currentUserId).ToList();
            

                List<MemberCategory> allMemberCategorys = context.MemberCategorys
                    .Where(mb => mb.MemberId == saveMember[0].Id)
                    .Include(mb => mb.Category)
                    .ToList();

                foreach (MemberCategory rec in allMemberCategorys)
                {
                    allBillCategorys.Add(rec.Category);
                }
            
                 List<BillCategory> editCategory = context.Categorys
                                .Where(c => c.Id == id)
                                .ToList();
                        EditCategoryViewModel editCategoryViewModel = new EditCategoryViewModel(
                           editCategory[0].Id,
                           editCategory[0].CategoryName
                           
                            );
            EditCategoryViewModel.Member = saveMember[0];
            EditCategoryViewModel.SaveCategorys = allBillCategorys.OrderBy(billCategory => billCategory.CategoryName).ToList();

            editCategoryViewModel.CategoryList = EditCategoryViewModel.SaveCategorys.GetRange(0, EditCategoryViewModel.SaveCategorys.Count);

            
            ViewBag.edit = "Edit";
            return View( editCategoryViewModel);
        }

    
        [HttpPost]
        [Route("/Home/EditCategory/{id}")]
        public IActionResult EditCategory(EditCategoryViewModel editCategoryViewModel)
    {
        editCategoryViewModel.CategoryName = editCategoryViewModel.EditCategoryName;
        BillCategory oldCategory = context.Categorys.Find(editCategoryViewModel.Id);

        if (ModelState.IsValid)
        {
            oldCategory.CategoryName = editCategoryViewModel.CategoryName;
          
            context.Categorys.Update(oldCategory);
            context.SaveChanges();
            
           

            return RedirectToAction("Index", "Category");
        }
        else
        {
            ViewBag.edit = "Edit";
            editCategoryViewModel.CategoryList = EditCategoryViewModel.SaveCategorys.GetRange(0, EditCategoryViewModel.SaveCategorys.Count);
            editCategoryViewModel.EditCategoryName = oldCategory.CategoryName;
            return View(editCategoryViewModel);
        }
    }


        [HttpGet]
        [Route("/Home/DeleteCategory/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            /*           List<BillCategory> allBillCategorys = new List<BillCategory>();
                       ClaimsPrincipal currentUser = this.User;
                       var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                       List<Member> saveMember = context.Members
                           .Where(m => m.UserId == currentUserId).ToList();


                       List<MemberCategory> allMemberCategorys = context.MemberCategorys
                           .Where(mb => mb.MemberId == saveMember[0].Id)
                           .Include(mb => mb.Category)
                           .ToList();

                       foreach (MemberCategory rec in allMemberCategorys)
                       {
                           allBillCategorys.Add(rec.Category);
                       }

                       List<BillCategory> editCategory = context.Categorys
                                      .Where(c => c.Id == id)
                                      .ToList();
            */
            List<BillCategory> allBillCategorys = new List<BillCategory>();
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Member> saveMember = context.Members
               .Where(m => m.UserId == currentUserId).ToList();
           

            List<MemberCategory> memberCategory = context.MemberCategorys
                .Where(mc => mc.MemberId == saveMember[0].Id)
                .Include(mb => mb.Category).ToList();
            foreach (MemberCategory rec in memberCategory)
            {
                allBillCategorys.Add(rec.Category);
            }


           List<MemberBill> saveAllMemberBills = context.MemberBills
                .Where(mr => mr.MemberId == saveMember[0].Id)
                .Include(mb => mb.Bill).ToList();
 /*           List<Bill> billWithCategory = new List<Bill>();
           foreach (MemberBill rec in saveAllMemberBills)
            {
                if (rec.Bill.CategoryId == id)
                {
                    billWithCategory.Add(rec.Bill);
                }
            }
*/
            List<BillCategory> deleteCategory = context.Categorys
              .Where(c => c.Id == id)
              .ToList();

            DeleteCategoryViewModel deleteCategoryViewModel = new DeleteCategoryViewModel(
               deleteCategory[0].Id,
               deleteCategory[0].CategoryName

                );

 /*           if (billWithCategory.Count > 0)
            {
                ModelState.AddModelError("deleteCategoryName", "Category cannot be deleted if it's used in a bill.");

 //               return View(deleteCategoryViewModel);
            }
*/

            DeleteCategoryViewModel.Member = saveMember[0];
            DeleteCategoryViewModel.SaveCategorys = allBillCategorys.OrderBy(billCategory => billCategory.CategoryName).ToList();

            deleteCategoryViewModel.CategoryList = DeleteCategoryViewModel.SaveCategorys.GetRange(0, DeleteCategoryViewModel.SaveCategorys.Count);


            ViewBag.edit = "Delete";
            return View(deleteCategoryViewModel);
        }

        [HttpPost]
        [Route("/Home/DeleteCategory/{id}")]
        public IActionResult DeleteCategory(DeleteCategoryViewModel deleteCategoryViewModel)
        {
           
            BillCategory oldCategory = context.Categorys.Find(deleteCategoryViewModel.Id);
            List<MemberCategory> memberCategory = context.MemberCategorys
                .Where(mc => mc.BillCategoryId == deleteCategoryViewModel.Id &&
                mc.MemberId == DeleteCategoryViewModel.Member.Id).ToList();
      

            List<MemberBill> saveAllMemberBills = context.MemberBills
                .Where(mr => mr.MemberId == DeleteCategoryViewModel.Member.Id)
                .Include(mb => mb.Bill).ToList();

            List<Bill> billWithCategory = new List<Bill>();
            foreach (MemberBill rec in saveAllMemberBills)
            {
                if (rec.Bill.CategoryId == deleteCategoryViewModel.Id)
                {
                    billWithCategory.Add(rec.Bill);
                } 
            }
            if (billWithCategory.Count > 0)
            {
                ModelState.AddModelError("deleteCategoryName", "Category cannot be deleted if it's used in a bill.");
                deleteCategoryViewModel.CategoryList = DeleteCategoryViewModel.SaveCategorys.GetRange(0, DeleteCategoryViewModel.SaveCategorys.Count);
                ViewBag.edit = "Delete";
                return View(deleteCategoryViewModel);
            }

                
            context.Categorys.Remove(oldCategory);
            context.MemberCategorys.Remove(memberCategory[0]);
            context.SaveChanges();
            return RedirectToAction("Index", "Category");

        }
    }
}
