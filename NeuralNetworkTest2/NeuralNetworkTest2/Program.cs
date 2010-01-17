using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;
using Tao.OpenGl;

namespace NeuralNetworkTest2
{
    class Program
    {
        private readonly List<IWorldObject> worldObjects = new List<IWorldObject>();
        private Surface screen;
        private int Width = 800;
        private int Height = 600;
        private Algorithm algorithm;

        static void Main(string[] args)
        {
            var program  = new Program();
            program.Run();
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        public void Run()
        {
            this.Initialize();
            this.Reshape();
            this.InitGL();
            this.worldObjects.Add(new Box(60, 120));
            this.worldObjects.Add(new Box(10, 10, 0 - 30 + 5, 0 + 60 - 5));
            this.worldObjects.Add(new Box(10, 10, 10 - 30 + 5, 0 + 60 - 5));
            this.worldObjects.Add(new Box(10, 10, 20 - 30 + 5, 0 + 60 - 5));
            this.worldObjects.Add(new Box(10, 10, 31 - 30 + 5, 0 + 60 - 5));
            
           // Gene length 2 - just X and Y position
           algorithm = new Algorithm();
           for (int i = 0; i < 10; ++i)
           {
               var box1 = new Box(10, 10);
               algorithm.AddBox(box1);
           }
           algorithm.Run();
           

            Events.Run();


         
        }

        private void InitGL()
        {
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glClearColor(0, 0, 0, 0.5f);
            Gl.glClearDepth(1.0f);
            Gl.glEnable(Gl.GL_DEPTH_TEST);
            Gl.glDepthFunc(Gl.GL_LEQUAL);
            //Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
        }

        private void Reshape()
        {
            this.Reshape(1000);
        }

        private void Reshape(int distance)
        {
            Gl.glViewport(0, 0, Width, Height);
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Glu.gluPerspective(45.0f, (Width / (float)Height), 0.1f, distance);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
        }


        private void SetWindowAttributes()
        {
            Video.WindowIcon();
            Video.WindowCaption = "Gl Test";
        }

        private void Initialize()
        {
            Video.WindowCaption = "Hello world";
            Events.KeyboardDown += this.Events_KeyboardDown;
            Events.Tick += this.Events_Tick;
            Events.Quit += this.Events_Quit;            
            Events.Fps = 60;
            this.SetWindowAttributes();
            this.screen = Video.SetVideoMode(Width, Height, true, true);
            
            /*float size = 0.15f;
            int levels = 8;
            float startX = -2f;
            float startY = -1.7f;
            for (int i = levels; i > 0; --i)
            {
                for (int j = 0; j < i; j++)
                {
                    var x = startX + j * size * 2.1f;
                    var y = startY + (levels - i) * size * 2;
                    var box = Box.Create(this.world, x, y, size, size, 1);
                    box.Color = Helper.GetRandomColor();
                    this.worldObjects.Add(box);
                }
            }*/
        }

        private void Events_Tick(object sender, TickEventArgs e)
        {
            algorithm.Epoch();
            this.worldObjects.RemoveRange(1, this.worldObjects.Count - 1);
            var ground = (Box)this.worldObjects[0];
            var decode = this.algorithm.Best.Decode();
            for (int i = 0; i < this.algorithm.Cargo.Count; i++)
            {
                var pos = decode[i];
                var box = this.algorithm.Cargo[i];
                var newBox = new Box(box.Width, box.Height);


                newBox.X = pos.X - (ground.Width / 2) + (newBox.Width / 2);
                newBox.Y = -1 * pos.Y + (ground.Height / 2) - (newBox.Height / 2);
                newBox.Color = box.Color;
                this.worldObjects.Add(newBox);
            }


            this.DrawScene();
            Video.GLSwapBuffers();          
        }

        private void Draw(Box box)
        {
            Gl.glLoadIdentity();
//            Gl.glTranslatef(box.Position.X, box.Position.Y, -6f);
            var ratio = 0.03f;
            Gl.glTranslatef(box.X * ratio, box.Y * ratio, -6f);

            var color = box.Color;
            
            this.SetGlColor(color);

            Gl.glRotatef(box.Rotation, 0.0f, 0.0f, 1.0f);
            Gl.glBegin(Gl.GL_QUADS);
            var z = 0.0f;
            var width = box.Width * ratio * 0.5f;
            var height = box.Height * ratio * 0.5f;
            Gl.glVertex3f(-width, height, z);
            Gl.glVertex3f(width, height, z);
            Gl.glVertex3f(width, -height, z);
            Gl.glVertex3f(-width, -height, z);
            Gl.glEnd();
        }

        private void SetGlColor(System.Drawing.Color color)
        {
            float ratio = 1 / 255f;
            Gl.glColor3f(ratio * color.R, ratio * color.G, ratio * color.B);
        }

        private void Draw(IWorldObject worldObject)
        {
            if (worldObject is Box)
            {
                this.Draw((Box)worldObject);
            }           
        }

        private void DrawScene()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();

            foreach (var worldObject in this.worldObjects)
            {
                Draw(worldObject);
            }

            Gl.glFlush();
        }

        private void Events_KeyboardDown(object sender, KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                case Key.Q:
                    Events.QuitApplication();
                    break;                
                /*  case Key.LeftArrow:
                if (this.lastCar != null)
                {
                    this.lastCar.Accelerate();
                }
                break;
            case Key.RightArrow:
                if (this.lastCar != null)
                {
                    this.lastCar.Deccelerate();
                }
                break;*/
            }
        }

        private void Events_Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }
    }

    internal interface IWorldObject
    {
        bool HitTest(double x, double y);
    }
}
