using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class Users
    {
        public Users()
        {
            MappingFace = new HashSet<MappingFace>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public bool? Active { get; set; }

        public virtual Roles Role { get; set; }
        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<MappingFace> MappingFace { get; set; }
    }
}
