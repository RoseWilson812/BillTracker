using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.Models
{
    public class BillCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public BillCategory()
        {
        }
        public BillCategory(string categoryName)
        {
            CategoryName = categoryName;
        }
        public override bool Equals(object obj)
        {
            return obj is BillCategory @billCategory &&
                Id == billCategory.Id;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

    }
}
