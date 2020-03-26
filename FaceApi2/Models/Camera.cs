using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class Camera
    {
        public Camera()
        {
            FaceDetector = new HashSet<FaceDetector>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Location { get; set; }
        public bool? Active { get; set; }

        public virtual ICollection<FaceDetector> FaceDetector { get; set; }
    }
}
