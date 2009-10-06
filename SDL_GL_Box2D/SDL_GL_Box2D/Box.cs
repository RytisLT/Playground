namespace SDL_GL_Box2D
{
    using System.Drawing;

    using Box2DX.Collision;
    using Box2DX.Dynamics;

    internal class Box : BasicWorldObject
    {
        private Box()
        {
        }

        public float Height
        {
            get;
            private set;
        }

        public bool IsHot
        {
            get;
            private set;
        }

        public float Width
        {
            get;
            private set;
        }

        public static Box Create(
                World world, float positionX, float positionY, float width, float height, float density)
        {
            return Create(world, positionX, positionY, width, height, density, 0.3f);
        }

        public static Box Create(
                World world, float positionX, float positionY, float width, float height, float density, float friction)
        {
            var result = new Box();
            result.Width = width;
            result.Height = height;
            result.Density = density;
            result.Color = System.Drawing.Color.White;
            result.CreatePhysics(world, positionX, positionY, friction);
            return result;
        }

        public override bool HitTest(double x, double y)
        {
            var rectangle = new RectangleF(this.Position.X, this.Position.Y, this.Width, this.Height);
            this.IsHot = rectangle.IntersectsWith(new RectangleF((float)x, (float)y, 1, 1));
            return this.IsHot;
        }

        protected override void CreatePhysics(World world, float positionX, float positionY, float friction)
        {
            var bodyDef = new BodyDef();
            bodyDef.Position.Set(positionX, positionY);
            this.body = world.CreateBody(bodyDef);
            var shapeDef = new PolygonDef();
            shapeDef.SetAsBox(this.Width, this.Height);
            shapeDef.Density = this.Density;
            shapeDef.Friction = friction;
            shapeDef.Restitution = 0.3f;
            this.body.CreateShape(shapeDef);
            this.body.SetMassFromShapes();
        }
    }
}