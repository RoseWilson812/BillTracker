using BillTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.ViewModels
{
    public class EditCategoryViewModel
    {
        public string CategoryName { get; set; }
        [BindProperty]
        public string UserId { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name cannot be blank!")]
        public string EditCategoryName { get; set; }
        
        public static List<Category> SaveCategorys { get; set; }
        public List<Category> CategoryList { get; set; }
      
        public EditCategoryViewModel()
        {

        }
        public EditCategoryViewModel(string category, string userId)
        {
            CategoryName = category;
            UserId = userId;
        }
        public EditCategoryViewModel(int id, string editCategory, string userId)
        {
            Id = id;
            CategoryName = "";
            EditCategoryName = editCategory;
            UserId = userId;
        }
    }
}
