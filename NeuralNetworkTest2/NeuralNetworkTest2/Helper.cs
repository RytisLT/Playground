using System;
using System.Collections.Generic;
using System.Text;

namespace SDL_GL_Box2D
{
    using System.Drawing;
    using System.Runtime.InteropServices;


    using SdlDotNet.Graphics;

    using Tao.OpenGl;

    class Helper
    {
        private static readonly Random Random = new Random();

      

        public static Color GetRandomColor()
        {
            return Color.FromArgb(Random.Next(255), Random.Next(255), Random.Next(255));
        }

       

        public static float DegreesToRad(float degrees)
        {
            var radians = (float)(degrees * System.Math.PI / 180);
            return radians;
        }

        public static float RadiansToDegrees(float radians)
        {
            var degrees = (float)(radians * 180 / System.Math.PI);
            return degrees;
        }
    }
}
