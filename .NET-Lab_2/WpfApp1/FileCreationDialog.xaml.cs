using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace WpfApp1
{
    public partial class FileCreationDialog : Window
    {
        private string newFilePath;
        private string newFileName;
        private string directoryPath;
        private bool successful = false;

        public FileCreationDialog(string path)
        {
            InitializeComponent();
            this.directoryPath = path;
        }

        public void OnOk(object sender, RoutedEventArgs e)
        {
            bool isFile = (bool)fileOption.IsChecked;
            bool isDirectory = (bool)directoryOption.IsChecked;

            // Sprawdź, czy wybrane opcje są poprawne
            if (!isFile && !isDirectory)
            {
                MessageBox.Show("Choose file or directory", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (isFile && !Regex.IsMatch(dialogName.Text, "^[a-zA-Z0-9_~-]{1,8}\\.(txt|php|html)$"))
            {
                MessageBox.Show("Wrong file name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Ustaw nową nazwę i ścieżkę
            newFileName = dialogName.Text;
            newFilePath = Path.Combine(directoryPath, newFileName);

            // Ustaw atrybuty pliku
            FileAttributes attributes = FileAttributes.Normal;
            if ((bool)readOnlyOption.IsChecked)
            {
                attributes |= FileAttributes.ReadOnly;
            }
            if ((bool)archiveOption.IsChecked)
            {
                attributes |= FileAttributes.Archive;
            }
            if ((bool)hiddenOption.IsChecked)
            {
                attributes |= FileAttributes.Hidden;
            }
            if ((bool)systemOption.IsChecked)
            {
                attributes |= FileAttributes.System;
            }

            // Utwórz plik lub katalog
            if (isFile)
            {
                File.Create(newFilePath);
            }
            else if (isDirectory)
            {
                Directory.CreateDirectory(newFilePath);
            }

            // Ustaw atrybuty dla pliku lub katalogu
            File.SetAttributes(newFilePath, attributes);

            // Zakończ pomyślnie i zamknij okno dialogowe
            successful = true;
            Close();
        }

        private void OnCancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public bool isSuccessful()
        {
            return successful;
        }

        public string GetNewFilePath()
        {
            return newFilePath;
        }
    }
}
