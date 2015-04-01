using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Gensler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class OccupancyView : Window
    {
        private List<Occupancy> _occupancies;

        private readonly OccupancyController _occupancyController;

        private ListCollectionView _occupancyCollectionView;

        private int _counter;

        private readonly SolidColorBrush _altBrush = new SolidColorBrush(Color.FromRgb(240,240,240));
        
        public OccupancyView(OccupancyController oc)
        {
            _occupancyController = oc;           
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _occupancies = _occupancyController.GetOccupancies();
            _occupancyCollectionView = new ListCollectionView(_occupancies);
            _occupancyCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("LevelName"));
            OccupancyGrid.ItemsSource = _occupancyCollectionView; 
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //private void OccupancyGrid_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        //{
        //    if (_counter++ % 2 == 0) e.Row.Background = _altBrush;
        //}
    }
}
