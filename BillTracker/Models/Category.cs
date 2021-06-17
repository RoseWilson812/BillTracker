using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string BillCategory { get; set; }

        public Category()
        {
        }
        public Category(string billCategory)
        {
            BillCategory = billCategory;
        }
        public override bool Equals(object obj)
        {
            return obj is Category @category &&
                Id == category.Id;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

    }
}
