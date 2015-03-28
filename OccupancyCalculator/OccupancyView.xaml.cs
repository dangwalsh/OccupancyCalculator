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

namespace Gensler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Occupancy> _occupancies;

        private OccupancyController _occupancyTable;

        public OccupancyController OccupancyTable
        {
            get { return _occupancyTable; }
            set { _occupancyTable = value; }
        }
        
        public MainWindow(OccupancyController oc)
        {
            _occupancyTable = oc;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _occupancies = _occupancyTable.GetOccupancies();
            _occupancyGrid.ItemsSource = _occupancies;
        }
    }
}
