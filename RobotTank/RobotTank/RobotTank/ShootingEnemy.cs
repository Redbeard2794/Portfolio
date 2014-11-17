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
    class ShootingEnemy
    {
        //variables for the enemy tank itself
        Matrix enemyTransform;
        float rotation;//rotation value
        public Vector3 enemyPos;//position
        Texture2D enemyTank;//texture

        //used for per pixel detection
        Color[] enemyTextureData;//colours on the edge of the textures
        Rectangle enemyRectangle;//rectangle around the tank

        int health = 100;
        float speed = 1;
        bool foundPlayer;//if the player is nearby
        bool shooting;//if the tank is shooting
        bool moving;//if the tank is moving
        Vector3 playersPosition;//used for soring the players position
        //direction the tank is facing
        bool left;
        bool right;
        bool up;
        bool down;

        
        bool alive;//is tank alive or not

        //variables for the bullet
        Matrix enemyBulletTransform;
        float bulletRotation;//rotation value
        Vector3 bulletPos;//bullet position
        Texture2D enemyBullet;//texture
        float bulletSpeed = 2;//speed

        //for per pixel detection
        Rectangle enemyBulletRectangle;
        Color[] enemyBulletTextureData;

        //if bullet is alive
        bool bulletAlive;

        bool playExplosion = false;//is the explosion playing
        int count = 0;//counter to change explosion sprites
        Matrix explosionTransform;
        //position of explosion
        float explosionX;
        float explosionY;

        //textures for the explosion sequence
        Texture2D explode1;
        Texture2D explode2;
        Texture2D explode3;
        Texture2D explode4;
        Texture2D explode5;
        Texture2D explode6;
        Texture2D explode7;
        Texture2D explode8;
        Texture2D explode9;
        Texture2D explode10;
        Texture2D explode11;
        Texture2D explode12;
        Texture2D explode13;
        Texture2D explode14;
        Texture2D explode15;
        Texture2D explode16;
        Texture2D explode17;
        Texture2D explode18;
        Texture2D explode19;
        Texture2D explode20;
        Texture2D explode21;
        Texture2D explode22;
        Texture2D explode23;
        Texture2D explode24;


        //constructor
        public ShootingEnemy(Random rand)
        {
            //tank
            enemyPos.X = rand.Next(-500, 700);//700,700
            enemyPos.Y = rand.Next(-300, 200);//-300,500
            enemyPos.Z = 0;
            rotation = 0;
            foundPlayer = false;
            shooting = false;
            moving = false;
            alive = true;

            //bullet
            bulletPos.Z = 0;
            bulletRotation = 0;
            bulletAlive = false;
        }

        //start properties
        public Vector3 EnemyPos
        {
            get { return enemyPos; }
            set { enemyPos = value; }
        }
        //rectangle for tank
        public Rectangle EnemyRectangle
        {
            get { return enemyRectangle; }
            set { enemyRectangle = value; }
        }
        //colour data for tank
        public Color[] EnemyTextureData
        {
            get { return enemyTextureData; }
            set { enemyTextureData = value; }
        }
        //rectangle for bullet
        public Rectangle EnemyBulletRectangle
        {
            get { return enemyBulletRectangle; }
            set { enemyBulletRectangle = value; }
        }
        public Color[] EnemyBulletTextureData
        {
            get { return enemyBulletTextureData; }
            set { enemyBulletTextureData = value; }
        }
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
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
        public bool BulletAlive
        {
            get { return bulletAlive; }
            set { bulletAlive = value; }
        }
        public bool Shooting
        {
            get { return shooting; }
            set { shooting = value; }
        }
        //end properties

        //load content
        public void LoadContent(ContentManager c)
        {
            //textures for the tank
            enemyTank = c.Load<Texture2D>("ShootingEnemy/blueTank");
            //get the rectangle of colours around the edge of the texture
            enemyTextureData = new Color[enemyTank.Width * enemyTank.Height];
            enemyTank.GetData(enemyTextureData);

            //textures for bullet
            enemyBullet = c.Load<Texture2D>("ShootingEnemy/enemyShell1");
            //get the rectangle of colours around the edge of the texture
            enemyBulletTextureData = new Color[enemyBullet.Width * enemyBullet.Height];
            enemyBullet.GetData(enemyBulletTextureData);

            //explosion sequence
            explode1 = c.Load<Texture2D>("ExplosionSequence/explosion1");
            explode2 = c.Load<Texture2D>("ExplosionSequence/explosion2");
            explode3 = c.Load<Texture2D>("ExplosionSequence/explosion3");
            explode4 = c.Load<Texture2D>("ExplosionSequence/explosion4");
            explode5 = c.Load<Texture2D>("ExplosionSequence/explosion5");
            explode6 = c.Load<Texture2D>("ExplosionSequence/explosion6");
            explode7 = c.Load<Texture2D>("ExplosionSequence/explosion7");
            explode8 = c.Load<Texture2D>("ExplosionSequence/explosion8");
            explode9 = c.Load<Texture2D>("ExplosionSequence/explosion9");
            explode10 = c.Load<Texture2D>("ExplosionSequence/explosion10");
            explode11 = c.Load<Texture2D>("ExplosionSequence/explosion11");
            explode12 = c.Load<Texture2D>("ExplosionSequence/explosion12");
            explode13 = c.Load<Texture2D>("ExplosionSequence/explosion13");
            explode14 = c.Load<Texture2D>("ExplosionSequence/explosion14");
            explode15 = c.Load<Texture2D>("ExplosionSequence/explosion15");
            explode16 = c.Load<Texture2D>("ExplosionSequence/explosion16");
            explode17 = c.Load<Texture2D>("ExplosionSequence/explosion17");
            explode18 = c.Load<Texture2D>("ExplosionSequence/explosion18");
            explode19 = c.Load<Texture2D>("ExplosionSequence/explosion19");
            explode20 = c.Load<Texture2D>("ExplosionSequence/explosion20");
            explode21 = c.Load<Texture2D>("ExplosionSequence/explosion21");
            explode22 = c.Load<Texture2D>("ExplosionSequence/explosion22");
            explode23 = c.Load<Texture2D>("ExplosionSequence/explosion23");
            explode24 = c.Load<Texture2D>("ExplosionSequence/explosion24");

        }

        //update method
        public void Update(Vector2 playerPos)
        {
            //store and update the players position
            playersPosition.X = playerPos.X;
            playersPosition.Y = playerPos.Y;
            playersPosition.Z = 0;

            //method to detect the player
            DetectPlayer(playersPosition);
            //method for moving the tank
            Move(playersPosition);
            
            //if tank has found the player, stop  and shoot
            if (foundPlayer == true)
            {
                shooting = true;
                moving = false;
            }
            //if the tank hasnt found the player, stop shooting and move
            if (foundPlayer == false)
            {
                shooting = false;
                moving = true;
            }

            //if tank is stopped and shooting
            if (shooting == true)
            {
                bulletAlive = true;
                Shoot();
            }

            if (shooting == false)
            {
                bulletPos.X = enemyPos.X;
                bulletPos.Y = enemyPos.Y;
            }
            //mthod for deciding which way the tank should face
            AssignRotation();

            //rectangles for tank and bullet used in per piexel collision detection
            enemyRectangle = new Rectangle((int)enemyPos.X, (int)enemyPos.Y, enemyTank.Width, enemyTank.Height);
            enemyBulletRectangle = new Rectangle((int)bulletPos.X, (int)bulletPos.Y, enemyBullet.Width, enemyBullet.Height);

            //for playing the explosion sequence
            if (playExplosion == true)
            {
                count++;
                explosionX = enemyPos.X;
                explosionY = enemyPos.Y;
            }
            if (count > 24)
            {
                playExplosion = false;
                count = 0;
            }
        }
        //tells the tank which way to face
        public void AssignRotation()
        {
            if (left == true)
                rotation = -1.5f;
            else if (right == true)
                rotation = 1.5f;
            else if (up == true)
                rotation = 0;
            else if (down == true)
                rotation = -3.2f;
        }

        //method for detecting if player is close enough to shoot
        public void DetectPlayer(Vector3 playersPos)
        {
            //essentially if the player is within 100 pixels in any direction
            if (enemyPos.X > playersPos.X - 100 && enemyPos.Y > playersPos.Y - 100)
            {
                if (enemyPos.X < playersPos.X + 100 && enemyPos.Y > playersPos.Y - 100)
                {
                    if (enemyPos.X > playersPos.X - 100 && enemyPos.Y < playersPos.Y + 100)
                    {
                        if (enemyPos.X < playersPos.X + 100 && enemyPos.Y < playersPos.Y + 100)
                        {
                            foundPlayer = true;
                        }
                    }
                }
            }

        }

        //metod for if player is nearby and will make the enemy move towrds them until they are in range to shoot at. otherwise the tanks will remain
        //stationary
        public void Move(Vector3 playersPos)
        {
            //essentially if player is within 300 pixels in any direction
            if (enemyPos.X > playersPos.X - 300 && enemyPos.Y > playersPos.Y)
            {
                if (enemyPos.X < playersPos.X + 300 && enemyPos.Y > playersPos.Y - 300)
                {
                    if (enemyPos.X > playersPos.X - 300 && enemyPos.Y < playersPos.Y + 300)
                    {
                        if (enemyPos.X < playersPos.X + 300 && enemyPos.Y < playersPos.Y + 300)
                        {
                            moving = true;
                        }
                    }
                }
            }
            //right
            if (enemyPos.X < playersPos.X && moving == true)
            {
                enemyPos.X += speed;
                right = true;
                left = false;
                up = false;
                down = false;
            }
            //left
            if (enemyPos.X > playersPos.X && moving == true)
            {
                enemyPos.X -= speed;
                left = true;
                right = false;
                up = false;
                down = false;
            }
            //down
            if (enemyPos.Y < playersPos.Y && moving == true)
            {
                enemyPos.Y += speed;
                down = true;
                up = false;
                left = false;
                right = false;
            }
            //up
            if (enemyPos.Y > playersPos.Y && moving == true)
            {
                enemyPos.Y -= speed;
                up = true;
                down = false;
                left = false;
                right = false;
            }
        }

        //method for shooting
        public void Shoot()
        {
            //this section decides which way the bullet should face
                if (bulletAlive == true && left == true && foundPlayer == true)
                {
                    bulletPos.X -= bulletSpeed;
                    bulletRotation = -1.6f;
                }
                else if (bulletAlive == true && right == true && foundPlayer == true)
                {
                    bulletPos.X += bulletSpeed;
                    bulletRotation = 1.6f;
                }
                else if (bulletAlive == true & up == true && foundPlayer == true)
                {
                    bulletPos.Y -= bulletSpeed;
                    bulletRotation = 0;
                }
                else if (bulletAlive == true && down == true && foundPlayer == true)
                {
                    bulletPos.Y += bulletSpeed;
                    bulletRotation = 3.2f;
                }
        }

        //used to respawn the enemy after a certain amount of time
        public void Respawn(Random rand)
        {
            alive = true;
            //this could be a bit excessive and could be causing the glitchiness
            enemyPos.X = rand.Next(-500, 500);
            enemyPos.Y = rand.Next(-300, 300);
            foundPlayer = false;
            shooting = false;
            health = 100;
            playExplosion = false;
            count = 0;
            ResetBullet();
        }

        //method for resetting the bullet
        public void ResetBullet()
        {
            bulletPos.X = enemyPos.X;
            bulletPos.Y = enemyPos.Y;
            bulletAlive = false;
        }

        //method for detecting when the bullet goes off the area of the screen visible to the player
        public void DetectBoundaryForTheBullet(Vector2 playerPos)
        {
            //right edge
            if (bulletPos.X > playerPos.X + 250)
                ResetBullet();
            //left edge
            else if (bulletPos.X < playerPos.X - 250)
                ResetBullet();
            //bottom edge
            if (bulletPos.Y > playerPos.Y + 110)
                ResetBullet();
            //top edge
            else if (bulletPos.Y < playerPos.Y - 170)
                ResetBullet();
        }

        //draw method
        public void Draw(SpriteBatch sBatch, Matrix camTransform)
        {
            //for drawing the tank
            Vector2 origin = new Vector2(enemyTank.Width / 2, enemyTank.Height / 2);
            enemyTransform = Matrix.CreateRotationZ(rotation) * Matrix.CreateTranslation(enemyPos.X, enemyPos.Y, enemyPos.Z) *camTransform;
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, enemyTransform);
            if (alive == true)
            {
                if (health > 20)
                    sBatch.Draw(enemyTank, Vector2.Zero, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
                else if (health == 20)
                    sBatch.Draw(enemyTank, Vector2.Zero, null, Color.Red, 0, origin, 1, SpriteEffects.None, 0);
            }
            sBatch.End();

            //for drawing the bullet
            Vector2 originBullet = new Vector2(enemyBullet.Width / 2, enemyBullet.Height / 2);
            enemyBulletTransform = Matrix.CreateRotationZ(bulletRotation) * Matrix.CreateTranslation(bulletPos.X, bulletPos.Y, bulletPos.Z) * camTransform;
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, enemyBulletTransform);
            if (bulletAlive == true)
            {
                sBatch.Draw(enemyBullet, Vector2.Zero, null, Color.White, 0, originBullet, 1, SpriteEffects.None, 0);
            }
            sBatch.End();



            //for drawing the explosion
            //Vector2 originExplosion = new Vector2(explosion3.Width / 2, explosion3.Height / 2);
            Vector2 originExplosion = new Vector2(explode3.Width / 2, explode3.Height / 2);
            explosionTransform = Matrix.CreateTranslation(enemyPos.X, enemyPos.Y,0)*camTransform;
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, explosionTransform);
            if (count == 1)
                sBatch.Draw(explode1, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count ==2)
                sBatch.Draw(explode2, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 3)
                sBatch.Draw(explode3, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 4)
                sBatch.Draw(explode4, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 5)
                sBatch.Draw(explode5, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 6)
                sBatch.Draw(explode6, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 7)
                sBatch.Draw(explode7, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 8)
                sBatch.Draw(explode8, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 9)
                sBatch.Draw(explode9, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 10)
                sBatch.Draw(explode10, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 11)
                sBatch.Draw(explode11, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 12)
                sBatch.Draw(explode12, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 13)
                sBatch.Draw(explode13, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 14)
                sBatch.Draw(explode14, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 15)
                sBatch.Draw(explode15, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 16)
                sBatch.Draw(explode16, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 17)
                sBatch.Draw(explode17, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 18)
                sBatch.Draw(explode18, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 19)
                sBatch.Draw(explode19, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 20)
                sBatch.Draw(explode20, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 21)
                sBatch.Draw(explode21, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 22)
                sBatch.Draw(explode22, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 23)
                sBatch.Draw(explode23, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            else if (count == 24)
                sBatch.Draw(explode24, Vector2.Zero, null, Color.White, 0, originExplosion, 1, SpriteEffects.None, 0);
            sBatch.End();
        }
    }
}
