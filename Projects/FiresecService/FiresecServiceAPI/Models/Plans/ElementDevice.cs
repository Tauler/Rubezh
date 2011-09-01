﻿using System.Runtime.Serialization;

namespace FiresecAPI.Models
{
    [DataContract]
    public class ElementDevice
    {
        [DataMember]
        public int idElementCanvas;

        [DataMember]
        public double Left { get; set; }

        [DataMember]
        public double Top { get; set; }

        [DataMember]
        public double Width { get; set; }

        [DataMember]
        public double Height { get; set; }

        [DataMember]
        public string Id { get; set; }
    }
}
