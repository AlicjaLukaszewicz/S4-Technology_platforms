using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Media;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PT_Lab_4
{
    public partial class MainWindow : Window
    {
        private CarBindingList<Car> myCarsBindingList;
        private BindingSource carBindingSource;
        private Dictionary<string, bool> sortingASC = new Dictionary<string, bool>();
        List<Car> myCars = new List<Car>(){
            new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
            new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
            new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
            new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
            new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
            new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
            new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
            new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
            new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
        };

        public MainWindow()
        {
            InitializeComponent();
            InitializeComboBox();
            InitializeSorting();

            myCarsBindingList = new CarBindingList<Car>(myCars);
            carBindingSource = new BindingSource();

            carBindingSource.DataSource = myCarsBindingList;
            dataGridView1.ItemsSource = carBindingSource;

            LinqQueries(myCars);
            SortAndDisplayCarsWithTDIMotorDescending(myCars);
        }

        private void InitializeComboBox()
        {
            BindingList<string> list = new BindingList<string>();
            // Stworzenie listy z wartościami
            list.Add("model");
            list.Add("year");
            list.Add("motor.displacement");
            list.Add("motor.model");
            list.Add("motor.horsePower");
            // Połączenie listy z "comboBox"
            comboBox.ItemsSource = list;
            comboBox.SelectedIndex = 0; // Podstawowa wartość
        }
        private void InitializeSorting()
        {
            // Ustawienie podstawowych wartości sortowań
            sortingASC.Clear();
            sortingASC.Add("model", false);
            sortingASC.Add("motor", false);
            sortingASC.Add("year", false);
        }

        private void FindOnClick(object sender, RoutedEventArgs e)
        {
            // W pierwszej kolejności wywoływana jest metoda UpdateCarBindingList(), która aktualizuje listę obiektów Car
            UpdateCarBindingList();
            List<Car> resultListOfCars;
            Int32 tmp;

            // Jeśli pole tekstowe nie jest puste, wywoływana jest metoda FindCars, która zwraca listę samochodów spełniających określone kryteria wyszukiwania
            if (!searchTextBox.Text.Equals(""))
            {
                string property = comboBox.SelectedItem.ToString();
                if (Int32.TryParse(searchTextBox.Text, out tmp))
                {
                    resultListOfCars = myCarsBindingList.FindMatchingCars(property, tmp);
                }
                else
                {
                    resultListOfCars = myCarsBindingList.FindMatchingCars(property, searchTextBox.Text);
                }

                // Aktualizacja wartości IsMatching w istniejącej liście obiektów Car
                // Każdy obiekt samochodu w liście ma ustawioną wartość isMatching na true, jeśli znajduje się na liście resultListOfCars, czyli spełnia określone kryteria wyszukiwania
                foreach (Car car in myCarsBindingList)
                {
                    car.isMatching = resultListOfCars.Contains(car);
                }

                // Po aktualizacji wartości IsMatching, wywoływana jest metoda UpdateDataGrid, która przekazuje zaktualizowaną listę 
                // obiektów Car do kontrolki DataGrid, gdzie wiersze opisujące samochody, które spełniają kryteria wyszukiwania, zostaną podświetlone na niebiesko
                UpdateDataGrid();
            }
        }
        private void SortOnClick(object sender, RoutedEventArgs e)
        {
            // Pozyskanie nazwy wybranej kolumny
            var columnHeader = sender as DataGridColumnHeader;
            string columnName = columnHeader.ToString().Split(' ')[1].ToLower();
            bool isAsc = sortingASC[columnName];
            InitializeSorting();

            // Sortowanie w zalezności od tego jak posortowana jest kolumna
            if (isAsc == true)
            {
                myCarsBindingList.Sort(columnName, ListSortDirection.Descending);
            }
            else
            {
                myCarsBindingList.Sort(columnName, ListSortDirection.Ascending);
            }
            sortingASC[columnName] = !isAsc;
            // Zaktualizowanie wyglądu tabeli
            UpdateDataGrid();
        }
        private void DeleteOnClick(object sender, RoutedEventArgs e)
        {
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                // Szukanie wybranego obiektu do usunięcia
                if (vis is DataGridRow)
                {
                    // Przypisanie kolumny do usunięcia
                    var row = (DataGridRow)vis;
                    Car car = (Car)row.Item;
                    // Usunięcie z listy samochodu
                    myCarsBindingList.Remove(car);
                    // Usunięcie z llisty inicjalizacyjnej
                    myCars.Remove(car);
                    // Zaktualizowanie wyglądu tabeli
                    UpdateDataGrid();
                    break;
                }
        }

        private void UpdateCarBindingList()
        {
            // Zaktualizowanie listy samochodów z użyciem listy inicjalizacyjnej "myCars"
            foreach (Car item in myCarsBindingList)
            {
                if (!myCars.Contains(item))
                {
                    myCars.Add(item);
                }
            }

            myCarsBindingList = new CarBindingList<Car>(myCars);
        }
        private void UpdateDataGrid()
        {
            carBindingSource.DataSource = myCarsBindingList;
            dataGridView1.ItemsSource = carBindingSource;
        }

        private static void LinqQueries(List<Car> myCars)
        {
            // Zapytanie nr. 1 -
            // Projekcja samochodów A6 na typ silnika i moc jednostkową
            var a6EnginePower = myCars
                .Where(car => car.model.Equals("A6"))
                .Select(car => new
                {
                    // Przypisz typ silnika w zależności od modelu
                    engineType = car.motor.model.Equals("TDI") ? "diesel" : "petrol",
                    // Oblicz moc jednostkową w koniach mechanicznych na litr
                    hppl = car.motor.horsePower / car.motor.displacement,
                });

            var message = new StringBuilder();
            foreach (var car in a6EnginePower)
            {
                message.AppendLine($"{car.engineType}: {car.hppl}");
            }
            MessageBox.Show(message.ToString(), "Wynik działania programu");
            

            // Zapytanie nr. 2 -
            // Grupuj według typu silnika i obliczaj średnią moc w koniach mechanicznych na litr
            var enginePowerByType = from car in a6EnginePower
                                    group car.hppl by car.engineType into engineGroup
                                    select new
                                    {
                                        // Pobierz klucz grupy, czyli typ silnika
                                        engineType = engineGroup.Key,
                                        // Oblicz średnią moc w koniach mechanicznych na litr
                                        avgHppl = engineGroup.Average()
                                    };


            message = new StringBuilder();
            foreach (var engineType in enginePowerByType)
            {
                message.AppendLine($"{engineType.engineType}: {engineType.avgHppl}");
            }
            MessageBox.Show(message.ToString(), "Wynik działania programu");
        }

        private void SortAndDisplayCarsWithTDIMotorDescending(List<Car> myCars)
        {
            myCars.Sort(new Comparison<Car>(arg1));
            myCars.FindAll(arg2).ForEach(arg3);
        }
        Func<Car, Car, int> arg1 = delegate (Car car1, Car car2)
        {
            return car2.motor.horsePower.CompareTo(car1.motor.horsePower);
        };
        Predicate<Car> arg2 = delegate (Car car)
        {
            return car.motor.model == "TDI";
        };
        Action<Car> arg3 = delegate(Car car)
        {
            string message = string.Format("{0} {1} ({2} hp) {3} {4}", 
                car.model, car.motor.displacement, car.motor.horsePower, car.motor.model, car.year);
            MessageBox.Show(message);
        };
    }
}
