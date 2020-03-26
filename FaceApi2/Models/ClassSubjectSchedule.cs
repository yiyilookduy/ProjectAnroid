using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class ClassSubjectSchedule
    {
        public ClassSubjectSchedule()
        {
            StudentStudyAttendance = new HashSet<StudentStudyAttendance>();
        }

        public int Id { get; set; }
        public int? ClassSubjectId { get; set; }
        public int? ScheduleId { get; set; }

        public virtual ClassSubject ClassSubject { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual ICollection<StudentStudyAttendance> StudentStudyAttendance { get; set; }
    }
}
