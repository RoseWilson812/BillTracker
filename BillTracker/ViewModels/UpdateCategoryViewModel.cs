using BillTracker.Models;
using System.Collections.Generic;


namespace BillTracker.ViewModels
{
    public class UpdateCategoryViewModel
    {
        public string CategoryName { get; set; }
        public string UserId { get; set; }
        public int Id { get; set; }
        public string DeleteCategoryName { get; set; }
        public string EditCategoryName { get; set; }

        public static List<Category> SaveCategorys { get; set; }

        public List<Category> CategoryList { get; set; }

        public UpdateCategoryViewModel()
        {

        }

        public UpdateCategoryViewModel(string editCategory, int id, string userId)
        {
            Id = id;
            CategoryName = "";
            EditCategoryName = editCategory;
            UserId = userId;
        }
        public UpdateCategoryViewModel(string deleteCategory, string userId, int id)
        {
            Id = id;
            CategoryName = "";
            DeleteCategoryName = deleteCategory;
            UserId = userId;
        }
    }
}
