using BillTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.ViewModels
{
    public class DeleteBillViewModel
    {
        public int Id { get; set; }
        [BindProperty]
        public string PaymentType {get; set;}

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? PaidDate { get; set; }

        public string Payee { get; set; }

     
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [BindProperty]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
        public string Memo { get; set; }
                
        public char TaxDeductible { get; set; }
        [BindProperty]
        public string UserId { get; set; }
        public List<SelectListItem> PossibleCategories { get; set; }
        public static List<Category> SaveCategories { get; set; }

        public Category Categorys { get; set; }
        public static string[] SelectedCategory { get; set; }

        public static Member Member { get; set; }
        public List<Bill> SaveBills { get; set; }


        public DeleteBillViewModel(int id, string paymentType, DateTime dueDate, DateTime? paidDate, string payee,
            decimal amount, int categoryId, string memo, char taxDeductible,
            string userId, Member member, List<Category> categories)
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
            Member = member;
            //SaveCategories = new List<BillCategory>();
            SaveCategories = categories.GetRange(0, categories.Count);
            
            foreach (var category in categories)
            {
                if (categoryId == category.Id)
                {
                    CategoryName = category.CategoryName;
                    break;
                }
            }
        }
        public DeleteBillViewModel()
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
