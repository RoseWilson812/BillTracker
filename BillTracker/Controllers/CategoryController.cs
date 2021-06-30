using BillTracker.Data;
using BillTracker.Models;
using BillTracker.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BillTracker.Controllers
{
    public class CategoryController : Controller
    {
        private BillDbContext context;

        public CategoryController(BillDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<BillCategory> allBillCategorys = new List<BillCategory>();
            ClaimsPrincipal currentUser = this.User;
            var currentUserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Member> saveMember = context.Members
                .Where(m => m.UserId == currentUserId).ToList();

            if (saveMember.Count > 0)
            {
                List<MemberCategory> allMemberCategorys = context.MemberCategorys
                    .Where(mb => mb.MemberId == saveMember[0].Id)
                    .Include(mb => mb.Category)
                    .ToList();

                foreach (MemberCategory rec in allMemberCategorys)
                {
                    allBillCategorys.Add(rec.Category);
                }
            }
            else
            {
                Member newMember = new Member(currentUserId);
                context.Members.Add(newMember);
                context.SaveChanges();
                saveMember = context.Members
                  .Where(m => m.UserId == currentUserId).ToList();
            }
            AddCategoryViewModel addCategoryViewModel = new AddCategoryViewModel();
            AddCategoryViewModel.Member = saveMember[0];

            return View(addCategoryViewModel);
        }
[HttpPost]
        public IActionResult Add(BillCategory category, AddCategoryViewModel addCategoryViewModel)
        {
            List<Member> saveMember = context.Members
             .Where(m => m.UserId == AddCategoryViewModel.Member.UserId).ToList();
            if (ModelState.IsValid)
            {
                context.Categorys.Add(category);
 //               List<Member> holdMember = new List<Member>();
  //              holdMember.Add(AddCategoryViewModel.Member);
                var holdMemberCategory = new MemberCategory
                {
                    Member = saveMember[0],
                    Category = category
                };
                context.MemberCategorys.Add(holdMemberCategory);
                context.SaveChanges();
                return Redirect("/Category");
            }
            return View(addCategoryViewModel);
        }
    }
}
