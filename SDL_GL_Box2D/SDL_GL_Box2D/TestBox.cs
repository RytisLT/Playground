using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDL_GL_Box2D
{
    using System.Drawing;

    using Box2DX.Collision;
    using Box2DX.Common;
    using Box2DX.Dynamics;

    class TestBox : IWorldObject
    {
        const float ratio = 100;

        private Body body;

        public Vec2 Position
        {
            get
            {
                return this.body.GetPosition();
            }           
        }

        public float Width
        {
            get;
            private set;
        }

        public float Height
        {
            get;
            private set;
        }

        public float Density
        {
            get;
            private set;
        }

        /// <summary>
        /// Rotation in degrees
        /// </summary>
        public float Rotation
        {
            get
            {
                return Helper.RadiansToDegrees(this.body.GetAngle());
            }
            set
            {
                this.body.SetXForm(this.body.GetPosition(), Helper.DegreesToRad(value));
            }

        }

        public System.Drawing.Color Color
        {
            get;
            set;
        }
        
        private TestBox()
        {
            
        }

        private void CreatePhysics(World world, float positionX, float positionY)
        {
            var bodyDef = new BodyDef();
            bodyDef.Position.Set(positionX, positionY);
            this.body = world.CreateBody(bodyDef);
            var shapeDef = new PolygonDef();
            shapeDef.SetAsBox(this.Width , this.Height);
            shapeDef.Density = this.Density;
            shapeDef.Friction = 0.3f;
            shapeDef.Restitution = 0.3f;
            body.CreateShape(shapeDef);
            body.SetMassFromShapes();
        }

        public static TestBox Create(World world, float positionX, float positionY, float width, float height, float density)
        {
            var result = new TestBox();            
            result.Width = width;
            result.Height = height;
            result.Density = density;
            result.Color = System.Drawing.Color.White;
            result.CreatePhysics(world, positionX, positionY);
            return result;  
        }

        public bool HitTest(double x, double y)
        {
            var rectangle = new RectangleF(this.Position.X, this.Position.Y, this.Width, this.Height);
            return rectangle.IntersectsWith(new RectangleF((float)x, (float)y, 1, 1));            
        }
    }

    internal interface IWorldObject
    {
        bool HitTest(double x, double y);

        System.Drawing.Color Color
        {
            get;
            set;
        }
    }
}
