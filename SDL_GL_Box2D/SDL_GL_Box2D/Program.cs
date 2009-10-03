using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SDLTest
{
    using System.Drawing;

    using SdlDotNet.Core;
    using SdlDotNet.Graphics;
    using SdlDotNet.Graphics.Primitives;
    using SdlDotNet.Input;
    using Tao.OpenGl;

    class Program
    {
        private int width = 640;

        private int height = 480;

        private Surface screen;

        private Box cube;

        private float triangleRotation = 10;

        private float quadRotation;

        private Random random = new Random();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var program = new Program();
            program.Go();
        }

        public Program()
        {
            this.Initialize();
        }

        private void InitGL()
        {
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glClearColor(0, 0, 0, 0.5f);
            Gl.glClearDepth(1.0f);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LEQUAL);
            Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
        }


        private void Reshape()
        {
            this.Reshape(1000);
        }

        private void Reshape(int distance)
        {
            Gl.glViewport(0, 0, width, height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Glu.gluPerspective(45.0f, (width / (float)height), 0.1f, distance);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }

        private void DrawScene()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            
            Gl.glTranslatef(-1.5f, 0, -6);
            Gl.glRotatef(triangleRotation, 0.0f, 1.0f, 0.0f);
            Gl.glBegin(Gl.GL_TRIANGLES);
                Gl.glColor3f(1.0f, 0.0f, 0.0f);
                Gl.glVertex3f(0, 1, 0);
                Gl.glColor3f(0.0f, 1.0f, 0.0f);
                Gl.glVertex3f(-1, -1, 0);
                Gl.glColor3f(0.0f, 0.0f, 1.0f);
                Gl.glVertex3f(1, -1, 0);
            Gl.glEnd();

            Gl.glLoadIdentity();
            Gl.glTranslatef(1.5f, 0, -6f);
            Gl.glRotatef(quadRotation, 0.0f, 0.0f, 1.0f);
            Gl.glColor3f(0.5f, 0.5f, 1.0f);
            Gl.glBegin(Gl.GL_QUADS);
                Gl.glVertex3f(-1.0f, 1.0f, 0.0f);
                Gl.glVertex3f(1.0f, 1.0f, 0.0f);
                Gl.glVertex3f(1.0f, -1.0f, 0.0f);
                Gl.glVertex3f(-1.0f, -1.0f, 0.0f);
            Gl.glEnd();
            
        }

        private void Initialize()
        {            
            Video.WindowCaption = "Hello world";
            Events.KeyboardDown += this.Events_KeyboardDown;
            Events.Tick += this.Events_Tick;
            Events.Quit += this.Events_Quit;
            Events.Fps = 50;
            this.SetWindowAttributes();
            this.screen = Video.SetVideoMode(width, height, true, true);
        }

        private void SetWindowAttributes()
        {
            Video.WindowIcon();
            Video.WindowCaption = "Gl Test";
        }

        private void Go()
        {
            this.Reshape();
            this.InitGL();
            Events.Run();            
        }

        void Events_Tick(object sender, TickEventArgs e)
        {            
            this.DrawScene();
            Video.GLSwapBuffers();
            triangleRotation++;
            quadRotation++;
        }

        void Events_KeyboardDown(object sender, SdlDotNet.Input.KeyboardEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Q)
            {
                Events.QuitApplication();
            }
        }

        void Events_Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }
    }
}
