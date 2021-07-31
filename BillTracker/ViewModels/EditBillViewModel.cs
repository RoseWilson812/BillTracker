using BillTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.ViewModels
{
    public class EditBillViewModel
    {
        public int Id { get; set; }

       
        public string PaymentType { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "A valid date must be entered!")]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false)]

        public DateTime? PaidDate { get; set; }

        [Required(ErrorMessage = "Payee is required!")]
        [MaxLength(40)]
        public string Payee { get; set; }

        [Required(ErrorMessage = "Amount is required!")]
        [DataType(DataType.Currency)]
        [Range(.01, 1000000000, ErrorMessage = "Amount must be greater than 0.00")]
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
        
        public Category Categorys { get; set; }
        public static string[] SelectedCategory { get; set; }
     
//        public static Member Member { get; set; }
        public List<Bill> SaveBills { get; set; }


        public EditBillViewModel(int id, string paymentType, DateTime dueDate, DateTime? paidDate, string payee,
            decimal amount, int categoryId, string memo, char taxDeductible, string userId,List<Category> categories) 
        {
            Id = id;
            PaymentType = paymentType;
            DueDate = dueDate;
            PaidDate = paidDate;
            Payee = payee;
            Amount = amount;
            CategoryId = categoryId;
            Memo = memo;
            TaxDeductible = taxDeductible;
            UserId = userId;
            PossibleCategories = new List<SelectListItem>();
            SaveCategories = new List<Category>();
            SaveCategories = categories.GetRange(0, categories.Count);

            foreach (var category in categories)
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

        public EditBillViewModel()
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
