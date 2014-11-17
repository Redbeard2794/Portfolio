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
    class Player
    {
        //variables for the tanks body
        Texture2D playerBody;//texture
        Matrix bodyTransform;//matrix used for movement and rotation
        float movementSpeed;//speed the tank moves up and down
        float rotation;//rotation angle
        
        Vector2 direction;//direction for tank to move in
      
        //x, y and z parts of the bodyTransform
        Vector2 playerPos;
        float zPos;

        //variables for the hud
        Vector2 hudPos;
        float hudZ;

        Texture2D hud;//texture for the hud
        Matrix hudTransform;//transform for the hud 

        //variables for the information on the hud (hjealth, lives, mini map)    score!!!
        SpriteFont Font1;
        //lives
        Vector2 lifePos;
        Matrix lifeTransform;
        int noOfLives = 3;
        //health
        Matrix healthTransform;
        Vector2 healthPos;
        int healthValue = 100;
        //score
        Matrix ScoreTransform;
        Vector2 scorePos;
        int scoreValue = 0;
        
        //rectangle aound the player
        Rectangle playerRectangle;
        Color[] playerTextureData;//array of colours on the edge of the texture

        //constructor
        public Player()
        {
            playerPos.X = 130;//380
            playerPos.Y = 120;//250
            zPos = 0;
            rotation = 0;
            //hud elements
            //hud itself
            hudPos.X = 29;
            hudPos.Y = 449;
            hudZ = 0;
            //health meter
            healthPos.X = 150;
            healthPos.Y = 225;
            //life counter
            lifePos.X = 50;
            lifePos.Y = 225;
            //score
            scorePos.X = 350;
            scorePos.Y = 225;
        }

        //start properties
        public Vector2 PlayerPos
        {
            get { return playerPos; }
            set { playerPos = value; }
        }
        public int NoOfLives
        {
            get { return noOfLives; }
            set { noOfLives = value; }
        }
        public Matrix BodyTransform
        {
            get { return bodyTransform; }
            set { bodyTransform = value; }
        }
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public int ScoreValue
        {
            get { return scoreValue; }
            set {scoreValue = value;}
        }
        public Rectangle PlayerRectangle
        {
            get { return playerRectangle; }
            set { playerRectangle = value; }
        }
        public Color[] PlayerTextureData
        {
            get { return playerTextureData; }
            set { playerTextureData = value; }
        }
        public int HealthValue
        {
            get { return healthValue; }
            set { healthValue = value; }
        }
        //end properties

        //load content
        public void LoadContent(ContentManager c)
        {
            //texture for player tank body
            playerBody = c.Load<Texture2D>("Player/playerTankBody");
            //texture for hud
            hud = c.Load<Texture2D>("Hud/hud2");
            //font
            Font1 = c.Load<SpriteFont>("Font1");

            //get the texture data
            playerTextureData = new Color[playerBody.Width * playerBody.Height];
            playerBody.GetData(playerTextureData);
        }


        //update
        public void Update(KeyboardState kInput)
        {
            //method for detecting the boundary and moving
            BoundaryDetectionAndMovement(kInput);
            //rotating the body for moving left and right
            if (kInput.IsKeyDown(Keys.Left))
                rotation -= 0.01f;
            else if (kInput.IsKeyDown(Keys.Right))
                rotation += 0.01f;

            //if rotation hits 6.3 or negative 6.3 set back to zero. makes it easier to calculate other stuff
            if (rotation >= 6.3)
                rotation = 0;
            if (rotation <= -6.3)
                rotation = 0;

            //if health hits 0, subtract a life and set health back to 100
            if (healthValue <= 0)
            {
                noOfLives -= 1;
                healthValue = 100;
            }

            //gets the rectangle for the player
            playerRectangle = new Rectangle((int)playerPos.X, (int)playerPos.Y, playerBody.Width, playerBody.Height);
        }


        //this method allows the user to move the tank and also detects when the tank hits the edge of the map
        public void BoundaryDetectionAndMovement(KeyboardState kInput)
        {
            //up and down movement
            if (kInput.IsKeyDown(Keys.Up))
            {
                //these if/else statements are for the boundary detection
                if (playerPos.X < -400)
                {
                    movementSpeed = 0;
                    playerPos.X = -390;
                }
                else if (playerPos.X > 710)
                {
                    movementSpeed = 0;
                    playerPos.X = 700;
                }
                else if (playerPos.Y < -330)
                {
                    movementSpeed = 0;
                    playerPos.Y = -320;
                }
                else if (playerPos.Y > 440)
                {
                    movementSpeed = 0;
                    playerPos.Y = 430;
                }
                else movementSpeed = 1;

                playerPos -= direction * movementSpeed;
            }
            else if (kInput.IsKeyDown(Keys.Down))
            {
                //these if/else statements are for the boundary detection
                if (playerPos.X < -400)
                {
                    movementSpeed = 0;
                    playerPos.X = -390;
                }
                else if (playerPos.X > 710)
                {
                    movementSpeed = 0;
                    playerPos.X = 700;
                }
                else if (playerPos.Y < -330)
                {
                    movementSpeed = 0;
                    playerPos.Y = -320;
                }
                else if (playerPos.Y > 440)
                {
                    movementSpeed = 0;
                    playerPos.Y = 430;
                }
                else movementSpeed = 1;

                //actual movement
                playerPos += direction * movementSpeed;
            }
        }

        //draw
        public void Draw(SpriteBatch sBatch, Matrix camTransform)//, Matrix miniMapTransform)
        {
            //for drawing the tanks body
            Vector2 origin = new Vector2(playerBody.Width / 2, playerBody.Height / 2);
            
            bodyTransform = Matrix.CreateRotationZ(rotation) * Matrix.CreateTranslation(playerPos.X, playerPos.Y, zPos) * camTransform;
            direction = new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));

            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, bodyTransform);
            //changes colour depending on the amount of lives
            if (noOfLives == 3)
                sBatch.Draw(playerBody, Vector2.Zero, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
            else if (noOfLives == 2)
                sBatch.Draw(playerBody, Vector2.Zero, null, Color.IndianRed, 0, origin, 1, SpriteEffects.None, 0);
            else if (noOfLives == 1)
                sBatch.Draw(playerBody, Vector2.Zero, null, Color.OrangeRed, 0, origin, 1, SpriteEffects.None, 0);
            else if (noOfLives == 0)
                sBatch.Draw(playerBody, Vector2.Zero, null, Color.Crimson, 0, origin, 1, SpriteEffects.None, 0);
            sBatch.End();

            //for drawing the elemnts of the hud
            Vector2 originHud = new Vector2(hud.Width / 2, hud.Height / 2);
            hudTransform = Matrix.CreateTranslation(hudPos.X, hudPos.Y, hudZ);
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, hudTransform);
            sBatch.Draw(hud, Vector2.Zero, null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
            sBatch.End();

            //life counter pos
            string lives = "Lives: " + noOfLives;
            Vector2 lifeOrigin = Font1.MeasureString(lives) / 2;
            lifeTransform = Matrix.CreateTranslation(lifePos.X, lifePos.Y, 0);
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, lifeTransform);
            sBatch.DrawString(Font1, lives, lifePos, Color.White, 0, lifeOrigin, 1.0f, SpriteEffects.None, 0.5f);
            sBatch.End();

            //health meter
            string health = "health: " + healthValue;
            Vector2 healthOrigin = Font1.MeasureString(health) / 2;
            healthTransform = Matrix.CreateTranslation(healthPos.X, healthPos.Y, 0);
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, healthTransform);
            sBatch.DrawString(Font1, health, healthPos, Color.White, 0, healthOrigin, 1.0f, SpriteEffects.None, 0.5f);
            sBatch.End();

            //score
            string score = "Score: " + scoreValue;
            Vector2 scoreOrigin = Font1.MeasureString(score) / 2;
            ScoreTransform = Matrix.CreateTranslation(scorePos.X, scorePos.Y, 0);
            sBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, healthTransform);
            sBatch.DrawString(Font1, score, scorePos, Color.White, 0, scoreOrigin, 1.0f, SpriteEffects.None, 0.5f);
            sBatch.End();
        }
    }
}
