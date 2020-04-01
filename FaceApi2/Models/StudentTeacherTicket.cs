using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class StudentTeacherTicket
    {
        public int Id { get; set; }
        public string TeacherId { get; set; }
        public string StudentId { get; set; }
        public int TicketId { get; set; }

        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
