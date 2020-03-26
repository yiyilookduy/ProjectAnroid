using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class MappingFace
    {
        public int Id { get; set; }
        public int? ImageId { get; set; }
        public string Username { get; set; }

        public virtual FaceDetector Image { get; set; }
        public virtual Users UsernameNavigation { get; set; }
    }
}
