using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace PT_Lab_3
{
    public class Engine
    {
        public double displacement { get; set; }
        public double horsePower { get; set; }
        [XmlAttribute("model")]
        public string model { get; set; }

        public Engine() { }
        public Engine(double displacement, double horsePower, string model)
        {
            this.displacement = displacement;
            this.horsePower = horsePower;
            this.model = model;
        }
    }
}
