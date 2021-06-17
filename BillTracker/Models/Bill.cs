using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public string DueDate { get; set; }
        public string PaidDate { get; set; }
        public string Payee { get; set; }
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public string Memo { get; set; }
        public char TaxDeductible { get; set; }

        public Bill()
        {
        }
        public Bill(string dueDate, string paidDate, string payee, decimal amount, string category, string memo, char taxDeductible)
        {
            DueDate = dueDate;
            PaidDate = paidDate;
            Payee = payee;
            Amount = amount;
            Category = category;
            Memo = memo;
            TaxDeductible = taxDeductible;
        }
        public override bool Equals(object obj)
        {
            return obj is Bill @bill &&
                Id == bill.Id;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}

