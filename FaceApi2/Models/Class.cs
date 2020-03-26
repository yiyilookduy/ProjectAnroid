using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class Class
    {
        public Class()
        {
            ClassSubject = new HashSet<ClassSubject>();
        }

        public string Id { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public virtual ICollection<ClassSubject> ClassSubject { get; set; }
    }
}
