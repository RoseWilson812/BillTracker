﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.Models
{
    public class CategoryBill
    {
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int BillId { get; set; }
        public Bill Bill { get; set; }



        public CategoryBill()
        {
        }
    }
}