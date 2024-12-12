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
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;
using SGT_V2.Models;

namespace SGT_V2.Views.UserControls
{

    /// <summary>
    /// Interaction logic for DashboardUC.xaml
    /// </summary>
    public partial class DashboardUC : UserControl
    {
        private readonly DbSgtContext dbSgtContext;

        public LiveCharts.SeriesCollection TicketStatusSeries { get; set; }
        public LiveCharts.SeriesCollection TicketTrendSeries { get; set; }

        public DashboardUC()
        {
            InitializeComponent();
            dbSgtContext = new DbSgtContext();
            dbSgtContext.Database.EnsureCreated();

            // Load data from the database
            dbSgtContext.Tickets.Load();
            dbSgtContext.Personnes.Load();

            // Set DataContext for data binding
            DataContext = new
            {
                TotalTickets = dbSgtContext.Tickets.Count(),
                OpenTickets = dbSgtContext.Tickets.Count(t => t.Status == "Ouvert"),
                ClosedTickets = dbSgtContext.Tickets.Count(t => t.Status == "Ferme"),
                InProgressTickets = dbSgtContext.Tickets.Count(t => t.Status == "En cours"),
                TicketStatusSeries = GetTicketStatusSeries(),
                TicketTrendSeries = GetTicketTrendSeries()
            };

            // Bind recent activity to DataGrid
            dataGridRecentActivity.ItemsSource = dbSgtContext.Tickets
                .OrderByDescending(t => t.Datefermeture)
                .Select(t => new
                {
                    Type = t.Type,
                    Titre = t.Titre,
                    Date = t.Datefermeture,
                    Personne = t.IdpersonneTicketsNavigation.Nom
                })
                .ToList();
        }

        private LiveCharts.SeriesCollection GetTicketStatusSeries()
        {
            return new LiveCharts.SeriesCollection
            {
                new PieSeries
                {
                    Title = "Ouvert",
                    Values = new LiveCharts.ChartValues<int> { dbSgtContext.Tickets.Count(t => t.Status == "Ouvert") }
                },
                new PieSeries
                {
                    Title = "Fermé",
                    Values = new LiveCharts.ChartValues<int> { dbSgtContext.Tickets.Count(t => t.Status == "Ferme") }
                },
                new PieSeries
                {
                    Title = "En cours",
                    Values = new LiveCharts.ChartValues<int> { dbSgtContext.Tickets.Count(t => t.Status == "En cours") }
                }
            };
        }

        private LiveCharts.SeriesCollection GetTicketTrendSeries()
        {
            return new LiveCharts.SeriesCollection
            {
                new LineSeries
                {
                    Title = "Tickets",
                    Values = new LiveCharts.ChartValues<DateTimePoint>(
                        dbSgtContext.Tickets
                            .GroupBy(t => t.Datecreation.Date)
                            .Select(g => new DateTimePoint(g.Key, g.Count()))
                            .ToList()
                    )
                }
            };
        }
    }
}
