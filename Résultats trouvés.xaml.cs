using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GROS_CTRL_F
{
    public partial class Résultats_trouvés : Window
    {
        public Résultats_trouvés(List<string> matchingFiles)
        {
            InitializeComponent();

            // Convertir les chemins de fichiers en objets FileInformation
            List<FileInformation> fileInfos = new List<FileInformation>();
            foreach (string filePath in matchingFiles)
            {
                fileInfos.Add(new FileInformation
                {
                    Name = System.IO.Path.GetFileName(filePath),
                    Path = filePath
                });
            }

            // Mettre à jour le DataGrid avec les informations des fichiers
            resultsDataGrid.ItemsSource = fileInfos;
        }
        private void Button(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }

    // Définir la classe FileInformation
    public class FileInformation
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
