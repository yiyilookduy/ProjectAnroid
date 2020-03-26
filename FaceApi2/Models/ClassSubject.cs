using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class ClassSubject
    {
        public ClassSubject()
        {
            ClassSubjectSchedule = new HashSet<ClassSubjectSchedule>();
            StudentStudy = new HashSet<StudentStudy>();
            TeacherTeach = new HashSet<TeacherTeach>();
        }

        public int Id { get; set; }
        public string ClassId { get; set; }
        public string SubjectId { get; set; }

        public virtual Class Class { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<ClassSubjectSchedule> ClassSubjectSchedule { get; set; }
        public virtual ICollection<StudentStudy> StudentStudy { get; set; }
        public virtual ICollection<TeacherTeach> TeacherTeach { get; set; }
    }
}
