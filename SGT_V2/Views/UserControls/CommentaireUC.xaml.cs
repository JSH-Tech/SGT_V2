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
    /// Interaction logic for CommentaireUC.xaml
    /// </summary>
    public partial class CommentaireUC : UserControl
    {
        readonly DbSgtContext dbSgtContext = new DbSgtContext();
        CollectionViewSource commentaireViewSource;
        CollectionViewSource ticketViewSource;
        public CommentaireUC()
        {
            InitializeComponent();
            commentaireViewSource = (CollectionViewSource)FindResource(nameof(commentaireViewSource));
            ticketViewSource = (CollectionViewSource)FindResource(nameof(ticketViewSource));
            dbSgtContext.Database.EnsureCreated();
            dbSgtContext.Commentaires.Include(c => c.IdticketCommentaireNavigation).Load();
            dbSgtContext.Tickets.Load();
            commentaireViewSource.Source = dbSgtContext.Commentaires.Local.ToObservableCollection();
            ticketViewSource.Source = dbSgtContext.Tickets.Local.ToObservableCollection();
        }

        private void btnAjouterCommentaire_Click(object sender, RoutedEventArgs e)
        {
            // Recuperation des valeurs des champs
            string contenu = txtBoxCommentaire.Text;
            Ticket? ticket = comboBoxTitreTicketCommentaire.SelectedItem as Ticket;
            DateTime dateCommentaire;

            Commentaire commentaire = new Commentaire();

            // Validation des champs
            if (string.IsNullOrEmpty(contenu) || ticket is null || datePickerCreationCommentaire.SelectedDate == null)
            {
                MessageBox.Show("Veuillez renseigner tous les champs");
                txtBoxCommentaire.Focus();
            }
            else
            {
                // Ajout du commentaire
                dateCommentaire = datePickerCreationCommentaire.SelectedDate.Value;

                commentaire.Contenu = contenu;
                commentaire.Datecommentaire = dateCommentaire;
                commentaire.IdticketCommentaire = ticket.Idticket;

                try
                {
                    dbSgtContext.Commentaires.Add(commentaire);
                    dbSgtContext.SaveChanges();
                    MessageBox.Show("Commentaire ajouté avec succès");
                    ViderChamps();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur: " + ex.Message);
                }

                // Rafraichissement de la liste
                commentaireViewSource.Source = dbSgtContext.Commentaires.Local.ToObservableCollection();
                commentaireViewSource.View.Refresh();
            }
        }

        //Vider les champs au chargement
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DataGridCommentaire.SelectedItem = null;
            comboBoxTitreTicketCommentaire.SelectedItem = null;
        }

        private void btnReinitialiserCommentaire_Click(object sender, RoutedEventArgs e)
        {
            ViderChamps();
            DataGridCommentaire.ItemsSource = dbSgtContext.Commentaires.Local.ToObservableCollection();
        }


        private void btnModifierCommentaire_Click(object sender, RoutedEventArgs e)
        {
            //Bouton modifier
            if (DataGridCommentaire.SelectedItem is Commentaire commentaire)
            {
                //Recuperation des valeurs des champs
                string contenu = txtBoxCommentaire.Text;
                Ticket? ticket = comboBoxTitreTicketCommentaire.SelectedItem as Ticket;
                DateTime dateCommentaire;

                //Validation des champs
                if (string.IsNullOrEmpty(contenu) || ticket is null || datePickerCreationCommentaire.SelectedDate == null)
                {
                    MessageBox.Show("Veuillez renseigner tous les champs");
                    txtBoxCommentaire.Focus();
                }
                else
                {
                    //Modification du commentaire
                    dateCommentaire = datePickerCreationCommentaire.SelectedDate.Value;

                    commentaire.Contenu = contenu;
                    commentaire.Datecommentaire = dateCommentaire;
                    commentaire.IdticketCommentaire = ticket.Idticket;

                    try
                    {
                        dbSgtContext.SaveChanges();
                        MessageBox.Show("Commentaire modifié avec succès");
                        ViderChamps();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur: " + ex.Message);
                    }

                    // Rafraichissement de la liste
                    commentaireViewSource.Source = dbSgtContext.Commentaires.Local.ToObservableCollection();
                    commentaireViewSource.View.Refresh();
                }
            }
        }

        private void btnSupprimerCommentaire_Click(object sender, RoutedEventArgs e)
        {
            //Button supprimer
            if (DataGridCommentaire.SelectedItem is Commentaire commentaire)
            {
                MessageBoxResult result = MessageBox.Show("Voulez-vous vraiment supprimer ce commentaire ?", "Confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.OK)
                {
                    dbSgtContext.Commentaires.Remove(commentaire);
                    dbSgtContext.SaveChanges();
                    MessageBox.Show("Commentaire supprimé avec succès");
                    ViderChamps();

                    // Rafraichissement de la liste
                    commentaireViewSource.Source = dbSgtContext.Commentaires.Local.ToObservableCollection();
                    commentaireViewSource.View.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un commentaire à supprimer");
            }
        }

        private void btnRechercheCommentaire_Click(object sender, RoutedEventArgs e)
        {
            string motRechercher = txtBoxRechercheCommentaire.Text;
            DateTime dateRecherche;

            var requete = dbSgtContext.Commentaires.AsQueryable();

            if (DateTime.TryParse(motRechercher, out dateRecherche))
            {
                if ( btnRadioDateMod.IsChecked == true)
                {
                    requete = requete.Where(c => c.Datecommentaire.Date == dateRecherche.Date);
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner un critère de recherche (Date de commentaire).");
                    return;
                }
            }
            else if (!string.IsNullOrEmpty(motRechercher))
            {
                requete = requete.Where(c => c.Contenu.Contains(motRechercher) ||
                                             c.IdticketCommentaireNavigation.Titre.Contains(motRechercher));
            }
            else
            {
                MessageBox.Show("Veuillez remplir le champ de recherche.");
                return;
            }

            DataGridCommentaire.ItemsSource = requete.ToList();
        }

        /// <summary>
        /// Vider les champs
        /// </summary>
        public void ViderChamps()
        {
            txtBoxCommentaire.Text = "";
            comboBoxTitreTicketCommentaire.SelectedItem = null;
            datePickerCreationCommentaire.SelectedDate = null;
            txtBoxRechercheCommentaire.Text = "";
            btnRadioDateMod.IsChecked = false;
        }

        private void DataGridCommentaire_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //A la selection d'une ligne tous les informations sont renvoyées dans les champs
            if (DataGridCommentaire.SelectedItem is null)
            {
                ViderChamps();
                return;
            }
            else
            {
                if (DataGridCommentaire.SelectedItem is Commentaire commentaire)
                {
                    txtBoxCommentaire.Text = commentaire.Contenu;
                    datePickerCreationCommentaire.SelectedDate = commentaire.Datecommentaire;
                    comboBoxTitreTicketCommentaire.SelectedItem = commentaire.IdticketCommentaireNavigation;
                }
            }
        }
    }
}
