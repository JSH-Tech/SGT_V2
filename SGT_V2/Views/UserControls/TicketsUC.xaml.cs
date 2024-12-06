using Microsoft.EntityFrameworkCore;
using SGT_V2.Models;
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
    /// Interaction logic for TicketsUC.xaml
    /// </summary>
    public partial class TicketsUC : UserControl
    {
        readonly DbSgtContext dbSgtContext = new DbSgtContext();
        CollectionViewSource ticketViewSource;
        CollectionViewSource personneViewSource;
        public TicketsUC()
        {
            InitializeComponent();
            ticketViewSource = (CollectionViewSource)FindResource(nameof(ticketViewSource));
            personneViewSource = (CollectionViewSource)FindResource(nameof(personneViewSource));
            dbSgtContext.Database.EnsureCreated();
            dbSgtContext.Tickets.Load();
            dbSgtContext.Personnes.Load();

            ticketViewSource.Source = dbSgtContext.Tickets.Local.ToObservableCollection();
            personneViewSource.Source = dbSgtContext.Personnes.Local.ToObservableCollection();
        }


        private void btnAjouterTicket_Click(object sender, RoutedEventArgs e)
        {
            //Recuperation des valeurs des champs
            
            

            //Validation des champs date
            if (datePickerCreation.SelectedDate == null)
            {
                MessageBox.Show("Veuillez renseigner la date de création du ticket");
                datePickerCreation.Focus();
            }
            else
            {
                DateTime dateCreation = datePickerCreation.SelectedDate.Value;
            }

            if (string.IsNullOrEmpty(txtBoxTitre.Text))
            {
                MessageBox.Show("Veuillez renseigner le titre du ticket");
                txtBoxTitre.Focus();
            }
            else
            {
                string titre = txtBoxTitre.Text;
            }

            if (cmbBoxType.SelectedItem is null || cmbBoxPriorite.SelectedItem is null || cmbBoxStatus.SelectedItem is null || cmbBoxCategorie.SelectedItem is null)
            {
                MessageBox.Show("Veuillez renseigner tous les champs");
                cmbBoxType.Focus();
            }
            else
            {
                string? type = cmbBoxType.SelectedItem.ToString();
                string? priorite = cmbBoxPriorite.SelectedItem.ToString();
                string? statut = cmbBoxStatus.SelectedItem.ToString();
                string? categorie = cmbBoxCategorie.SelectedItem.ToString();
            }

            if (cmbBoxPersonne.SelectedItem as Personne is null)
            {
                MessageBox.Show("Veuillez selectionner une personne");
                cmbBoxPersonne.Focus();
            }
            else
            {

                Personne? personne = cmbBoxPersonne.SelectedItem as Personne;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridTickets.SelectedItem = null;
            cmbBoxPersonne.SelectedItem = null;
        }

        private void btnReinitialiserTicket_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}