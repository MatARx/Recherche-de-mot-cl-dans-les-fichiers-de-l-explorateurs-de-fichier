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
    /// <summary>
    /// Logique d'interaction pour Aucun_Résultat_Trouvé.xaml
    /// </summary>
    public partial class Aucun_Résultat_Trouvé : Window
    {
        public Aucun_Résultat_Trouvé()
        {
            InitializeComponent();
        }
        private void OK(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
