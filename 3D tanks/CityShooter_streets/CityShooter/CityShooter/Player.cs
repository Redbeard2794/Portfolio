using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace CityShooter
{
    class Player
    {
        int lightTimer;
        Model tank;
        Matrix world;
        Vector3 pos;

        float rotation;

        Vector3 direction;
        float speed;


        int score;
        int lives;
        float health;
        int enemiesKilled;

        float turretRotation;


        Matrix turretRot;
        float turretSpeed;
        bool turretActive;

        //wheel rotation
        Matrix wheelRot;
        float wheelSpeed;

        bool wheelsForward;
        bool wheelsBack;

        //flip
        float flipRotation;

        bool hitBuilding;


        Vector3 forwardVector;

        bool forwardCollision;
        bool backwardsCollision;
        Vector3 turretDirection;

        float totalTurretRot;

        Vector3 prevPos;

        Matrix turretTransform;

        Matrix tankRotation;

        Matrix fullTurretRotation;

 

        public Player()
        {
            lightTimer = 0;

            pos.X = 67;
            pos.Y = 0;
            pos.Z = 82;

            rotation = -1.5f;
            speed = 0.1f;
            turretRotation = 0;
            
            turretSpeed = 0.01f;
            wheelSpeed = 0.05f;

            wheelsForward = false;
            wheelsBack = false;


            flipRotation = 0;

            turretActive = false;

            hitBuilding = false;

            forwardCollision = false;
            backwardsCollision = false;

            totalTurretRot = 0;
            prevPos = new Vector3(0, 0, 0);
            lives = 3;
            score = 0;
            health = 100;
            enemiesKilled = 0;
        }
 
        public void LoadContent(ContentManager c)
        {
            tank = c.Load<Model>("tank");
        }

        public void Update(KeyboardState ks)
        {
            forwardVector = direction * speed;
 
            lightTimer++;
            if (lightTimer == 40)//60
                lightTimer = 0;


            if (ks.IsKeyDown(Keys.A))
            {
                rotation -= 0.01f;
            }
            if (ks.IsKeyDown(Keys.D))
            {
                rotation += 0.01f;
            }
            
            if (ks.IsKeyDown(Keys.W))
            {
                //tells the wheels which way they need to turn
                wheelsForward = true;
                wheelsBack = false;
                wheelSpeed += 0.1f;

                pos -= forwardVector;//direction * speed is the forward vector
            }
            if (ks.IsKeyDown(Keys.S))
            {
                //tells the wheels which way they need to turn
                wheelsForward = false;
                wheelsBack = true;
                wheelSpeed -= 0.1f;

                pos += forwardVector;
            }

            //stop wheel moving
            if (ks.IsKeyUp(Keys.W) && ks.IsKeyUp(Keys.S))
            {
                wheelsBack = false; 
                wheelsForward = false;
            }

            RotateTurret(ks);
            OnCollision();

        }
        public void OnCollision()
        {
            if (hitBuilding == false)
                prevPos = new Vector3(pos.X + 0.1f, pos.Y, pos.Z + 0.1f);

            if (hitBuilding == true)
            {
                pos = prevPos;
                if (hitBuilding == true)
                {
                    prevPos = new Vector3(pos.X - 0.1f, pos.Y, pos.Z - 0.1f);
                    hitBuilding = false;
                }
                hitBuilding = false;
            }
        }
        public void RotateTurret(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.Left))
            {
                turretActive = true;
                if (turretActive == true)
                {
                    turretRotation += turretSpeed;
                    totalTurretRot += turretRotation;
                }
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                turretActive = true;
                if (turretActive == true)
                {
                    turretRotation -= turretSpeed;
                    totalTurretRot += turretRotation;
                }
            }
            if (ks.IsKeyUp(Keys.Right) && ks.IsKeyUp(Keys.Left))
            {
                
                turretActive = false;
            }
            if (turretActive == false)
                turretRotation = 0;
        }

        public void Draw(SpriteBatch sBatch, GraphicsDeviceManager graphics, Matrix view, Matrix proj)
        {
            tankRotation = Matrix.CreateRotationY(-rotation - MathHelper.TwoPi / 4);

            world = Matrix.CreateScale(0.005f) * tankRotation * Matrix.CreateRotationZ(flipRotation) * Matrix.CreateTranslation(pos);// *Matrix.CreateRotationY(rotationY);

            direction.X = (float)Math.Cos(rotation);
            direction.Z = (float)Math.Sin(rotation);

            turretDirection.X = (float)Math.Cos(totalTurretRot);
            turretDirection.Z = (float)Math.Sin(totalTurretRot);

            Matrix[] transforms = new Matrix[tank.Bones.Count];

            tank.CopyAbsoluteBoneTransformsTo(transforms);

            turretRot = Matrix.CreateRotationY(turretRotation);

            fullTurretRotation = Matrix.CreateRotationY(totalTurretRot);


            turretTransform = tank.Bones[9].Transform;

            if (wheelsForward == true)
                wheelRot = Matrix.CreateRotationX(wheelSpeed);
            else if (wheelsBack == true)
                wheelRot = Matrix.CreateRotationX(-wheelSpeed);
            if (wheelsForward == false && wheelsBack == false)
                wheelRot = Matrix.CreateRotationX(0);

            foreach (ModelMesh mesh in tank.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effects settings go here
                    effect.LightingEnabled = true;
                    if (health != 20)
                    {
                        if (lightTimer < 10)//15
                            effect.EmissiveColor = new Vector3(1f, 0f, 0f);
                        else if (lightTimer > 10 && lightTimer < 20)//15,30
                            effect.EmissiveColor = new Vector3(1f, 0f, 1f);
                        else if (lightTimer > 20 && lightTimer < 30)//30,45
                            effect.EmissiveColor = new Vector3(0f, 0f, 1f);
                        else if (lightTimer > 30 && lightTimer < 40)//45,60
                            effect.EmissiveColor = new Vector3(0f, 1f, 1f);
                    }
                    else if (health == 20)
                    {
                        if (lightTimer < 10)//15
                            effect.EmissiveColor = new Vector3(1f, 0f, 0f);
                        else if (lightTimer > 10 && lightTimer < 20)//15,30
                            effect.EmissiveColor = new Vector3(0f, 0f, 0f);
                        else if (lightTimer > 20 && lightTimer < 30)//30,45
                            effect.EmissiveColor = new Vector3(1f, 0f, 0f);
                        else if (lightTimer > 30 && lightTimer < 40)//45,60
                            effect.EmissiveColor = new Vector3(0f, 0f, 0f);
                    }
                    effect.Alpha = 1;

                    if (mesh.Name == "turret_geo" || mesh.Name == "hatch_geo" || mesh.Name == "canon_geo")
                    {
                        effect.World = transforms[mesh.ParentBone.Index] * fullTurretRotation * world;
                    }
                    //else if (mesh.Name == "r_back_wheel_geo" || mesh.Name == "r_front_wheel_geo" || mesh.Name == "l_back_wheel_geo" || mesh.Name == "l_front_wheel_geo")// || mesh.Name == "r_steer_geo" || mesh.Name == "l_steer_geo")
                    //{
                    //    effect.World = transforms[mesh.ParentBone.Index] * wheelRot * world;
                    //}
                    else effect.World = transforms[mesh.ParentBone.Index] * world;
                        

                    effect.Projection = proj;
                    effect.View = view;
                }
                mesh.Draw();
            }
        }
        //properties
        public Matrix World
        {
            get { return world; }
            set { world = value; }
        }
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public Vector3 Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        public Vector3 TurretDirection
        {
            get { return turretDirection; }
            set { turretDirection = value; }
        }
        public Matrix TurretRot
        {
            get { return turretRot; }
            set { turretRot = value; }
        }
        public Model Tank
        {
            get { return tank; }
            set { tank = value; }
        }
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public bool HitBuilding
        {
            get { return hitBuilding; }
            set { hitBuilding = value; }
        }
        public Vector3 ForwardVector
        {
            get { return forwardVector; }
            set { forwardVector = value; }
        }
        public float TotalTurretRotation
        {
            get { return totalTurretRot; }
            set { totalTurretRot = value; }
        }
        public Matrix TurretTransform
        {
            get { return turretTransform; }
            set { turretTransform = value; }
        }
        public Matrix TankRotation
        {
            get { return tankRotation; }
            set { tankRotation = value; }
        }
        public Matrix FullTurretRotation
        {
            get { return fullTurretRotation; }
            set { fullTurretRotation = value; }
        }
        //int score;
        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        //int lives;
        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }
        //float health;
        public float Health
        {
            get { return health; }
            set { health = value; }
        }
        public int EnemiesKilled
        {
            get { return enemiesKilled; }
            set { enemiesKilled = value; }
        }
    }
}
