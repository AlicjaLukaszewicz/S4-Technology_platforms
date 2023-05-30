using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace PT_Lab_3
{
    class Program
    {
        static void Main(string[] args)
        {
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

            LinqQueries(myCars);
            SerializeAndDeserialize(myCars);
            ProcessCarsData();
            createXmlFromLinq(myCars);
            GenerateHtmlTable(myCars);
            ModifyCarsCollectionXML();
        }

        // Zadanie 1
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

            foreach (var car in a6EnginePower)
            {
                Console.WriteLine($"{car.engineType}: {car.hppl}");
            }

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

            foreach (var engineType in enginePowerByType)
            {
                Console.WriteLine($"{engineType.engineType}: {engineType.avgHppl}");
            }
        }

        // Zadanie 2 - Serializacja i deserializacja
        public static void SerializeAndDeserialize(List<Car> myCars)
        {
            // Uzyskanie ścieżki do pliku
            var fileName = "CarsCollection.xml";
            var currentDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentDirectory, fileName);

            // Serializacja obiektów do pliku XML
            Serialize(myCars, filePath);

            // Deserializacja danych z pliku XML do listy obiektów Car
            var deserializedList = Deserialize(filePath);
        }
        public static void Serialize(List<Car> myCars, string filename)
        {
            XmlSerializer serializer =
                new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));

            using (TextWriter writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, myCars);
            }
        }
        public static List<Car> Deserialize(string filePath)
        {
            List<Car> list = new List<Car>();
            // Tworzenie obiektu XmlSerializer dla klasy List<Car> z atrybutem XmlRoot o nazwie "cars".
            XmlSerializer serializer =
                new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));

            // Otwarcie strumienia do pliku XML.
            using (Stream reader = new FileStream(filePath, FileMode.Open))
            {
                // Deserializacja pliku XML i zapisanie danych w liście.
                list = (List<Car>)serializer.Deserialize(reader);
            }
            return list;
        }

        // Zadanie 3
        private static void ProcessCarsData()
        {
            // Załadowanie pliku XML do elementu XElement
            XElement rootNode = XElement.Load("CarsCollection.xml");

            // Oblicz przeciętną moc samochodów o silnikach innych niż TDI
            double avgHP = (double)rootNode.XPathEvaluate("sum(//car/engine[@model!=\"TDI\"]/horsePower) div count(//car/engine[@model!=\"TDI\"]/horsePower)");

            // Zwróć modele samochodów bez powtórzeń
            IEnumerable<string> models = rootNode.XPathSelectElements("//car[not(model = preceding-sibling::car/model)]/model").Select(model => (string)model);

            Console.WriteLine($"\nŚrednia: {avgHP}");

            foreach (var model in models)
            {
                Console.WriteLine($"Model: {model}");
            };
        }

        // Zadanie 4
        private static void createXmlFromLinq(List<Car> myCars)
        {
            // Tworzenie węzłów XML z wykorzystaniem LINQ
            IEnumerable<XElement> nodes = from car in myCars
                                          select new XElement("car",
                                                     new XElement("model", car.model),
                                                     new XElement("year", car.year),
                                                     new XElement("engine",
                                                         new XElement("displacement", car.motor.displacement),
                                                         new XElement("horsePower", car.motor.horsePower),
                                                         new XAttribute("model", car.motor.model))
                                                     ); ;
            // Tworzenie korzenia XML i zapisanie do pliku
            XElement rootNode = new XElement("cars", nodes);
            rootNode.Save("CarsFromLinq.xml");
        }

        // Zadanie 5
        private static void GenerateHtmlTable(List<Car> myCars)
        {
            XElement template = XElement.Load("template.html");

            // Znajdujemy element, w którym chcemy wstawić tabelę
            XElement tableContainer = template.Element("{http://www.w3.org/1999/xhtml}body");

            // Tworzymy element table
            XElement table = new XElement("table",
                // Dla każdego obiektu Car w kolekcji tworzymy wiersz tabeli
                from car in myCars
                select new XElement("tr", new XAttribute("style", "border: 2px solid black"),
                    new XElement("td", new XAttribute("style", "border: 2px double black"), car.model),
                    new XElement("td", new XAttribute("style", "border: 2px double black"), car.motor.model),
                    new XElement("td", new XAttribute("style", "border: 2px double black"), car.motor.displacement),
                    new XElement("td", new XAttribute("style", "border: 2px double black"), car.motor.horsePower),
                    new XElement("td", new XAttribute("style", "border: 2px double black"), car.year)
                )
            );

            // Dodajemy tabelę do dokumentu
            tableContainer.Add(table);

            // Zapisujemy dokument
            template.Save("output.html");
        }

        // Zadanie 6
        private static void ModifyCarsCollectionXML()
        {
            // Załadowanie pliku XML do elementu XElement
            XElement rootNode = XElement.Load("CarsCollection.xml");

            foreach (XElement car in rootNode.Elements("car"))
            {
                // Znajdź węzeł engine i zmień nazwę elementu horsePower na hp
                XElement motorElement = car.Element("engine");
                motorElement.Element("horsePower").Name = "hp";

                // Pobierz element model i year
                XElement modelElement = car.Element("model");
                XElement yearElement = car.Element("year");

                // Ustaw wartość atrybutu year dla elementu model i usuń element year
                modelElement.SetAttributeValue("year", yearElement.Value);
                yearElement.Remove();
            }
            // Zapisz zmodyfikowany plik XML
            rootNode.Save("ModifiedCarsCollection.xml");
        }
    }
}
