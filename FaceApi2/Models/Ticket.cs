using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class Ticket
    {
        public Ticket()
        {
            StudentTeacherTicket = new HashSet<StudentTeacherTicket>();
        }

        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }

        public virtual ICollection<StudentTeacherTicket> StudentTeacherTicket { get; set; }
    }
}
