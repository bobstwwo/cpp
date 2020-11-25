/*
©2012 Alex Kazaev
This product is licensed under Ms-PL http://www.opensource.org/licenses/MS-PL
*/

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace GrahamScan
{
    /// <summary>
    /// VIewModel layer for the demo project.
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region constants
        const int MinConvexHullPointsCount = 3;
        const int MinClearPointsCount = 1;
        #endregion

        #region fields
        private ObservableCollection<Point> points;
        private PointCollection hullPoints;
        #endregion

        #region properties
        public ObservableCollection<Point> Points
        {
            private set
            {
                points = value;
                NotifyChanged("Points");
            }
            get
            {
                return points;
            }
        }
        public PointCollection HullPoints
        {
            private set
            {
                hullPoints = value;
                NotifyChanged("HullPoints");
            }
            get
            {
                return hullPoints;
            }
        }
        #endregion

        #region command definitions
        public static RoutedCommand CreateConvexHull = new RoutedCommand();
        public static RoutedCommand ClearPoints = new RoutedCommand();
        #endregion

        #region constructor
        public MainWindowViewModel()
        {
            Points = new ObservableCollection<Point>();
            HullPoints = new PointCollection();
        }
        #endregion

        #region methods
        /// <summary>
        /// Adds point represented by Point struct to the collection of points
        /// </summary>
        /// <param name="p">point to add</param>
        public void AddPoint(Point p)
        {
            Points.Add(p);
        }

        /// <summary>
        /// Creates convex hull points. Used in CreateConvexHull command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ExecuteCreateConvexHull(object sender,  ExecutedRoutedEventArgs e)
        {
            List<Point> source = points.ToList();
            source = ConvexHull.CreateConvexHull(source);
            HullPoints = new PointCollection(source);
        }

        /// <summary>
        /// Determines if it is possible to draw a convex hull. Used in CreateConvexHull command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CanExecuteCreateConvexHull(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Points.Count >= MinConvexHullPointsCount;
        }

        /// <summary>
        /// Clears the point collection. Used in ClearPoints command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ExecuteClearPoints(object sender, ExecutedRoutedEventArgs e)
        {
            Points.Clear();
            HullPoints.Clear();
            HullPoints = new PointCollection();
        }

        /// <summary>
        /// Determines if it is possible to clear the points collection. Used in ClearPoints command. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CanExecuteClearPoints(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Points.Count >= MinClearPointsCount;
        }
        #endregion

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        
        /// <summary>
        /// Helper method. Raises PropertyChanged event if there are any subscribers.
        /// </summary>
        /// <param name="prop"></param>
        private void NotifyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        #endregion
    }
}
