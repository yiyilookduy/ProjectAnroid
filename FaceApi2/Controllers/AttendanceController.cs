using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BabyTrackingAPI.ModelAPIs;
using FaceApi2.ModelAPIs;
using FaceApi2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace FaceApi2.Controllers
{
    public class AttendanceController : Controller
    {

        [HttpGet("/Attendance/GetScheduleOnCurrentWeek")]
        public IActionResult GetScheduleOnCurrentWeek(string studentId)
        {
            var listClassSubject = GetClassSubjectByStudentId(studentId);

            Dictionary<string, Object> dicClassSubjectSchedule = new Dictionary<string, object>();

            DateTime ed = new DateTime(2020,03,17);

            var ks = ed.StartOfWeek(DayOfWeek.Monday);

            var se = ks.AddDays(6.0);

            foreach (var classSubject in listClassSubject)
            {
                var ds = GetScheduleByClassSubjectId(classSubject.Id);

                var scheduleOfClassSubject = GetScheduleByClassSubjectId(classSubject.Id).Where(x => x.Date >= ks && x.Date <= se).ToList();

                 foreach (var schedule in scheduleOfClassSubject)
                {
                    string s = $"{schedule.Date.DayOfWeek.ToString()}/{schedule.Slot}";

                    int a = schedule.ClassSubjectSchedule.ToList()[0].Id;

                    var attendance = GetAttendance(studentId, schedule.ClassSubjectSchedule.ToList()[0].Id,
                        classSubject.Id);

                    dicClassSubjectSchedule.Add(s, new
                    {
                        subject = classSubject.SubjectId,
                        classId = classSubject.ClassId,
                        atten = attendance.Attendance
                    });
                }
            }

            return Ok(new BaseResponse(dicClassSubjectSchedule, "", true));


        }


        [HttpGet("/Attendance/GetStudentSubjectStudy")]
        public IActionResult GetStudentSubject(string studentId)
        {
            try
            {
                var listClassSubject = GetClassSubjectByStudentId(studentId);

                if (listClassSubject.Count != 0)
                {
                    return Ok(new BaseResponse(listClassSubject, "", true));
                }
                else
                {
                    return NotFound(new BaseResponse(listClassSubject, "Student havent study anything", false));
                }
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, "There are some error while getting data", false));
            }
        }

        [HttpGet("/Attendance/GetClassSubjectSchedule")]
        public IActionResult GetScheduleByClassSubject(int ClassSubjectId)
        {
            try
            {
                var ListSchedule = GetScheduleByClassSubjectId(ClassSubjectId);

                if (ListSchedule.Count != 0)
                {
                    return Ok(new BaseResponse(ListSchedule, "", true));

                }
                else
                    return NotFound(new BaseResponse(ListSchedule, "Not have any schedule on database", false));
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, "There are some error while getting data", false));
            }
        }

        [HttpGet("/Attendance/GetStudentStudyAttendance")]
        public IActionResult GetStudentStudyAttendance(string studentId, int classSubjectScheduleId, int classSubjectId)
        {
            try
            {
                var context = new FaceIOContext();

                var StudentStudy = context.StudentStudy.Where(x => x.StudentId == studentId && x.ClassSubjectId == classSubjectId).FirstOrDefault() as StudentStudy;

                var attendance = GetAttendance(studentId, classSubjectScheduleId, classSubjectId);

                if (attendance != null)
                {
                    return Ok(new BaseResponse(attendance, "", true));
                }
                else
                    return NotFound(new BaseResponse(attendance, "Attendance failed", false));
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, "There are some error while getting data", false));
            }
        }



        [HttpPost("/Attendance/CheckAttendance")]
        public IActionResult CheckAttendance(string studentId, bool attendance)
        {
            var timeRequest = DateTime.Now;
            var slotJoin = getSlot(timeRequest);
            var check = false;

            try
            {
                var listClassSubject = GetClassSubjectByStudentId(studentId);

                foreach (var classSubject in listClassSubject)
                {
                    var ListSchedule = GetScheduleByClassSubjectId(classSubject.Id).Where(x => x.Date == timeRequest.Date);
                    foreach (var schedule in ListSchedule)
                    {
                        if (schedule.Slot == slotJoin)
                        {
                            var context = new FaceIOContext();
                            var classSubjectSchedule = context.ClassSubjectSchedule.Where(x =>
                                x.ClassSubjectId == classSubject.Id && x.ScheduleId == schedule.Id).FirstOrDefault();

                            var studentStudyAttendance =
                                context.StudentStudyAttendance.Where(x =>
                                    x.ClassSubjectScheduleId == classSubjectSchedule.Id).FirstOrDefault();

                            if (studentStudyAttendance != null)
                            {
                                studentStudyAttendance.Attendance = true;

                                context.StudentStudyAttendance.Update(studentStudyAttendance);

                                context.SaveChanges();

                                check = true;
                                break;
                            }
                        }

                        else continue;
                    }

                    if (check) break;
                }

                if (check == false)
                {
                    return NotFound(new BaseResponse(null, "Attendance failed", false));
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, "There are some error while getting data", false));
            }
        }


        public StudentStudyAttendance GetAttendance(string studentId, int classSubjectScheduleId,
            int classSubjectId)
        {
            var context = new FaceIOContext();

            var StudentStudy = context.StudentStudy.Where(x => x.StudentId == studentId && x.ClassSubjectId == classSubjectId).FirstOrDefault() as StudentStudy;

            var attendance = context.StudentStudyAttendance.Where(x => x.StudentStudyId == StudentStudy.Id && x.ClassSubjectScheduleId == classSubjectScheduleId).FirstOrDefault();
            return attendance;
        }
        private int getSlot(DateTime timeRequest)
        {
            if (TimeBetween(timeRequest, new TimeSpan(07, 0, 0), new TimeSpan(8, 30, 0)))
            {
                return 1;
            }
            else if (TimeBetween(timeRequest, new TimeSpan(08, 45, 0), new TimeSpan(10, 15, 0)))
            {
                return 2;
            }
            else if (TimeBetween(timeRequest, new TimeSpan(10, 30, 0), new TimeSpan(12, 00, 0)))
            {
                return 3;
            }
            else if (TimeBetween(timeRequest, new TimeSpan(12, 30, 0), new TimeSpan(2, 0, 0)))
            {
                return 4;
            }
            else if (TimeBetween(timeRequest, new TimeSpan(2, 15, 0), new TimeSpan(3, 45, 0)))
            {
                return 5;
            }
            else if (TimeBetween(timeRequest, new TimeSpan(4, 00, 0), new TimeSpan(5, 30, 0)))
            {
                return 6;
            }
            else
                return 0;
        }

        private bool TimeBetween(DateTime datetime, TimeSpan start, TimeSpan end)
        {
            // convert datetime to a TimeSpan
            TimeSpan now = datetime.TimeOfDay;
            // see if start comes before end
            if (start < end)
                return start <= now && now <= end;
            // start is after end, so do the inverse comparison
            return !(end < now && now < start);
        }

        private List<ClassSubject> GetClassSubjectByStudentId(string studentId)
        {
            var context = new FaceIOContext();
            var classSubjectId = context.StudentStudy.Where(s => s.StudentId == studentId).ToList<StudentStudy>();
            var listClassSubject = new List<ClassSubject>();
            foreach (StudentStudy ss in classSubjectId)
            {
                var classSubject = context.ClassSubject.Where(c => c.Id == ss.ClassSubjectId).FirstOrDefault();
                listClassSubject.Add(classSubject);
            }

            return listClassSubject;
        }

        private List<Schedule> GetScheduleByClassSubjectId(int ClassSubjectId)
        {
            var context = new FaceIOContext();

            var ListClassSubjectSchedule = context.ClassSubjectSchedule.Where(x => x.ClassSubjectId == ClassSubjectId).ToList<ClassSubjectSchedule>();

            var ListSchedule = new List<Schedule>();

            foreach (var ClassSubjectSchedule in ListClassSubjectSchedule)
            {
                var Schedule = context.Schedule.Where(x => x.Id == ClassSubjectSchedule.ScheduleId).FirstOrDefault();
                ListSchedule.Add(Schedule);
            }

            return ListSchedule;
        }
    }
}