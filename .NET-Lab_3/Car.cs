using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace PT_Lab_3
{
    [Serializable()]
    [XmlType(TypeName = "car")]
    public class Car
    {
        public string model { get; set; }
        public int year { get; set; }

        [XmlElement(ElementName = "engine")]
        public Engine motor { get; set; }

        public Car() { }

        public Car(string model, Engine motor, int year)
        {
            this.model = model;
            this.motor = motor;
            this.year = year;
        }
    }
}
