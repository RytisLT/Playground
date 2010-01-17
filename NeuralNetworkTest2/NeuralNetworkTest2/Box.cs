using System;
using SDL_GL_Box2D;

namespace NeuralNetworkTest2
{
    public class Box : IWorldObject
    {
        public Box()
        {
            this.Color = Helper.GetRandomColor();
        }

        public Box(int width, int height)
            : this()
        {
            this.Height = height;
            this.Width = width;
        }

        public Box(int width, int height, int x, int y)
            : this()
        {
            this.X = x;
            this.Y = y;
            this.Height = height;
            this.Width = width;
        }

        public System.Drawing.Color Color
        {
            get;
            set;
        }

        /// <summary>
        /// Rotation in degrees
        /// </summary>
        public float Rotation
        {
            get; set;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public bool Overlaps(Box box)
        {
            bool result = false;
            if (this.X + this.Width >= box.X && box.X + box.Width >= this.X &&
                this.Y + this.Height >= box.Y && box.Y + box.Height >= this.Y)
            {
                result = true;
            }

            return result;
        }

        public Point2D Center
        {
            get
            {
                return new Point2D((this.X + this.Width) / 2, (this.Y + this.Height) / 2);
            }
        }

        public bool HitTest(double x, double y)
        {
            throw new NotImplementedException();
        }
    }
}