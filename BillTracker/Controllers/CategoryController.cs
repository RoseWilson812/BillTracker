using BillTracker.Data;
using BillTracker.Models;
using BillTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

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

            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Member> saveMember = context.Members
                .Where(m => m.UserId == currentUserId).ToList();
            List<Category> allBillCategorys = context.Categorys
                   .Where(c => c.UserId == currentUserId)
                   .ToList();
            if (saveMember.Count == 0)
            {

                Member newMember = new Member(currentUserId);
                context.Members.Add(newMember);
                context.SaveChanges();

            }
            UpdateCategoryViewModel updateCategoryViewModel = new UpdateCategoryViewModel();
            updateCategoryViewModel.UserId = currentUserId;
            UpdateCategoryViewModel.SaveCategorys = new List<Category>();

            UpdateCategoryViewModel.SaveCategorys = allBillCategorys.OrderBy(category => category.CategoryName).ToList();
            updateCategoryViewModel.CategoryList = UpdateCategoryViewModel.SaveCategorys.GetRange(0, UpdateCategoryViewModel.SaveCategorys.Count);
            ViewBag.edit = "";
            return View("UpdateCategory", updateCategoryViewModel);


        }
        [HttpPost]
        public IActionResult AddCategory(Category category, UpdateCategoryViewModel updateCategoryViewModel)
        {
            updateCategoryViewModel.CategoryList = UpdateCategoryViewModel.SaveCategorys.GetRange(0, UpdateCategoryViewModel.SaveCategorys.Count);
            if (updateCategoryViewModel.UserId == null)
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                category.UserId = currentUserId;
            }
            if (updateCategoryViewModel.CategoryName == null ||
                updateCategoryViewModel.CategoryName == "" ||
                updateCategoryViewModel.CategoryName.All(char.IsWhiteSpace))
            {
                ModelState.AddModelError("categoryName", "Category Name is required.");
            }

            if (ModelState.IsValid)
            {
                context.Categorys.Add(category);
                context.SaveChanges();
                ViewBag.edit = "";
                return Redirect("/Category");
            }
            ViewBag.edit = "";
            return View("UpdateCategory", updateCategoryViewModel);
        }


        [HttpGet]
        [Route("/Home/EditCategory/{id}")]
        public IActionResult EditCategory(int id)
        {

            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<Category> allBillCategorys = context.Categorys
                    .Where(c => c.UserId == currentUserId)
                    .ToList();

            Category editCategory = context.Categorys.Find(id);
                                
                        UpdateCategoryViewModel updateCategoryViewModel = new UpdateCategoryViewModel(
                           editCategory.CategoryName,
                           editCategory.Id,
                           editCategory.UserId
                            );
 
            UpdateCategoryViewModel.SaveCategorys = allBillCategorys.OrderBy(billCategory => billCategory.CategoryName).ToList();

            updateCategoryViewModel.CategoryList = UpdateCategoryViewModel.SaveCategorys.GetRange(0, UpdateCategoryViewModel.SaveCategorys.Count);

            
            ViewBag.edit = "Edit";
            return View("UpdateCategory", updateCategoryViewModel);
        }

    
        [HttpPost]
        [Route("/Home/EditCategory/{id}")]
        public IActionResult EditCategory(UpdateCategoryViewModel updateCategoryViewModel)
    {
        Category oldCategory = context.Categorys.Find(updateCategoryViewModel.Id);
        if (updateCategoryViewModel.EditCategoryName is null ||
            updateCategoryViewModel.EditCategoryName == "" ||
            updateCategoryViewModel.EditCategoryName.All(char.IsWhiteSpace))
            {
                ModelState.AddModelError("editCategoryName", "Category Name cannot be blank.");
            }

            if (ModelState.IsValid)
        {
            oldCategory.CategoryName = updateCategoryViewModel.EditCategoryName;
          
            context.Categorys.Update(oldCategory);
            context.SaveChanges();
            
           

            return RedirectToAction("Index", "Category");
        }
        else
        {
            ViewBag.edit = "Edit";
            updateCategoryViewModel.CategoryList = UpdateCategoryViewModel.SaveCategorys.GetRange(0, UpdateCategoryViewModel.SaveCategorys.Count);
            updateCategoryViewModel.EditCategoryName = oldCategory.CategoryName;
            return View("UpdateCategory", updateCategoryViewModel);
        }
    }


        [HttpGet]
        [Route("/Home/DeleteCategory/{id}")]
        public IActionResult DeleteCategory(int id)
        {

            Category deleteCategory = context.Categorys.Find(id);

  // prevents error when using back arrow
            if (deleteCategory == null)
            {
                return RedirectToAction("Index", "Category");
            }

            List<Category> allBillCategorys = context.Categorys
                .Where(c => c.UserId == deleteCategory.UserId)
                .ToList();

            UpdateCategoryViewModel updateCategoryViewModel = new UpdateCategoryViewModel(
               deleteCategory.CategoryName,
               deleteCategory.UserId,
                deleteCategory.Id
                );

            UpdateCategoryViewModel.SaveCategorys = allBillCategorys.OrderBy(billCategory => billCategory.CategoryName).ToList();

            updateCategoryViewModel.CategoryList = UpdateCategoryViewModel.SaveCategorys.GetRange(0, UpdateCategoryViewModel.SaveCategorys.Count);


            ViewBag.edit = "Delete";
            return View("UpdateCategory", updateCategoryViewModel);
        }

        [HttpPost]
        [Route("/Home/DeleteCategory/{id}")]
        public IActionResult DeleteCategory(UpdateCategoryViewModel updateCategoryViewModel)
        {
           
            Category oldCategory = context.Categorys.Find(updateCategoryViewModel.Id);

            List<Bill> billWithCategory = context.Bills
                .Where(b => b.UserId == updateCategoryViewModel.UserId &&
                            b.CategoryId == updateCategoryViewModel.Id)
                .ToList();

            if (billWithCategory.Count > 0)
            {
                ModelState.AddModelError("deleteCategoryName", "Category cannot be deleted if it's used in a bill.");
                updateCategoryViewModel.CategoryList = UpdateCategoryViewModel.SaveCategorys.GetRange(0, UpdateCategoryViewModel.SaveCategorys.Count);
                ViewBag.edit = "Delete";
                return View("UpdateCategory", updateCategoryViewModel);
            }

                
            context.Categorys.Remove(oldCategory);
            context.SaveChanges();
            return RedirectToAction("Index", "Category");

        }
    }
}
