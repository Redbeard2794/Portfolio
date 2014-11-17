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

namespace RobotTank
{
    class PlayerTurret
    {
        Texture2D playerTurret;//texture
        Matrix turretTransform;//transform
        float rotation;//rotation value

        Vector2 pos;//position
        float zPos;//place holder for z=zero when modifying the transform

        SpriteFont testFont;
        //constructor
        public PlayerTurret()
        {
            pos.X = 0;
            pos.Y = 0;
            zPos = 0;
            rotation = 0;
        }

        //start properties
        public Matrix TurretTransform
        {
            get { return turretTransform; }
            set { turretTransform = value; }
        }
        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        // end properties

        //load content
        public void LoadContent(ContentManager c)
        {
            playerTurret = c.Load<Texture2D>("Player/playerTurret2");
            testFont = c.Load<SpriteFont>("Font1");
        }

        //update allows the player to rotate the turret
        public void Update(KeyboardState kInput)
        {
            if (kInput.IsKeyDown(Keys.Z))
            {
                rotation -= 0.01f;
            }
            else if (kInput.IsKeyDown(Keys.X))
            {
                rotation += 0.01f;
            }
            //sets rotation back to zero to make it easier to pick direction
            if (rotation >= 6.27)
                rotation = 0;
            else if (rotation <= -6.27)
                rotation = 0;
        }
        public void Draw(SpriteBatch sBatch, Matrix bodyTransform, int noOfLives)
        {
            Vector2 origin = new Vector2(31,9);//sets origin

            turretTransform = Matrix.CreateRotationZ(rotation) * Matrix.CreateTranslation(pos.X, pos.Y, zPos) * bodyTransform;

            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, turretTransform);
            //changes colour depending on how may lives left
            if (noOfLives == 3)
                sBatch.Draw(playerTurret, Vector2.Zero, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
            else if (noOfLives == 2)
                sBatch.Draw(playerTurret, Vector2.Zero, null, Color.IndianRed, 0, origin, 1, SpriteEffects.None, 0);
            else if (noOfLives == 1)
                sBatch.Draw(playerTurret, Vector2.Zero, null, Color.OrangeRed, 0, origin, 1, SpriteEffects.None, 0);
            else if (noOfLives == 0)
                sBatch.Draw(playerTurret, Vector2.Zero, null, Color.Crimson, 0, origin, 1, SpriteEffects.None, 0);
            sBatch.End();
        }
    }
}
