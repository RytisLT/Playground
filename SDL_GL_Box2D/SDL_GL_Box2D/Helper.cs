using System;
using System.Collections.Generic;
using System.Text;

namespace SDL_GL_Box2D
{
    using System.Drawing;
    using System.Runtime.InteropServices;

    using Box2DX.Common;

    using SdlDotNet.Graphics;

    using Tao.OpenGl;

    class Helper
    {
        private static readonly Random Random = new Random();

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

        public static Vec3 GetOGLPos(int x, int y)
        {
            int[] viewport = new int[4];
            double[] modelviewMatrix = new double[16];
            double[] projectionMatrix = new double[16];

            Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, modelviewMatrix);
            Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, projectionMatrix);
            Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);

            float winX, winY, winZ;
            float[] winz = new float[1];
            winX = x;
            winY = (float)viewport[3] - (float)y;
            Gl.glReadPixels(x, (int)winY, 1, 1, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, winz);
            /*var winZ = (float)Marshal.PtrToStructure(ptr, typeof(float));
            Marshal.FreeHGlobal(ptr);*/
            winZ = winz[0];

            double nearX, nearY, nearZ;
            double farX, farY, farZ;

            double posX, posY, posZ;
            int success;
            success = Glu.gluUnProject(winX, winY, 0.0f, modelviewMatrix, projectionMatrix, viewport,
                                       out nearX, out nearY, out nearZ);
            if (success == 0)
            {
                throw new ApplicationException("Failed");
            }

            success = Glu.gluUnProject(winX, winY, 1000, modelviewMatrix, projectionMatrix, viewport,
                                           out farX, out farY, out farZ);

            if (success == 0)
            {
                throw new ApplicationException("Failed");
            }

            posX = nearX - farX;
            posY = nearY - farY;
            posZ = nearZ - farY;

/*            Console.WriteLine(
                    "X:Y {0}:{1} posX:posY:posZ {2}:{3}:{4}", x, y, posX, posY, posZ);*/

            var ration = 0.0165f;
            var result = new Vec3((float)posX / ration, (float)posY / ration, (float)posZ / ration);
            return result;
        }

        public static Color GetRandomColor()
        {
            return Color.FromArgb(Random.Next(255), Random.Next(255), Random.Next(255));
        }

        public static Vec3 Foo(int x, int y)
        {
            double outX, outY, outZ;

            double[] modelviewMatrix = new double[16];
            double[] projectionMatrix = new double[16];
            int[] viewport = new int[4];
            float[] pixels = new float[1];
            
            Gl.glGetDoublev(Gl.GL_MODELVIEW_MATRIX, modelviewMatrix);
            Gl.glGetDoublev(Gl.GL_PROJECTION_MATRIX, projectionMatrix);
            Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);

            Gl.glReadPixels(x, viewport[3] - y, 1, 1, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, pixels);

            Glu.gluUnProject(x, viewport[3] - y, pixels[0], modelviewMatrix, projectionMatrix, viewport, out outX, out outY, out outZ);
            return new Vec3((float)outX, (float)outY, (float)outZ);

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
