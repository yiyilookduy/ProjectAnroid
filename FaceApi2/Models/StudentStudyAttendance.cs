using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class StudentStudyAttendance
    {
        public int Id { get; set; }
        public int StudentStudyId { get; set; }
        public bool Attendance { get; set; }
        public int ClassSubjectScheduleId { get; set; }

        public virtual ClassSubjectSchedule ClassSubjectSchedule { get; set; }
        public virtual StudentStudy StudentStudy { get; set; }
    }
}
