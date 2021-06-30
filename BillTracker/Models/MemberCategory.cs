using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.Models
{
    public class MemberCategory
    {
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int BillCategoryId { get; set; }
        public BillCategory Category { get; set; }

        public MemberCategory()
        {
        }
    }
}
