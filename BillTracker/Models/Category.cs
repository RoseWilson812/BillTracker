using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string UserId { get; set; }

        public Category()
        {
        }
        public Category(string categoryName, string userId)
        {
            CategoryName = categoryName;
            UserId = userId;
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
