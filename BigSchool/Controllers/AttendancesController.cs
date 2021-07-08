using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BigSchool.Models;
using Microsoft.AspNet.Identity;

namespace BigSchool.Controllers
{
    public class AttendancesController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Attend(Course attendanceDto)
        {
            var userId = User.Identity.GetUserId();
            BigSchoolContext context = new BigSchoolContext();
            if (context.Attendance.Any(p => p.Attendee == userId && p.CourseId == attendanceDto.Id))
            {
                return BadRequest("The attendance already exists!!!");
            }
            var attendance = new Attendance() { CourseId = attendanceDto.Id, Attendee = User.Identity.GetUserId() };
            context.Attendance.Add(attendance);
            context.SaveChanges();

            return Ok();
        }
    }
}
