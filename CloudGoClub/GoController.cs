using CloudGoClub.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Web.Mvc;

namespace CloudGoClub
{
    public abstract class GoController : Controller
    {
        protected ApplicationDbContext db = new ApplicationDbContext();
        protected ApplicationUserManager userManager;

        public GoController() 
            : base()
        {
            userManager = new ApplicationUserManager(new ApplicationUserStore(db));
        }

        protected ApplicationUser CurrentUser
        {
            get { return userManager.FindById(User.Identity.GetUserId<int>()); }
        }

    }
}