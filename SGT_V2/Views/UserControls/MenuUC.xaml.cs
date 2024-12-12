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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SGT_V2.Views.UserControls
{
    /// <summary>
    /// Interaction logic for MenuUC.xaml
    /// </summary>
    public partial class MenuUC : UserControl
    {
        public MenuUC()
        {
            InitializeComponent();
        }

        private void btnFenTicket_Click(object sender, RoutedEventArgs e)
        {
            Fenetres.FenTicket fenTicket = new Fenetres.FenTicket();
            fenTicket.ShowDialog();
        }

        private void btnFenPerson_Click(object sender, RoutedEventArgs e)
        {
            Fenetres.FenPersons fenPersons = new Fenetres.FenPersons();
            fenPersons.ShowDialog();
        }

        private void btnFenComment_Click(object sender, RoutedEventArgs e)
        {
            Fenetres.FenCommentaire fenComment = new Fenetres.FenCommentaire();
            fenComment.ShowDialog();
        }

        private void btnFenDashboard_Click(object sender, RoutedEventArgs e)
        {
            Fenetres.FenDashboard fenDashboard = new Fenetres.FenDashboard();
            fenDashboard.ShowDialog();
        }
    }
}
