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
    /// Interaction logic for PersonUC.xaml
    /// </summary>
    public partial class PersonUC : UserControl
    {
        readonly DbSgtContext sgtContext = new DbSgtContext();

        CollectionViewSource  collectionViewSourcePerson = new CollectionViewSource();
        public PersonUC()
        {
            InitializeComponent();
            collectionViewSourcePerson = (CollectionViewSource) FindResource(nameof(collectionViewSourcePerson));
            sgtContext.Database.EnsureCreated();
            sgtContext.Personnes.Load();
        }

        

        //Button ajouter
        private void btnAjouterPerson_Click(object sender, RoutedEventArgs e)
        {
            //Récupération des données
            string nom = txtBoxNom.Text;
            string courriel = txtBoxCourriel.Text;
            string matricule = txtBoxMatricule.Text;
            string departement = txtBoxDepartement.Text;

            //Validation des données
            if (nom == "" || courriel == "" || matricule == "" || departement == "")
            {
                MessageBox.Show("Veuillez remplir tous les champs");
                txtBoxMatricule.Focus();
            }

            int matriculeConvertie;
            bool result = int.TryParse(matricule, out matriculeConvertie);
            if (result)
            {
                Personne personne = new Personne();
                personne.Nom = nom;
                personne.Courriel = courriel;
                personne.Matricule = matriculeConvertie;
                personne.Departement = departement;

                //Ajout de la personne
                
                try
                {
                    sgtContext.Personnes.Add(personne);
                    sgtContext.SaveChanges();
                    MessageBox.Show("Personne ajoutée avec succès");
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Erreur"+ex.Message);
                }
                
                txtBoxMatricule.Focus();
                //Rafraichissement de la liste
                collectionViewSourcePerson.Source = sgtContext.Personnes.Local.ToObservableCollection();

                //Vider les champs
                ViderChamps();
            }
        }

        //A la selection d'une ligne
        private void dataGridPerson_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Recuperation de la personne selectionnée

            if(dataGridPerson.SelectedItem is null)
            {
                ViderChamps();
                return;
            }
            if (dataGridPerson.SelectedItem is Personne personne)
            {
                txtBoxMatricule.Text = personne.Matricule.ToString();
                txtBoxNom.Text = personne.Nom;
                txtBoxDepartement.Text = personne.Departement;
                txtBoxCourriel.Text = personne.Courriel;
            }
        }

        //Button modifier
        private void btnModifierPerson_Click(object sender, RoutedEventArgs e)
        {
            //Récupération des données
            string nom = txtBoxNom.Text;
            string courriel = txtBoxCourriel.Text;
            string matricule = txtBoxMatricule.Text;
            string departement = txtBoxDepartement.Text;
            //Validation des données
            if (nom == "" || courriel == "" || matricule == "" || departement == "")
            {
                MessageBox.Show("Veuillez remplir tous les champs");
                txtBoxMatricule.Focus();
            }

            int matriculeConvertie;
            bool result = int.TryParse(matricule, out matriculeConvertie);
            if (result)
            {
                if (dataGridPerson.SelectedItem is Personne personne)
                {
                    personne.Nom = nom;
                    personne.Courriel = courriel;
                    personne.Departement = departement;
                    personne.Matricule = int.Parse(matricule);
                    try
                    {
                        sgtContext.Update(personne);
                        sgtContext.SaveChanges();
                        MessageBox.Show("Personne modifiée avec succès");
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Erreur" + ex.Message);
                    }
                    dataGridPerson.Items.Refresh();
                    ViderChamps();
                }
            }
        }

        //Button supprimer
        private void btnSupprimerPerson_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridPerson.SelectedItem is Personne personne)
            {
                personne.Nom = txtBoxNom.Text;
                personne.Courriel = txtBoxCourriel.Text;
                personne.Departement = txtBoxDepartement.Text;
                personne.Matricule = int.Parse(txtBoxMatricule.Text);

                try
                {
                    sgtContext.Remove(personne);
                    sgtContext.SaveChanges();
                    MessageBox.Show("Personne supprimée avec succès");
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Erreur"+ex.Message);
                }

                dataGridPerson.Items.Refresh();
                ViderChamps();
            }
        }

        //Button reinitialiser
        private void btnReinitialiserPerson_Click(object sender, RoutedEventArgs e)
        {
            ViderChamps();
            dataGridPerson.ItemsSource=sgtContext.Personnes.ToList();
        }

        private void ViderChamps()
        {
            txtBoxMatricule.Text = "";
            txtBoxNom.Text = "";
            txtBoxDepartement.Text = "";
            txtBoxCourriel.Text = "";
            txtBoxMatricule.Focus();
            txtBoxRecherche.Text = "";
            checkboxMatricul.IsChecked = false;
        }

        
        private void btnRecherchePersone_Click(object sender, RoutedEventArgs e)
        {
            string motRechercher = txtBoxRecherche.Text;
            if (!string.IsNullOrEmpty(motRechercher) )
            {
                if (checkboxMatricul.IsChecked==true)
                {
                    int valeurMatricul;
                    bool result =int.TryParse(motRechercher, out valeurMatricul);
                    if (result)
                    {
                        var requete = sgtContext.Personnes.AsQueryable();
                        requete = requete.Where(p=> p.Matricule == valeurMatricul);
                        dataGridPerson.ItemsSource = requete.ToList();
                    }
                    else
                    {
                        MessageBox.Show("La valeur de recherche d'un numero matricule doit etre un entier");
                    }
                }
                else
                {
                    var requete = sgtContext.Personnes.AsQueryable();
                    requete = requete.Where(p => p.Nom.Contains(motRechercher) || p.Courriel.Contains(motRechercher) || p.Departement.Contains(motRechercher));
                    dataGridPerson.ItemsSource=requete.ToList();
                }
            }
            else
            {
                MessageBox.Show("Veuillez remplir le champ de recherche");
            }
            UserControl_Loaded(sender, e);
        }

        public void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dataGridPerson.SelectedItem = null;
        }


    }
}
