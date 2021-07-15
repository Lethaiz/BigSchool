using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BigSchool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BigSchoolContext context = new BigSchoolContext();
            var upcomingCourse = context.Course.Where(p => p.Datetime > DateTime.Now).OrderBy(p => p.Datetime).ToList();
            var userId = User.Identity.GetUserId();
            foreach (Course i in upcomingCourse)
            {
                ApplicationUser User = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(i.LeturerIdId);

                i.Name = User.Name;

                if (userId != null)
                {
                    i.isLogin = true;
                    Attendance find = context.Attendance.FirstOrDefault(p => p.CourseId == i.Id && p.Attendee == userId);
                    if (find == null)
                        i.isShowGoing = true;

                    Following findFollow = context.Following.FirstOrDefault(p => p.FollowerId == userId && p.FolloweeId == i.LeturerIdId);
                    if (findFollow == null)
                        i.isShowFollow = true;
                }
            }
                return View(upcomingCourse);
            }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}