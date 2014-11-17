
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
    class Rocket
    {
        Vector3 pos;
        Model rocketModel;
        Matrix world;
        Vector3 forwardVector;
        Vector3 prevPos;
        Vector3 direction;
        float speed;
        float rotation;

        int lightTimer;

        public Rocket(Vector3 playerPos, ContentManager c, float turretRotation, float tankRot)
        {
            LoadContent(c);
            //positions the rocket so it comes out of the barrel of the gun
            pos = playerPos;
            pos.Y += 1.7f;
            speed = 0.2f;
            prevPos = new Vector3(0, 0, 0);
            lightTimer = 0;
            //determines which way the rocket will be facing
            rotation = -turretRotation + tankRot;
        }
        public Vector3 Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        public Model RocketModel
        {
            get { return rocketModel; }
            set { rocketModel = value; }
        }
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
        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        
        public void LoadContent(ContentManager c)
        {
            //loads the model
            rocketModel = c.Load<Model>("missile");
        }
        public void Update(KeyboardState ks)
        {
            //moves the rocket in the direction the turret is facing
            //when it was fired
            forwardVector = -direction * speed;
            pos += forwardVector;

            //timer used to determine which colour the rocket is flashing
            lightTimer++;
            if (lightTimer == 40)
                lightTimer = 0;
        }
        public void Draw(SpriteBatch sBatch, GraphicsDeviceManager graphics, Matrix view, Matrix proj, float turretRotation, float tankRot)
        {
            world = Matrix.CreateScale(0.2f) * Matrix.CreateRotationY(-rotation + MathHelper.TwoPi / 4) * Matrix.CreateTranslation(pos);

            direction = new Vector3((float)Math.Cos(rotation), 0, (float)Math.Sin(rotation));

            foreach (ModelMesh mesh in rocketModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effects settings go here
                    effect.LightingEnabled = true;
                    //changes the colour the rocket flashes based on the timer
                    if (lightTimer < 10)
                        effect.EmissiveColor = new Vector3(1f, 0f, 0f);
                    else if (lightTimer > 10 && lightTimer < 20)//15,30
                        effect.EmissiveColor = new Vector3(1f, 0f, 1f);
                    else if (lightTimer > 20 && lightTimer < 30)//30,45
                        effect.EmissiveColor = new Vector3(0f, 0f, 1f);
                    else if (lightTimer > 30 && lightTimer < 40)//45,60
                        effect.EmissiveColor = new Vector3(0f, 1f, 1f);
                    effect.Alpha = 1;


                    effect.World = world;
                    effect.Projection = proj;
                    effect.View = view;
                }
                mesh.Draw();
            }
        }
    }
}
