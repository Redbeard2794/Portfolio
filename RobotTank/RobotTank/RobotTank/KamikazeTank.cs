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
    class KamikazeTank
    {
        Matrix KamikazeTransform;//transform for tank
        Vector2 pos;//position
        float zPos;//used as a placeholder for z = zero when modifying the transform
        Texture2D kamikazeTank;//texture
        Color[] kamikazeTextureData;//array of colurs around the edge of the texture
        Rectangle kamikazeRectangle;//rectangle around the tank for collision
        bool alive;//whether the tank is alive or not
        float speed;//the speed at which it will move

        float tankOrientation;//the direction it will travel in as it follows the player
        float turnSpeed = 0.1f;//how fast it can turn

        int health;//the health

        //textures for the explosion
        Texture2D explosion1;
        Texture2D explosion2;
        Texture2D explosion3;
        Texture2D explosion4;
        Texture2D explosion5;
        Texture2D explosion6;
        Texture2D explosion7;
        Texture2D explosion8;
        Texture2D explosion9;
        Texture2D explosion10;
        Texture2D explosion11;
        Texture2D explosion12;
        Texture2D explosion13;
        Texture2D explosion14;
        Texture2D explosion15;
        Texture2D explosion16;
        Texture2D explosion17;
        Texture2D explosion18;
        Texture2D explosion19;
        Texture2D explosion20;
        Texture2D explosion21;
        Texture2D explosion22;
        Texture2D explosion23;
        Texture2D explosion24;

        bool playExplosion = false;//is the explosion sequence playing
        int count = 0;//counter for the explosion sequence
        Matrix explosionTransform;//tranform for the explosion
        Vector2 explosionPos;//position for the explosion to be drawn at

        public KamikazeTank()
        {
            pos.X = -100;
            pos.Y = 0;
            zPos = 0;
            speed = 1;
            alive = true;//starts as true but will be set to false when it hits the player or is destroyed by the player. a timer in the game class will set it to true again
            health = 100;
        }
        //start properties
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }
        public Rectangle KamikazeRectangle
        {
            get { return kamikazeRectangle; }
            set { kamikazeRectangle = value; }
        }
        public Color[] KamikazeTextureData
        {
            get { return kamikazeTextureData; }
            set { kamikazeTextureData = value; }
        }
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public bool PlayExplosion
        {
            get { return playExplosion; }
            set { playExplosion = value; }
        }
        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        //end properties

        //load content
        public void LoadContent(ContentManager c)
        {
            kamikazeTank = c.Load<Texture2D>("KamikazeEnemy/kamikazeTank");
            kamikazeTextureData = new Color[kamikazeTank.Width * kamikazeTank.Height];
            kamikazeTank.GetData(kamikazeTextureData);

            //for the explosion sequence
            explosion1 = c.Load<Texture2D>("ExplosionSequence/explosion1");
            explosion2 = c.Load<Texture2D>("ExplosionSequence/explosion2");
            explosion3 = c.Load<Texture2D>("ExplosionSequence/explosion3");
            explosion4 = c.Load<Texture2D>("ExplosionSequence/explosion4");
            explosion5 = c.Load<Texture2D>("ExplosionSequence/explosion5");
            explosion6 = c.Load<Texture2D>("ExplosionSequence/explosion6");
            explosion7 = c.Load<Texture2D>("ExplosionSequence/explosion7");
            explosion8 = c.Load<Texture2D>("ExplosionSequence/explosion8");
            explosion9 = c.Load<Texture2D>("ExplosionSequence/explosion9");
            explosion10 = c.Load<Texture2D>("ExplosionSequence/explosion10");
            explosion11 = c.Load<Texture2D>("ExplosionSequence/explosion11");
            explosion12 = c.Load<Texture2D>("ExplosionSequence/explosion12");
            explosion13 = c.Load<Texture2D>("ExplosionSequence/explosion13");
            explosion14 = c.Load<Texture2D>("ExplosionSequence/explosion14");
            explosion15 = c.Load<Texture2D>("ExplosionSequence/explosion15");
            explosion16 = c.Load<Texture2D>("ExplosionSequence/explosion16");
            explosion17 = c.Load<Texture2D>("ExplosionSequence/explosion17");
            explosion18 = c.Load<Texture2D>("ExplosionSequence/explosion18");
            explosion19 = c.Load<Texture2D>("ExplosionSequence/explosion19");
            explosion20 = c.Load<Texture2D>("ExplosionSequence/explosion20");
            explosion21 = c.Load<Texture2D>("ExplosionSequence/explosion21");
            explosion22 = c.Load<Texture2D>("ExplosionSequence/explosion22");
            explosion23 = c.Load<Texture2D>("ExplosionSequence/explosion23");
            explosion24 = c.Load<Texture2D>("ExplosionSequence/explosion24");
        }

        //update method
        public void Update(float playersRotation, Vector2 playerPos)
        {
            if (alive == true)
            {
                //gives the tank the orientation it needs so that it can face towards the player
                tankOrientation = TurnToFace(pos, playerPos, tankOrientation, turnSpeed);
                //the direction the tank will move in
                Vector2 heading = new Vector2((float)Math.Cos(tankOrientation), (float)Math.Sin(tankOrientation));
                //movement
                pos += heading * speed;
            }
            //for playing out the explosion sequence
            if (playExplosion == true)
            {
                count++;
                explosionPos.X = pos.X;
                explosionPos.Y = pos.Y;
            }
            if (count > 24)
            {
                playExplosion = false;
                count = 0;
            }

            //for getting the rectangle around the tank(used for per pixel collision)
            kamikazeRectangle = new Rectangle((int)pos.X, (int)pos.Y, kamikazeTank.Width, kamikazeTank.Height);
        }

        //method for getting the kamikaze to face towards the player
        float TurnToFace(Vector2 tankPos, Vector2 playerPos, float tankOrientation, float turnSpeed)
        {
            float x = playerPos.X - tankPos.X;
            float y = playerPos.Y - tankPos.Y;

            //gets the ngle between the tank and the player
            float desiredAngle = (float)Math.Atan2(y, x);
            //works out the difference between them
            float difference = WrapAngle(desiredAngle - tankOrientation);
            //clamps the difference between the turn speed and its negative
            difference = MathHelper.Clamp(difference, -turnSpeed, turnSpeed);
            return WrapAngle(tankOrientation + difference);
        }


        //takes in the angle to wrap, in radians
        //the input angle is expressed inn radians from -Pi to Pi
        //returns the angle expressed in radians between -Pi and Pi
        float WrapAngle(float radians)
        {
            //while less than -180 degrees
            while (radians < -MathHelper.Pi)
                radians += MathHelper.TwoPi;
            //while greater than 180 degrees
            while (radians > MathHelper.Pi)
                radians -= MathHelper.TwoPi;

            return radians;
        }

        //for respawing the kamikaze
        public void Respawn(Random rand)
        {
            //maybe increase this range as it sometimes spawns on top of the player. (instant explosion!)
            pos.X = rand.Next(-200, 100);
            pos.Y = rand.Next(-200, 100);
            playExplosion = false;
        }

        public void Draw(SpriteBatch sBatch, Matrix camTransform)
        {
            Vector2 origin = new Vector2(kamikazeTank.Width / 2, kamikazeTank.Height / 2);
            KamikazeTransform = Matrix.CreateRotationZ(tankOrientation) * Matrix.CreateTranslation(pos.X, pos.Y, zPos) * camTransform;
                                                                       
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, KamikazeTransform);
            if (alive == true)                                             //change first zero to a 1 to make it face the right way
                sBatch.Draw(kamikazeTank, Vector2.Zero, null, Color.White, 1, origin, 1, SpriteEffects.None, 0);
            sBatch.End();

            //for drawing the explosion sequence when player destroys the kamikaze or it hits the player
            Vector2 originExplosion = new Vector2(explosion3.Width / 2, explosion3.Height / 2);
            explosionTransform = Matrix.CreateTranslation(explosionPos.X, explosionPos.Y, 0) * camTransform;
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, explosionTransform);

            if (count == 1)
                sBatch.Draw(explosion1, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 2)
                sBatch.Draw(explosion2, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 3)
                sBatch.Draw(explosion3, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 4)
                sBatch.Draw(explosion4, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 5)
                sBatch.Draw(explosion5, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 6)
                sBatch.Draw(explosion6, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 7)
                sBatch.Draw(explosion7, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 8)
                sBatch.Draw(explosion8, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 9)
                sBatch.Draw(explosion9, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 10)
                sBatch.Draw(explosion10, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 11)
                sBatch.Draw(explosion11, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 12)
                sBatch.Draw(explosion12, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 13)
                sBatch.Draw(explosion13, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 14)
                sBatch.Draw(explosion14, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 15)
                sBatch.Draw(explosion15, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 16)
                sBatch.Draw(explosion16, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 17)
                sBatch.Draw(explosion17, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 18)
                sBatch.Draw(explosion18, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 19)
                sBatch.Draw(explosion19, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 20)
                sBatch.Draw(explosion20, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 21)
                sBatch.Draw(explosion21, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 22)
                sBatch.Draw(explosion22, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 23)
                sBatch.Draw(explosion23, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 24)
                sBatch.Draw(explosion24, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            
            sBatch.End();
        }
    }
}
