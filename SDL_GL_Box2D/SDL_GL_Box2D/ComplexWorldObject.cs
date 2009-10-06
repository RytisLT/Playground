namespace SDL_GL_Box2D
{
    using Box2DX.Common;
    using Box2DX.Dynamics;

    internal abstract class ComplexWorldObject : IWorldObject
    {
        protected ComplexWorldObject()
        {
        }

        public abstract bool HitTest(double x, double y);


        protected abstract void CreatePhysics(World world, float positionX, float positionY);
    }
}