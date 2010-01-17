using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuralNetworkTest2
{
    public class Point2D
    {
        public Point2D()
        {
        }

        public Point2D(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public double Distance(Point2D point)
        {
            int x = Math.Abs(this.X - point.X);
            int y = Math.Abs(this.Y - point.Y);
            double distance = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            return distance;
        }
    }
}
