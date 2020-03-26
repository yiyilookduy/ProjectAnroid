using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class Subject
    {
        public Subject()
        {
            ClassSubject = new HashSet<ClassSubject>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ClassSubject> ClassSubject { get; set; }
    }
}
