
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
        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]

        public ActionResult Edit(int id)
        {
            BigSchoolContext content = new BigSchoolContext();
            Course course = content.Course.SingleOrDefault(p => p.Id == id);
            course.ListCategory = content.Category.ToList();
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        [Authorize]
        [HttpPost]

        public ActionResult Edit(Course course)
        {
            BigSchoolContext content = new BigSchoolContext();
            Course courseUpdate = content.Course.FirstOrDefault(p => p.Id == course.Id);
            if (courseUpdate != null)
            {
                content.Course.AddOrUpdate(course);
                content.SaveChanges();
                return RedirectToAction("Index", "Home");

            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Delete(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            var course = context.Course.SingleOrDefault(p => p.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteCourses(int id)
        {
            BigSchoolContext context = new BigSchoolContext();

            var Xoacourse = context.Course.SingleOrDefault(p => p.Id == id);
            if (Xoacourse != null)
            {
                Attendance xoaAtendence = context.Attendance.SingleOrDefault(p => p.CourseId == id);
                if (xoaAtendence != null)
                {
                    context.Attendance.Remove(xoaAtendence);
                    context.Course.Remove(Xoacourse);
                }
                context.Course.Remove(Xoacourse);
                context.SaveChanges();
                return RedirectToAction("Mine", "Courses");
            }

            return RedirectToAction("Mine", "Courses");


        }
        public ActionResult LectureIamGoing()
        {
            //ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            //    .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            //BigSchoolContext context = new BigSchoolContext();

            //var listFollowee = context.Following.Where(p => p.FollowerId == currentUser.Id).ToList();

            //var listAttendances = context.Attendance.Where(p => p.Attendee == currentUser.Id).ToList();
            //var courses = new List<Course>();
            //foreach( var course in listAttendances)
            //{
            //    foreach(var item in listFollowee)
            //    {
            //        if(item.FolloweeId == course.Course.LeturerIdId)
            //        {
            //            Course objCourse = course.Course;
            //            objCourse.Name = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            //                .FindById(objCourse.LeturerIdId).Name;
            //            courses.Add(objCourse);
            //        }
            //    }
            //}
            //return View(courses);  
            ApplicationUser currentUser =
System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
.FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            //danh sách giảng viên được theo dõi bởi người dùng (đăng nhập) hiện tại
            var listFollwee = context.Following.Where(p => p.FollowerId ==

            currentUser.Id).ToList();

            //danh sách các khóa học mà người dùng đã đăng ký
            var listAttendances = context.Attendance.Where(p => p.Attendee ==

            currentUser.Id).ToList();

            var courses = new List<Course>();
            foreach (var course in listAttendances)

            {
                foreach (var item in listFollwee)
                {
                    if (item.FolloweeId == course.Course.LeturerIdId)
                    {
                        Course objCourse = course.Course;
                        objCourse.LectureName =
                        System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                        .FindById(objCourse.LeturerIdId).Name;
                        courses.Add(objCourse);
                    }
                }
            }
            return View(courses);
        }
    }
}
