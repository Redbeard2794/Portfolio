using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Media;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace JointProject2C00172382
{
    //this class inherits from the bikeEnemy class
    public class PatrolEnemy : BikeEnemy
    {
        //textures
        Texture2D patrolRight1;
        Texture2D patrolRight2;
        Texture2D patrolRight3;
        Texture2D patrolRight4;
        Texture2D patrolRight5;
        Texture2D patrolRight6;
        Texture2D patrolLeft1;
        Texture2D patrolLeft2;
        Texture2D patrolLeft3;
        Texture2D patrolLeft4;
        Texture2D patrolLeft5;
        Texture2D patrolLeft6;
        Texture2D bulletTexture;
        //counter for changing sprites
        int spriteCount = 0;
        int spriteCountLeft = 0;
        //direction it is facing in
        bool moveLeft = false;
        bool moveRight = true;
        //speed
        int patrolSpeed = 1;
        //bullet position
        public Vector2 bulletPos;
        //distance to player
        int distanceToPlayerX = 0;
        //if it can shoot
        bool canShoot = false;
        //if bullet is alive
        bool bulletAlive = false;
        //bullet speed
        public int bulletSpeed = 5;

        public bool CanShoot
        {
            get { return canShoot; }
            set { canShoot = value; }
        }
        public bool BulletAlive
        {
            get { return bulletAlive; }
            set { bulletAlive = value; }
        }
        //inherits default constructor from bikeEnemy
        public PatrolEnemy(Texture2D right1, Texture2D right2, Texture2D right3, Texture2D right4, Texture2D right5, Texture2D right6, Texture2D left1, Texture2D left2, Texture2D left3, Texture2D left4, Texture2D left5, Texture2D left6, Texture2D bullet)
            : base()
        {
            patrolRight1 = right1;
            patrolRight2 = right2;
            patrolRight3 = right3;
            patrolRight4 = right4;
            patrolRight5 = right5;
            patrolRight6 = right6;
            patrolLeft1 = left1;
            patrolLeft2 = left2;
            patrolLeft3 = left3;
            patrolLeft4 = left4;
            patrolLeft5 = left5;
            patrolLeft6 = left6;
            bulletTexture = bullet;
            bulletPos.X = bikePos.X;
            bulletPos.Y = bikePos.Y;
        }
        //move method
        public void Move()
        {
            if (alive == true)
            {
                //if facing left
                if (moveLeft == true)
                {
                    bikePos.X -= patrolSpeed;
                    spriteCountLeft++;
                    if (spriteCountLeft == 12)
                        spriteCountLeft = 0;
                }
                    //if facing right
                else if (moveRight == true)
                {
                    bikePos.X += patrolSpeed;
                    spriteCount++;
                    if (spriteCount == 12)
                        spriteCount = 0;
                }
                //changes direction
                if (bikePos.X < 200)
                {
                    moveLeft = false;
                    moveRight = true;
                }
                //changes direction
                else if (bikePos.X > 400)
                {
                    moveRight = false;
                    moveLeft = true;
                }
            }
        }
        public void Shoot(int playerX, int playerY, SoundEffect shot)
        {
            
            if (alive == true)
            {
                //distance from player
                distanceToPlayerX = playerX - (int)bikePos.X;
                //sets can shoot to true
                if (distanceToPlayerX >= -150)
                {
                    canShoot = true;
                }
                if (canShoot == true)
                {
                    bulletAlive = true;
                }
                //moves bullet
                if (bulletAlive == true)
                    bulletPos.X -= bulletSpeed;
                //if off edge of screen
                if (bulletPos.X <= 0)
                {
                    canShoot = false;
                    bulletAlive = false;
                    bulletPos.X = bikePos.X - 5;
                    bulletPos.Y = bikePos.Y + 15;
                }
                //if off edge of screen
                else if (bulletPos.X >= 700)
                {
                    canShoot = false;
                    bulletAlive = false;
                    bulletPos.X = bikePos.X - 5;
                    bulletPos.Y = bikePos.Y + 15;
                }
                if (bulletAlive == false && canShoot == false)
                {
                    bulletPos.X = bikePos.X - 5;
                    bulletPos.Y = bikePos.Y + 15;
                }
            }
        }
        //rectangle collision with player
        public override bool CollisionWithPlayer(int playerX, int playerY, int playerWidth, int playerHeight)
        {
            if (alive == true)
            {
                if (playerX >= bulletPos.X + 15)
                {
                    return false;
                }
                if (playerY + playerHeight <= bulletPos.Y)
                {
                    return false;
                }
                if (playerX + playerWidth <= bulletPos.X)
                {
                    return false;
                }
                if (playerY >= bulletPos.Y + 15)
                {
                    return false;
                }
                return true;
            }
            else return false;
        }
        //collision between player and patrols bullet
        public bool CollisionBetweenPlayerAndPatrolBullet(int playerX, int playerY, int playerWidth, int playerHeight)
        {
            if (bulletAlive == true)
            {
                if (playerX >= bulletPos.X + bulletTexture.Width)
                {
                    return false;
                }
                if (playerY + playerHeight <= bikePos.Y)
                {
                    return false;
                }
                if (playerX + playerWidth <= bikePos.X)
                {
                    return false;
                }
                if (playerY >= bulletPos.Y + bulletTexture.Height)
                {
                    return false;
                }
                return true;
            }
            else return false;
        }
        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            //if facing right draw these sprites depending on counter
            if (moveRight == true && alive == true)
            {
                if (spriteCount <= 2)
                    spriteBatch.Draw(patrolRight1, new Rectangle((int)bikePos.X, (int)bikePos.Y, patrolRight1.Width, patrolRight1.Height), Color.White);
                else if (spriteCount <= 4)
                    spriteBatch.Draw(patrolRight2, new Rectangle((int)bikePos.X, (int)bikePos.Y, patrolRight1.Width, patrolRight1.Height), Color.White);
                else if (spriteCount <= 6)
                    spriteBatch.Draw(patrolRight3, new Rectangle((int)bikePos.X, (int)bikePos.Y, patrolRight1.Width, patrolRight1.Height), Color.White);
                else if (spriteCount <= 8)
                    spriteBatch.Draw(patrolRight4, new Rectangle((int)bikePos.X, (int)bikePos.Y, patrolRight1.Width, patrolRight1.Height), Color.White);
                else if (spriteCount <= 10)
                    spriteBatch.Draw(patrolRight5, new Rectangle((int)bikePos.X, (int)bikePos.Y, patrolRight1.Width, patrolRight1.Height), Color.White);
                else if (spriteCount <= 12)
                    spriteBatch.Draw(patrolRight6, new Rectangle((int)bikePos.X, (int)bikePos.Y, patrolRight1.Width, patrolRight1.Height), Color.White);
            }
            //if facing left draw these sprites depending on counter
            else if (moveLeft == true && alive == true)
            {
                if (spriteCountLeft <= 2)
                    spriteBatch.Draw(patrolLeft1, new Rectangle((int)bikePos.X, (int)bikePos.Y, patrolRight1.Width, patrolRight1.Height), Color.White);
                else if (spriteCountLeft <= 4)
                    spriteBatch.Draw(patrolLeft2, new Rectangle((int)bikePos.X, (int)bikePos.Y, patrolRight1.Width, patrolRight1.Height), Color.White);
                else if (spriteCountLeft <= 6)
                    spriteBatch.Draw(patrolLeft3, new Rectangle((int)bikePos.X, (int)bikePos.Y, patrolRight1.Width, patrolRight1.Height), Color.White);
                else if (spriteCountLeft <= 8)
                    spriteBatch.Draw(patrolLeft4, new Rectangle((int)bikePos.X, (int)bikePos.Y, patrolRight1.Width, patrolRight1.Height), Color.White);
                else if (spriteCountLeft <= 10)
                    spriteBatch.Draw(patrolLeft5, new Rectangle((int)bikePos.X, (int)bikePos.Y, patrolRight1.Width, patrolRight1.Height), Color.White);
                else if (spriteCountLeft <= 12)
                    spriteBatch.Draw(patrolLeft6, new Rectangle((int)bikePos.X, (int)bikePos.Y, patrolRight1.Width, patrolRight1.Height), Color.White);
                //if shooting, draws the bullet
                if (canShoot == true && alive == true && bulletAlive == true)
                {
                    spriteBatch.Draw(bulletTexture, new Rectangle((int)bulletPos.X, (int)bulletPos.Y, bulletTexture.Width, bulletTexture.Height), Color.White);
                }
            }
            
        }
    }
}
