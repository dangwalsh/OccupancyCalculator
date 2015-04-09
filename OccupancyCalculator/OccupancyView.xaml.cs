using System.Windows;
using System.Windows.Data;

namespace Gensler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class OccupancyView : Window
    {
        private readonly OccupancyModel _occupancyModel;

        private OccupancyModel OccupancyModel
        {
            get { return _occupancyModel; }
        }

        private ListCollectionView _occupancyCollectionView;

        private ListCollectionView OccupancyCollectionView
        {
            get { return _occupancyCollectionView; }
            set { _occupancyCollectionView = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="om"></param>
        public OccupancyView(OccupancyModel om)
        {
            _occupancyModel = om;
            InitializeComponent();
            OccupancyCollectionView = new ListCollectionView(OccupancyModel.Occupancies);
            if (OccupancyCollectionView.GroupDescriptions == null) return;
            OccupancyCollectionView.GroupDescriptions.Add(new PropertyGroupDescription(@"LevelName"));
            OccupancyGrid.ItemsSource = OccupancyCollectionView;
        }
        
        /// <summary>
        /// Event handler sets "Occupant Load" Parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            OccupancyModel.SetOccupantLoadParameter();          
            Close();
        }
    }
}
