using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace JointProject2C00172382
{
    class PlayerBullet
    {
        //bullet alive
        public bool bulletAlive;
        //bullet position
        public Vector2 bulletPosition;
        //bullet speed
        public int bulletMoveSpeed;
        //direction
        const byte None = 0, North = 1, East = 2, South = 3, West = 4;
        //texture
        Texture2D bullet;
        //collision with turret
        public bool collisionTurret1 = false;
        //direction
        public int realOrientation = South;

        //constructor
        public PlayerBullet(Texture2D bulletTexture, int playerDirection)
        {
            
            bullet = bulletTexture;
            bulletAlive = false;
            bulletMoveSpeed = 4;
        }
        //move
        public void Move(int playerX, int playerY)//, int orientation)
        {
            if (bulletAlive == true)
            {
                //depending on direction moves
                if (realOrientation == West)
                    bulletPosition.X += bulletMoveSpeed;
                if (realOrientation == East)
                    bulletPosition.X = bulletPosition.X - bulletMoveSpeed;
                if (realOrientation == South)
                    bulletPosition.Y = bulletPosition.Y + bulletMoveSpeed;
                if (realOrientation == North)
                    bulletPosition.Y = bulletPosition.Y - bulletMoveSpeed;
                //if off edge of screen
                if (bulletPosition.X >= 700)// || bulletPosition.X <= 0 || bulletPosition.Y >= 400 || bulletPosition.Y <= 0)
                {
                    bulletAlive = false;
                    bulletPosition.X = playerX + 10;
                    bulletPosition.Y = playerY + 10;
                }
                if (bulletPosition.X <= 0)
                {
                    bulletAlive = false;
                    bulletPosition.X = playerX + 10;
                    bulletPosition.Y = playerY + 10;
                }
                if (bulletPosition.Y >= 400)
                {
                    bulletAlive = false;
                    bulletPosition.X = playerX + 10;
                    bulletPosition.Y = playerY + 10;
                }
                if (bulletPosition.Y <= 0)
                {
                    bulletAlive = false;
                    bulletPosition.X = playerX + 10;
                    bulletPosition.Y = playerY + 10;
                }
            }
        }
        //collision with turret
        public bool Collision(int turret1X, int turret1Y, int turretWidth, int turretHeight)//, int turret2X, int turret2Y)
        {
            if (turret1X >= bulletPosition.X + bullet.Width)
            {
                return false;
            }
            if (turret1Y + turretHeight <= bulletPosition.Y)
            {
                return false;
            }
            if (turret1X + turretWidth <= bulletPosition.X)
            {
                return false;
            }
            if (turret1Y >= bulletPosition.Y + bullet.Height)
            {
                return false;
            }
            return true;
        }
        //collision with bike
        public bool CollisionWithBike(int bikeX, int bikeY, int bikeWidth, int bikeHeight)
        {
            if (bikeX >= bulletPosition.X + bullet.Width)
            {
                return false;
            }
            if (bikeY + bikeHeight <= bulletPosition.Y)
            {
                return false;
            }
            if (bikeX + bikeWidth <= bulletPosition.X)
            {
                return false;
            }
            if (bikeY >= bulletPosition.Y + bullet.Height)
            {
                return false;
            }
            return true;
        }
        //collision with patrol
        public bool CollisionWithPatrol(int patrolX, int patrolY, int patrolWidth, int patrolHeight)
        {
            if (patrolX >= bulletPosition.X + bullet.Width)
            {
                return false;
            }
            if (patrolY + patrolHeight <= bulletPosition.Y)
            {
                return false;
            }
            if (patrolX + patrolWidth <= bulletPosition.X)
            {
                return false;
            }
            if (patrolY >= bulletPosition.Y + bullet.Height)
            {
                return false;
            }
            return true;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (bulletAlive == true)
                spriteBatch.Draw(bullet, bulletPosition, Color.White);
        }
    }
}
