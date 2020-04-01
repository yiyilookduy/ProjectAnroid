using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class Student
    {
        public Student()
        {
            StudentStudy = new HashSet<StudentStudy>();
            StudentTeacherTicket = new HashSet<StudentTeacherTicket>();
        }

        public string Id { get; set; }
        public string Fullname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool? Active { get; set; }
        public string PersonId { get; set; }
        public string Lpgid { get; set; }

        public virtual Users IdNavigation { get; set; }
        public virtual ICollection<StudentStudy> StudentStudy { get; set; }
        public virtual ICollection<StudentTeacherTicket> StudentTeacherTicket { get; set; }
    }
}
