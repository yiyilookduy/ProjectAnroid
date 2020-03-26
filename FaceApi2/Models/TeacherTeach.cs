using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class TeacherTeach
    {
        public int Id { get; set; }
        public int ClassSubjectId { get; set; }
        public string TeacherId { get; set; }

        public virtual ClassSubject ClassSubject { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
