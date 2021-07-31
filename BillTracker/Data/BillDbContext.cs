using BillTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillTracker.Data
{
    public class BillDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Member> Members { get; set; }
       
        public BillDbContext(DbContextOptions<BillDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
