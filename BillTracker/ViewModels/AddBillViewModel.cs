using BillTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillTracker.ViewModels
{
    public class AddBillViewModel
    {
        
        public string PaymentType { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "A valid date must be entered!")]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false)]

        public DateTime? PaidDate { get; set; }

        [Required(ErrorMessage = "Payee is required!")]
        [MaxLength(40)]
        public string Payee { get; set; }

        [DataType(DataType.Currency)]
        [Range(.01, 1000000000, ErrorMessage = "Amount must be greater than 0.00")]
        [Required(ErrorMessage = "Amount is required!")]
        public decimal Amount { get; set; }

        [BindProperty]
        public int CategoryId { get; set; }

        [MaxLength(40)]
        public string Memo { get; set; }

        [Required(ErrorMessage = "Tax Deductible must be Y or N")]
        public char TaxDeductible { get; set; }
        [BindProperty]
        public string UserId { get; set; }
        public List<SelectListItem> PossibleCategories { get; set; }
        public static List<Category> SaveCategories { get; set; }
 
        public static string[] SelectedCategory { get; set; }
        public List<DisplayBill> SaveBills { get; set; }


        public AddBillViewModel(List<Category> categories, string userId) 
        {
            TaxDeductible = 'N';
            UserId = userId;
            SaveCategories = new List<Category>();
            SaveCategories = categories.GetRange(0, categories.Count);
            CreateDropdown();
 
        }

        public AddBillViewModel()
        {
        }          
        public void CreateDropdown()
        {
            PossibleCategories = new List<SelectListItem>();
            foreach (var category in SaveCategories)
            {
                PossibleCategories.Add(
                    new SelectListItem
                    {
                        Value = category.Id.ToString(),
                        Text = category.CategoryName
                    }
                ); ;
            }
        }

    }
}
