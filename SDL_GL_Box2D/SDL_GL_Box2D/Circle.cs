using System;
using System.Collections.Generic;
using System.Text;

namespace SDL_GL_Box2D
{
    using System.Drawing;

    using Box2DX.Collision;
    using Box2DX.Common;
    using Box2DX.Dynamics;

    class Circle : WorldObject
    {
        private Circle()
        {
            
        }

        public float Radius
        {
            get;
            set;
        }

        public System.Drawing.Color RadiusColor
        {
            get;
            set;
        }

        public override bool HitTest(double x, double y)
        {
            return false;
        }

        protected override void CreatePhysics(World world, float positionX, float positionY)
        {         
            var bodyDef = new BodyDef();
            bodyDef.Position.Set(positionX, positionY);
            bodyDef.LinearDamping = 0.3f;
            bodyDef.AngularDamping = 0.6f;
            var circleDef = new CircleDef();
            circleDef.Density = this.Density;
            circleDef.Radius = Radius *0.5f;
            circleDef.Friction = 0.3f;
            circleDef.Restitution = 0.3f;
            this.body = world.CreateBody(bodyDef);
            this.body.CreateShape(circleDef);
            this.body.SetMassFromShapes();
        }

        public static Circle Create(World world, float positionX, float positionY, float radius, float density)
        {
            var result = new Circle();
            result.Radius = radius;
            result.Density = density;
            result.Color = System.Drawing.Color.White;
            result.RadiusColor = Helper.GetRandomColor();
            result.CreatePhysics(world, positionX, positionY);
            return result;
        }
    }
}
