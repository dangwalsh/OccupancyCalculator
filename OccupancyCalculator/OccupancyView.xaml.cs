using System.Collections.Generic;
using System.Windows;

namespace Gensler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class OccupancyView : Window
    {
        private List<Occupancy> _occupancies;

        private readonly OccupancyController _occupancyController;

        //private CollectionView _occupancyCollectionView;

        //private int _counter;

        //private SolidColorBrush _altBrush = new SolidColorBrush(Colors.LightGray);
        
        public OccupancyView(OccupancyController oc)
        {
            _occupancyController = oc;           
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _occupancies = _occupancyController.GetOccupancies();
            //_occupancyCollectionView = new CollectionView(_occupancies);
            OccupancyGrid.ItemsSource = _occupancies; //_occupancyCollectionView;
            //if (_occupancyCollectionView == null) throw new NullReferenceException();
            //if (_occupancyCollectionView.CanGroup)
            //{
            //    _occupancyCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("LevelName"));
            //}
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //private void OccupancyGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        //{
        //    if (_counter++%2 == 0) e.Row.Background = _altBrush;
        //}
    }
}
