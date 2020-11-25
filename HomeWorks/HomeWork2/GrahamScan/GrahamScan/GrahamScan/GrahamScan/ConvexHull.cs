/*
©2012 Alex Kazaev
This product is licensed under Ms-PL http://www.opensource.org/licenses/MS-PL
*/

using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GrahamScan
{
    /// <summary>
    /// Builds convex hull using Graham scan algorithm
    /// </summary>
    public static class ConvexHull
    {
        /// <summary>
        /// Three points are a counter-clockwise turn if ccw > 0, clockwise if
        /// ccw < 0, and collinear if ccw = 0 because ccw is a determinant that
        /// gives the signed area of the triangle formed by p1, p2 and p3.
        /// </summary>
        /// <param name="p1">point 1 coords</param>
        /// <param name="p2">point 2 coords</param>
        /// <param name="p3">point 3 coords</param>
        /// <returns></returns>
        private static double ConterClockWise(Point p1, Point p2, Point p3)
        {
            return (p2.X - p1.X) * (p3.Y - p1.Y) - (p2.Y - p1.Y) * (p3.X - p1.X);
        }

        /// <summary>
        /// Sorts the list of points using PolarAngleComparer
        /// </summary>
        /// <param name="source"></param>
        private static void SortByPolarAngle(List<Point> source)
        {
            Point point0;
            // determine the point with min Y
            double minY = source.Min(pointCoordY => pointCoordY.Y);
            var leftPoints = source.Where(point=>point.Y == minY);
            if (leftPoints.Count() > 1)
            {
                // if there are more than 1 point, get the point with min X
                double minX = leftPoints.Min(pointCoordX => pointCoordX.X);
                point0 = leftPoints.First(point => point.X == minX);
            }
            else
            {
                point0 = leftPoints.First();
            }
            source.Sort(new PolarAngleComparer(point0));
        }

        /// <summary>
        /// Generates list of convex hull points from the given list of points
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static List<Point> CreateConvexHull(List<Point> source)
        {
            //1. create a stack of points
            Stack<Point> result = new Stack<Point>();
            //2. sort the incoming points
            SortByPolarAngle(source);
            //3. init stack with 2 first points
            result.Push(source[0]);
            result.Push(source[1]);

            //4. perform test for every other point
            for (int i = 2; i < source.Count; i++)
            {
                //5. the angle between NEXT_TO_TOP[S], TOP[S], and p(i) makes a nonleft turn -> remove if not a vertex
                while (ConterClockWise(result.ElementAt(1), result.Peek(), source[i]) > 0)
                {
                    result.Pop();

                }
                result.Push(source[i]);
            }
            return new List<Point>(result);
        }

        /// <summary>
        /// Compares points by polar angle to the 0 point.
        /// </summary>
        class PolarAngleComparer : IComparer<Point>
        {
            private Point point0;

            /// <summary>
            /// Creates an instance of PolarAngleComparer
            /// </summary>
            /// <param name="point0">the zero (top left) point</param>
            public PolarAngleComparer(Point point0)
            {
                this.point0 = point0;
            }

            /// <summary>
            /// Compares 2 point values in order to determine the one with minimal polar angle to given zero point
            /// </summary>
            /// <param name="a">first point</param>
            /// <param name="b">second point</param>
            /// <returns>a<b => value < 0; a==b => value == 0; a>b => value > 0</returns>
            public int Compare(Point a, Point b)
            {
                double angleA = (point0.X - a.X) / (a.Y - point0.Y);
                double angleB = (point0.X - b.X) / (b.Y - point0.Y);

                int result = (-angleA).CompareTo(-angleB);
                if (result == 0)
                {
                    double distanceA = (a - point0).LengthSquared;
                    double distanceB = (b - point0).LengthSquared;
                    result = distanceA.CompareTo(distanceB);
                }
                return result;
            }
        }
    }

}
