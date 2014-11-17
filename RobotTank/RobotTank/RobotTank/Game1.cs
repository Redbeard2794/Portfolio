//Description of RobotTank
//
//The player can move the tank with the arrow keys. They can rotate the turret with 'Z' and 'X'and shoot by pressing space
//the shooting enemies(blue) will track the player and start shooting in the direction they are facing when they get within
//a certain distance of the player and stop moving. This is INTENTIONAL as they are the "stupid/easy" enemy in my game. 
//They are meant to be easy to avoid and get away from. But if they get close they can very quickly kill you as they can rapidly fire at you 
//when you are close enough. This was done to try to achieve some form of balance for the game
//they respawn on a timer when killed
//The kamikaze enemy follows the player around and tries to drive into them causing a lot of damage.(this is the boss enemy, biggest challenge)
//again for balance
//it respawns on a timer when killed
//The repair power up allows the player to replenish their health if they pick it up. It respawns on a timer when picked up
//The mini map diplays the player(blue), shootingEnemies(red), kamikaze(orange) and the repair power up(grey)
//The Hud displays the players health,score and lives
//The game is won when the player gets 500 score and lost when both health and lives are zero
//The camera is zoomed in and centred on the player
//

//Completion date: 22/11/2013
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace RobotTank
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //textures
        Texture2D background;
        //create these objects
        Player player1;
        PlayerTurret turret1;
        PlayerBullet playerBullet1;
        Camera camera1;
        ShootingEnemy[] shootingEnemyArray;
        RepairPowerUp powerUp1;
        KamikazeTank kamikaze1;
        MiniMap miniMap1;
        //transform for the terrain
        Matrix terrainTransfrom;
        //max number of shooting enemies(they are in an array)
        int maxShootingEnemy = 3;
        //random number generator
        Random rand = new Random();
        SpriteFont Font1;

        //timers for spawning enemies and power up
        int kamikazeTimer;
        int healthPowerUpTimer;
        int shootingEnemySpawnTimer;

        //game mode
        const byte StartScreen = 0, Game = 1, Win = 2, Lose = 3; // used as gameMode values
        int gameMode = StartScreen;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //timers used to handle how often things spawn
            healthPowerUpTimer = 0;
            kamikazeTimer = 0;
            shootingEnemySpawnTimer = 0;

            player1 = new Player();//player object
            turret1 = new PlayerTurret();//turret object
            playerBullet1 = new PlayerBullet(turret1.Pos.X, turret1.Pos.Y, player1.PlayerPos);//player bullet object

            //background texture. have to load it here or it dosent work
            background = Content.Load<Texture2D>("Background/field2");

            camera1 = new Camera(GraphicsDevice.Viewport, background);//camera object
           
            //array of shooting enemies
            shootingEnemyArray = new ShootingEnemy[maxShootingEnemy];

            for (int i = 0; i < maxShootingEnemy; i++)
            {
              shootingEnemyArray[i] = new ShootingEnemy(rand);
            }

            kamikaze1 = new KamikazeTank();//kamikaze object

            powerUp1 = new RepairPowerUp(rand);//repair power up object

            miniMap1 = new MiniMap();//mini map object

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //player body and hud
            player1.LoadContent(Content);
            
            //player turret
            turret1.LoadContent(Content);

            //players bullet
            playerBullet1.LoadContent(Content);

            //shooting enemy
            for (int i = 0; i < maxShootingEnemy; i++)
            {
                shootingEnemyArray[i].LoadContent(Content);
            }
            //kamikaze
            kamikaze1.LoadContent(Content);
            //font
            Font1 = Content.Load<SpriteFont>("Font1");
            //power up repair
            powerUp1.LoadContent(Content);
            //mini map
            miniMap1.LoadContent(Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //set up keystate variable
            KeyboardState kS = Keyboard.GetState();
            switch (gameMode)
            {
                    //start up screen with instructions
                case StartScreen:
                    if (kS.IsKeyDown(Keys.Enter))
                        gameMode = Game;
                    // Allows the game to exit
                    if (kS.IsKeyDown(Keys.Escape))
                        this.Exit();
                    break;
                    //game
                case Game:
                    // Allows the game to exit
                    if (kS.IsKeyDown(Keys.Escape))
                        this.Exit();

                    //timers for spawning enemies and powerUp
                    //for tracking the spawn timer for the shooting enemy
                    shootingEnemySpawnTimer++;
                    if (shootingEnemySpawnTimer == 100)
                        shootingEnemySpawnTimer = 0;
                    //for tracking the spawn rate of the healthpowerup
                    if (powerUp1.Active == false)
                        healthPowerUpTimer++;
                    if (healthPowerUpTimer == 400)
                    {
                        powerUp1.Active = true;
                        healthPowerUpTimer = 0;
                        powerUp1.Reset(rand);
                    }
                    //for tracking the spawn rate of the kamikaze
                    kamikazeTimer++;
                    if (kamikazeTimer == 300 && kamikaze1.Alive == false)
                    {
                        kamikaze1.Alive = true;
                        kamikazeTimer = 0;
                    }
                    else if (kamikazeTimer == 300 && kamikaze1.Alive == false)
                        kamikazeTimer = 0;

                    //if the game is running and a shooting enemy is dead. respawn it. stop playing the explosion and reset its bullet
                    if (player1.ScoreValue <= 500)
                    {
                        if (shootingEnemySpawnTimer >= 60)
                        {
                            for (int i = 0; i < maxShootingEnemy; i++)
                            {
                                if (shootingEnemyArray[i].Alive == false)
                                {
                                    shootingEnemyArray[i].Respawn(rand); 
                                    shootingEnemyArray[i].PlayExplosion = false;
                                    shootingEnemyArray[i].ResetBullet();
                                }

                            }
                        }
                    }
                    //call each objects update method
                    player1.Update(kS);
                    turret1.Update(kS);
                    playerBullet1.Update(kS, turret1.Rotation, player1.PlayerPos, player1.Rotation);
                    camera1.Update(kS, player1.PlayerPos);
                    kamikaze1.Update(player1.Rotation, player1.PlayerPos);
                    //for miniMap
                    miniMap1.Update(player1.PlayerPos, kamikaze1.Pos, powerUp1.RepairPos, shootingEnemyArray);
                    powerUp1.Update();
                    for (int i = 0; i < maxShootingEnemy; i++)
                    {
                        shootingEnemyArray[i].Update(player1.PlayerPos);
                        //boundary detect for shooting enemies bullets(when it is no longer visible to the player)
                        shootingEnemyArray[i].DetectBoundaryForTheBullet(player1.PlayerPos);
                    }

                    //calls the collision detections
                    CollisionDetections();

                    //win
                    if (player1.ScoreValue >= 500)
                        gameMode = Win;

                    //lose
                    if (player1.NoOfLives <= 0 && player1.HealthValue <= 0)
                        gameMode = Lose;

                    break;

                    //win and lose states use the same code so make a reset method and call that instead
                case Win:
                    ResartGame(kS);
                    break;

                case Lose:
                    ResartGame(kS);
                    break;

                // TODO: Add your update logic here
            }
            base.Update(gameTime);
        }

        //used to restart the game when it ends
        public void ResartGame(KeyboardState kS)
        {
            if (kS.IsKeyDown(Keys.Enter))
            {
                for (int i = 0; i < maxShootingEnemy; i++)
                {
                    shootingEnemyArray[i].Respawn(rand);
                    shootingEnemyArray[i].ResetBullet();
                }
                kamikazeTimer = 0;
                kamikaze1.Alive = false;
                player1.HealthValue = 100;
                player1.NoOfLives = 3;
                player1.ScoreValue = 0;
                gameMode = Game;
                playerBullet1.Alive = false;
            }
            else if (kS.IsKeyDown(Keys.Escape))
                this.Exit();
        }

        //this method is used to call the per pixel detection method for the different objects
        public void CollisionDetections()
        {
            //check collision between players bullet and enemy tank
            if (playerBullet1.Alive == true)
            {
                for (int i = 0; i < maxShootingEnemy; i++)
                {
                    if (PerPixelDetection(playerBullet1.BulletRectangle, playerBullet1.BulletTextureData, shootingEnemyArray[i].EnemyRectangle, shootingEnemyArray[i].EnemyTextureData) && shootingEnemyArray[i].Alive == true)
                    {
                        if (shootingEnemyArray[i].Health <= 20)
                        {
                            player1.ScoreValue += 50;
                                shootingEnemyArray[i].Alive = false;
                                shootingEnemyArray[i].PlayExplosion = true;
                        }
                        else shootingEnemyArray[i].Health -= 20;

                        playerBullet1.Alive = false;
                            playerBullet1.Reset(player1.PlayerPos);

                    }
                }
            }

            //check collision between kamikaze and player
            if (kamikaze1.Alive == true)
            {
                if (PerPixelDetection(player1.PlayerRectangle, player1.PlayerTextureData, kamikaze1.KamikazeRectangle, kamikaze1.KamikazeTextureData))
                {
                    kamikazeTimer = 0;
                    kamikaze1.Alive = false;
                    kamikaze1.PlayExplosion = true;
                    player1.HealthValue -= 50;
                    if (kamikaze1.PlayExplosion == false)
                        kamikaze1.Respawn(rand);
                }
            }

            //check collision between kamikaze and player bullet
            if (playerBullet1.Alive == true && kamikaze1.Alive == true)
            {
                if (PerPixelDetection(playerBullet1.BulletRectangle, playerBullet1.BulletTextureData, kamikaze1.KamikazeRectangle, kamikaze1.KamikazeTextureData))
                {
                    kamikazeTimer = 0;
                    kamikaze1.Alive = false;
                    kamikaze1.PlayExplosion = true;
                    playerBullet1.Alive = false;
                    player1.ScoreValue += 100;
                    if (kamikaze1.PlayExplosion == false)
                        kamikaze1.Respawn(rand);
                }
            }

            //check collision with player and repair powerUp
            if (PerPixelDetection(player1.PlayerRectangle, player1.PlayerTextureData, powerUp1.RepairRectangle, powerUp1.RepairTextureData) && powerUp1.Active == true)
            {
                powerUp1.Active = false;
                player1.HealthValue += 50;
            }

            //check collision between player and enemies bullet
            for (int i = 0; i < maxShootingEnemy; i++)
            {
                if (PerPixelDetection(shootingEnemyArray[i].EnemyBulletRectangle, shootingEnemyArray[i].EnemyBulletTextureData, player1.PlayerRectangle, player1.PlayerTextureData) && shootingEnemyArray[i].BulletAlive == true)
                {
                    shootingEnemyArray[i].BulletAlive = false;
                    shootingEnemyArray[i].Shooting = false;
                    shootingEnemyArray[i].ResetBullet();
                    player1.HealthValue -= 20;
                }
            }

            //check collision between player and shooting enemy
            for (int i = 0; i < maxShootingEnemy; i++)
            {
                if (PerPixelDetection(shootingEnemyArray[i].EnemyRectangle, shootingEnemyArray[i].EnemyTextureData, player1.PlayerRectangle, player1.PlayerTextureData) && shootingEnemyArray[i].Alive == true)
                {
                    shootingEnemyArray[i].Alive = false;
                    player1.HealthValue -= 10;
                    player1.ScoreValue += 10;
                }
            }
        }

        //per pixel collision detection method. reused for all collisions
        static bool PerPixelDetection(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB)
        {
            //find bounds of rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            //check for every point
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {//get colour of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) + (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) + (y - rectangleB.Top) * rectangleB.Width];
                    //if both pixels are not completely transparent
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        //then an intersection has been found
                        return true;
                    }
                }

            }
            //no intersection
            return false;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            // TODO: Add your drawing code here
            switch (gameMode)
            {
                    //start screen has the instructions drawon it with strings positioned with matrices
                case StartScreen:
                    string start1 = "Use the arrow keys to move your tank";
                    string start2 = "Use 'z' and 'x' to rotate your turret";
                    string start3 = "Use space to shoot";
                    string start4 = "Score 500 to win";
                    string start5 = "Press enter to start the game";

                    Vector2 startPos1 = new Vector2(20, 20);
                    Vector2 startPos2 = new Vector2(15, 40);
                    Vector2 startPos3 = new Vector2(120, 60);
                    Vector2 startPos4 = new Vector2(130, 80);
                    Vector2 startPos5 = new Vector2(130, 100);

                    Vector2 startOrigin1 = Font1.MeasureString(start1) / 2;
                    Vector2 startOrigin2 = Font1.MeasureString(start2) / 2;
                    Vector2 startOrigin3 = Font1.MeasureString(start3) / 2;
                    Vector2 startOrigin4 = Font1.MeasureString(start4) / 2;
                    Vector2 startOrigin5 = Font1.MeasureString(start5) / 2;

                    Matrix startTransform1 = Matrix.CreateTranslation(startPos1.X, startPos1.Y, 0);
                    Matrix startTransform2 = Matrix.CreateTranslation(startPos2.X, startPos2.Y, 0);
                    Matrix startTransform3 = Matrix.CreateTranslation(startPos3.X, startPos3.Y, 0);
                    Matrix startTransform4 = Matrix.CreateTranslation(startPos4.X, startPos4.Y, 0);
                    Matrix startTransform5 = Matrix.CreateTranslation(startPos5.X, startPos5.Y, 0);

                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, startTransform1);
                    spriteBatch.DrawString(Font1, start1, startOrigin1, Color.White, 0, startOrigin4, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, startTransform2);
                    spriteBatch.DrawString(Font1, start2, startOrigin2, Color.White, 0, startOrigin4, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, startTransform3);
                    spriteBatch.DrawString(Font1, start3, startOrigin3, Color.White, 0, startOrigin4, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, startTransform4);
                    spriteBatch.DrawString(Font1, start4, startOrigin4, Color.White, 0, startOrigin4, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, startTransform5);
                    spriteBatch.DrawString(Font1, start5, startOrigin5, Color.White, 0, startOrigin5, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();
                    break;

                case Game:

                    //terrain draw and matrix set up
                    Vector2 origin = new Vector2(background.Width / 2, background.Height / 2);
                    terrainTransfrom = Matrix.CreateTranslation(-background.Width / 2, -background.Height / 2, 0) * camera1.CameraTransform;

                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, terrainTransfrom);
                    spriteBatch.Draw(background, new Vector2(750,500), null, Color.White, 0, origin, 1, SpriteEffects.None, 0);
                    spriteBatch.End();

                    //call the player body and hud draw method
                    player1.Draw(spriteBatch, camera1.CameraTransform);//, miniMapTransform);

                    //call the player turrets draw method
                    turret1.Draw(spriteBatch, player1.BodyTransform, player1.NoOfLives);

                    //call the player bullets draw method
                    if (playerBullet1.Alive == true)
                        playerBullet1.Draw(spriteBatch, turret1.TurretTransform, camera1.CameraTransform, turret1.Rotation, player1.Rotation);

                    //method for drawing shooting enemy
                    for (int i = 0; i < maxShootingEnemy; i++)
                    {
                        shootingEnemyArray[i].Draw(spriteBatch, camera1.CameraTransform);
                    }
                    //kamikaze draw
                    kamikaze1.Draw(spriteBatch, camera1.CameraTransform);
                    //repair power up draw
                    powerUp1.Draw(spriteBatch, camera1.CameraTransform);
                    //mini map draw
                    miniMap1.Draw(spriteBatch, kamikaze1.Alive, powerUp1.Active, shootingEnemyArray);
            break;

                case Win:
                    //has message drawn with strings positioned with matrices
                    GraphicsDevice.Clear(Color.Blue);

                    string win1 = "Congrats, you won";
                    string win2 = "Press enter to play again";
                    string win3 = "Or press escape to exit";

                    Vector2 win1Pos = new Vector2(200, 20);
                    Vector2 win2Pos = new Vector2(200, 40);
                    Vector2 win3Pos = new Vector2(200, 60);

                    Vector2 winOrigin1 = Font1.MeasureString(win1) / 2;
                    Vector2 winOrigin2 = Font1.MeasureString(win2) / 2;
                    Vector2 winOrigin3 = Font1.MeasureString(win3) / 2;

                    Matrix winTransform1 = Matrix.CreateTranslation(win1Pos.X, win1Pos.Y, 0);
                    Matrix winTransform2 = Matrix.CreateTranslation(win2Pos.X, win2Pos.Y, 0);
                    Matrix winTransform3 = Matrix.CreateTranslation(win3Pos.X, win3Pos.Y, 0);

                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, winTransform1);
                    spriteBatch.DrawString(Font1, win1, winOrigin1, Color.White, 0, winOrigin1, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, winTransform2);
                    spriteBatch.DrawString(Font1, win2, winOrigin2, Color.White, 0, winOrigin2, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();

                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, winTransform3);
                    spriteBatch.DrawString(Font1, win3, winOrigin3, Color.White, 0, winOrigin3, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();
                break;

                case Lose:
                //has message drawn with strings positioned with matrices
                    GraphicsDevice.Clear(Color.MidnightBlue);

                    string lose1 = "Sorry you weren't good enough this time";
                    string lose2 = "Do you want to try again? (press enter)";
                    string lose3 = "Or do you want to quit? (press escape)";

                    Vector2 lose1Pos = new Vector2(200, 20);
                    Vector2 lose2Pos = new Vector2(200, 40);
                    Vector2 lose3Pos = new Vector2(200, 60);

                    Vector2 loseOrigin1 = Font1.MeasureString(lose1) / 2;
                    Vector2 loseOrigin2 = Font1.MeasureString(lose2) / 2;
                    Vector2 loseOrigin3 = Font1.MeasureString(lose3) / 2;

                    Matrix loseTransform1 = Matrix.CreateTranslation(lose1Pos.X, lose1Pos.Y, 0);
                    Matrix loseTransform2 = Matrix.CreateTranslation(lose2Pos.X, lose2Pos.Y, 0);
                    Matrix loseTransform3 = Matrix.CreateTranslation(lose3Pos.X, lose3Pos.Y, 0);

                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, loseTransform1);
                    spriteBatch.DrawString(Font1, lose1, loseOrigin1, Color.White, 0, loseOrigin1, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();

                    
                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, loseTransform2);
                    spriteBatch.DrawString(Font1, lose2, loseOrigin2, Color.White, 0, loseOrigin2, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();

                    
                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, loseTransform3);
                    spriteBatch.DrawString(Font1, lose3, loseOrigin3, Color.White, 0, loseOrigin3, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.End();
                break;

        }
            base.Draw(gameTime);
        }
    }
}
