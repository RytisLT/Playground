namespace SDL_GL_Box2D
{
    using Box2DX.Common;
    using Box2DX.Dynamics;

    internal abstract class BasicWorldObject : IWorldObject
    {
        protected BasicWorldObject()
        {
        }

        protected Body body;

        public Vec2 Position
        {
            get
            {
                return this.body.GetPosition();
            }           
        }

        public float Density
        {
            get;
            protected set;
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

        public abstract bool HitTest(double x, double y);

        public System.Drawing.Color Color
        {
            get;
            set;
        }

        public Body Body
        {
            get
            {
                return this.body;
            }            
        }

        protected abstract void CreatePhysics(World world, float positionX, float positionY, float friction);
    }
}