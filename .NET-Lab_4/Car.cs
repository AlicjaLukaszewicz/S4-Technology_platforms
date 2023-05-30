using System;
using System.Xml.Serialization;

namespace PT_Lab_4
{
    [Serializable()]
    [XmlType(TypeName = "car")]
    public class Car : IComparable<Car>
    {
        public string model { get; set; }
        public int year { get; set; }

        [XmlElement(ElementName = "engine")]
        public Engine motor { get; set; }
        public bool isMatching { get; set; }

        public Car() { }
        public Car(string model, Engine motor, int year)
        {
            this.model = model;
            this.motor = motor;
            this.year = year;
        }

        public int CompareTo(Car other)
        {
            // Porównaj auta na podstawie roku produkcji
            return this.year.CompareTo(other.year);
        }
    }
}
