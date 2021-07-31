using BillTracker.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.ViewModels
{
    public class AddCategoryViewModel
    {
        [Required(ErrorMessage = "Category Name is required!")]
        public string CategoryName { get; set; }
        [BindProperty]
        public string UserId { get; set; }
        public int Id { get; set; }

        public static List<Category> SaveCategorys { get; set; }

        public List<Category> CategoryList { get; set; }
      
        public AddCategoryViewModel()
        {

        }
        public AddCategoryViewModel(string category, string userId)
        {
            CategoryName = category;
            UserId = userId;
        }

    } 
}
