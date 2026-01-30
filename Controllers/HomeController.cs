using System.Collections.Generic;
using System.Web.Mvc;
using TourismPlatformMVC.ViewModels;

namespace TourismPlatformMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var members = new List<GroupMemberViewModel>
            {
                new GroupMemberViewModel { StudentId = "20021750", FullName = "Marcos Yukihiro Vieira Yamashita" },
                new GroupMemberViewModel { StudentId = "20028065", FullName = "GURJOT SINGH" },
                new GroupMemberViewModel { StudentId = "20032744", FullName = "Aven Matthew MAJELLANO" }
            };

            return View(members);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult LoginSuccess()
        {
            return View();
        }
    }
}
