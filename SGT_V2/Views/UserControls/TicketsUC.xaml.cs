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
using System.Xml.Xsl;

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
            dbSgtContext.Tickets.Include(t => t.IdpersonneTicketsNavigation).Load();
            dbSgtContext.Personnes.Load();

            ticketViewSource.Source = dbSgtContext.Tickets.Local.ToObservableCollection();
            personneViewSource.Source = dbSgtContext.Personnes.Local.ToObservableCollection();
        }


        private void btnAjouterTicket_Click(object sender, RoutedEventArgs e)
        {
            // Recuperation des valeurs des champs
            string titre = txtBoxTitre.Text;
            string? type = (cmbBoxType.SelectedItem as ComboBoxItem)?.Content.ToString();
            string? priorite = (cmbBoxPriorite.SelectedItem as ComboBoxItem)?.Content.ToString();
            string? statut = (cmbBoxStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            Personne? personne = cmbBoxPersonne.SelectedItem as Personne;
            string? categorie = (cmbBoxCategorie.SelectedItem as ComboBoxItem)?.Content.ToString();
            DateTime dateCreation;

            Ticket ticket = new Ticket();

            // Validation des champs
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(priorite) || string.IsNullOrEmpty(statut) || string.IsNullOrEmpty(categorie) || personne is null || string.IsNullOrEmpty(titre) || datePickerCreation.SelectedDate == null)
            {
                MessageBox.Show("Veuillez renseigner tous les champs");
                txtBoxTitre.Focus();
            }
            else
            {
                // Ajout du ticket
                dateCreation = datePickerCreation.SelectedDate.Value;

                ticket.Titre = titre;
                ticket.Type = type;
                ticket.Priorite = priorite;
                ticket.Status = statut;
                ticket.Datecreation = dateCreation;
                ticket.IdpersonneTickets = personne.Idpersonne;
                ticket.Categorie = categorie;

                try
                {
                    dbSgtContext.Tickets.Add(ticket);
                    dbSgtContext.SaveChanges();
                    MessageBox.Show("Ticket ajouté avec succès");
                    ViderChamps();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur: " + ex.Message);
                }

                // Rafraichissement de la liste
                ticketViewSource.Source = dbSgtContext.Tickets.Local.ToObservableCollection();
                ticketViewSource.View.Refresh();
            }
        }



        //Vider leas champs au chargement
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridTickets.SelectedItem = null;
            cmbBoxPersonne.SelectedItem = null;
        }

        private void btnReinitialiserTicket_Click(object sender, RoutedEventArgs e)
        {
            ViderChamps();
            dataGridTickets.ItemsSource = dbSgtContext.Tickets.Local.ToObservableCollection();
        }

        /// <summary>
        /// Vider les champs
        /// </summary>
        public void ViderChamps()
        {
            txtBoxTitre.Text = "";
            cmbBoxType.SelectedItem = null;
            cmbBoxPriorite.SelectedItem = null;
            cmbBoxStatus.SelectedItem = null;
            cmbBoxPersonne.SelectedItem = null;
            cmbBoxCategorie.SelectedItem = null;
            datePickerCreation.SelectedDate = null;
            datePickerFermeture.SelectedDate = null;
            txtBoxRecherche.Text = "";
            chkBoxCreation.IsChecked = false;
            chkBoxFermeture.IsChecked = false;
        }

        private void dataGridTickets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //A la selection d'une ligne tous les information sort renvoyer dans les champs
            if (dataGridTickets.SelectedItem is null)
            {
                ViderChamps();
                return;
            }
            else
            {

                if (dataGridTickets.SelectedItem is Ticket ticket)
                {
                    txtBoxTitre.Text = ticket.Titre;
                    cmbBoxType.Text = ticket.Type;
                    cmbBoxPriorite.Text = ticket.Priorite;
                    cmbBoxStatus.Text = ticket.Status;
                    cmbBoxCategorie.Text = ticket.Categorie;
                    datePickerCreation.SelectedDate = ticket.Datecreation;
                    datePickerFermeture.SelectedDate = ticket.Datefermeture;
                    cmbBoxPersonne.SelectedItem = ticket.IdpersonneTicketsNavigation;
                }
            }
        }

        private void btnModifierTicket_Click(object sender, RoutedEventArgs e)
        {
            //Bonton modifier
            if (dataGridTickets.SelectedItem is Ticket ticket)
            {
                //Recuperation des valeurs des champs
                string titre = txtBoxTitre.Text;
                string? type = (cmbBoxType.SelectedItem as ComboBoxItem)?.Content.ToString();
                string? priorite = (cmbBoxPriorite.SelectedItem as ComboBoxItem)?.Content.ToString();
                string? statut = (cmbBoxStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
                Personne? personne = cmbBoxPersonne.SelectedItem as Personne;
                string? categorie = (cmbBoxCategorie.SelectedItem as ComboBoxItem)?.Content.ToString();
                DateTime dateCreation;

                //Validation des champs
                if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(priorite) || string.IsNullOrEmpty(statut) || string.IsNullOrEmpty(categorie) || personne is null || string.IsNullOrEmpty(titre) || datePickerCreation.SelectedDate == null)
                {
                    MessageBox.Show("Veuillez renseigner tous les champs");
                    txtBoxTitre.Focus();
                }
                else
                {
                    //Modification du ticket
                    dateCreation = datePickerCreation.SelectedDate.Value;

                    ticket.Titre = titre;
                    ticket.Type = type;
                    ticket.Priorite = priorite;
                    ticket.Status = statut;
                    ticket.Datecreation = dateCreation;
                    ticket.IdpersonneTickets = personne.Idpersonne;
                    ticket.Categorie = categorie;

                    try
                    {
                        dbSgtContext.SaveChanges();
                        MessageBox.Show("Ticket modifié avec succès");
                        ViderChamps();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur: " + ex.Message);
                    }

                    // Rafraichissement de la liste
                    ticketViewSource.Source = dbSgtContext.Tickets.Local.ToObservableCollection();
                    ticketViewSource.View.Refresh();
                }
            }
        }

        private void btnSupprimerTicket_Click(object sender, RoutedEventArgs e)
        {
            //Button supprimer
            if (dataGridTickets.SelectedItem is Ticket ticket)
            {
                MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer ce ticket ?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.OK)
                {
                    dbSgtContext.Tickets.Remove(ticket);
                    dbSgtContext.SaveChanges();
                    MessageBox.Show("Ticket supprimé avec succès");
                    ViderChamps();

                    // Rafraichissement de la liste
                    ticketViewSource.Source = dbSgtContext.Tickets.Local.ToObservableCollection();
                    ticketViewSource.View.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un ticket à supprimer");
            }
        }


        private void btnRechercheTicket_Click(object sender, RoutedEventArgs e)
        {
            string motRechercher = txtBoxRecherche.Text;
            DateTime dateRecherche;

            var requete = dbSgtContext.Tickets.AsQueryable();

            if (DateTime.TryParse(motRechercher, out dateRecherche))
            {
                if (chkBoxCreation.IsChecked == true)
                {
                    requete = requete.Where(t => t.Datecreation.Date == dateRecherche.Date);
                }
                else if (chkBoxFermeture.IsChecked == true)
                {
                    requete = requete.Where(t => t.Datefermeture.HasValue && t.Datefermeture.Value.Date == dateRecherche.Date);
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner un critère de recherche (Création ou Fermeture).");
                    return;
                }
            }
            else if (!string.IsNullOrEmpty(motRechercher))
            {
                requete = requete.Where(t => t.Titre.Contains(motRechercher) ||
                                             t.Status.Contains(motRechercher) ||
                                             t.Categorie.Contains(motRechercher) ||
                                             t.Type.Contains(motRechercher) ||
                                             t.Priorite.Contains(motRechercher) ||
                                             t.IdpersonneTicketsNavigation.Nom.Contains(motRechercher));
            }
            else
            {
                MessageBox.Show("Veuillez remplir le champ de recherche.");
                return;
            }

            dataGridTickets.ItemsSource = requete.ToList();
        }



    }
}