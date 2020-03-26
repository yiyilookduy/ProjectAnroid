using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class Schedule
    {
        public Schedule()
        {
            ClassSubjectSchedule = new HashSet<ClassSubjectSchedule>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Slot { get; set; }

        public virtual ICollection<ClassSubjectSchedule> ClassSubjectSchedule { get; set; }
    }
}
