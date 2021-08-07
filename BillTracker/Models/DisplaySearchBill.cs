using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.Models
{
    public class DisplaySearchBill
    {
        public int Id { get; set; }
        public string PaymentType { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public string Payee { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public string Memo { get; set; }
        public char TaxDeductible { get; set; }
        public string UserId { get; set; }
        public string CategoryName { get; set; }
       

        public DisplaySearchBill()
        {
        }
        public DisplaySearchBill(int id, string paymentType, DateTime dueDate, DateTime? paidDate, string payee,
            decimal amount, int categoryId, string memo, char taxDeductible, string userId,
            string categoryName)
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
            CategoryName = categoryName;
           
        }
        public DisplaySearchBill(decimal subtotal)
        {
            Id = 0;
            PaymentType = " ";
            DueDate = null;
            PaidDate = null;
            Payee = " ";
            Amount = subtotal;
            CategoryId = 0;
            Memo = "Subtotal";
            TaxDeductible = ' ';
            CategoryName = " ";
        }
        public override bool Equals(object obj)
        {
            return obj is DisplayBill @displayBill &&
                Id == displayBill.Id;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
