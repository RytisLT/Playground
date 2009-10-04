namespace SDL_GL_Box2D
{
    using System;

    using Box2DX.Collision;
    using Box2DX.Common;
    using Box2DX.Dynamics;

    internal class BipedDef 
    {
        private const float k_scale = 3.0f;

        private static short count;

        public BodyDef ChestDef = new BodyDef();

        public PolygonDef ChestPoly = new PolygonDef();

        public CircleDef HeadCirc  = new CircleDef();

        public BodyDef HeadDef = new BodyDef();

        public RevoluteJointDef LAnkleDef = new RevoluteJointDef();

        public BodyDef LCalfDef = new BodyDef();

        public PolygonDef LCalfPoly = new PolygonDef();

        public RevoluteJointDef LElbowDef = new RevoluteJointDef();

        public BodyDef LFootDef = new BodyDef();

        public PolygonDef LFootPoly = new PolygonDef();

        public BodyDef LForearmDef = new BodyDef();

        public PolygonDef LForearmPoly = new PolygonDef();

        public BodyDef LHandDef = new BodyDef();

        public PolygonDef LHandPoly = new PolygonDef();

        public RevoluteJointDef LHipDef = new RevoluteJointDef();

        public RevoluteJointDef LKneeDef = new RevoluteJointDef();

        public RevoluteJointDef LowerAbsDef = new  RevoluteJointDef();

        public RevoluteJointDef LowerNeckDef = new  RevoluteJointDef();

        public RevoluteJointDef LShoulderDef = new RevoluteJointDef();

        public BodyDef LThighDef = new  BodyDef();

        public PolygonDef LThighPoly = new  PolygonDef();

        public BodyDef LUpperArmDef = new BodyDef();

        public PolygonDef LUpperArmPoly = new PolygonDef();

        public RevoluteJointDef LWristDef = new RevoluteJointDef();

        public BodyDef NeckDef = new BodyDef();

        public PolygonDef NeckPoly = new  PolygonDef();

        public BodyDef PelvisDef = new  BodyDef();

        public PolygonDef PelvisPoly = new PolygonDef();

        public RevoluteJointDef RAnkleDef = new  RevoluteJointDef();

        public BodyDef RCalfDef = new BodyDef();

        public PolygonDef RCalfPoly = new   PolygonDef();

        public RevoluteJointDef RElbowDef = new RevoluteJointDef();

        public BodyDef RFootDef = new BodyDef();

        public PolygonDef RFootPoly = new PolygonDef();

        public BodyDef RForearmDef = new BodyDef();

        public PolygonDef RForearmPoly = new PolygonDef();

        public BodyDef RHandDef = new BodyDef();

        public PolygonDef RHandPoly = new PolygonDef();

        public RevoluteJointDef RHipDef = new RevoluteJointDef();

        public RevoluteJointDef RKneeDef = new RevoluteJointDef();

        public RevoluteJointDef RShoulderDef = new RevoluteJointDef();

        public BodyDef RThighDef = new BodyDef();

        public PolygonDef RThighPoly = new PolygonDef();

        public BodyDef RUpperArmDef = new BodyDef();

        public PolygonDef RUpperArmPoly = new PolygonDef();

        public RevoluteJointDef RWristDef = new RevoluteJointDef();

        public BodyDef StomachDef = new BodyDef();

        public PolygonDef StomachPoly = new PolygonDef();

        public RevoluteJointDef UpperAbsDef = new RevoluteJointDef();

        public RevoluteJointDef UpperNeckDef = new RevoluteJointDef();


        public BipedDef()
        {
            this.SetMotorTorque(2.0f);
            this.SetMotorSpeed(0.0f);
            this.SetDensity(20.0f);
            this.SetRestitution(0.0f);
            this.SetLinearDamping(0.0f);
            this.SetAngularDamping(0.005f);
            this.SetGroupIndex(--count);
            this.EnableMotor();
            this.EnableLimit();

            this.DefaultVertices();
            this.DefaultPositions();
            this.DefaultJoints();

            this.LFootPoly.Friction = this.RFootPoly.Friction = 0.85f;
        }

        ~BipedDef()
        {
        }

        private void DefaultJoints()
        {
            //b.LAnkleDef.body1		= LFoot;
            //b.LAnkleDef.body2		= LCalf;
            //b.RAnkleDef.body1		= RFoot;
            //b.RAnkleDef.body2		= RCalf;
            {
                // ankles
                var anchor = k_scale * new Vec2(-.045f, -.75f);
                this.LAnkleDef.LocalAnchor1 = this.RAnkleDef.LocalAnchor1 = anchor - this.LFootDef.Position;
                this.LAnkleDef.LocalAnchor1 = this.RAnkleDef.LocalAnchor1 = anchor - this.LCalfDef.Position;
                this.LAnkleDef.ReferenceAngle = this.RAnkleDef.ReferenceAngle = 0.0f;
                this.LAnkleDef.LowerAngle = this.RAnkleDef.LowerAngle = -0.523598776f;
                this.LAnkleDef.UpperAngle = this.RAnkleDef.UpperAngle = 0.523598776f;
            }

            //b.LKneeDef.body1		= LCalf;
            //b.LKneeDef.body2		= LThigh;
            //b.RKneeDef.body1		= RCalf;
            //b.RKneeDef.body2		= RThigh;
            {
                // knees
                var anchor = k_scale * new Vec2(-.030f, -.355f);
                this.LKneeDef.LocalAnchor1 = this.RKneeDef.LocalAnchor1 = anchor - this.LCalfDef.Position;
                this.LKneeDef.LocalAnchor1 = this.RKneeDef.LocalAnchor1 = anchor - this.LThighDef.Position;
                this.LKneeDef.ReferenceAngle = this.RKneeDef.ReferenceAngle = 0.0f;
                this.LKneeDef.LowerAngle = this.RKneeDef.LowerAngle = 0;
                this.LKneeDef.UpperAngle = this.RKneeDef.UpperAngle = 2.61799388f;
            }

            //b.LHipDef.body1			= LThigh;
            //b.LHipDef.body2			= Pelvis;
            //b.RHipDef.body1			= RThigh;
            //b.RHipDef.body2			= Pelvis;
            {
                // hips
                var anchor = k_scale * new Vec2(.005f, -.045f);
                this.LHipDef.LocalAnchor1 = this.RHipDef.LocalAnchor1 = anchor - this.LThighDef.Position;
                this.LHipDef.LocalAnchor1 = this.RHipDef.LocalAnchor1 = anchor - this.PelvisDef.Position;
                this.LHipDef.ReferenceAngle = this.RHipDef.ReferenceAngle = 0.0f;
                this.LHipDef.LowerAngle = this.RHipDef.LowerAngle = -2.26892803f;
                this.LHipDef.UpperAngle = this.RHipDef.UpperAngle = 0;
            }

            //b.LowerAbsDef.body1		= Pelvis;
            //b.LowerAbsDef.body2		= Stomach;
            {
                // lower abs
                var anchor = k_scale * new Vec2(.035f, .135f);
                this.LowerAbsDef.LocalAnchor1 = anchor - this.PelvisDef.Position;
                this.LowerAbsDef.LocalAnchor1 = anchor - this.StomachDef.Position;
                this.LowerAbsDef.ReferenceAngle = 0.0f;
                this.LowerAbsDef.LowerAngle = -0.523598776f;
                this.LowerAbsDef.UpperAngle = 0.523598776f;
            }

            //b.UpperAbsDef.body1		= Stomach;
            //b.UpperAbsDef.body2		= Chest;
            {
                // upper abs
                var anchor = k_scale * new Vec2(.045f, .320f);
                this.UpperAbsDef.LocalAnchor1 = anchor - this.StomachDef.Position;
                this.UpperAbsDef.LocalAnchor1 = anchor - this.ChestDef.Position;
                this.UpperAbsDef.ReferenceAngle = 0.0f;
                this.UpperAbsDef.LowerAngle = -0.523598776f;
                this.UpperAbsDef.UpperAngle = 0.174532925f;
            }

            //b.LowerNeckDef.body1	= Chest;
            //b.LowerNeckDef.body2	= Neck;
            {
                // lower neck
                var anchor = k_scale * new Vec2(-.015f, .575f);
                this.LowerNeckDef.LocalAnchor1 = anchor - this.ChestDef.Position;
                this.LowerNeckDef.LocalAnchor1 = anchor - this.NeckDef.Position;
                this.LowerNeckDef.ReferenceAngle = 0.0f;
                this.LowerNeckDef.LowerAngle = -0.174532925f;
                this.LowerNeckDef.UpperAngle = 0.174532925f;
            }

            //b.UpperNeckDef.body1	= Chest;
            //b.UpperNeckDef.body2	= Head;
            {
                // upper neck
                var anchor = k_scale * new Vec2(-.005f, .630f);
                this.UpperNeckDef.LocalAnchor1 = anchor - this.ChestDef.Position;
                this.UpperNeckDef.LocalAnchor1 = anchor - this.HeadDef.Position;
                this.UpperNeckDef.ReferenceAngle = 0.0f;
                this.UpperNeckDef.LowerAngle = -0.610865238f;
                this.UpperNeckDef.UpperAngle = 0.785398163f;
            }

            //b.LShoulderDef.body1	= Chest;
            //b.LShoulderDef.body2	= LUpperArm;
            //b.RShoulderDef.body1	= Chest;
            //b.RShoulderDef.body2	= RUpperArm;
            {
                // shoulders
                var anchor = k_scale * new Vec2(-.015f, .545f);
                this.LShoulderDef.LocalAnchor1 = this.RShoulderDef.LocalAnchor1 = anchor - this.ChestDef.Position;
                this.LShoulderDef.LocalAnchor1 = this.RShoulderDef.LocalAnchor1 = anchor - this.LUpperArmDef.Position;
                this.LShoulderDef.ReferenceAngle = this.RShoulderDef.ReferenceAngle = 0.0f;
                this.LShoulderDef.LowerAngle = this.RShoulderDef.LowerAngle = -1.04719755f;
                this.LShoulderDef.UpperAngle = this.RShoulderDef.UpperAngle = 3.14159265f;
            }

            //b.LElbowDef.body1		= LForearm;
            //b.LElbowDef.body2		= LUpperArm;
            //b.RElbowDef.body1		= RForearm;
            //b.RElbowDef.body2		= RUpperArm;
            {
                // elbows
                var anchor = k_scale * new Vec2(-.005f, .290f);
                this.LElbowDef.LocalAnchor1 = this.RElbowDef.LocalAnchor1 = anchor - this.LForearmDef.Position;
                this.LElbowDef.LocalAnchor1 = this.RElbowDef.LocalAnchor1 = anchor - this.LUpperArmDef.Position;
                this.LElbowDef.ReferenceAngle = this.RElbowDef.ReferenceAngle = 0.0f;
                this.LElbowDef.LowerAngle = this.RElbowDef.LowerAngle = -2.7925268f;
                this.LElbowDef.UpperAngle = this.RElbowDef.UpperAngle = 0;
            }

            //b.LWristDef.body1		= LHand;
            //b.LWristDef.body2		= LForearm;
            //b.RWristDef.body1		= RHand;
            //b.RWristDef.body2		= RForearm;
            {
                // wrists
                var anchor = k_scale * new Vec2(-.010f, .045f);
                this.LWristDef.LocalAnchor1 = this.RWristDef.LocalAnchor1 = anchor - this.LHandDef.Position;
                this.LWristDef.LocalAnchor1 = this.RWristDef.LocalAnchor1 = anchor - this.LForearmDef.Position;
                this.LWristDef.ReferenceAngle = this.RWristDef.ReferenceAngle = 0.0f;
                this.LWristDef.LowerAngle = this.RWristDef.LowerAngle = -0.174532925f;
                this.LWristDef.UpperAngle = this.RWristDef.UpperAngle = 0.174532925f;
            }
        }

        private void DefaultPositions()
        {
            this.LFootDef.Position = this.RFootDef.Position = k_scale * new Vec2(-.122f, -.901f);
            this.LCalfDef.Position = this.RCalfDef.Position = k_scale * new Vec2(-.177f, -.771f);
            this.LThighDef.Position = this.RThighDef.Position = k_scale * new Vec2(-.217f, -.391f);
            this.LUpperArmDef.Position = this.RUpperArmDef.Position = k_scale * new Vec2(-.127f, .228f);
            this.LForearmDef.Position = this.RForearmDef.Position = k_scale * new Vec2(-.117f, -.011f);
            this.LHandDef.Position = this.RHandDef.Position = k_scale * new Vec2(-.112f, -.136f);
            this.PelvisDef.Position = k_scale * new Vec2(-.177f, -.101f);
            this.StomachDef.Position = k_scale * new Vec2(-.142f, .088f);
            this.ChestDef.Position = k_scale * new Vec2(-.132f, .282f);
            this.NeckDef.Position = k_scale * new Vec2(-.102f, .518f);
            this.HeadDef.Position = k_scale * new Vec2(.022f, .738f);
        }

        private void DefaultVertices()
        {
            {
                // feet
                this.LFootPoly.VertexCount = this.RFootPoly.VertexCount = 5;
                this.LFootPoly.Vertices[0] = this.RFootPoly.Vertices[0] = k_scale * new Vec2(.033f, .143f);
                this.LFootPoly.Vertices[1] = this.RFootPoly.Vertices[1] = k_scale * new Vec2(.023f, .033f);
                this.LFootPoly.Vertices[2] = this.RFootPoly.Vertices[2] = k_scale * new Vec2(.267f, .035f);
                this.LFootPoly.Vertices[3] = this.RFootPoly.Vertices[3] = k_scale * new Vec2(.265f, .065f);
                this.LFootPoly.Vertices[4] = this.RFootPoly.Vertices[4] = k_scale * new Vec2(.117f, .143f);
            }
            {
                // calves
                this.LCalfPoly.VertexCount = this.RCalfPoly.VertexCount = 4;
                this.LCalfPoly.Vertices[0] = this.RCalfPoly.Vertices[0] = k_scale * new Vec2(.089f, .016f);
                this.LCalfPoly.Vertices[1] = this.RCalfPoly.Vertices[1] = k_scale * new Vec2(.178f, .016f);
                this.LCalfPoly.Vertices[2] = this.RCalfPoly.Vertices[2] = k_scale * new Vec2(.205f, .417f);
                this.LCalfPoly.Vertices[3] = this.RCalfPoly.Vertices[3] = k_scale * new Vec2(.095f, .417f);
            }
            {
                // thighs
                this.LThighPoly.VertexCount = this.RThighPoly.VertexCount = 4;
                this.LThighPoly.Vertices[0] = this.RThighPoly.Vertices[0] = k_scale * new Vec2(.137f, .032f);
                this.LThighPoly.Vertices[1] = this.RThighPoly.Vertices[1] = k_scale * new Vec2(.243f, .032f);
                this.LThighPoly.Vertices[2] = this.RThighPoly.Vertices[2] = k_scale * new Vec2(.318f, .343f);
                this.LThighPoly.Vertices[3] = this.RThighPoly.Vertices[3] = k_scale * new Vec2(.142f, .343f);
            }
            {
                // pelvis
                this.PelvisPoly.VertexCount = 5;
                this.PelvisPoly.Vertices[0] = k_scale * new Vec2(.105f, .051f);
                this.PelvisPoly.Vertices[1] = k_scale * new Vec2(.277f, .053f);
                this.PelvisPoly.Vertices[2] = k_scale * new Vec2(.320f, .233f);
                this.PelvisPoly.Vertices[3] = k_scale * new Vec2(.112f, .233f);
                this.PelvisPoly.Vertices[4] = k_scale * new Vec2(.067f, .152f);
            }
            {
                // stomach
                this.StomachPoly.VertexCount = 4;
                this.StomachPoly.Vertices[0] = k_scale * new Vec2(.088f, .043f);
                this.StomachPoly.Vertices[1] = k_scale * new Vec2(.284f, .043f);
                this.StomachPoly.Vertices[2] = k_scale * new Vec2(.295f, .231f);
                this.StomachPoly.Vertices[3] = k_scale * new Vec2(.100f, .231f);
            }
            {
                // chest
                this.ChestPoly.VertexCount = 4;
                this.ChestPoly.Vertices[0] = k_scale * new Vec2(.091f, .042f);
                this.ChestPoly.Vertices[1] = k_scale * new Vec2(.283f, .042f);
                this.ChestPoly.Vertices[2] = k_scale * new Vec2(.177f, .289f);
                this.ChestPoly.Vertices[3] = k_scale * new Vec2(.065f, .289f);
            }
            {
                // head
                this.HeadCirc.Radius = k_scale * .115f;
            }
            {
                // neck
                this.NeckPoly.VertexCount = 4;
                this.NeckPoly.Vertices[0] = k_scale * new Vec2(.038f, .054f);
                this.NeckPoly.Vertices[1] = k_scale * new Vec2(.149f, .054f);
                this.NeckPoly.Vertices[2] = k_scale * new Vec2(.154f, .102f);
                this.NeckPoly.Vertices[3] = k_scale * new Vec2(.054f, .113f);
            }
            {
                // upper arms
                this.LUpperArmPoly.VertexCount = this.RUpperArmPoly.VertexCount = 5;
                this.LUpperArmPoly.Vertices[0] = this.RUpperArmPoly.Vertices[0] = k_scale * new Vec2(.092f, .059f);
                this.LUpperArmPoly.Vertices[1] = this.RUpperArmPoly.Vertices[1] = k_scale * new Vec2(.159f, .059f);
                this.LUpperArmPoly.Vertices[2] = this.RUpperArmPoly.Vertices[2] = k_scale * new Vec2(.169f, .335f);
                this.LUpperArmPoly.Vertices[3] = this.RUpperArmPoly.Vertices[3] = k_scale * new Vec2(.078f, .335f);
                this.LUpperArmPoly.Vertices[4] = this.RUpperArmPoly.Vertices[4] = k_scale * new Vec2(.064f, .248f);
            }
            {
                // forearms
                this.LForearmPoly.VertexCount = this.RForearmPoly.VertexCount = 4;
                this.LForearmPoly.Vertices[0] = this.RForearmPoly.Vertices[0] = k_scale * new Vec2(.082f, .054f);
                this.LForearmPoly.Vertices[1] = this.RForearmPoly.Vertices[1] = k_scale * new Vec2(.138f, .054f);
                this.LForearmPoly.Vertices[2] = this.RForearmPoly.Vertices[2] = k_scale * new Vec2(.149f, .296f);
                this.LForearmPoly.Vertices[3] = this.RForearmPoly.Vertices[3] = k_scale * new Vec2(.088f, .296f);
            }
            {
                // hands
                this.LHandPoly.VertexCount = this.RHandPoly.VertexCount = 5;
                this.LHandPoly.Vertices[0] = this.RHandPoly.Vertices[0] = k_scale * new Vec2(.066f, .031f);
                this.LHandPoly.Vertices[1] = this.RHandPoly.Vertices[1] = k_scale * new Vec2(.123f, .020f);
                this.LHandPoly.Vertices[2] = this.RHandPoly.Vertices[2] = k_scale * new Vec2(.160f, .127f);
                this.LHandPoly.Vertices[3] = this.RHandPoly.Vertices[3] = k_scale * new Vec2(.127f, .178f);
                this.LHandPoly.Vertices[4] = this.RHandPoly.Vertices[4] = k_scale * new Vec2(.074f, .178f);
                ;
            }
        }

        private void DisableLimit()
        {
            this.SetLimit(false);
        }

        private void DisableMotor()
        {
            this.SetMotor(false);
        }

        private void EnableLimit()
        {
            this.SetLimit(true);
        }

        private void EnableMotor()
        {
            this.SetMotor(true);
        }

        private void IsFast(bool b)
        {
            /*
	LFootDef.isFast			= b;
	RFootDef.isFast			= b;
	LCalfDef.isFast			= b;
	RCalfDef.isFast			= b;
	LThighDef.isFast		= b;
	RThighDef.isFast		= b;
	PelvisDef.isFast		= b;
	StomachDef.isFast		= b;
	ChestDef.isFast			= b;
	NeckDef.isFast			= b;
	HeadDef.isFast			= b;
	LUpperArmDef.isFast		= b;
	RUpperArmDef.isFast		= b;
	LForearmDef.isFast		= b;
	RForearmDef.isFast		= b;
	LHandDef.isFast			= b;
	RHandDef.isFast			= b;
	*/
        }

        private void SetAngularDamping(float f)
        {
            this.LFootDef.AngularDamping = f;
            this.RFootDef.AngularDamping = f;
            this.LCalfDef.AngularDamping = f;
            this.RCalfDef.AngularDamping = f;
            this.LThighDef.AngularDamping = f;
            this.RThighDef.AngularDamping = f;
            this.PelvisDef.AngularDamping = f;
            this.StomachDef.AngularDamping = f;
            this.ChestDef.AngularDamping = f;
            this.NeckDef.AngularDamping = f;
            this.HeadDef.AngularDamping = f;
            this.LUpperArmDef.AngularDamping = f;
            this.RUpperArmDef.AngularDamping = f;
            this.LForearmDef.AngularDamping = f;
            this.RForearmDef.AngularDamping = f;
            this.LHandDef.AngularDamping = f;
            this.RHandDef.AngularDamping = f;
        }

        private void SetDensity(float f)
        {
            this.LFootPoly.Density = f;
            this.RFootPoly.Density = f;
            this.LCalfPoly.Density = f;
            this.RCalfPoly.Density = f;
            this.LThighPoly.Density = f;
            this.RThighPoly.Density = f;
            this.PelvisPoly.Density = f;
            this.StomachPoly.Density = f;
            this.ChestPoly.Density = f;
            this.NeckPoly.Density = f;
            this.HeadCirc.Density = f;
            this.LUpperArmPoly.Density = f;
            this.RUpperArmPoly.Density = f;
            this.LForearmPoly.Density = f;
            this.RForearmPoly.Density = f;
            this.LHandPoly.Density = f;
            this.RHandPoly.Density = f;
        }

        private void SetGroupIndex(short i)
        {
            this.LFootPoly.Filter.GroupIndex = i;
            this.RFootPoly.Filter.GroupIndex = i;
            this.LCalfPoly.Filter.GroupIndex = i;
            this.RCalfPoly.Filter.GroupIndex = i;
            this.LThighPoly.Filter.GroupIndex = i;
            this.RThighPoly.Filter.GroupIndex = i;
            this.PelvisPoly.Filter.GroupIndex = i;
            this.StomachPoly.Filter.GroupIndex = i;
            this.ChestPoly.Filter.GroupIndex = i;
            this.NeckPoly.Filter.GroupIndex = i;
            this.HeadCirc.Filter.GroupIndex = i;
            this.LUpperArmPoly.Filter.GroupIndex = i;
            this.RUpperArmPoly.Filter.GroupIndex = i;
            this.LForearmPoly.Filter.GroupIndex = i;
            this.RForearmPoly.Filter.GroupIndex = i;
            this.LHandPoly.Filter.GroupIndex = i;
            this.RHandPoly.Filter.GroupIndex = i;
        }

        private void SetLimit(bool b)
        {
            this.LAnkleDef.EnableLimit = b;
            this.RAnkleDef.EnableLimit = b;
            this.LKneeDef.EnableLimit = b;
            this.RKneeDef.EnableLimit = b;
            this.LHipDef.EnableLimit = b;
            this.RHipDef.EnableLimit = b;
            this.LowerAbsDef.EnableLimit = b;
            this.UpperAbsDef.EnableLimit = b;
            this.LowerNeckDef.EnableLimit = b;
            this.UpperNeckDef.EnableLimit = b;
            this.LShoulderDef.EnableLimit = b;
            this.RShoulderDef.EnableLimit = b;
            this.LElbowDef.EnableLimit = b;
            this.RElbowDef.EnableLimit = b;
            this.LWristDef.EnableLimit = b;
            this.RWristDef.EnableLimit = b;
        }

        private void SetLinearDamping(float f)
        {
            this.LFootDef.LinearDamping = f;
            this.RFootDef.LinearDamping = f;
            this.LCalfDef.LinearDamping = f;
            this.RCalfDef.LinearDamping = f;
            this.LThighDef.LinearDamping = f;
            this.RThighDef.LinearDamping = f;
            this.PelvisDef.LinearDamping = f;
            this.StomachDef.LinearDamping = f;
            this.ChestDef.LinearDamping = f;
            this.NeckDef.LinearDamping = f;
            this.HeadDef.LinearDamping = f;
            this.LUpperArmDef.LinearDamping = f;
            this.RUpperArmDef.LinearDamping = f;
            this.LForearmDef.LinearDamping = f;
            this.RForearmDef.LinearDamping = f;
            this.LHandDef.LinearDamping = f;
            this.RHandDef.LinearDamping = f;
        }

        private void SetMotor(bool b)
        {
            this.LAnkleDef.EnableMotor = b;
            this.RAnkleDef.EnableMotor = b;
            this.LKneeDef.EnableMotor = b;
            this.RKneeDef.EnableMotor = b;
            this.LHipDef.EnableMotor = b;
            this.RHipDef.EnableMotor = b;
            this.LowerAbsDef.EnableMotor = b;
            this.UpperAbsDef.EnableMotor = b;
            this.LowerNeckDef.EnableMotor = b;
            this.UpperNeckDef.EnableMotor = b;
            this.LShoulderDef.EnableMotor = b;
            this.RShoulderDef.EnableMotor = b;
            this.LElbowDef.EnableMotor = b;
            this.RElbowDef.EnableMotor = b;
            this.LWristDef.EnableMotor = b;
            this.RWristDef.EnableMotor = b;
        }

        private void SetMotorSpeed(float f)
        {
            this.LAnkleDef.MotorSpeed = f;
            this.RAnkleDef.MotorSpeed = f;
            this.LKneeDef.MotorSpeed = f;
            this.RKneeDef.MotorSpeed = f;
            this.LHipDef.MotorSpeed = f;
            this.RHipDef.MotorSpeed = f;
            this.LowerAbsDef.MotorSpeed = f;
            this.UpperAbsDef.MotorSpeed = f;
            this.LowerNeckDef.MotorSpeed = f;
            this.UpperNeckDef.MotorSpeed = f;
            this.LShoulderDef.MotorSpeed = f;
            this.RShoulderDef.MotorSpeed = f;
            this.LElbowDef.MotorSpeed = f;
            this.RElbowDef.MotorSpeed = f;
            this.LWristDef.MotorSpeed = f;
            this.RWristDef.MotorSpeed = f;
        }

        private void SetMotorTorque(float f)
        {
            this.LAnkleDef.MaxMotorTorque = f;
            this.RAnkleDef.MaxMotorTorque = f;
            this.LKneeDef.MaxMotorTorque = f;
            this.RKneeDef.MaxMotorTorque = f;
            this.LHipDef.MaxMotorTorque = f;
            this.RHipDef.MaxMotorTorque = f;
            this.LowerAbsDef.MaxMotorTorque = f;
            this.UpperAbsDef.MaxMotorTorque = f;
            this.LowerNeckDef.MaxMotorTorque = f;
            this.UpperNeckDef.MaxMotorTorque = f;
            this.LShoulderDef.MaxMotorTorque = f;
            this.RShoulderDef.MaxMotorTorque = f;
            this.LElbowDef.MaxMotorTorque = f;
            this.RElbowDef.MaxMotorTorque = f;
            this.LWristDef.MaxMotorTorque = f;
            this.RWristDef.MaxMotorTorque = f;
        }

        private void SetRestitution(float f)
        {
            this.LFootPoly.Restitution = f;
            this.RFootPoly.Restitution = f;
            this.LCalfPoly.Restitution = f;
            this.RCalfPoly.Restitution = f;
            this.LThighPoly.Restitution = f;
            this.RThighPoly.Restitution = f;
            this.PelvisPoly.Restitution = f;
            this.StomachPoly.Restitution = f;
            this.ChestPoly.Restitution = f;
            this.NeckPoly.Restitution = f;
            this.HeadCirc.Restitution = f;
            this.LUpperArmPoly.Restitution = f;
            this.RUpperArmPoly.Restitution = f;
            this.LForearmPoly.Restitution = f;
            this.RForearmPoly.Restitution = f;
            this.LHandPoly.Restitution = f;
            this.RHandPoly.Restitution = f;
        }       
    }
}