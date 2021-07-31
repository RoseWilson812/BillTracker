﻿using BillTracker.Data;
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
//            List<Category> allBillCategorys = new List<Category>();
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Member> saveMember = context.Members
                .Where(m => m.UserId == currentUserId).ToList();
            List<Category> allBillCategorys = context.Categorys
                   .Where(c => c.UserId == currentUserId)
                   .ToList();
            if (saveMember.Count == 0)
            {
               // List<Category> allBillCategorys = context.Categorys
                //    .Where(c => c.UserId == currentUserId)
                //    .ToList();

               
   //         }
   //         else
   //         {
                Member newMember = new Member(currentUserId);
                context.Members.Add(newMember);
                context.SaveChanges();
                //List<Category> allBillCategorys = new List<Category>();
                //                saveMember = context.Members
                //                  .Where(m => m.UserId == currentUserId).ToList();
            }
                AddCategoryViewModel addCategoryViewModel = new AddCategoryViewModel();
                addCategoryViewModel.UserId = currentUserId;
                AddCategoryViewModel.SaveCategorys = new List<Category>();

                AddCategoryViewModel.SaveCategorys = allBillCategorys.OrderBy(category => category.CategoryName).ToList();
                addCategoryViewModel.CategoryList = AddCategoryViewModel.SaveCategorys.GetRange(0, AddCategoryViewModel.SaveCategorys.Count);
                ViewBag.edit = "";
                return View("AddCategory", addCategoryViewModel);


        }
        [HttpPost]
        public IActionResult AddCategory(Category category, AddCategoryViewModel addCategoryViewModel)
        {
            addCategoryViewModel.CategoryList = AddCategoryViewModel.SaveCategorys.GetRange(0, AddCategoryViewModel.SaveCategorys.Count);
 //           List<Member> saveMember = context.Members
//             .Where(m => m.UserId == AddCategoryViewModel.Member.UserId).ToList();
            if (ModelState.IsValid)
            {
                context.Categorys.Add(category);

 //               var holdMemberCategory = new MemberCategory
//                {
//                    Member = saveMember[0],
//                    Category = category
//                };
//                context.MemberCategorys.Add(holdMemberCategory);
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
//            List<Category> allBillCategorys = new List<Category>();
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
//              List<Member> saveMember = context.Members
//                .Where(m => m.UserId == currentUserId).ToList();
            

                List<Category> allBillCategorys = context.Categorys
                    .Where(c => c.UserId == currentUserId)
                    .ToList();

 //               foreach (MemberCategory rec in allMemberCategorys)
 //               {
 //                   allBillCategorys.Add(rec.Category);
 //               }
            
                 List<Category> editCategory = context.Categorys
                                .Where(c => c.Id == id)
                                .ToList();
                        EditCategoryViewModel editCategoryViewModel = new EditCategoryViewModel(
                           editCategory[0].Id,
                           editCategory[0].CategoryName,
                           editCategory[0].UserId
                            );
 //           EditCategoryViewModel.UserId = currentUserId;
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
        Category oldCategory = context.Categorys.Find(editCategoryViewModel.Id);

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

            //           List<Category> allBillCategorys = new List<Category>();
            //            ClaimsPrincipal currentUser = this.User;
            //           var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            //            List<Member> saveMember = context.Members
            //               .Where(m => m.UserId == currentUserId).ToList();

            List<Category> deleteCategory = context.Categorys
             .Where(c => c.Id == id)
              .ToList();

            List<Category> allBillCategorys = context.Categorys
                .Where(c => c.UserId == deleteCategory[0].UserId)
                .ToList();
//            foreach (MemberCategory rec in memberCategory)
//            {
//                allBillCategorys.Add(rec.Category);
//            }


//           List<MemberBill> saveAllMemberBills = context.MemberBills
//                .Where(mr => mr.MemberId == saveMember[0].Id)
 //               .Include(mb => mb.Bill).ToList();



            DeleteCategoryViewModel deleteCategoryViewModel = new DeleteCategoryViewModel(
               deleteCategory[0].Id,
               deleteCategory[0].CategoryName,
               deleteCategory[0].UserId

                );


//            DeleteCategoryViewModel.Member = saveMember[0];
            DeleteCategoryViewModel.SaveCategorys = allBillCategorys.OrderBy(billCategory => billCategory.CategoryName).ToList();

            deleteCategoryViewModel.CategoryList = DeleteCategoryViewModel.SaveCategorys.GetRange(0, DeleteCategoryViewModel.SaveCategorys.Count);


            ViewBag.edit = "Delete";
            return View(deleteCategoryViewModel);
        }

        [HttpPost]
        [Route("/Home/DeleteCategory/{id}")]
        public IActionResult DeleteCategory(DeleteCategoryViewModel deleteCategoryViewModel)
        {
           
            Category oldCategory = context.Categorys.Find(deleteCategoryViewModel.Id);
//            List<MemberCategory> memberCategory = context.MemberCategorys
//                .Where(mc => mc.CategoryId == deleteCategoryViewModel.Id &&
//                mc.MemberId == DeleteCategoryViewModel.Member.Id).ToList();
      

            List<Bill> billWithCategory = context.Bills
                .Where(b => b.UserId == deleteCategoryViewModel.UserId &&
                            b.CategoryId == deleteCategoryViewModel.Id)
                .ToList();

//            List<Bill> billWithCategory = new List<Bill>();
//             foreach (MemberBill rec in saveAllMemberBills)
 //           {
//                if (rec.Bill.CategoryId == deleteCategoryViewModel.Id)
//                {
//                    billWithCategory.Add(rec.Bill);
//                } 
//            }
            if (billWithCategory.Count > 0)
            {
                ModelState.AddModelError("deleteCategoryName", "Category cannot be deleted if it's used in a bill.");
                deleteCategoryViewModel.CategoryList = DeleteCategoryViewModel.SaveCategorys.GetRange(0, DeleteCategoryViewModel.SaveCategorys.Count);
                ViewBag.edit = "Delete";
                return View(deleteCategoryViewModel);
            }

                
            context.Categorys.Remove(oldCategory);
//            context.MemberCategorys.Remove(memberCategory[0]);
            context.SaveChanges();
            return RedirectToAction("Index", "Category");

        }
    }
}
