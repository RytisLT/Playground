using System;

namespace SDL_GL_Box2D
{
    using System.Collections.Generic;

    using Box2DX.Collision;
    using Box2DX.Common;
    using Box2DX.Dynamics;

    using SdlDotNet.Core;
    using SdlDotNet.Graphics;
    using SdlDotNet.Input;

    using Tao.OpenGl;
    
    class Program
    {
        private const int Width = 800;

        private const int Height = 600;

        private float triangleRotation = 10;

        private float quadRotation;

        private readonly List<IWorldObject> worldObjects = new List<IWorldObject>();

        private readonly Random random = new Random();

        private Surface screen;

        private World world;

        private Vec3 mousePos;

        private Vec2 windowMousePos;

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

        private void InitBox2D()
        {
            var aabb = new AABB();
            aabb.LowerBound = new Vec2(-200, -200);
            aabb.UpperBound = new Vec2(200, 200);
            this.world = new World(aabb, new Vec2(0, -10), true);                       
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

        private void Draw(IWorldObject worldObject)
        {
            if (worldObject is TestBox)
            {
                this.Draw((TestBox)worldObject);                
            }          
            else if (worldObject is Circle)
            {
                this.Draw((Circle)worldObject);
            }        
        }

        private void Draw(Circle circle)
        {
            Gl.glLoadIdentity();
            var x = circle.Position.X;
            var y = circle.Position.Y;
            Gl.glTranslatef(x, y, -6f);
            Gl.glRotatef(circle.Rotation, 0.0f, 0.0f, 1.0f);
            Gl.glTranslatef(-x, -y, 6f);
            this.SetGlColor(circle.Color);            
            Gl.glBegin(Gl.GL_TRIANGLE_FAN);
            var radius = circle.Radius*0.5F;
            for (int i = 0; i < 360; i += 5)
            {
                var rads = Helper.DegreesToRad(i);                
                Gl.glVertex3f((float)(x + System.Math.Sin(rads) * radius), (float)(y + System.Math.Cos(rads) * radius), -6.0f);
            }
            Gl.glEnd();

            this.SetGlColor(circle.RadiusColor);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3f(x , y , -6.0f);
            Gl.glVertex3f(x + radius, y, -6.0f);
            Gl.glEnd();
        }

        private void Draw(TestBox box)
        {
            Gl.glLoadIdentity();
            Gl.glTranslatef(box.Position.X, box.Position.Y, -6f);

            var color = box.Color;
            if (box.IsHot)
            {
                color = System.Drawing.Color.Red;
            }
            this.SetGlColor(color);

            Gl.glRotatef(box.Rotation, 0.0f, 0.0f, 1.0f);
            Gl.glBegin(Gl.GL_QUADS);
            var z = 0.0f;
            Gl.glVertex3f(-box.Width, box.Height, z);
            Gl.glVertex3f(box.Width, box.Height, z);
            Gl.glVertex3f(box.Width, -box.Height, z);
            Gl.glVertex3f(-box.Width, -box.Height, z);
            Gl.glEnd();
        }

        private void SetGlColor (System.Drawing.Color color)
        {
            float ratio = 1 / 255f;                        
            Gl.glColor3f(ratio * color.R, ratio * color.G, ratio * color.B);
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

        private void Initialize()
        {            
            Video.WindowCaption = "Hello world";
            Events.KeyboardDown += this.Events_KeyboardDown;
            Events.Tick += this.Events_Tick;
            Events.Quit += this.Events_Quit;
            Events.MouseMotion += this.Events_MouseMotion;
            Events.Fps = 60;
            this.SetWindowAttributes();
            this.screen = Video.SetVideoMode(Width, Height, true, true);

            this.InitBox2D();

            var ground = TestBox.Create(this.world, 0.0f, -2f, 3f, 0.1f, 0);
            ground.Color = System.Drawing.Color.Olive;
            this.worldObjects.Add(ground);



            float size = 0.15f;
            int levels = 8;
            float startX = -2f;
            float startY = -1.7f;
            for (int i = levels; i > 0; --i)
            {                
                for (int j = 0; j < i; j++)
                {
                    var x = startX + j * size * 2.1f ;
                    var y = startY + (levels - i) * size * 2;
                    var box = TestBox.Create(this.world, x, y, size, size, 1);
                    box.Color = Helper.GetRandomColor();
                    this.worldObjects.Add(box);
                }                
            }
            
        }

        void Events_MouseMotion(object sender, MouseMotionEventArgs e)
        {
            this.windowMousePos = new Vec2(e.X, e.Y);
           
            this.mousePos = Helper.GetOGLPos(e.X, e.Y);
            var foo = Helper.Foo(e.X, e.Y);

            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();	

            foreach (var worldObject in this.worldObjects)
            {
                if (worldObject.HitTest(this.mousePos.X, this.mousePos.Y))
                {
                    break;
                }
            }
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
            this.world.Step(1.0f / Events.Fps, 10, 10);
            this.triangleRotation++;
            this.quadRotation++;
        }

        void Events_KeyboardDown(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Key.Escape || e.Key == Key.Q)
            {
                Events.QuitApplication();
            }
            if (e.Key == Key.A)
            {
                var box = TestBox.Create(this.world, this.mousePos.X, this.mousePos.Y, 0.1f, 0.1f, 1f);
                box.Color = Helper.GetRandomColor();
                this.worldObjects.Add(box);
            }
            else if (e.Key == Key.C)
            {
                var circle = Circle.Create(world, this.mousePos.X, this.mousePos.Y, 0.3f, 1.0f);
                circle.Color = Helper.GetRandomColor();
                this.worldObjects.Add(circle);
            }
        }

        void Events_Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }
    }
}