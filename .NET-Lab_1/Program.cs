using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace PT_Lab_1
{
    static class Program
    {
        static void Main(string[] args)
        {
            string directoryPath = args[0];
            DirectoryInfo directory = new DirectoryInfo(directoryPath);
            DisplayDirectoryContent(directory, 0);
            DisplayDateOfOldestFile(directory);
            SortedDictionary<string, int> collection = UploadDirectoryToCollection(directory);
            Serialize(collection);
        }
        private static void DisplayDirectoryContent(DirectoryInfo directory, int intendLevel)
        {
            DirectoryInfo[] subdirectories = directory.GetDirectories();
            FileInfo[] files = directory.GetFiles();

            // Wyświetlenie podkatalogów
            foreach (var subdirectory in subdirectories)
            {
                PrintIndentation(intendLevel);
                // W przypadku katalogu rozmiar to liczba elementów, które bezpośrednio zawiera
                int numberOfEntries = 
                    subdirectory.GetDirectories().Length + subdirectory.GetFiles().Length;

                Console.WriteLine("{0} ({1}) {2}",
                    subdirectory.Name, numberOfEntries, subdirectory.GetDOSAttributes());

                // Wyświetlenie zawartości podkatalogu rekurencyjnie
                DisplayDirectoryContent(subdirectory, intendLevel+1);
            }

            // Wyświetlenie plików nie będących katalogami
            foreach (var file in files)
            {
                PrintIndentation(intendLevel);
                Console.WriteLine("{0} {1} bajtow {2}",
                    file.Name, file.Length, file.GetDOSAttributes());
            }
        }
        private static void PrintIndentation(int intendLevel)
        {
            for (int i = 0; i < intendLevel; i++) Console.Write("        ");
        }
        public static string GetDOSAttributes(this FileSystemInfo info)
        {
            string output = "----";             // String, który zostanie poddany edycji i zwrócony 
            char[] rahs = output.ToCharArray(); // Zamiana stringa na tablice char, w celu umożliwienia edycji
            FileAttributes fileAttributes = info.Attributes;

            // Zamiana poszczególnych części tablicy rahs w zależności od atrybutów pliku
            if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                rahs[0] = 'r';
            if ((fileAttributes & FileAttributes.Archive) == FileAttributes.Archive)
                rahs[1] = 'a';
            if ((fileAttributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                rahs[2] = 'h';
            if ((fileAttributes & FileAttributes.System) == FileAttributes.System)
                rahs[3] = 's';

            output = new string(rahs);          // Zaktualizowanie output'u na podstawie zmodyfikowanej tablicy

            return output;
        }
        public static DateTime GetDateOfOldestFileInDirectory(this DirectoryInfo directory)
        {
            DateTime dateOfTheOldestFile = DateTime.MaxValue;

            DirectoryInfo[] subdirestories = directory.GetDirectories();
            FileInfo[] files = directory.GetFiles();

            foreach (var subdirectory in subdirestories)
            {
                // Pierwotnie data najstarszego pliku podkatalogu to data stworzenia podkatalogu
                DateTime dateOfTheOldestFileInSubdirectory = subdirectory.CreationTime;
                dateOfTheOldestFileInSubdirectory = GetDateOfOldestFileInDirectory(subdirectory);

                if (dateOfTheOldestFileInSubdirectory < dateOfTheOldestFile)
                    dateOfTheOldestFile = dateOfTheOldestFileInSubdirectory;
            }

            foreach (var file in files)
            {
                DateTime creationDateOfFile = file.CreationTime;
                if (creationDateOfFile < dateOfTheOldestFile)
                    dateOfTheOldestFile = creationDateOfFile;
            }

            return dateOfTheOldestFile;
        }
        private static void DisplayDateOfOldestFile(DirectoryInfo directory)
        {
            Console.WriteLine("\nNajstarszy plik: {0}\n", GetDateOfOldestFileInDirectory(directory));
        }
        private static SortedDictionary<string, int> UploadDirectoryToCollection(DirectoryInfo directory)
        {
            String directoryPath = directory.FullName;

            SortedDictionary<string, int> collection =
                 new SortedDictionary<string, int>(new StringComparer());

            // Iteracja każdego elementu zawartego w katalogu i dodanie go do kolekcji
            foreach (var entry in Directory.EnumerateFileSystemEntries(directoryPath))
            {
                if (Directory.Exists(entry))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(entry);
                    int numEntries =
                        directoryInfo.GetDirectories().Length + directoryInfo.GetFiles().Length;

                    collection.Add(directoryInfo.Name, numEntries);
                    // W przypadku podkatalogu dodajemy również jego zawartość poprzez rekurencyjne wywołanie funkci
                    UploadDirectoryToCollection(directoryInfo);
                }
                else if (File.Exists(entry))
                {
                    FileInfo fileInfo = new FileInfo(entry);
                    collection.Add(fileInfo.Name, (int)fileInfo.Length);
                }
            }
            return collection;
        }
        public static void Serialize(SortedDictionary<string, int> collection)
        {
            FileStream stream = new FileStream("DataFile.dat", FileMode.Create);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, collection);

            stream.Close();
            collection = Deserialize(collection);
        }
        public static SortedDictionary<string, int> Deserialize(SortedDictionary<string, int> collection)
        {
            FileStream stream = new FileStream("DataFile.dat", FileMode.Open);

            BinaryFormatter formatter = new BinaryFormatter();
            collection = (SortedDictionary<string, int>)formatter.Deserialize(stream);

            foreach (var element in collection)
                Console.WriteLine("{0} -> {1}", element.Key, element.Value);
           
            return collection;
        }
    }
}
