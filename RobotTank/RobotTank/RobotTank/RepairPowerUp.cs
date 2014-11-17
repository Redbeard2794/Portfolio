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
    class RepairPowerUp
    {
        Matrix powerUpTransform;
        float rotation;//the value of the power ups rotation
        Vector2 repairPos;//the position
        Texture2D repairTexture;//texture
        Color[] repairTextureData;//array of colours around the edge of the texture
        Rectangle repairRectangle;//rectangle around the powerup for collision detection
        bool active;//whether the power up is active

        public RepairPowerUp(Random rand)
        {
            repairPos.X = rand.Next(1, 50);
            repairPos.Y = rand.Next(1, 50);
            rotation = 0;
            active = false;
        }

        //start properties
        public Rectangle RepairRectangle
        {
            get { return repairRectangle; }
            set { repairRectangle = value; }
        }
        public Color[] RepairTextureData
        {
            get { return repairTextureData; }
            set { repairTextureData = value; }
        }
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }
        public Vector2 RepairPos
        {
            get { return repairPos; }
            set { repairPos = value; }
        }
        //end properties

        //load content
        public void LoadContent(ContentManager c)
        {
            repairTexture = c.Load<Texture2D>("PowerUp/RepairSymbol2");
            repairTextureData = new Color[repairTexture.Width * repairTexture.Height];
            repairTexture.GetData(repairTextureData);
        }
        //update
        public void Update()
        {
            repairRectangle = new Rectangle((int)repairPos.X, (int)repairPos.Y, repairTexture.Width, repairTexture.Height);
            rotation += 0.01f;
        }

        //for resetting the position after it has been taken by the player
        public void Reset(Random rand)
        {
            repairPos.X = rand.Next(-100, 300);
            repairPos.Y = rand.Next(-100, 300);
        }

        //draw
        public void Draw(SpriteBatch sBatch, Matrix camTransform)
        {
            Vector2 origin = new Vector2(repairTexture.Width / 2, repairTexture.Height / 2);//sets the origin
            powerUpTransform = Matrix.CreateRotationZ(rotation) * Matrix.CreateTranslation(repairPos.X, repairPos.Y, 0) * camTransform;

            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, powerUpTransform);
            if (active == true)
                sBatch.Draw(repairTexture, Vector2.Zero, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
            sBatch.End();
        }

    }
}
