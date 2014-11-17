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
    class PlayerBullet
    {
        Texture2D playerBullet;//texture
        Color[] bulletTextureData;//colours around the edge
        Matrix bulletTransform;//transform for the bullet

        float rotation;//rotation value

        KeyboardState prevState = Keyboard.GetState();//previous keystate
        bool alive = false;//is bullet alive or not

        Vector2 bulletPos;//position of bullet
        Vector2 bulletVelocity;//velocity of bullet
        Vector2 verticalBulletVelocity;//vertical velocity

        Rectangle bulletRectangle;//rectangle around the bullet for collision detection

        //these bools are used to determine the direction for the bullet
        bool left;
        bool right;
        bool up;
        bool down;

        //constructor
        public PlayerBullet(float turretX, float turretY, Vector2 playerPos)
        {
            rotation = 1.0f;

            bulletPos.X = playerPos.X;
            bulletPos.Y = playerPos.Y;

            bulletVelocity = new Vector2(2.10f, 0);
            verticalBulletVelocity = new Vector2(0, 2.10f);

            //tank faces left by default at the start
            left = true;
            right = false;
            up = false;
            down = false;
        }

        //start properites
        public Matrix BulletTransform
        {
            get { return bulletTransform; }
            set { bulletTransform = value; }
        }
        public Vector2 BulletPos
        {
            get { return bulletPos; }
            set { bulletPos = value; }
        }
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }
        public Rectangle BulletRectangle
        {
            get { return bulletRectangle; }
            set { bulletRectangle = value; }
        }
        public Color[] BulletTextureData
        {
            get { return bulletTextureData; }
            set { bulletTextureData = value; }
        }
        //end properties

        //load content
        public void LoadContent(ContentManager c)
        {
            //player bullet texture
            playerBullet = c.Load<Texture2D>("Player/shell1");
            //get the colour data for the bullet texture
            bulletTextureData = new Color[playerBullet.Width * playerBullet.Height];
            playerBullet.GetData(bulletTextureData);
        }

        //update
        public void Update(KeyboardState kInput, float turret_rotation, Vector2 playerPos, float tank_rotation)
        {
            //if space was pressed, set bullet to alive and set the direction
            if (kInput.IsKeyDown(Keys.Space) && prevState.IsKeyUp(Keys.Space))
            {
                    alive = true;
                    SetDirection(turret_rotation, tank_rotation);
            }

            prevState = kInput;//stores keyboard input into prevState

            //if bullet is alive, move
            if (alive == true)
                Move(tank_rotation, turret_rotation);

            //method for detecting the edge of the screen
            DetectEdgeOfScreen(playerPos);

            //get bullet rectangle.this is for the collision detection
            bulletRectangle = new Rectangle((int)bulletPos.X, (int)bulletPos.Y, playerBullet.Width, playerBullet.Height);
        }

        //method for resetiing the bullet
        public void Reset(Vector2 playerPos)
        {
            bulletPos.X = playerPos.X;
            bulletPos.Y = playerPos.Y;
            bulletVelocity = new Vector2(2.10f, 0);
            verticalBulletVelocity = new Vector2(0, 2.10f);
            rotation = 1.0f;
        }

        //method for moving
        public void Move(float tank_rotation, float turret_rotation)
        {
            if (left == true)
            {
                rotation = turret_rotation + 4.7f;
                bulletPos -= bulletVelocity;
            }
            else if (right == true)
            {
                rotation = turret_rotation - 4.7f;
                bulletPos += bulletVelocity;
            }
            else if (up == true)
            {
                rotation = turret_rotation + 6.3f;
                bulletPos -= verticalBulletVelocity;
            }
            else if (down == true)
            {
                rotation = turret_rotation - 9.3f;
                bulletPos += verticalBulletVelocity;
            }
        }

        //sets the direction for the bullet to move in
        public void SetDirection(float turret_rotation, float tank_rotation)
        {
            Matrix rot = Matrix.CreateRotationZ(turret_rotation);
            bulletVelocity = Vector2.Transform(bulletVelocity, rot);
            verticalBulletVelocity = Vector2.Transform(verticalBulletVelocity, rot);

            //while turning left
            if (tank_rotation < 0.5 && tank_rotation > 5.5 || tank_rotation <-2.5 && tank_rotation > - 3.5)
            {
                left = true;
                up = false;
                down = false;
                right = false;
            }

            //up
            if (tank_rotation > 0.5 && tank_rotation < 2.5 || tank_rotation < -3.5 && tank_rotation > -5.5)
            {
                up = true;
                down = false;
                left = false;
                right = false;
            }
            //right
            if (tank_rotation > 2.5 && tank_rotation < 3.5 || tank_rotation <-0.5 && tank_rotation >-5.5)
            {
                right = true;
                left = false;
                down = false;
                up = false;
            }
            //down
            if (tank_rotation > 3.5 && tank_rotation < 5.5 ||tank_rotation < -0.5 && tank_rotation > -2.5)
            {
                down = true;
                up = false;
                left = false;
                right = false;
            }
            
        }

        //detects when the bullet goes off the edge of the screen visible to the player
        public void DetectEdgeOfScreen(Vector2 playerPos)
        {
            //right edge
            if (bulletPos.X > playerPos.X + 250)
            {
                alive = false;
                Reset(playerPos);
            }
            //left edge
            else if (bulletPos.X < playerPos.X - 250)
            {
                alive = false;
                Reset(playerPos);
            }
            //bottom edge
            if (bulletPos.Y > playerPos.Y + 110)
            {
                alive = false;
                Reset(playerPos);
            }
            //top edge
            else if (bulletPos.Y < playerPos.Y - 170)
            {
                alive = false;
                Reset(playerPos);
            }
        }

        //draw method
        public void Draw(SpriteBatch sBatch, Matrix turretTransform, Matrix camTransform, float turret_rotation, float tank_rotation)
        {
            Vector2 origin = new Vector2(playerBullet.Width / 2, playerBullet.Height / 2);

            bulletTransform = Matrix.CreateRotationZ(rotation) * Matrix.CreateTranslation(bulletPos.X, bulletPos.Y, 0) * camTransform;

            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, bulletTransform);
            sBatch.Draw(playerBullet, Vector2.Zero, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
            sBatch.End();

        }
    }
}
