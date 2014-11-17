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
    class Turret
    {
        //textures
        Texture2D turretTexture;
        Texture2D turretBullet;
        //turret position
        public Vector2 turretPos;
        //turret bullet position
        public Vector2 turretBulletPos;
        //if turret can shoot
        public bool canShoot = false;
        //if turret is alive
        public bool alive1 = true;
        //if bullet is alive
        public bool bullet1Alive = false;
        //distance from player
        public int distanceFromPlayerToTurret1;
        //constructor
        public Turret(Texture2D enemTurretLeft, Texture2D turretBulletTexture)
        {
            //saves textures
            turretTexture = enemTurretLeft;
            turretBullet = turretBulletTexture;
        }
        //method for the turret to shoot
        //if the player is within a certain distance of the turret it will fire
        public void Shoot(int playerX, int playerY)
        {
            if (alive1 == true)
            {
                distanceFromPlayerToTurret1 = playerX - (int)turretPos.X;
                if (distanceFromPlayerToTurret1 >= -300)
                {
                    canShoot = true;
                }
                if (canShoot == true)
                {
                    bullet1Alive = true;
                }
                if (bullet1Alive == true)
                    turretBulletPos.X -= 5;
                //if off edge of screen
                if (turretBulletPos.X <= 0)
                {
                    canShoot = false;
                    bullet1Alive = false;
                    turretBulletPos.X = turretPos.X - 5;
                    turretBulletPos.Y = turretPos.Y + 15;
                }
                //if off edge of screen
                else if (turretBulletPos.X >= 700)
                {
                    canShoot = false;
                    bullet1Alive = false;
                    turretBulletPos.X = turretPos.X - 5;
                    turretBulletPos.Y = turretPos.Y + 15;
                }
                //sets bullet pos back at turret
                if (bullet1Alive == false && canShoot == false)
                {
                    turretBulletPos.X = turretPos.X - 5;
                    turretBulletPos.Y = turretPos.Y + 15;
                }
            }
        }
        //for moving the turrets bullet
        public void TurretBulletMove()
        {
            turretBulletPos.X -= 5;
        }
        //rectangle collision between turret bullet and player
        public bool Bullet1CollisionWithPlayer(int playerX, int playerY, int playerWidth, int playerHeight)
        {
            if (playerX >= turretBulletPos.X + turretBullet.Width)
            {
                return false;
            }
            if (playerY + playerHeight <= turretBulletPos.Y)
            {
                return false;
            }
            if (playerX + playerWidth <= turretBulletPos.X)
            {
                return false;
            }
            if (playerY >= turretBulletPos.Y + turretBullet.Height)
            {
                return false;
            }
            return true;
        }
        //saves turrets data
        public void WriteToTextFile(StreamWriter outfile)
        {
            outfile.Write(alive1 + (","));
            outfile.Write(turretPos.X + (","));
            outfile.Write(turretPos.Y + (","));
            outfile.Write(turretBulletPos.X + (","));
            outfile.Write(turretBulletPos.Y + (","));
        }
        //loads turrets data
        public void LoadFromTextFile(string bLine)
        {
            string[] wordArray;
            wordArray = bLine.Split(',');
            alive1 = Convert.ToBoolean(wordArray[0]);
            turretPos.X = Convert.ToInt32(wordArray[1]);
            turretPos.Y = Convert.ToInt32(wordArray[2]);
            turretBulletPos.X = Convert.ToInt32(wordArray[3]);
            turretBulletPos.Y = Convert.ToInt32(wordArray[4]);
        }
        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (alive1 == true)
            {
                spriteBatch.Draw(turretTexture, new Rectangle((int)turretPos.X, (int)turretPos.Y, turretTexture.Width, turretTexture.Height), Color.White);
            }
            if (canShoot == true && alive1 == true && bullet1Alive == true)
                spriteBatch.Draw(turretBullet, new Rectangle((int)turretBulletPos.X, (int)turretBulletPos.Y, turretBullet.Width, turretBullet.Height), Color.White);
        }
    }
}
