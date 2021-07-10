using BillTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.ViewModels
{
    public class EditCategoryViewModel
    {
  //      [Required(ErrorMessage = "Category Name is required!")]
        public string CategoryName { get; set; }
        public static Member Member { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name cannot be blank!")]
        public string EditCategoryName { get; set; }
        
        public static List<BillCategory> SaveCategorys { get; set; }
        public List<BillCategory> CategoryList { get; set; }
      
        public EditCategoryViewModel()
        {

        }
        public EditCategoryViewModel(string category)
        {
            CategoryName = category;
        }
        public EditCategoryViewModel(int id, string editCategory)
        {
            Id = id;
            CategoryName = "";
            EditCategoryName = editCategory;
        }
    }
}
