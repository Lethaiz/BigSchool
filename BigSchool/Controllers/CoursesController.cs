
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigSchool;
using BigSchool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace lab4.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Create()
        {
            BigSchoolContext context = new BigSchoolContext();
            Course objCourse = new Course();
            objCourse.ListCategory = context.Category.ToList();
            return View(objCourse);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse)
        {
            BigSchoolContext context = new BigSchoolContext();
            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.ListCategory = context.Category.ToList();
                return View("Create", objCourse);
            }
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LeturerIdId = user.Id;

            context.Course.Add(objCourse);
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendance.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Attendance temp in listAttendances)
            {
                Course objCourse = temp.Course;
                objCourse.Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                     .FindById(objCourse.LeturerIdId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }
        public ActionResult Mine()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var courses = context.Course.Where(c => c.LeturerIdId == currentUser.Id  /*&& c.Datetime > Datetime.Now*/).ToList();

            foreach (Course i in courses)
            {
                i.LeturerIdId = currentUser.Name;
            }
            return View(courses);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(int id)
        {

            BigSchoolContext context = new BigSchoolContext();
            List<Course> listBook = context.Course.ToList();
            Course course = context.Course.SingleOrDefault(p => p.Id == id);
            course = new Course();
            if (course != null)
            {
                return HttpNotFound();
            }
            return View(course);

        }
        [HttpPost]

        public ActionResult Edit(Course course)
        {

            BigSchoolContext context = new BigSchoolContext();
            List<Course> listBook = context.Course.ToList();
            Course dbUpdate = context.Course.SingleOrDefault(p => p.Id == course.Id);
                if (dbUpdate != null)
                {
                context.Course.AddOrUpdate(course);
                    context.SaveChanges();
                }
                return RedirectToAction("index");
            }

            //    BigSchoolContext context = new BigSchoolContext();
            //    var UserId = User.Identity.GetUserId();
            //    var course = context.Course.Single(m => m.Id == id && m.LeturerIdId == UserId);
            //    var viewModel = new Course
            //    {

            //    ListCategory = context.Category.ToList()
            //    Datetime = course.Datetime.ToString()


            //    }
            //    return View();
        }
      
    }
