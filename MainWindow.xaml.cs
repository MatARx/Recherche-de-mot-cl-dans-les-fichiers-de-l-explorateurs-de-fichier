using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using Microsoft.Win32;

namespace GROS_CTRL_F
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            // Ouvre une boîte de dialogue pour sélectionner un dossier
            var folderDialog = new OpenFileDialog
            {
                Title = "Sélectionner un dossier",
                Filter = "Dossier|*.", // Filtre pour afficher uniquement les dossiers
                FileName = "Sélectionner un dossier", // Texte par défaut dans la boîte de dialogue
                CheckFileExists = false, // Ne pas vérifier si le fichier existe
                CheckPathExists = true, // Vérifier si le chemin existe
                Multiselect = false, // Ne pas autoriser la sélection de plusieurs fichiers
                ValidateNames = false // Ne pas valider les noms de fichiers
            };

            // Si l'utilisateur sélectionne un dossier dans la boîte de dialogue
            if (folderDialog.ShowDialog() == true)
            {
                // Met à jour le contenu de la zone de texte avec le chemin du dossier sélectionné
                FolderPathTextBox.Text = Path.GetDirectoryName(folderDialog.FileName);
            }
        }

        public async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Récupère le chemin du dossier et le mot-clé saisi par l'utilisateur
            string folderPath = FolderPathTextBox.Text;
            string keyword = KeywordTextBox.Text;

            // Vérifie si l'utilisateur a saisi un chemin de dossier et un mot-clé
            if (string.IsNullOrEmpty(folderPath) || string.IsNullOrEmpty(keyword))
            {
                // Affiche un message d'erreur si l'utilisateur n'a pas saisi de chemin de dossier ou de mot-clé
                MessageBox.Show("Veuillez sélectionner un dossier et saisir un mot-clé.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Arrête l'exécution de la méthode
            }

            try
            {
                // Réinitialiser la liste des résultats à chaque nouvelle recherche
                List<string> matchingFiles = new List<string>();

                // Calculer le nombre total de résultats attendus
                int totalExpectedResults = matchingFiles.Count;

                // Rechercher récursivement tous les fichiers dans le dossier spécifié
                await SearchFilesAsync(folderPath, keyword, matchingFiles);

                NavigateToResultPage(matchingFiles);
                this.Close(); // Ferme la MainWindow si vous ne voulez pas qu'elle reste ouverte
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche : {ex.Message}");
            }

        }

        public async Task SearchFilesAsync(string folderPath, string keyword, List<string> matchingFiles)
        {
            try
            {
                // Obtenir tous les fichiers dans le dossier actuel
                string[] files = Directory.GetFiles(folderPath);

                // Parcourir tous les fichiers dans le dossier actuel
                foreach (string file in files)
                {

                    // Lire le contenu du fichier
                    string fileContent = await ReadFileAsync(file);

                    // Vérifier si le mot-clé est présent dans le contenu du fichier
                    if (fileContent.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    {

                            matchingFiles.Add(file); // Ajouter le chemin du fichier à la liste des résultats
                            
                    }
                    
                }

                // Obtenir tous les sous-dossiers dans le dossier actuel
                string[] subdirectories = Directory.GetDirectories(folderPath);

                // Parcourir tous les sous-dossiers récursivement
                foreach (string subdirectory in subdirectories)
                {
                    await SearchFilesAsync(subdirectory, keyword, matchingFiles); // Appel récursif pour chaque sous-dossier
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la recherche dans {folderPath} : {ex.Message}");
            }
        }
        private Task<string> ReadFileAsync(string filePath) 
        {
            return Task.Run(() =>
            {
                try
                {
                    // Lire le contenu complet du fichier
                    string content = File.ReadAllText(filePath, Encoding.UTF8);
                    return content;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur lors de la lecture du fichier {filePath} : {ex.Message}");
                    return string.Empty;
                }
            });
        }
        private void NavigateToResultPage(List<string> matchingFiles)
        {

            if (matchingFiles.Count > 0)
            {
                Résultats_trouvés resultat_trouves = new Résultats_trouvés(matchingFiles);
                resultat_trouves.Show();
                this.Close();
            }
            else if (matchingFiles.Count == 0)
                {
                    Aucun_Résultat_Trouvé noresultat_trouves = new Aucun_Résultat_Trouvé();
                    noresultat_trouves.Show();
                    this.Close();
                }
        }

    }
}
