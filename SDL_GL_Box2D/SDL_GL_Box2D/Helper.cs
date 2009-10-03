using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDL_GL_Box2D
{
    using Box2DX.Common;

    using SdlDotNet.Graphics;

    class Helper
    {
        public static Vec2 ConvertScreenToWorld(int x, int y)
        {
            var width = (float)Video.Screen.Width;
            var height = (float) Video.Screen.Height;
            float u = x / width;            
            float v = (height - y) / height;

            float ratio = width / height;
            var extents = new Vec2(ratio * 25.0f, 25.0f);
            float viewZoom = 1.0f;
            extents *= viewZoom;

            Vec2 viewCenter = new Vec2(0.0f, 0.0f);
            Vec2 lower = viewCenter - extents;
            Vec2 upper = viewCenter + extents;

            Vec2 p = new Vec2();
            p.X = (1.0f - u) * lower.X + u * upper.X;
            p.Y = (1.0f - v) * lower.Y + v * upper.Y;
            return p;
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
