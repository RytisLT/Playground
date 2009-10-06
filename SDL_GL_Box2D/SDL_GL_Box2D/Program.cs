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

    internal class Program
    {
        private const int Width = 800;

        private const int Height = 600;

        private readonly List<IWorldObject> worldObjects = new List<IWorldObject>();

        private Surface screen;

        private World world;

        private Vec3 mousePos;

        private Vec2 windowMousePos;

        private Car lastCar;

        private Joint mouseJoint;

        private Key pressedKey;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
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
            //Gl.glHint(Gl.GL_PERSPECTIVE_CORRECTION_HINT, Gl.GL_NICEST);
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
            if (worldObject is Box)
            {
                this.Draw((Box)worldObject);
            }
            else if (worldObject is Circle)
            {
                this.Draw((Circle)worldObject);
            }
            else if (worldObject is Car)
            {
                this.Draw((Car)worldObject);
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
            var radius = circle.Radius * 0.5F;
            for (int i = 0; i < 360; i += 5)
            {
                var rads = Helper.DegreesToRad(i);
                Gl.glVertex3f(
                        (float)(x + System.Math.Sin(rads) * radius), (float)(y + System.Math.Cos(rads) * radius), -6.0f);
            }
            Gl.glEnd();

            this.SetGlColor(circle.RadiusColor);
            Gl.glBegin(Gl.GL_LINES);
            Gl.glVertex3f(x, y, -6.0f);
            Gl.glVertex3f(x + radius, y, -6.0f);
            Gl.glEnd();
        }

        private void Draw(Box box)
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
            var width = box.Width;
            var height = box.Height;
            Gl.glVertex3f(-width, height, z);
            Gl.glVertex3f(width, height, z);
            Gl.glVertex3f(width, -height, z);
            Gl.glVertex3f(-width, -height, z);
            Gl.glEnd();
        }

        private void Draw(Car car)
        {
            Draw(car.CarBody);
            Draw(car.FrontWheel);
            Draw(car.BackWheel);
            Draw(car.Pole);
        }

        private void SetGlColor(System.Drawing.Color color)
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
            Events.KeyboardUp += this.Events_KeyboardUp;
            Events.Tick += this.Events_Tick;
            Events.Quit += this.Events_Quit;
            Events.MouseMotion += this.Events_MouseMotion;
            Events.MouseButtonDown += this.Events_MouseButtonDown;
            Events.Fps = 60;
            this.SetWindowAttributes();
            this.screen = Video.SetVideoMode(Width, Height, true, true);

            this.InitBox2D();

            var ground = Box.Create(this.world, 0.0f, -2f, 3f, 0.1f, 0, 1.0f);
            ground.Color = System.Drawing.Color.Olive;
            this.worldObjects.Add(ground);

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

        private void Events_KeyboardUp(object sender, KeyboardEventArgs e)
        {
            pressedKey = Key.Escape;
        }

        private void Events_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var worldObject in worldObjects)
            {
                var vec3 = Helper.Foo(e.X, e.Y);
                if (worldObject.HitTest(vec3.X, vec3.Y))
                {
                    MouseJointDef mouseJointDef = new MouseJointDef();
                    mouseJointDef.Body1 = ((Box)worldObject).Body;
                    this.mouseJoint = this.world.CreateJoint(mouseJointDef);
                }
            }
        }

        private void Events_MouseMotion(object sender, MouseMotionEventArgs e)
        {
            this.windowMousePos = new Vec2(e.X, e.Y);

            int[] viewport = new int[4];

            Gl.glGetIntegerv(Gl.GL_VIEWPORT, viewport);

            var wx = (float)e.X;
            var wy = (float)viewport[3] - (float)e.Y;

            float[] wz = new float[1];
            Gl.glReadPixels((int)wx, (int)wy, 1, 1, Gl.GL_DEPTH_COMPONENT, Gl.GL_FLOAT, wz);

            Console.WriteLine("wz {0}", wz[0]);

            this.mousePos = Helper.GetOGLPos(e.X, e.Y);
            var foo = Helper.Foo(e.X, e.Y);

            Console.WriteLine("x:y:z {0}:{1}:{2}", foo.X, foo.Y, foo.Z);

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

        private void Events_Tick(object sender, TickEventArgs e)
        {
            this.DrawScene();
            Video.GLSwapBuffers();
            this.world.Step(1.0f / Events.Fps, 10, 10);
            switch (pressedKey)
            {
                case Key.LeftArrow:
                    if (this.lastCar != null)
                    {
                        this.lastCar.ApplyTorque(0.1f);
                    }
                    break;
                case Key.RightArrow:
                    if (this.lastCar != null)
                    {
                        this.lastCar.ApplyTorque(-0.1f);
                    }
                    break;
            }
        }

        private void Events_KeyboardDown(object sender, KeyboardEventArgs e)
        {
            this.pressedKey = e.Key;
            switch (e.Key)
            {
                case Key.Escape:
                case Key.Q:
                    Events.QuitApplication();
                    break;
                case Key.A:
                    var box = Box.Create(this.world, this.mousePos.X, this.mousePos.Y, 0.1f, 0.1f, 1f);
                    box.Color = Helper.GetRandomColor();
                    this.worldObjects.Add(box);
                    break;
                case Key.C:
                    var circle = Circle.Create(world, this.mousePos.X, this.mousePos.Y, 0.3f, 1.0f);
                    circle.Color = Helper.GetRandomColor();
                    this.worldObjects.Add(circle);

                    break;
                case Key.R:
                    var car = Car.Create(this.world, this.mousePos.X, this.mousePos.Y, 0.4f, 0.8f);
                    this.worldObjects.Add(car);
                    this.lastCar = car;
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
}