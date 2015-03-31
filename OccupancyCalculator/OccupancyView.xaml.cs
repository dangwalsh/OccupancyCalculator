using System.Collections.Generic;
using System.Windows;

namespace Gensler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Occupancy> _occupancies;

        private readonly OccupancyController _occupancyTable;
        
        public MainWindow(OccupancyController oc)
        {
            _occupancyTable = oc;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _occupancies = _occupancyTable.GetOccupancies();
            OccupancyGrid.ItemsSource = _occupancies;
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
