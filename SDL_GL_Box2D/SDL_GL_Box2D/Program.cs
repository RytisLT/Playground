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

        private List<IWorldObject> objects = new List<IWorldObject>();
        

        private Surface screen;

        private World world;

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
            aabb.LowerBound = new Vec2(-100, -100);
            aabb.UpperBound = new Vec2(100, 100);
            this.world = new World(aabb, new Vec2(0, -10), true);
            var groudBodyDef = new BodyDef();
            
            
            groudBodyDef.Position.Set(0, 10);
            var groundBody = world.CreateBody(groudBodyDef);
            var groundShapeDef = new PolygonDef();
            groundShapeDef.SetAsBox(50, 10);
            groundBody.CreateShape(groundShapeDef);

           /* var bodyDef = new BodyDef();
            bodyDef.Position.Set(0, 4);
            var body = world.CreateBody(bodyDef);
            var shapeDef = new PolygonDef();
            shapeDef.SetAsBox(1, 1);
            shapeDef.Density = 1;
            shapeDef.Friction = 0.3f;
            body.CreateShape(shapeDef);
            body.SetMassFromShapes();*/
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
        }
        
        private void Draw(TestBox box)
        {            
           Gl.glLoadIdentity();
              Gl.glTranslatef(box.Position.X, box.Position.Y, -6f);
              float step = 1 / 255f;
              Gl.glColor3f(step * box.Color.R, step * box.Color.G, step * box.Color.B);
              Gl.glRotatef(box.Rotation, 0.0f, 0.0f, 1.0f);
              Gl.glBegin(Gl.GL_QUADS);
              Gl.glVertex3f(-box.Width, box.Height, 0.0f);
              Gl.glVertex3f(box.Width, box.Height, 0.0f);
              Gl.glVertex3f(box.Width, -box.Height, 0.0f);
              Gl.glVertex3f(-box.Width, -box.Height, 0.0f);
            Gl.glEnd();
        }

        private void DrawScene()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT);
            Gl.glLoadIdentity();
            
            /*Gl.glTranslatef(-1.5f, 0, -6);
            Gl.glRotatef(this.triangleRotation, 0.0f, 1.0f, 0.0f);
            Gl.glBegin(Gl.GL_TRIANGLES);
            Gl.glColor3f(1.0f, 0.0f, 0.0f);
            Gl.glVertex3f(0, 1, 0);
            Gl.glColor3f(0.0f, 1.0f, 0.0f);
            Gl.glVertex3f(-1, -1, 0);
            Gl.glColor3f(0.0f, 0.0f, 1.0f);
            Gl.glVertex3f(1, -1, 0);
            Gl.glEnd();*/

            foreach (var worldObject in objects)
            {
                Draw(worldObject);
            }           
        }

        private void Initialize()
        {            
            Video.WindowCaption = "Hello world";
            Events.KeyboardDown += this.Events_KeyboardDown;
            Events.Tick += this.Events_Tick;
            Events.Quit += this.Events_Quit;
            Events.Fps = 50;
            this.SetWindowAttributes();
            this.screen = Video.SetVideoMode(Width, Height, true, true);

            this.InitBox2D();

            var ground = TestBox.Create(this.world, 0.0f, -2f, 3f, 0.1f, 0);
            ground.Color = System.Drawing.Color.Olive;
            this.objects.Add(ground);



            float size = 0.3f;
            int levels = 3;
            float startX = -2f;
            float startY = -0.5f;
            for (int i = levels; i > 0; --i)
            {                
                for (int j = 0; j < i; j++)
                {
                    var x = startX + j * size + 0.1f;
                    var y = startY - i * size + 0.1f;
                    var box = TestBox.Create(this.world, x, y, size, size, 1);
                    objects.Add(box);
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
        }

        void Events_Quit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }
    }
}