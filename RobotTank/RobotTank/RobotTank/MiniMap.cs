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
    class MiniMap
    {
        //positions for miniMap,player,kamikaze,shooting enemies and repair power up
        Vector3 miniMapPos;
        Vector3 playersPos;
        Vector3 kamikazePos;
        Vector3 shootingEnemy1Pos;
        Vector3 shootingEnemy2Pos;
        Vector3 shootingEnemy3Pos;
        Vector3 repairPos;

        //textrues for miniMap,player,kamikaze,shooting enemies and repair power up
        Texture2D mapTexture;
        Texture2D playerTexture;
        Texture2D kamikazeTexture;
        Texture2D shootingEnemyTexture;
        Texture2D repairTexture;

        //transforms for miniMap,player,kamikaze,shooting enemies and repair power up
        Matrix mapTransform;

        Matrix playerTransform;

        Matrix kamikazeTransform;

        Matrix shootingEnemy1Transform;
        Matrix shootingEnemy2Transform;
        Matrix shootingEnemy3Transform;

        Matrix repairTransform;

        //zooom
        Vector2 scale;
        Vector2 tankScale;
        //constructor
        public MiniMap()
        {
            //gives default values for all positions
            miniMapPos.X = 500;
            miniMapPos.Y = 100;
            miniMapPos.Z = 0;

            playersPos.X = 0;
            playersPos.Y = 0;
            playersPos.Z = 0;

            kamikazePos.X = 0;
            kamikazePos.Y = 0;
            kamikazePos.Z = 0;

            shootingEnemy1Pos.X = 0;
            shootingEnemy1Pos.Y = 0;
            shootingEnemy1Pos.Z = 0;

            shootingEnemy2Pos.X = 0;
            shootingEnemy2Pos.Y = 0;
            shootingEnemy2Pos.Z = 0;

            shootingEnemy3Pos.X = 0;
            shootingEnemy3Pos.Y = 0;
            shootingEnemy3Pos.Z = 0;

            repairPos.X = 0;
            repairPos.Y = 0;
            repairPos.Z = 0;

            //scale for drawing the map itself
            scale.X = 0.30f;//.25
            scale.Y = 0.30f;//.25
            //scale for miniMap,player,kamikaze,shooting enemies and repair power up
            tankScale.X = 0.25f;//.25
            tankScale.Y = 0.25f;//.25
        }

        //load content
        public void LoadContent(ContentManager c)
        {
            //map texture
            mapTexture = c.Load<Texture2D>("Background/field2");
            //player texture
            playerTexture = c.Load<Texture2D>("playerOnRadar2");
            //kamikaze texture
            kamikazeTexture = c.Load<Texture2D>("kamikazeOnRadar");
            //shooting enemy texture
            shootingEnemyTexture = c.Load<Texture2D>("shootingEnemyOnRadar");
            //repair texture
            repairTexture = c.Load<Texture2D>("repairOnRadar");
        }

        public void Update(Vector2 playerRealPos, Vector2 kamikaze1Pos, Vector2 repairP, ShootingEnemy[] shootArray)
        {
            //updates the players position on the mini map if they are not off the edge of it. stops at edge if they go past boundary
            if (playerRealPos.X > -300 && playerRealPos.X < 600)
            {
                if (playerRealPos.Y > -190 && playerRealPos.Y < 400)
                {
                    playersPos.X = playerRealPos.X + 300;
                    playersPos.Y = playerRealPos.Y + 200;
                }
            }       

            //sets the kamikaze enemies real position on the mini map and updates it
            kamikazePos.X = kamikaze1Pos.X + 300;
            kamikazePos.Y = kamikaze1Pos.Y + 200;

            //sets the repair powerups real position on the mini map and updates it
            repairPos.X = repairP.X + 300;
            repairPos.Y = repairP.Y + 200;

            //sets the shooting enemies real positions on the mini map and updates them
            //they are in an array in the game but it is easier to sort them for the mini map if they are separate here
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                {
                    shootingEnemy1Pos.X = shootArray[i].EnemyPos.X + 300;
                    shootingEnemy1Pos.Y = shootArray[i].EnemyPos.Y + 200;
                }
                if (i == 1)
                {
                    shootingEnemy2Pos.X = shootArray[i].EnemyPos.X + 300;
                    shootingEnemy2Pos.Y = shootArray[i].EnemyPos.Y + 200;
                }
                if (i == 2)
                {
                    shootingEnemy3Pos.X = shootArray[i].EnemyPos.X + 300;
                    shootingEnemy3Pos.Y = shootArray[i].EnemyPos.Y + 200;
                }
            }

        }

        //draw method
        public void Draw(SpriteBatch sBatch, bool kamikazeAlive, bool repairActive, ShootingEnemy[] shootArray)
        {
            //for the map itself
            Vector2 mapOrigin = new Vector2(mapTexture.Width / 2, mapTexture.Height / 2);//set origin
            mapTransform = Matrix.CreateTranslation(-mapTexture.Width / 2, -mapTexture.Height / 2, 0) * Matrix.CreateScale(scale.X, scale.Y, 0);//scales the transform
            sBatch.Begin(SpriteSortMode.BackToFront,null,null,null,null,null,mapTransform);
            sBatch.Draw(mapTexture, new Vector2(750, 500), null, Color.Green, 0, mapOrigin, 1, SpriteEffects.None, 0);
            sBatch.End();


            //for the player
            Vector2 playerOrigin = new Vector2(playerTexture.Width / 2, playerTexture.Height / 2);//set origin
            playerTransform = Matrix.CreateTranslation(playersPos.X, playersPos.Y, 0) * Matrix.CreateScale(tankScale.X, tankScale.Y, 0);//scales the transform
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, playerTransform);
            sBatch.Draw(playerTexture, Vector2.Zero, null, Color.White, 0, playerOrigin, 1, SpriteEffects.None, 0);
            sBatch.End();


            //for the kamikaze enemy
            Vector2 kamikazeOrigin = new Vector2(kamikazeTexture.Width / 2, kamikazeTexture.Height / 2);//sets the origin
            kamikazeTransform = Matrix.CreateTranslation(kamikazePos.X, kamikazePos.Y, 0) * Matrix.CreateScale(tankScale.X, tankScale.Y, 0);//scales the transform
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, kamikazeTransform);
            if (kamikazeAlive == true)
                sBatch.Draw(kamikazeTexture, Vector2.Zero, null, Color.White, 0, kamikazeOrigin, 1, SpriteEffects.None, 0);
            sBatch.End();


            //for the shooting enemies
            //all have the same origin
            Vector2 shootingEnemyOrigin = new Vector2(shootingEnemyTexture.Width / 2, shootingEnemyTexture.Height / 2);//sets the origin
            ////enemy 1
            shootingEnemy1Transform = Matrix.CreateTranslation(shootingEnemy1Pos.X, shootingEnemy1Pos.Y, 0) * Matrix.CreateScale(tankScale.X, tankScale.Y, 0);//scales the transform
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, shootingEnemy1Transform);
            if (shootArray[0].Alive == true)
                sBatch.Draw(shootingEnemyTexture, Vector2.Zero, null, Color.White, 0, shootingEnemyOrigin, 1, SpriteEffects.None, 0);
            sBatch.End();

            ////enemy 2
            shootingEnemy2Transform = Matrix.CreateTranslation(shootingEnemy2Pos.X, shootingEnemy2Pos.Y, 0) * Matrix.CreateScale(tankScale.X, tankScale.Y, 0);//scales the transform
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, shootingEnemy2Transform);
            if (shootArray[2].Alive == true)
                sBatch.Draw(shootingEnemyTexture, Vector2.Zero, null, Color.White, 0, shootingEnemyOrigin, 1, SpriteEffects.None, 0);
            sBatch.End();

            ////enemy 3
            shootingEnemy3Transform = Matrix.CreateTranslation(shootingEnemy3Pos.X, shootingEnemy3Pos.Y, 0) * Matrix.CreateScale(tankScale.X, tankScale.Y, 0);//scales the transform
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, shootingEnemy3Transform);
            if (shootArray[2].Alive == true)
                sBatch.Draw(shootingEnemyTexture, Vector2.Zero, null, Color.White, 0, shootingEnemyOrigin, 1, SpriteEffects.None, 0);
            sBatch.End();


            //for the repair power up
            Vector2 repairOrigin = new Vector2(repairTexture.Width / 2, repairTexture.Height / 2);//set the origin
            repairTransform = Matrix.CreateTranslation(repairPos.X, repairPos.Y, 0) * Matrix.CreateScale(tankScale.X, tankScale.Y, 0);//scales the transform
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, repairTransform);
            if (repairActive == true)
                sBatch.Draw(repairTexture, Vector2.Zero, null, Color.White, 0, repairOrigin, 1, SpriteEffects.None, 0);
            sBatch.End();
        }
    }
}
