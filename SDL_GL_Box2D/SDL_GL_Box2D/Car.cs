using System;
using System.Collections.Generic;
using System.Text;

namespace SDL_GL_Box2D
{
    using System.Drawing;

    using Box2DX.Common;
    using Box2DX.Dynamics;

    class Car : WorldObject
    {
        private RevoluteJoint frontWheelJoint;

        private RevoluteJoint backWheelJoint;

        private Car()
        {
            
        }

        public float Width
        {
            get;
            set;
        }

        public float Height
        {
            get;
            set;
        }

        public System.Drawing.Color BodyColor
        {
            get
            {
                return this.CarBody.Color;
            }
            set
            {
                this.CarBody.Color = value;
            }
        }

        public System.Drawing.Color WheelsColor
        {
            set
            {
                this.FrontWheel.Color = value;
                this.BackWheel.Color = value;
            }
        }

        public override bool HitTest(double x, double y)
        {
            return false;
        }

        protected override void CreatePhysics(World world, float positionX, float positionY)
        {           
            var frontWheelJointDef = new RevoluteJointDef();
            frontWheelJointDef.Initialize(CarBody.Body, FrontWheel.Body, FrontWheel.Body.GetWorldCenter());                           
            var backWheelJointDef = new RevoluteJointDef();
            backWheelJointDef.Initialize(CarBody.Body, BackWheel.Body, BackWheel.Body.GetWorldCenter());
            frontWheelJoint = (RevoluteJoint)world.CreateJoint(frontWheelJointDef);
            backWheelJoint = (RevoluteJoint)world.CreateJoint(backWheelJointDef);
            
            world.CreateJoint(backWheelJointDef);
        }

        

        public Circle BackWheel
        {
            get;
            private set;
        }

        public Circle FrontWheel
        {
            get;
            private set;
        }

        public TestBox CarBody
        {
            get;
            private set;
        }

        public static Car Create(World world, float positionX, float positionY, float width, float height)
        {
            var car = new Car();
            car.Width = width;
            car.Height = height;

            float bodyHeight = car.Height / 2.5f;
            float wheelRadius = car.Height / 1.5f;
            float bodyDensity = 1.5f;
            float wheelDensity = 0.4f;

            car.CarBody = TestBox.Create(world, positionX, positionY, car.Width, bodyHeight, bodyDensity);
            car.FrontWheel = Circle.Create(world, positionX - car.Width + wheelRadius, positionY - car.CarBody.Height, wheelRadius, wheelDensity);
            car.BackWheel = Circle.Create(world, positionX + car.Width - wheelRadius, positionY - car.CarBody.Height, wheelRadius, wheelDensity);

            car.BodyColor = Helper.GetRandomColor();
            car.WheelsColor = Helper.GetRandomColor();
            car.CreatePhysics(world,positionX, positionY);
            return car;
        }
    }
}
