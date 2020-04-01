using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            StudentTeacherTicket = new HashSet<StudentTeacherTicket>();
            TeacherTeach = new HashSet<TeacherTeach>();
        }

        public string Id { get; set; }
        public string PersonId { get; set; }
        public string Pgid { get; set; }

        public virtual Users IdNavigation { get; set; }
        public virtual ICollection<StudentTeacherTicket> StudentTeacherTicket { get; set; }
        public virtual ICollection<TeacherTeach> TeacherTeach { get; set; }
    }
}
