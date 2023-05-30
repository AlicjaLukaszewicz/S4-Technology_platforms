using System;
using System.Xml.Serialization;

namespace PT_Lab_4
{
    public class Engine : IComparable
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

        public int CompareTo(object obj)
        {
            Engine other = (Engine)obj;
            return horsePower.CompareTo(other.horsePower);
        }
    }
}
