using BillTracker.Data;
using BillTracker.Models;
using BillTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


namespace BillTracker.Controllers
{
    public class SearchBillsController : Controller
    {
        private BillDbContext context;

        public SearchBillsController(BillDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        [Authorize]
        public IActionResult Index()
        {
            List<Category> sortedCategories = new List<Category>();

            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Category> allBillSearchCategorys = context.Categorys
                   .Where(c => c.UserId == currentUserId)
                   .ToList();
            List<Member> saveMember = context.Members
                .Where(m => m.UserId == currentUserId).ToList();
            if (saveMember.Count == 0)
            {
                Member newMember = new Member(currentUserId);
                context.Members.Add(newMember);
                context.SaveChanges();

            }
            sortedCategories = allBillSearchCategorys.OrderBy(c => c.CategoryName).ToList();
            SearchBillsViewModel searchBillsViewModel = new SearchBillsViewModel(currentUserId,sortedCategories );
           
            SearchBillsViewModel.SaveSearchCategories = new List<Category>();

            SearchBillsViewModel.SaveSearchCategories = allBillSearchCategorys.OrderBy(category => category.CategoryName).ToList();
            return View(searchBillsViewModel);
        }
        public IActionResult Search(SearchBillsViewModel searchBillsViewModel)
        {
            List<Bill> searchResults = new List<Bill>();
            if (searchBillsViewModel.DateOption == null)
            {
                searchResults = context.Bills
                 .Where(b => b.UserId == searchBillsViewModel.UserId).ToList();
            }
            else if (searchBillsViewModel.DateOption == "Due Date")
            {
                if (searchBillsViewModel.FromDate is null &&
                    !(searchBillsViewModel.ToDate is null))
                {
                    searchResults = context.Bills
                  .Where(b => b.UserId == searchBillsViewModel.UserId &&
                              b.DueDate <= searchBillsViewModel.ToDate).ToList();
                }
                else if (!(searchBillsViewModel.FromDate is null) &&
                          (searchBillsViewModel.ToDate is null))
                {
                    searchResults = context.Bills
                     .Where(b => b.UserId == searchBillsViewModel.UserId &&
                          b.DueDate >= searchBillsViewModel.FromDate).ToList();
                }
                else if (searchBillsViewModel.FromDate is null &&
                         searchBillsViewModel.ToDate is null)
                {
                    searchResults = context.Bills
                     .Where(b => b.UserId == searchBillsViewModel.UserId).ToList();
                }
                else
                {
                    searchResults = context.Bills
                 .Where(b => b.UserId == searchBillsViewModel.UserId &&
                             b.DueDate >= searchBillsViewModel.FromDate &&
                             b.DueDate <= searchBillsViewModel.ToDate).ToList();
                }
            }
            else // searchBillViewModel. DateOption = "Paid Date"
            {
                if (searchBillsViewModel.FromDate is null &&
                    !(searchBillsViewModel.ToDate is null))
                {
                    searchResults = context.Bills
                  .Where(b => b.UserId == searchBillsViewModel.UserId &&
                              b.PaidDate <= searchBillsViewModel.ToDate).ToList(); 
                }
                else if (!(searchBillsViewModel.FromDate is null) &&
                          (searchBillsViewModel.ToDate is null))
                {
                    searchResults = context.Bills
                     .Where(b => b.UserId == searchBillsViewModel.UserId &&
                          b.PaidDate >= searchBillsViewModel.FromDate).ToList();
                }
                else if ( searchBillsViewModel.FromDate is null &&
                          searchBillsViewModel.ToDate is null)
                {
                    searchResults = context.Bills
                  .Where(b => b.UserId == searchBillsViewModel.UserId &&
                              b.PaymentType == null).ToList();
                }
                else
                {
                    searchResults = context.Bills
                  .Where(b => b.UserId == searchBillsViewModel.UserId &&
                              b.PaidDate >= searchBillsViewModel.FromDate &&
                              b.PaidDate <= searchBillsViewModel.ToDate).ToList();
                }
            }
            
           
        
            if (!(searchBillsViewModel.PaymentType is null))
            {
                string enteredPaymentType = searchBillsViewModel.PaymentType.ToLower();
                string savedPaymentType = null;
                for (int i = 0; i < searchResults.Count; i++)
                { 
                    savedPaymentType = null;
                    if (!(searchResults[i].PaymentType is null))
                    {
                        savedPaymentType = searchResults[i].PaymentType.ToLower();
                    }
                    if (savedPaymentType != enteredPaymentType)
                    {
                        searchResults.Remove(searchResults[i]);
                        if (searchResults.Count > 0)
                        {
                            i--;
                        }
                    }
                }
            }

           
            if(!(searchBillsViewModel.Payee is null))
            {
                string enteredPayee = searchBillsViewModel.Payee.ToLower();
                string savedPayee = "";
                int index = 0;
                for (int i = 0; i < searchResults.Count; i++)
                {
                    savedPayee = searchResults[i].Payee.ToLower();
                   
                    index = savedPayee.IndexOf(enteredPayee);
                    if (index < 0)
                    {
                       searchResults.Remove(searchResults[i]);
                        if (searchResults.Count > 0)
                        {  
                           i--;
                        }
                    }
                    
                }
                
            }
            if (!(searchBillsViewModel.Amount is null))
            {
                for (int i = 0; i < searchResults.Count; i++)
                {
                    
                    if (searchResults[i].Amount != searchBillsViewModel.Amount)
                    {
                        searchResults.Remove(searchResults[i]);
                        if (searchResults.Count > 0)
                        {
                            i--;
                        }
                    }
                }
            }
            if (searchBillsViewModel.CategoryId != 0)
            {
                for (int i = 0; i < searchResults.Count; i++)
                {

                    if (searchResults[i].CategoryId != searchBillsViewModel.CategoryId)
                    {
                        searchResults.Remove(searchResults[i]);
                        if (searchResults.Count > 0)
                        {
                            i--;
                        }
                    }
                }
            }
            if (!(searchBillsViewModel.Memo is null))
            {
                string enteredMemo = searchBillsViewModel.Memo.ToLower();
                string savedMemo = "";
                int index = 0;
                for (int i = 0; i < searchResults.Count; i++)
                {
                    if (!(searchResults[i].Memo is null))
                    {
                        savedMemo = searchResults[i].Memo.ToLower();
                    }
                    else
                    {
                        savedMemo = "";
                    }
                    index = savedMemo.IndexOf(enteredMemo);
                   if (index < 0)
                   {
                       searchResults.Remove(searchResults[i]);
                        if (searchResults.Count > 0)
                        {
                           i--;
                       }
                   }
                    
                }
            }

            if (searchBillsViewModel.TaxDeductible != '0')
            {
                if (searchBillsViewModel.TaxDeductible == '1')
                {
                    searchBillsViewModel.TaxDeductible = 'Y';
                }
                else
                {
                    searchBillsViewModel.TaxDeductible = 'N';
                }
                for (int i = 0; i < searchResults.Count; i++)
                {

                    if (searchResults[i].TaxDeductible != searchBillsViewModel.TaxDeductible)
                    {
                        searchResults.Remove(searchResults[i]);
                        if (searchResults.Count > 0) 
                        {
                            i--;
                        }
                    }
                }
            }
            decimal billTotals = 0;
            List<DisplaySearchBill> allSearchBills = new List<DisplaySearchBill>(); 
            foreach (Bill rec in searchResults)
            {
                Category findCategory = context.Categorys.Find(rec.CategoryId);
                rec.Amount = decimal.Round(rec.Amount, 2);
                DisplaySearchBill newDisplaySearchBill = new DisplaySearchBill(rec.Id, rec.PaymentType, rec.DueDate,
                    rec.PaidDate, rec.Payee, rec.Amount, rec.CategoryId,
                    rec.Memo, rec.TaxDeductible, rec.UserId, findCategory.CategoryName);
                billTotals += rec.Amount;
                allSearchBills.Add(newDisplaySearchBill);

            }
            List<DisplaySearchBill> sortedSearchBills = new List<DisplaySearchBill>();
            List<DisplaySearchBill> totaledSearchBills = new List<DisplaySearchBill>();

            Decimal subtotal = 0;
            if (searchBillsViewModel.SortTotal == "0")
            {
                DateTime prevDateTime = DateTime.Now;
                sortedSearchBills = allSearchBills.OrderBy(displaySearchBill => displaySearchBill.DueDate).ToList();
                for (int i = 0; i < sortedSearchBills.Count; i++)
                {
                    if (i == 0)
                    {
                        prevDateTime = (DateTime)sortedSearchBills[0].DueDate;
                    }

                    if (prevDateTime == sortedSearchBills[i].DueDate)
                    {
                        subtotal += sortedSearchBills[i].Amount;
                        totaledSearchBills.Add(sortedSearchBills[i]);
                    }
                    else
                    {
                        DisplaySearchBill subtotalRecord = new DisplaySearchBill(subtotal);
                        totaledSearchBills.Add(subtotalRecord);
                        totaledSearchBills.Add(sortedSearchBills[i]);
                        subtotal = 0;
                        subtotal += sortedSearchBills[i].Amount;
                        prevDateTime = (DateTime)sortedSearchBills[i].DueDate;
                    }
                } 
            }
            else if (searchBillsViewModel.SortTotal == "1")
            {
                DateTime prevDateTime = DateTime.Now;
                sortedSearchBills = allSearchBills.OrderBy(displaySearchBill => displaySearchBill.DueDate).ToList();
                for (int i = 0; i < sortedSearchBills.Count; i++)
                {
                    if (i == 0)
                    {
                        prevDateTime = (DateTime)sortedSearchBills[0].DueDate;
                    }

                    if (prevDateTime == sortedSearchBills[i].DueDate)
                    {
                        subtotal += sortedSearchBills[i].Amount;
                        totaledSearchBills.Add(sortedSearchBills[i]);
                    }
                    else
                    {
                        DisplaySearchBill subtotalRecord = new DisplaySearchBill(subtotal);
                        totaledSearchBills.Add(subtotalRecord);
                        totaledSearchBills.Add(sortedSearchBills[i]);
                        subtotal = 0;
                        subtotal += sortedSearchBills[i].Amount;
                        prevDateTime = (DateTime)sortedSearchBills[i].DueDate;
                    }
                }
            }
            else if (searchBillsViewModel.SortTotal == "2")
            {
                DateTime? prevDateTime = DateTime.Now;
                sortedSearchBills = allSearchBills.OrderBy(displaySearchBill => displaySearchBill.PaidDate).ToList();
                for (int i = 0; i < sortedSearchBills.Count; i++)
                {
                    if (i == 0)
                    {
                        prevDateTime = (DateTime?)sortedSearchBills[0].PaidDate;
                    }

                    if (prevDateTime == sortedSearchBills[i].PaidDate)
                    {
                        subtotal += sortedSearchBills[i].Amount;
                        totaledSearchBills.Add(sortedSearchBills[i]);
                    }
                    else
                    {
                        DisplaySearchBill subtotalRecord = new DisplaySearchBill(subtotal);
                        totaledSearchBills.Add(subtotalRecord);
                        totaledSearchBills.Add(sortedSearchBills[i]);
                        subtotal = 0;
                        subtotal += sortedSearchBills[i].Amount;
                        prevDateTime = (DateTime?)sortedSearchBills[0].PaidDate;
                    }
                }
            }
            else if (searchBillsViewModel.SortTotal == "3") 
            {
                string prevPaymentType = " ";
                sortedSearchBills = allSearchBills.OrderBy(displaySearchBill => displaySearchBill.PaymentType).ToList();
                for (int i = 0; i < sortedSearchBills.Count; i++)
                {
                    if (i == 0)
                    {
                        prevPaymentType = sortedSearchBills[0].PaymentType;
                    }

                    if (prevPaymentType == sortedSearchBills[i].PaymentType)
                    {
                        subtotal += sortedSearchBills[i].Amount;
                        totaledSearchBills.Add(sortedSearchBills[i]);
                    }
                    else
                    {
                        DisplaySearchBill subtotalRecord = new DisplaySearchBill(subtotal);
                        totaledSearchBills.Add(subtotalRecord);
                        totaledSearchBills.Add(sortedSearchBills[i]);
                        subtotal = 0;
                        subtotal += sortedSearchBills[i].Amount;
                        prevPaymentType = sortedSearchBills[i].PaymentType;
                    }
                }
            }
            else if (searchBillsViewModel.SortTotal == "4") 
            {
                string prevPayee = " ";
                sortedSearchBills = allSearchBills.OrderBy(displaySearchBill => displaySearchBill.Payee).ToList();
                for (int i = 0; i < sortedSearchBills.Count; i++)
                {
                    if (i == 0)
                    {
                        prevPayee = sortedSearchBills[0].Payee;
                    }

                    if (prevPayee == sortedSearchBills[i].Payee)
                    {
                        subtotal += sortedSearchBills[i].Amount;
                        totaledSearchBills.Add(sortedSearchBills[i]);
                    }
                    else
                    {
                        DisplaySearchBill subtotalRecord = new DisplaySearchBill(subtotal);
                        totaledSearchBills.Add(subtotalRecord);
                        totaledSearchBills.Add(sortedSearchBills[i]);
                        subtotal = 0;
                        subtotal += sortedSearchBills[i].Amount;
                        prevPayee = sortedSearchBills[i].Payee;
                    }
                }
            }
            else if (searchBillsViewModel.SortTotal == "5")  
            {
                string prevCategoryName = " ";
                sortedSearchBills = allSearchBills.OrderBy(displaySearchBill => displaySearchBill.CategoryName).ToList();
                for (int i = 0; i < sortedSearchBills.Count; i++)
                {
                    if (i == 0)
                    {
                        prevCategoryName = sortedSearchBills[0].CategoryName;
                    }

                    if (prevCategoryName == sortedSearchBills[i].CategoryName)
                    {
                        subtotal += sortedSearchBills[i].Amount;
                        totaledSearchBills.Add(sortedSearchBills[i]);
                    }
                    else
                    {
                        DisplaySearchBill subtotalRecord = new DisplaySearchBill(subtotal);
                        totaledSearchBills.Add(subtotalRecord);
                        totaledSearchBills.Add(sortedSearchBills[i]);
                        subtotal = 0;
                        subtotal += sortedSearchBills[i].Amount;
                        prevCategoryName = sortedSearchBills[i].CategoryName;
                    }
                }
            }
            else  
            {
                char prevTaxDeductible = ' ';
                sortedSearchBills = allSearchBills.OrderBy(displaySearchBill => displaySearchBill.TaxDeductible).ToList();
                for (int i = 0; i < sortedSearchBills.Count; i++)
                {
                    if (i == 0)
                    {
                        prevTaxDeductible = sortedSearchBills[0].TaxDeductible;
                    }

                    if (prevTaxDeductible == sortedSearchBills[i].TaxDeductible)
                    {
                        subtotal += sortedSearchBills[i].Amount;
                        totaledSearchBills.Add(sortedSearchBills[i]);
                    }
                    else
                    {
                        DisplaySearchBill subtotalRecord = new DisplaySearchBill(subtotal);
                        totaledSearchBills.Add(subtotalRecord);
                        totaledSearchBills.Add(sortedSearchBills[i]);
                        subtotal = 0;
                        subtotal += sortedSearchBills[i].Amount;
                        prevTaxDeductible = sortedSearchBills[i].TaxDeductible;
                    }
                }
            }

            DisplaySearchBill lastSubtotalRecord = new DisplaySearchBill(subtotal);
            totaledSearchBills.Add(lastSubtotalRecord);
            searchBillsViewModel.SaveSearchBills = totaledSearchBills;
            searchBillsViewModel.GrandTotal = billTotals;
            return View(searchBillsViewModel);
        }
    }
}
