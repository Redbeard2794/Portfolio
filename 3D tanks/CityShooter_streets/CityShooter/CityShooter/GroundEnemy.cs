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
    class GroundEnemy : GameObject
    {
        Matrix world;
        Vector3 pos;
        Vector3 startPos;
        Model model;
        float rotation;
        int timer = 0;
        Vector3 col = new Vector3(1,0,0);
        float speed = 0.1f;
        float turretRotValue;
        Matrix turretRot;

        bool right = false;
        bool left = false;
        bool up = false;
        bool down = false;

        Texture2D detectionSquare;

        Vector3 turretDirection;

        int health;

        public GroundEnemy(Game game)
             : base(game)
         {
             rotation = 0;
             turretRotValue = 0;
             health = 100;
         }
         public override void Init()
         {
            effect = new BasicEffect(graphicsDevice);
            world = Matrix.Identity;
            world = Matrix.CreateTranslation(pos);
             //determines which way the enemy faces at the start based on their starting
             //position
            //top left
            if (startPos.X == 7.5 && startPos.Y == 0 && startPos.Z == 7.5)
            {
                down = true;
                up = false;
                left = false;
                right = false;
            }
            //top right
            else if (startPos.X == 142.5 && startPos.Y == 0 && startPos.Z == 7.5)
            {
                left = true;
                right = false;
                up = false;
                down = false;
            }
            //bottom left
            else if (startPos.X == 7.5 && startPos.Y == 0 && startPos.Z == 142.5)
            {
                right = true;
                left = false;
                up = false;
                down = false;
            }
            //bottom right
            else if (startPos.X == 142.5 && startPos.Y == 0 && startPos.Z == 142.5)
            {
                up = true;
                down = false;
                left = false;
                right = false;
            }
         }
         public override void Update(GameTime gameTime)
         {
             //moves based on the rotation and makes sure they face the way
             //thay are travelling
             if (down == true)
             {
                 rotation = 0;
                 pos.Z += speed;
             }
             else if (up == true)
             {
                 rotation = 3.14f;
                 pos.Z -= speed;
             }
             else if (left == true)
             {
                 rotation = -1.5f;
                 pos.X -= speed;
             }
             else if (right == true)
             {
                 rotation = 1.5f;
                 pos.X += speed;
             }

             //direction changed
             //top left
             if (pos.X <= 7.5 && pos.Z <= 7.5)
             {
                 left = false;
                 right = false;
                 up = false;
                 down = true;
             }
             //bottom left
             else if (pos.X <= 7.5 && pos.Z >= 142.5)
             {
                 right = true;
                 down = false;
                 left = false;
                 up = false;
             }
                //bottom right
             else if (pos.X >= 142.5 && pos.Z >= 142.5)
             {
                 right = false;
                 left = false;
                 down = false;
                 up = true;
             }
             //top right
             else if (pos.X >= 142.5 && pos.Z <= 7.5)
             {
                 up = false;
                 down = false;
                 right = false;
                 left = true;
             }
         }
         public void Shoot(Vector3 playerPos)
         {
             //stops if the player is within range
             if (playerPos.X < pos.X + 15 && playerPos.Z < pos.Z + 15 && playerPos.X > pos.X - 15 && playerPos.Z > pos.Z - 15)
             {
                 speed = 0;

             }
             else
             {
                 speed = 0.1f;
             }
         }
         public override void Draw(GameTime gametime, Camera camera)
         {
             world = Matrix.CreateScale(0.005f) * Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(pos);
             Matrix[] transforms = new Matrix[model.Bones.Count];

             model.CopyAbsoluteBoneTransformsTo(transforms);

             timer++;
             if (health != 20)
             {
                 if (timer > 0 && timer < 20)
                     col = new Vector3(0, 0, 1);
                 else if (timer > 20 && timer < 40)
                     col = new Vector3(1, 1, 1);
                 else if (timer > 40)
                     timer = 0;
             }
             else if (health == 20)
             {
                 if (timer > 0 && timer < 20)
                     col = new Vector3(1, 0, 0);
                 else if (timer > 20 && timer < 40)
                     col = new Vector3(0, 0, 0);
                 else if (timer > 40)
                     timer = 0;
             }

             turretDirection = new Vector3((float)Math.Cos(turretRotValue), 0, (float)Math.Sin(turretRotValue));


             turretRot = Matrix.CreateRotationY(turretRotValue);

             Matrix wheelRot = Matrix.CreateRotationX(0.1f);


             foreach (ModelMesh mesh in model.Meshes)
             {
                 foreach (BasicEffect effect in mesh.Effects)
                 {
                     //effects settings go here
                     effect.EmissiveColor = col;
                     effect.LightingEnabled = true;

                     if (mesh.Name == "turret_geo" || mesh.Name == "hatch_geo" || mesh.Name == "canon_geo")
                     {
                         effect.World = transforms[mesh.ParentBone.Index] * turretRot * world;
                     }
                     else effect.World = transforms[mesh.ParentBone.Index] * world;

                     effect.View = camera.View;
                     effect.Projection = camera.Projection;
                 }
                 mesh.Draw();
             }
             //sBatch.Begin();
             
             //sBatch.End();



         }
         public Model Model
         {
             get { return model; }
             set { model = value; }
         }
         public Vector3 Pos
         {
             get { return pos; }
             set { pos = value; }
         }
         public Vector3 StartPos
         {
             get { return startPos; }
             set { startPos = value; }
         }
         public Texture2D DetectionSquare
         {
             get { return detectionSquare; }
             set { detectionSquare = value; }
         }
         public Matrix World
         {
             get { return world; }
             set { world = value; }
         }
         public float Speed
         {
             get { return speed; }
             set { speed = value; }
         }
         public float TurretRotValue
         {
             get { return turretRotValue; }
             set { turretRotValue = value; }
         }
         public Matrix TurretRot
         {
             get { return turretRot; }
             set { turretRot = value; }
         }
         public Vector3 TurretDirection
         {
             get { return turretDirection; }
             set { turretDirection = value; }
         }
         public float Rotation
         {
             get { return rotation; }
             set { rotation = value; }
         }
         public int Health
         {
             get { return health; }
             set { health = value; }
         }
    }
}
