﻿using BillTracker.Models;
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
        public static Member Member { get; set; }
        public int Id { get; set; }

        public static List<BillCategory> SaveCategorys { get; set; }
       
        public List<BillCategory> CategoryList { get; set; }
        public AddCategoryViewModel()
        {

        }
        public AddCategoryViewModel(string category)
        {
            CategoryName = category;
        }

    } 
}
