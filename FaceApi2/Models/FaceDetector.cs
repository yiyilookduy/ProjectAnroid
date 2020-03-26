using System;
using System.Collections.Generic;

namespace FaceApi2.Models
{
    public partial class FaceDetector
    {
        public FaceDetector()
        {
            MappingFace = new HashSet<MappingFace>();
        }

        public int Id { get; set; }
        public int? CameraId { get; set; }
        public string Url { get; set; }
        public DateTime? TimeCaptured { get; set; }

        public virtual Camera Camera { get; set; }
        public virtual ICollection<MappingFace> MappingFace { get; set; }
    }
}
