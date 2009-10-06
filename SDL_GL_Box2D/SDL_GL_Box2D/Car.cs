using System;
using System.Collections.Generic;
using System.Text;

namespace SDL_GL_Box2D
{
    using System.Drawing;

    using Box2DX.Common;
    using Box2DX.Dynamics;

    //TODO primitive/complex object
    class Car : ComplexWorldObject
    {
        private RevoluteJoint frontWheelJoint;

        private RevoluteJoint backWheelJoint;

        private RevoluteJoint poleJoint;

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
            frontWheelJoint = (RevoluteJoint)world.CreateJoint(frontWheelJointDef);
            
            var backWheelJointDef = new RevoluteJointDef();            
            backWheelJointDef.Initialize(CarBody.Body, BackWheel.Body, BackWheel.Body.GetWorldCenter());
            backWheelJoint = (RevoluteJoint)world.CreateJoint(backWheelJointDef);


            var poleJointDef = new RevoluteJointDef();
            
            var anchor = this.Pole.Body.GetWorldCenter();
            anchor.Y -= Pole.Height  ;
            poleJointDef.CollideConnected = false;
            poleJointDef.EnableLimit = true;
            poleJointDef.LowerAngle = Helper.DegreesToRad(-90);
            poleJointDef.UpperAngle = Helper.DegreesToRad(90);
            poleJointDef.Initialize(CarBody.Body, Pole.Body, anchor);
            poleJoint = (RevoluteJoint)world.CreateJoint(poleJointDef);
            
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

        public Box CarBody
        {
            get;
            private set;
        }

        public static Car Create(World world, float positionX, float positionY, float width, float height)
        {
            float poleHeightCarHeightRatio = 0.7f;

            var car = new Car();
            car.Width = width;
            car.Height = height;

            float carHeight = height * (1.0f - poleHeightCarHeightRatio);
            float bodyHeightWheelHeightRatio = 0.4f;
            float bodyHeight = carHeight * bodyHeightWheelHeightRatio;
            float wheelRadius = carHeight - bodyHeight;
            float bodyDensity = 1.5f;
            float wheelDensity = 0.4f;
            float wheelFriction = 0.9f;
            var poleDensity = 0.2f;
            
            float poleWidth = car.Width / 10.0f;
            float poleHeight = car.Height * poleHeightCarHeightRatio;
            

            car.CarBody = Box.Create(world, positionX, positionY, car.Width, bodyHeight, bodyDensity);
            car.FrontWheel = Circle.Create(
                    world,
                    positionX - car.Width + wheelRadius / 2.0f,
                    positionY - car.CarBody.Height,
                    wheelRadius,
                    wheelDensity,
                    wheelFriction);
            car.BackWheel = Circle.Create(
                    world,
                    positionX + car.Width - wheelRadius / 2.0f,
                    positionY - car.CarBody.Height,
                    wheelRadius,
                    wheelDensity,
                    wheelFriction);
            car.Pole = Box.Create(
                    world, positionX, positionY + bodyHeight*1f + poleHeight, poleWidth, poleHeight, poleDensity);


            car.BodyColor = Helper.GetRandomColor();
            car.WheelsColor = Helper.GetRandomColor();
            car.Pole.Color = Helper.GetRandomColor();
            car.CreatePhysics(world,positionX, positionY);
            return car;
        }

        public Box Pole
        {
            get;
            private set;
        }

        public void Accelerate()
        {
            this.FrontWheel.Body.ApplyTorque(1.0f);
        }

        public void Deccelerate()
        {
            this.FrontWheel.Body.ApplyTorque(-1.0f);
        }

        public void ApplyTorque(float torque)
        {
            this.FrontWheel.Body.ApplyTorque(torque);
        }
    }

}
