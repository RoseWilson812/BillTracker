using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Member()
        {
        }
        public Member(string userId)
        {
            UserId = userId;
        }
        public override bool Equals(object obj)
        {
            return obj is Member @member &&
                Id == member.Id;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
