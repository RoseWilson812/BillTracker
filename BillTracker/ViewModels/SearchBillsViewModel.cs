using BillTracker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.ViewModels
{
    public class SearchBillsViewModel
    {
        public string DateOption { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }

        public string PaymentType { get; set; }
        public string Payee { get; set; }

       
        [DataType(DataType.Currency)]
        public decimal? Amount { get; set; }

        public int CategoryId { get; set; }

       
        public string Memo { get; set; }

       
        public char TaxDeductible { get; set; }
        public string SortTotal { get; set; }
      
        public string UserId { get; set; }
        public List<SelectListItem> PossibleSearchCategories { get; set; }
        public static List<Category> SaveSearchCategories { get; set; }
        public List<SelectListItem> PossibleTaxDeduction { get; set; }
        public List<SelectListItem> PossibleSortTotal { get; set; }

        public decimal? GrandTotal { get; set; }

        public Category Categorys { get; set; }
        public static string[] SelectedCategory { get; set; }

        public List<DisplaySearchBill> SaveSearchBills { get; set; }


        public SearchBillsViewModel( string userId, List<Category> categories)
        {
            
            UserId = userId;
            PossibleSearchCategories = new List<SelectListItem>();
            SaveSearchCategories = new List<Category>();
            SaveSearchCategories = categories.GetRange(0, categories.Count);
            SaveSearchBills = new List<DisplaySearchBill>();
            CreateDropdown();
    
        }

        public SearchBillsViewModel()
        {
        }
        public void CreateDropdown()
        {
            PossibleSearchCategories = new List<SelectListItem>();
            PossibleSearchCategories.Add(new SelectListItem { Value = 0.ToString(), Text = "select item" }); ;
            foreach (var category in SaveSearchCategories)
            {
                PossibleSearchCategories.Add(
                    new SelectListItem
                    {
                        Value = category.Id.ToString(),
                        Text = category.CategoryName
                    }
                ); ;
            }
            PossibleTaxDeduction = new List<SelectListItem>();
            PossibleTaxDeduction.Add(
                 new SelectListItem { Value = 0.ToString(), Text = "select item" });
            PossibleTaxDeduction.Add(
                 new SelectListItem { Value = 1.ToString(), Text = "Y" });
            PossibleTaxDeduction.Add(
                 new SelectListItem { Value = 2.ToString(), Text = "N", Selected = true });

            PossibleSortTotal = new List<SelectListItem>();
            PossibleSortTotal.Add(
                 new SelectListItem { Value = 0.ToString(), Text = "select item" });
            PossibleSortTotal.Add(
                new SelectListItem { Value = 1.ToString(), Text = "Due Date" }); 
            PossibleSortTotal.Add(
                 new SelectListItem { Value = 2.ToString(), Text = "Paid Date" });
            PossibleSortTotal.Add(
                new SelectListItem { Value = 3.ToString(), Text = "Payment Type" });
            PossibleSortTotal.Add(
                new SelectListItem { Value = 4.ToString(), Text = "Payee" });
            PossibleSortTotal.Add(
                new SelectListItem { Value = 5.ToString(), Text = "Category" });
            PossibleSortTotal.Add(
                new SelectListItem { Value = 6.ToString(), Text = "Tax Deductible" });
        }
    }
}
    

