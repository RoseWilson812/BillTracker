﻿using BillTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.ViewModels
{
    public class DeleteCategoryViewModel
    {
        
        public string CategoryName { get; set; }
        public static Member Member { get; set; }
        public int Id { get; set; }
        [Required]
        public string DeleteCategoryName { get; set; }

        public static List<BillCategory> SaveCategorys { get; set; }
        public List<BillCategory> CategoryList { get; set; }

        public DeleteCategoryViewModel()
        {
        }
       
        public DeleteCategoryViewModel(int id, string deleteCategory)
        {
            Id = id;
            CategoryName = "";
            DeleteCategoryName = deleteCategory;
        }
    }
}