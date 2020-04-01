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
        #region Schedule
        [HttpGet("/Attendance/GetScheduleOnCurrentWeek")]
        public IActionResult GetScheduleOnCurrentWeek(string studentId)
        {
            CheckValid valid = new CheckValid();
            try
            {
                if (!valid.IsExistedStudentId(studentId))
                    valid.IsValid = false;

                if (valid.IsValid)
                {
                    var listClassSubject = GetClassSubjectByStudentId(studentId);

                    if (listClassSubject.Count > 0)
                    {
                        Dictionary<string, Object> dicClassSubjectSchedule = new Dictionary<string, object>();

                        var mondayInWeek = DateTime.Now.StartOfWeek(DayOfWeek.Monday);

                        var sundayInWeek = mondayInWeek.AddDays(6.0);

                        foreach (var classSubject in listClassSubject)
                        {
                            var ds = GetScheduleByClassSubjectId(classSubject.Id);

                            var scheduleOfClassSubject = GetScheduleByClassSubjectId(classSubject.Id).Where(x => x.Date >= mondayInWeek && x.Date <= sundayInWeek).ToList();

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

                        if (dicClassSubjectSchedule.Count > 0)
                        {
                            return Ok(new BaseResponse(dicClassSubjectSchedule, "Success with schedule", true));
                        }
                        else
                        {
                            return Ok(new BaseResponse(dicClassSubjectSchedule, "Success but not have any schedule on this week", true));
                        }

                    }
                    else
                    {
                        return NotFound(new BaseResponse(null, "Student is not study any subject", false));
                    }
                }

                return NotFound(new BaseResponse(null, "Student ID not found", false));



            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));
            }
        }

        /// <summary>
        /// Get Schedule on week by Student Id and Date
        /// </summary>
        /// <param name="studentId">Student ID of student</param>
        /// <param name="date">What date you want query, it will return a week contain of this date</param>
        /// <returns>Return a dictionary of schedule with key is Date/Slot and Value is Subject Name, Class Name, Status of attendance</returns>
        [HttpGet("/Attendance/GetScheduleOnWeek")]
        public IActionResult GetScheduleOnWeek(string studentId, DateTime date)
        {
            CheckValid valid = new CheckValid();
            try
            {
                if (!valid.IsExistedStudentId(studentId))
                    valid.IsValid = false;
                if (valid.StringIsNullOrEmpty(date.ToString()))
                    valid.IsValid = false;

                if (valid.IsValid)
                {
                    var listClassSubject = GetClassSubjectByStudentId(studentId);

                    if (listClassSubject != null)
                    {
                        Dictionary<string, Object> dicClassSubjectSchedule = new Dictionary<string, object>();

                        var mondayInWeek = date.StartOfWeek(DayOfWeek.Monday);

                        var sundayInWeek = mondayInWeek.AddDays(6.0);

                        foreach (var classSubject in listClassSubject)
                        {
                            var ds = GetScheduleByClassSubjectId(classSubject.Id);

                            var scheduleOfClassSubject = GetScheduleByClassSubjectId(classSubject.Id).Where(x => x.Date >= mondayInWeek && x.Date <= sundayInWeek).ToList();

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

                        if (dicClassSubjectSchedule.Count > 0)
                        {
                            return Ok(new BaseResponse(dicClassSubjectSchedule, "Success with schedule", true));
                        }

                        return Ok(new BaseResponse(dicClassSubjectSchedule, "Success but not have any schedule on this week", true));

                    }

                    return NotFound(new BaseResponse(null, "Student is not study any subject", false));
                }

                return NotFound(new BaseResponse(null, valid.ErrorMessage, false));

            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));
            }


        }
        #endregion

        #region Query in database
        /// <summary>
        /// Get subjects which studying by student
        /// </summary>
        /// <param name="studentId">Student ID</param>
        /// <returns></returns>
        [HttpGet("/Attendance/GetStudentSubjectStudy")]
        public IActionResult GetStudentSubject(string studentId)
        {
            CheckValid valid = new CheckValid();
            try
            {
                if (!valid.IsExistedStudentId(studentId))
                    valid.IsValid = false;
                if (valid.IsValid)
                {
                    var listClassSubject = GetClassSubjectByStudentId(studentId);

                    if (listClassSubject.Count > 0)
                    {
                        return Ok(new BaseResponse(listClassSubject, "Get list class subject success", true));
                    }
                    else
                    {
                        return NotFound(new BaseResponse(listClassSubject, "Student havent study anything", false));
                    }
                }

                return NotFound(new BaseResponse(null, valid.ErrorMessage, false));

            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));
            }
        }

        //TODO Check valid input
        [HttpGet("/Attendance/GetClassSubjectSchedule")]
        public IActionResult GetScheduleByClassSubject(int ClassSubjectId)
        {
            try
            {
                var ListSchedule = GetScheduleByClassSubjectId(ClassSubjectId);

                if (ListSchedule.Count != 0)
                {
                    return Ok(new BaseResponse(ListSchedule, "Get schedule by classSubject is success", true));

                }
                else
                    return NotFound(new BaseResponse(ListSchedule, "Not have any schedule on database", false));
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));
            }
        }

        //TODO Check valid input
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
        #endregion

        #region Attendance
        [HttpPost("/Attendance/UpdateAttendance")]
        public IActionResult UpdateAttendance(string studentId, bool attendance)
        {
            CheckValid valid = new CheckValid();
            var timeRequest = DateTime.Now;
            var slotJoin = getSlot(timeRequest);
            var check = false;

            try
            {
                if (!valid.IsExistedStudentId(studentId))
                    valid.IsValid = false;
                if (valid.StringIsNullOrEmpty(attendance.ToString()))
                    valid.IsValid = false;

                if (valid.IsValid)
                {
                    var listClassSubject = GetClassSubjectByStudentId(studentId);

                    if (listClassSubject.Count > 0)
                    {
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
                            return Ok( new BaseResponse(null, "Update attendance success", true));
                        }
                    }

                    return NotFound(new BaseResponse(null, "Not found any subject by student ID", false));
                }

                return NotFound(new BaseResponse(null, valid.ErrorMessage, false));


            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse(null, e.Message, false));
            }
        }
        #endregion

        #region Functions
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
        #endregion
    }
}