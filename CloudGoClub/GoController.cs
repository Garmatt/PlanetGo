using CloudGoClub.Models;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;

namespace CloudGoClub
{
    public abstract class GoController : Controller
    {
        protected ApplicationDbContext db;
        protected ApplicationUserManager userManager;

        public GoController() 
            : base()
        {
            db = new ApplicationDbContext();
            userManager = new ApplicationUserManager(new ApplicationUserStore(db));
        }

        protected ApplicationUser CurrentUser => userManager.FindById(User.Identity.GetUserId<int>());
    }
}