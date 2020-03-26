using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class StudentStudy
    {
        public StudentStudy()
        {
            StudentStudyAttendance = new HashSet<StudentStudyAttendance>();
        }

        public int Id { get; set; }
        public string StudentId { get; set; }
        public int ClassSubjectId { get; set; }

        public virtual ClassSubject ClassSubject { get; set; }
        public virtual Student Student { get; set; }
        public virtual ICollection<StudentStudyAttendance> StudentStudyAttendance { get; set; }
    }
}
