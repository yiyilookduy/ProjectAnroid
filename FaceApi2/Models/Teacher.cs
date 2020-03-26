using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class Teacher
    {
        public Teacher()
        {
            TeacherTeach = new HashSet<TeacherTeach>();
        }

        public string Id { get; set; }
        public string FullName { get; set; }
        public bool Active { get; set; }
        public string PersonId { get; set; }
        public string Lpgid { get; set; }

        public virtual Users IdNavigation { get; set; }
        public virtual ICollection<TeacherTeach> TeacherTeach { get; set; }
    }
}
