using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PT_Lab_4
{
    public class CarBindingList<K> : BindingList<Car>
        where K : IComparable<K>
    {
        // Właściwość do sortowania
        private PropertyDescriptor _sortProperty;
        // Kierunek sortowania (rosnąco lub malejąco)
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;

        public CarBindingList(List<Car> cars)
        {
            foreach (var car in cars)
            {
                Add(car);
            }
        }
        // Metoda sortująca
        public void Sort(string property, ListSortDirection direction)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(Car));
            PropertyDescriptor prop = properties.Find(property, true);
            if (prop != null)
            {
                ApplySortCore(prop, direction);
            }
            else
            {
                throw new NotSupportedException($"Cannot sort by {prop.Name}, this property doesn\'t exist.");
            }
        }
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            var sortedList = new List<object>();
            var unsortedList = new List<Car>(Count);

            if (prop.PropertyType.GetInterface("IComparable") != null)
            {
                _sortProperty = prop;
                _sortDirection = direction;

                foreach (Car car in Items)
                {
                    if (!sortedList.Contains(prop.GetValue(car)))
                    {
                        sortedList.Add(prop.GetValue(car));
                    }
                }

                sortedList.Sort();

                if (direction == ListSortDirection.Descending)
                {
                    sortedList.Reverse();
                }

                foreach (var propValue in sortedList)
                {
                    var foundItems = FindCore(_sortProperty.Name, propValue);
                    unsortedList.AddRange(foundItems);
                }

                if (unsortedList != null)
                {
                    Clear();

                    foreach (Car elem in unsortedList)
                    {
                        Add(elem);
                    }

                    OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
                }
            }
        }
        // Metoda szukająca elementów na podstawie wartości właściwości
        protected List<Car> FindCore(string propertyName, object value)
        {
            var items = new List<Car>();
            foreach (var car in Items)
            {
                var prop = TypeDescriptor.GetProperties(car)[propertyName];
                var propValue = prop.GetValue(car);
                if (propValue.Equals(value))
                {
                    items.Add(car);
                }
            }
            return items;
        }
        public List<Car> FindMatchingCars(string property, object key)
        {
            List<Car> listOfMatchingCars = new List<Car>();
            bool isEngine = property.Contains("motor.");

            PropertyDescriptorCollection properties =
                isEngine ? TypeDescriptor.GetProperties(typeof(Engine)) : TypeDescriptor.GetProperties(typeof(Car));
            PropertyDescriptor prop =
                properties.Find(isEngine ? property.Split('.').Last() : property, true);

            if (prop != null)
            {
                foreach (var item in this)
                {
                    if (prop.GetValue(isEngine ? item.motor : item).Equals(key))
                    {
                        listOfMatchingCars.Add(item);
                    }
                }
                return listOfMatchingCars;
            }
            return null;
        }
    }
}
