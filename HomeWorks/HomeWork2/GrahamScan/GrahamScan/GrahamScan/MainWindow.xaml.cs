/*
©2012 Alex Kazaev
This product is licensed under Ms-PL http://www.opensource.org/licenses/MS-PL
*/

using System.Windows;
using System.Windows.Input;

namespace GrahamScan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel = new MainWindowViewModel();
        
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = viewModel;
            CommandBindings.Add(new CommandBinding(MainWindowViewModel.CreateConvexHull, viewModel.ExecuteCreateConvexHull, viewModel.CanExecuteCreateConvexHull));
            CommandBindings.Add(new CommandBinding(MainWindowViewModel.ClearPoints, viewModel.ExecuteClearPoints, viewModel.CanExecuteClearPoints));
        }

        private void MainCanvas_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point coords = e.GetPosition(MainItemsControl);
            viewModel.AddPoint(coords);
        }

    }
}
