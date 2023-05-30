using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExitOnClick(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OpenOnClick(object sender, RoutedEventArgs e)
        {
            // Tworzenie okna dialogowego wyboru folderu
            var dlg = new FolderBrowserDialog() {Description = "Select directory to open"};

            // Wyświetlanie okna dialogowego i sprawdzanie, czy użytkownik wybrał folder
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Tworzenie obiektu reprezentującego wybrany folder
                DirectoryInfo rootDirectoryInfo = new DirectoryInfo(dlg.SelectedPath);

                // Tworzenie elementu drzewa katalogów reprezentującego wybrany folder
                var root = CreateTreeViewDirectory(rootDirectoryInfo);

                // Czyszczenie drzewa i dodawanie nowego elementu reprezentującego wybrany folder
                treeView.Items.Clear();
                treeView.Items.Add(root);
            }
        }

        private TreeViewItem CreateTreeViewDirectory(DirectoryInfo directory)
        {
            // Tworzenie elementu drzewa reprezentującego folder
            var root = new TreeViewItem
            {
                Header = directory.Name,    // Ustawianie nazwy folderu jako nazwy węzła drzewa
                Tag = directory.FullName    // Zapisywanie pełnej ścieżki do folderu jako dane wezła drzewa
            };

            // Dodawanie do elementu drzewa podfolderów folderu reprezentowanego przez obiekt directory
            foreach (DirectoryInfo subdirectory in directory.GetDirectories())
            {
                root.Items.Add(CreateTreeViewDirectory(subdirectory));
            }

            // Dodawanie do elementu drzewa plików w folderze reprezentowanym przez obiekt directory
            foreach (FileInfo file in directory.GetFiles())
            {
                root.Items.Add(CreateTreeViewFile(file));
            }

            // Tworzenie kontekstowego menu dla folderu reprezentowanego przez element drzewa
            CreateDirectoryContextMenu(root);
            
            // Zwracanie utworzonego elementu drzewa
            return root;
        }

        private object CreateTreeViewFile(FileInfo file)
        {
            // Tworzenie elementu drzewa reprezentującego plik
            var item = new TreeViewItem
            {
                Header = file.Name, // Ustawianie nazwy pliku jako nazwy węzła drzewa
                Tag = file.FullName // Zapisywanie pełnej ścieżki do pliku jako dane wezła drzewa
            };

            // Tworzenie kontekstowego menu dla pliku reprezentowanego przez element drzewa
            CreateFileContextMenu(item);

            // Zwracanie utworzonego elementu drzewa
            return item;
        }

        private void CreateDirectoryContextMenu(TreeViewItem directory)
        {
            // Tworzenie kontekstowego menu dla folderu reprezentowanego przez element drzewa
            directory.ContextMenu = new ContextMenu();

            // Dodawanie opcji "Create" do kontekstowego menu
            var createOption = new MenuItem { Header = "Create" };
            createOption.Click += new RoutedEventHandler(DirectoryCreateOnClick);
            directory.ContextMenu.Items.Add(createOption);

            // Dodawanie opcji "Delete" do kontekstowego menu
            var deleteOption = new MenuItem { Header = "Delete" };
            deleteOption.Click += new RoutedEventHandler(DirectoryDeleteOnClick);
            directory.ContextMenu.Items.Add(deleteOption);

            // Dodawanie obsługi zdarzenia wyboru elementu drzewa, w celu aktualizacji paska stanu
            directory.Selected += new RoutedEventHandler(UpdateStatusBar);
        }

        private void CreateFileContextMenu(TreeViewItem file)
        {
            // Tworzenie kontekstowego menu dla folderu reprezentowanego przez element drzewa
            file.ContextMenu = new ContextMenu();

            // Dodawanie opcji "Open" do kontekstowego menu
            var openOption = new MenuItem { Header = "Open" };
            openOption.Click += new RoutedEventHandler(FileOpenOnClick);
            file.ContextMenu.Items.Add(openOption);

            // Dodawanie opcji "Delete" do kontekstowego menu
            var deleteOption = new MenuItem { Header = "Delete" };
            deleteOption.Click += new RoutedEventHandler(FileDeleteOnClick);
            file.ContextMenu.Items.Add(deleteOption);

            // Dodawanie obsługi zdarzenia wyboru elementu drzewa, w celu aktualizacji paska stanu
            file.Selected += new RoutedEventHandler(UpdateStatusBar);
        }

        // Funkcja obsługująca kliknięcie w opcję "Open" w menu kontekstowym pliku
        private void FileOpenOnClick(object sender, RoutedEventArgs e)
        {
            // Sprawdź, czy wybrany element w treeView jest TreeViewItem
            if (treeView.SelectedItem is TreeViewItem item)
            {
                // Pobierz pełną ścieżkę do pliku
                string path = (string)item.Tag;
                // Pobierz rozszerzenie pliku
                string extension = Path.GetExtension(path);

                // Sprawdź, czy plik ma rozszerzenie .txt
                if (extension == ".txt")
                {
                    // Odczytaj zawartość pliku i wyświetl ją w fileContents
                    fileContents.Text = File.ReadAllText((string)item.Tag);
                }
            }
        }

        // Funkcja obsługująca kliknięcie w opcję "Delete" w menu kontekstowym pliku
        private void FileDeleteOnClick(object sender, RoutedEventArgs e)
        {
            // Sprawdzenie czy zaznaczony element w drzewie to plik
            if (treeView.SelectedItem is TreeViewItem item)
            {
                // Pobranie ścieżki pliku z atrybutu "Tag" zaznaczonego elementu
                string path = (string)item.Tag;
                // Ustawienie atrybutów pliku w taki sposób, aby istniała możliwość odczytu
                File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.ReadOnly);
                // Usunięcie pliku
                File.Delete(path);
                // Usunięcie zaznaczonego elementu z drzewa, jeśli ma rodzica
                if (item.Parent is TreeViewItem parent)
                {
                    parent.Items.Remove(item);
                }
            }
        }

        // Funkcja obsługująca kliknięcie w opcję "Create" w menu kontekstowym folderu
        private void DirectoryCreateOnClick(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem is TreeViewItem selectedItem) {
                // Otwarcie okna dialogowego do tworzenia pliku/katalogu
                var fileCreationDialog = new FileCreationDialog((string)selectedItem.Tag);
                fileCreationDialog.ShowDialog();

                // Sprawdzenie, czy użytkownik potwierdził utworzenie nowego elementu
                if (fileCreationDialog.isSuccessful())
                {
                    string newFilePath = fileCreationDialog.GetNewFilePath();
                    
                    if (Directory.Exists(newFilePath))
                    {
                        // Jeśli użytkownik wybrał utworzenie nowego katalogu, to zostanie on utworzony w wybranym katalogu nadrzędnym i dodany do drzewa
                        DirectoryInfo directory = new DirectoryInfo(newFilePath);
                        selectedItem.Items.Add(CreateTreeViewDirectory(directory));
                    }
                    else
                    {
                        // Jeśli użytkownik wybrał utworzenie nowego pliku, to zostanie on utworzony w wybranym katalogu nadrzędnym i dodany do drzewa
                        FileInfo file = new FileInfo(newFilePath);
                        selectedItem.Items.Add(CreateTreeViewFile(file));
                    }
                }
            }
        }

        // Funkcja obsługująca kliknięcie w opcję "Delet" w menu kontekstowym folderu
        private void DirectoryDeleteOnClick(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem is TreeViewItem item)
            {
                // Pobieramy ścieżkę zaznaczonego elementu
                string path = (string)item.Tag;
                // Usuwamy zawartość folderu
                DeleteDirectoryContent(path);

                // Jeśli usuwamy element root, czyścimy całe drzewo
                if (treeView.Items[0] == item)
                {
                    treeView.Items.Clear();
                }
                // W przeciwnym przypadku usuwamy element z drzewa
                else if (item.Parent is TreeViewItem parent)
                {
                    parent.Items.Remove(item);
                }
            }
        }

        private void DeleteDirectoryContent(string path)
        {
            // Tworzenie obiektu DirectoryInfo dla danego folderu
            DirectoryInfo directory = new DirectoryInfo(path);
            // Rekurencyjnie usuwanie zawartości folderów w folderze
            foreach (DirectoryInfo subdirectory in directory.GetDirectories())
            {
                DeleteDirectoryContent(subdirectory.FullName);
            }
            // Usuwanie plików z folderu
            foreach (FileInfo file in directory.GetFiles())
            {
                // Usunięcie atrybutu ReadOnly z pliku, aby umożliwić usunięcie
                FileAttributes fileAttributes = File.GetAttributes(file.FullName);
                File.SetAttributes(file.FullName, fileAttributes & ~FileAttributes.ReadOnly);
                // Usunięcie pliku
                file.Delete();
            }
            // Usunięcie atrybutu ReadOnly z folderu, aby umożliwić usunięcie
            directory.Attributes &= ~FileAttributes.ReadOnly;
            // Usunięcie folderu
            directory.Delete(true);
        }

        private void UpdateStatusBar(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem is TreeViewItem selectedItem)
            {
                // Pobieramy ścieżkę pliku lub folderu 
                string path = (string)selectedItem.Tag;
                // Pobieramy atrybuty pliku lub folderu
                FileAttributes attributes = File.GetAttributes(path);

                // Tworzymy ciąg znaków reprezentujący domyślny układ atrybutów pliku w DOS
                string defaultDOS = "----";
                char[] rash = defaultDOS.ToCharArray();

                // Jeśli plik jest tylko do odczytu, ustawiamy pierwszy znak na "r"
                if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    rash[0] = 'r';
                }
                // Jeśli plik jest zaznaczony jako archiwum, ustawiamy drugi znak na "a"
                if ((attributes & FileAttributes.Archive) == FileAttributes.Archive)
                {
                    rash[1] = 'a';
                }
                // Jeśli plik jest zaznaczony jako systemowy, ustawiamy trzeci znak na "s"
                if ((attributes & FileAttributes.System) == FileAttributes.System)
                {
                    rash[2] = 's';
                }
                // Jeśli plik jest ukryty, ustawiamy czwarty znak na "h"
                if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                {
                    rash[3] = 'h';
                }

                // Wyświetlamy aktualny układ atrybutów w StatusBarze
                statusBarText.Text = new string(rash);
            }
        }
    }
}
