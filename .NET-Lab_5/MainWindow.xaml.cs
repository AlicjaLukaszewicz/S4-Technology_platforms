using System;
using System.ComponentModel;
using System.IO.Compression;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Linq;
using System.Net;

namespace PT_Lab_5
{
    public partial class MainWindow : Window
    {
        private BackgroundWorker fibonacciWorker;

        public MainWindow()
        {
            InitializeComponent();

            // utworzenie instancji klasy BackgroundWorker
            fibonacciWorker = new BackgroundWorker();

            // ustawienie właściwości WorkerReportsProgress na true
            fibonacciWorker.WorkerReportsProgress = true;

            // obsługa zdarzenia DoWork
            fibonacciWorker.DoWork += new DoWorkEventHandler(FibonacciWorker_DoWork); ;

            // obsługa zdarzenia ProgressChanged
            fibonacciWorker.ProgressChanged += new ProgressChangedEventHandler(FibonacciWorker_ProgressChanged);

            // obsługa zdarzenia RunWorkerCompleted
            fibonacciWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(FibonacciWorker_RunWorkerCompleted);
        }

        // Zadanie 1
        private void CalculateTaskButton_Click(object sender, RoutedEventArgs e)
        {
            int n = int.Parse(NTextBox.Text);
            int k = int.Parse(KTextBox.Text);

            Task<long> denumerator = Task.Factory.StartNew(() =>
            {
                long result = 1;
                for (int i = 1; i <= k; ++i)
                    result *= i;
                return result;
            });

            long numerator = 1;
            for (int i = 1; i <= k; ++i)
                numerator *= n - i + 1;

            denumerator.Wait();

            TasksResultTextBlock.Text = (numerator / denumerator.Result).ToString();
        }
        private void CalculateDelegateButton_Click(object sender, RoutedEventArgs e)
        {
            int n = int.Parse(NTextBox.Text);
            int k = int.Parse(KTextBox.Text);

            Func<int, int> numerator = Factorial;
            Func<int, int> denumerator = Factorial;
            IAsyncResult numeratorResult = numerator.BeginInvoke(n, null, null);
            IAsyncResult denominatorResult1 = denumerator.BeginInvoke(k, null, null);
            IAsyncResult denominatorResult2 = denumerator.BeginInvoke(n-k, null, null);

            long numeratorValue = numerator.EndInvoke(numeratorResult);
            long denominatorValue1 = denumerator.EndInvoke(denominatorResult1);
            long denominatorValue2 = denumerator.EndInvoke(denominatorResult2);
            long denominatorValue = denominatorValue1 * denominatorValue2;

            long result = numeratorValue/denominatorValue;
            TasksResultTextBlock.Text = result.ToString();
        }
        private async void CalculateAsyncAwaitButton_Click(object sender, RoutedEventArgs e)
        {
            int n = int.Parse(NTextBox.Text);
            int k = int.Parse(KTextBox.Text);

            long numerator = await Task.Run(() => Factorial(n));
            long denominator = await Task.Run(() => Factorial(k) * Factorial(n - k));
            long result = numerator/denominator;

            AsyncResultTextBlock.Text = result.ToString();
        }
        private int Factorial(int n)
        {
            int result = 1;

            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }

        // Zadanie 2
        private void CalculateFibButton_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse(FibTextBox.Text);         // numer wyrazu ciągu do obliczenia
            fibonacciWorker.RunWorkerAsync(i);
        }
        private void FibonacciWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = (int)e.Argument;
            long fibonacci = 0, prev1 = 1, prev2 = 0;

            for (int n = 1; n <= i; n++)
            {
                fibonacci = prev1 + prev2;
                prev2 = prev1;
                prev1 = fibonacci;

                // raportowanie postępu
                int progress = (int)((double)n / i * 100);
                fibonacciWorker.ReportProgress(progress);

                // spowolnienie obliczeń
                Thread.Sleep(20);
            }

            // przekazanie wyniku obliczeń
            e.Result = fibonacci;
        }
        private void FibonacciWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FibProgressBar.Value = e.ProgressPercentage;
        }
        private void FibonacciWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FibResultTextBlock.Text = ((long) e.Result).ToString();
        }

        // Zadanie 3
        private void Compress_Click(object sender, RoutedEventArgs e)
        {
            // Wybierz folder, który zawiera pliki do skompresowania
            var folderDialog = new FolderBrowserDialog();
            var result = folderDialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) return;
            var folderPath = folderDialog.SelectedPath;
            DirectoryInfo directory = new DirectoryInfo(folderPath);

            // Pobierz listę plików w wybranym folderze
            var files = directory.GetFiles();

            // Skompresuj każdy plik w osobnym wątku i zapisz jako plik .gz
            Parallel.ForEach(files, file =>
            {
                using (var fileToCompress = file.OpenRead())
                using (var compressedFile = File.Create(Path.Combine(directory.FullName, file.Name) + ".gz"))
                using (var compressedStream = new GZipStream(compressedFile, CompressionMode.Compress))
                {
                    fileToCompress.CopyTo(compressedStream);
                }
            });

            // Usunięcie wszystkich nieskompresowanych plików
            files.AsParallel().Where(f => f.Extension != ".gz").ForAll((f) => f.Delete());
        }
        private void Decompress_Click(object sender, RoutedEventArgs e)
        {
            // Wybierz folder, który zawiera pliki do dekompresowania
            var folderDialog = new FolderBrowserDialog();
            var result = folderDialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK) return;
            var folderPath = folderDialog.SelectedPath;
            var directory = new DirectoryInfo(folderPath);

            // Pobierz listę plików .gz w wybranym folderze
            FileInfo[] files = directory.GetFiles("*.gz");

            // Zdekompresuj każdy plik w osobnym wątku i zapisz jako oryginalny plik bez rozszerzenia .gz
            Parallel.ForEach(files, file =>
            {
                using (var fileToDecompress = file.OpenRead())
                using (var compressedStream = new GZipStream(fileToDecompress, CompressionMode.Decompress))
                using (var decompressedFile = File.Create(file.FullName.Substring(0, file.FullName.Length - 3)))
                {
                    compressedStream.CopyTo(decompressedFile);
                }
            });
        }

        // Zadanie 4
        private void Resolve_Click(object sender, RoutedEventArgs e)
        {
            string[] hostNames = { "www.microsoft.com", "www.apple.com",
                "www.google.com", "www.ibm.com", "cisco.netacad.net",
                "www.oracle.com", "www.nokia.com", "www.hp.com", "www.dell.com",
                "www.samsung.com", "www.toshiba.com", "www.siemens.com",
                "www.amazon.com", "www.sony.com", "www.canon.com",
                "www.alcatellucent.com", "www.acer.com", "www.motorola.com" };

            var ipAddresses = hostNames.AsParallel()
                                .Select(hostName =>
                                {
                                    try
                                    {
                                        var ipAddress = Dns.GetHostAddresses(hostName);
                                        return new { HostName = hostName, IPAddress = ipAddress };
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Unable to resolve {hostName}: {ex.Message}");
                                        return null;
                                    }
                                })
                                .Where(ipAddress => ipAddress != null)
                                .ToList();

            DNSTextBox.Text = string.Join("\n", ipAddresses.Select(ipAddress => $"{ipAddress.HostName} => \n {string.Join(",", ipAddress.IPAddress.Select(ip => ip.ToString()))}"));
        }
    }
}
