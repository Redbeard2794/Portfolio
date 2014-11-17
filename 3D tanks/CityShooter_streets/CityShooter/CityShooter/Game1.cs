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

namespace CityShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont Font1;
        
        //array of strings. box chars are streets. numbers = buildings(num floors)
        String[] map ={  
                    "╬═╦═╦════╗",
                    "║1║3║1111║",
                    "║2║4╠════╣",
                    "╠═╩═╣7777║",
                    "║555║7777║",
                    "╠═╦═╬════╣",
                    "║3║9║1111║",
                    "║3╚═╬═╗66║",
                    "║343╠═╝99║",
                    "╚═══╩════╝"          
        };

        String[] npcMap ={
                    "E00000000E",
                    "0000000000",
                    "0000000000",
                    "0000000000",
                    "0000000000",
                    "0000000000",
                    "0000000000",
                    "0000000000",
                    "0000000000",
                    "E00000000E"  
        };

        
        List<Street> streets;
        Rectangle blockSize;

        List<Building> buildings;
        Rectangle buildingSize;

        List<Roof> rooves;
        Rectangle roofSize;

        List<GroundEnemy> enemies;
        Rectangle boxSize;
        

        

        Camera camera;

        Player player1;
        Matrix view;
        Matrix proj;
        CollisionManager collisionManager;
        List<Rocket> playerRockets;
        List<Rocket> enemyRockets;
        int reloadTimeEnemy = 60;


        int reloadTime;


        ParticleEmitter testEmitter;
        Texture2D hud;
        int hudTimer = 0;
        Color col = Color.DarkOrange;

        //game state
        const byte PLAY = 0, WIN = 1, LOSE = 2;
        int state = PLAY;

        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Services.AddService(typeof(ContentManager), Content);
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
            blockSize = new Rectangle(0, 0, 15, 15);
            buildingSize = new Rectangle(0, 0, 15, 15);
            roofSize = new Rectangle(0, 0, 15, 15);
            boxSize = new Rectangle(0, 0, 15, 15);

            streets = new List<Street>();

            StreetFactory.Init(this, blockSize);
            
            for (int z = 0; z < map.Length; z++)
            {
                for (int x = 0; x < map[z].Length; x++)
                {
                    char c = map[z][x];
                    if (StreetFactory.streetSymbols.Contains(c))//if it contains char from street factory
                    {
                        Street s = StreetFactory.makeStreet(map[z][x], new Vector2(x, z));//char, position
                        streets.Add(s);
                    }
                }
            }

            buildings = new List<Building>();
            BuildingFactory.Init(this, buildingSize);
            for (int z = 0; z < map.Length; z++)
            {
                for (int x = 0; x < map[z].Length; x++)
                {
                    char c = map[z][x];
                    if (BuildingFactory.buildingSymbols.Contains(c))
                    {
                        Building b = BuildingFactory.makeBuilding(map[z][x], new Vector2(x, z));
                        buildings.Add(b);
                    }
                }
            }
            rooves = new List<Roof>();
            RoofFactory.Init(this, roofSize);
            for (int z = 0; z < map.Length; z++)
            {
                for (int x = 0; x < map.Length; x++)
                {
                    char c = map[z][x];
                    if (RoofFactory.buildingSymbols.Contains(c))
                    {
                        Roof r = RoofFactory.makeBuilding(map[z][x], new Vector2(x, z));
                        rooves.Add(r);
                    }
                }
            }

            enemies = new List<GroundEnemy>();
            npcManager.Init(this, boxSize);
            for (int z = 0; z < npcMap.Length; z++)
            {
                for (int x = 0; x < npcMap.Length; x++)
                {
                    char c = npcMap[z][x];
                    if (npcManager.npcSymbol.Contains(c))
                    {
                        GroundEnemy e = npcManager.makeGroundEnemy(npcMap[z][x], new Vector2(x, z));
                        enemies.Add(e);
                    }
                }
            }

            

            player1 = new Player();

            playerRockets = new List<Rocket>();
            enemyRockets = new List<Rocket>();

            camera = new Camera();
            camera.Init(new Vector3(0, 10, 0), new Vector3(50, 10, 50), Vector3.Up,0.6f,graphics.GraphicsDevice.Viewport.AspectRatio,1,1000);

            collisionManager = new CollisionManager();

            reloadTime = 0;




            testEmitter = new ParticleEmitter(player1.Pos);//new Vector3(player1.World.M41, player1.World.M42, player1.World.M43));
            testEmitter.SpawnParticles = true;


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
            Window.Title = "DISCO TANK";

            hud = Content.Load<Texture2D>("HUD");
            Font1 = Content.Load<SpriteFont>("Font1");
            player1.LoadContent(Content);
            
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState ks = Keyboard.GetState();
            // TODO: Add your update logic here
            switch (state)
            {
                case PLAY:
             camera.Update(gameTime, new Vector3(player1.World.M41, player1.World.M42, player1.World.M43), player1.Direction, player1.TurretDirection, player1.TurretTransform);


            //to exit
            if (ks.IsKeyDown(Keys.Escape))
                this.Exit();
            //for the player to fire rockets
            if (ks.IsKeyDown(Keys.Space) && reloadTime == 0)
            {
                playerRockets.Add(new Rocket(new Vector3(player1.World.M41, player1.World.M42,player1.World.M43), Content, player1.TotalTurretRotation, player1.Rotation));
                
                reloadTime+= 50;
            }
                    //to stop the player spamming rockets
            if(ks.IsKeyUp(Keys.Space))
            {
                if (reloadTime > 0)
                    reloadTime--;
            }
            //update the players rockets
            for (int i = 0; i < playerRockets.Count; i++)
            {
                playerRockets[i].Update(ks);
            }
            //update the enemies rockets
            for (int i = 0; i < enemyRockets.Count; i++)
            {
                enemyRockets[i].Update(ks);
            }

            //update the camera
            camera.updateCameraState(new Vector3(player1.World.M41, player1.World.M42, player1.World.M43), new Vector3(player1.Direction.X, player1.Direction.Y, player1.Direction.Z), player1.ForwardVector, player1.TurretDirection, player1.FullTurretRotation, player1.TankRotation);
            //update the player
            player1.Update(ks);

            foreach (GroundEnemy g in enemies)
            {
                g.Update(gameTime);
            }
            //method that handles all collisions
            HandleCollisions(gameTime, ks);
            hudTimer++;

            if (hudTimer > 0 && hudTimer < 20)
            {
                col = Color.LimeGreen;
            }
            else if (hudTimer > 20 && hudTimer < 40)
            {
                col = Color.WhiteSmoke;
            }
            else if (hudTimer > 40 && hudTimer < 60)
            {
                col = Color.DarkOrange;
            }
            else if (hudTimer > 60 && hudTimer < 80)
            {
                hudTimer = 0;
            }


            EnemyShooting();
            //testEmitter.Update(new Vector3(player1.World.M41, player1.World.M42, player1.World.M43), Content, graphics);
            testEmitter.Update(player1.Pos, Content, GraphicsDevice);
            if (enemies.Count == 0)
                state = WIN;

                    break;

                case WIN:
                camera.Update(gameTime, new Vector3(player1.World.M41, player1.World.M42, player1.World.M43), player1.Direction, player1.TurretDirection, player1.TurretTransform);

                if (ks.IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                }
                hudTimer++;

                if (hudTimer > 0 && hudTimer < 20)
                {
                    col = Color.GhostWhite;
                }
                else if (hudTimer > 20 && hudTimer < 40)
                {
                    col = Color.LimeGreen;
                }
                else if (hudTimer > 40 && hudTimer < 60)
                {
                    col = Color.Cyan;
                }
                else if (hudTimer > 60 && hudTimer < 80)
                {
                    hudTimer = 0;
                }
                    break;

                case LOSE:
                camera.Update(gameTime, new Vector3(player1.World.M41, player1.World.M42, player1.World.M43), player1.Direction, player1.TurretDirection, player1.TurretTransform);

                if (ks.IsKeyDown(Keys.Escape))
                {
                    this.Exit();
                }
                hudTimer++;

                if (hudTimer > 0 && hudTimer < 20)
                {
                    col = Color.GhostWhite;
                }
                else if (hudTimer > 20 && hudTimer < 40)
                {
                    col = Color.DarkRed;
                }
                else if (hudTimer > 40 && hudTimer < 60)
                {
                    col = Color.Crimson;
                }
                else if (hudTimer > 60 && hudTimer < 80)
                {
                    hudTimer = 0;
                }
                    break;
            }

            


            base.Update(gameTime);
        }
        //method that makes the enemy turns its turret and shoot
        public void EnemyShooting()
        {
            foreach (GroundEnemy e in enemies)
            {
                e.Shoot(new Vector3(player1.World.M41, player1.World.M42,player1.World.M43));
                if (e.Speed == 0)
                {
                    reloadTimeEnemy--;
                    Vector3 vectToPlayer = new Vector3(player1.World.M41 - e.World.M41, player1.World.M42 - e.World.M42, player1.World.M43 - e.World.M43);
                    float amountToRotate;
                    if(e.Rotation == 1.5f || e.Rotation == -1.5f)
                        amountToRotate = Vector3.Dot((e.TurretDirection * Vector3.UnitX), vectToPlayer);
                    else amountToRotate = Vector3.Dot((e.TurretDirection * -Vector3.UnitX), vectToPlayer);
                    if (e.TurretRotValue < amountToRotate)
                    {
                        e.TurretRotValue = -amountToRotate;
                    }
                    //if (amountToRotate > 0)
                    //amountToRotate = amountToRotate / 1.5f;
                    //if (amountToRotate <= 0 && reloadTimeEnemy == 0)
                    if (reloadTimeEnemy == 0)
                    {
                        reloadTimeEnemy = 60;
                        //subtract or add based on enemies rotation???
                        if(e.Rotation == 1.5f || e.Rotation == -1.5f)
                            enemyRockets.Add(new Rocket(new Vector3(e.World.M41, e.World.M42, e.World.M43), Content, (e.TurretRotValue - 1.7f), e.Rotation));
                        else if(e.Rotation == 0 || e.Rotation == 3.14f)
                            enemyRockets.Add(new Rocket(new Vector3(e.World.M41, e.World.M42, e.World.M43), Content, (e.TurretRotValue + 1.7f), e.Rotation));

                        break;
                    }
                }
                else e.TurretRotValue = 0;
            }

        }
        //handles the collisions
        public void HandleCollisions(GameTime gameTime, KeyboardState ks)
        {
            //between player and buildings
            for (int i = 0; i < buildings.Count; i++)
            {
                if (collisionManager.ModelBuildingCollision(player1.Tank, player1.World, buildings[i].Position, 15, buildings[i].BuildingHeight) == true)
                {
                    player1.HitBuilding = true;
                }
                
            }
            //between players rockets and buildings
            for (int i = 0; i < buildings.Count; i++)
            {
                for (int j = 0; j < playerRockets.Count; j++)
                {
                    if (collisionManager.ModelBuildingCollision(playerRockets[j].RocketModel, playerRockets[j].World, buildings[i].Position, 15, buildings[i].BuildingHeight) == true)
                    {
                        playerRockets.RemoveAt(j);
                    }
                }
            }
            //between enemy rockets and buidlings
            for (int i = 0; i < buildings.Count; i++)
            {
                for (int j = 0; j < enemyRockets.Count; j++)
                {
                    if (collisionManager.ModelBuildingCollision(enemyRockets[j].RocketModel, enemyRockets[j].World, buildings[i].Position, 15, buildings[i].BuildingHeight) == true)
                    {
                        enemyRockets.RemoveAt(j);
                    }
                }
            }
            //between enemy rockets and player
            for (int i = 0; i < enemyRockets.Count; i++)
            {
                if (collisionManager.ModelCollision(enemyRockets[i].RocketModel, enemyRockets[i].World, player1.Tank, player1.World) == true)
                {
                    enemyRockets.RemoveAt(i);
                    if (player1.Health != 20)
                    {
                        player1.Health -= 20;
                    }
                    else if (player1.Health == 20 && player1.Lives != 0)
                    {
                        player1.Lives -= 1;
                        player1.Health = 100;
                        player1.Pos = new Vector3(67, 0, 82);
                    }
                    else state = LOSE;
                }
            }
            //between player rockets and enemy
            for (int i = 0; i < enemies.Count; i++)
            {
                for (int j = 0; j < playerRockets.Count; j++)
                {
                    if (collisionManager.ModelCollision(playerRockets[j].RocketModel, playerRockets[j].World, enemies[i].Model, enemies[i].World) == true)
                    {
                        
                        if (enemies[i].Health != 20)
                        {
                            enemies[i].Health -= 20;
                        }
                        else if (enemies[i].Health == 20)
                        {
                            enemies.RemoveAt(i);
                            player1.Score += 50;
                            player1.EnemiesKilled += 1;
                        }
                        playerRockets.RemoveAt(j);
                    }
                }
            }
            //between player and enemy
            for (int i = 0; i < enemies.Count; i++)
            {
                if (collisionManager.ModelCollision(player1.Tank, player1.World, enemies[i].Model, enemies[i].World) == true)
                {
                    if(enemies[i].Rotation == 0)
                        player1.Pos = new Vector3(player1.Pos.X, player1.Pos.Y, player1.Pos.Z - 5);
                    else if (enemies[i].Rotation == 3.14f)
                        player1.Pos = new Vector3(player1.Pos.X, player1.Pos.Y, player1.Pos.Z + 5);
                    else if (enemies[i].Rotation == 1.5f)
                        player1.Pos = new Vector3(player1.Pos.X - 5, player1.Pos.Y, player1.Pos.Z);
                    else if (enemies[i].Rotation == -1.5f)
                        player1.Pos = new Vector3(player1.Pos.X + 5, player1.Pos.Y, player1.Pos.Z);
                    //player1.Health -= 20;
                    //enemies[i].Health -= 20;
                }
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.MidnightBlue);

            GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp; // need to do this on reach devices to allow non 2^n textures
            RasterizerState rs = RasterizerState.CullNone;
            
            GraphicsDevice.RasterizerState = rs;
            // TODO: Add your drawing code here

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (Street s in streets)
            {
                s.Draw(gameTime, camera);
            }
            foreach (Building b in buildings)
            {
                b.Draw(gameTime, camera);
            }
            foreach (Roof r in rooves)
            {
                r.Draw(gameTime, camera);
            }
            for (int i = 0; i < playerRockets.Count; i++)
            {
                playerRockets[i].Draw(spriteBatch, graphics, camera.View, camera.Projection, player1.TotalTurretRotation, player1.Rotation);
            }

            foreach (GroundEnemy e in enemies)
            {
                e.Draw(gameTime, camera);
                for (int i = 0; i < enemyRockets.Count; i++)
                {
                    enemyRockets[i].Draw(spriteBatch, graphics, camera.View, camera.Projection, e.TurretRotValue, e.Rotation);
                }
            }


            player1.Draw(spriteBatch, graphics, camera.View, camera.Projection);

            testEmitter.Draw(gameTime, camera, GraphicsDevice);

            spriteBatch.Begin();
            if (camera.CamState != 0)
            {
                spriteBatch.Draw(hud, new Rectangle(0, 401, hud.Width + 206, hud.Height), Color.White);
                spriteBatch.DrawString(Font1, "Lives: " + player1.Lives, new Vector2(20, 410), col);
                spriteBatch.DrawString(Font1, "Health: " + player1.Health + "%", new Vector2(20, 440), col);
                spriteBatch.DrawString(Font1, "Score: " + player1.Score, new Vector2(600, 410), col);
                spriteBatch.DrawString(Font1, "Enemies killed: " + player1.EnemiesKilled, new Vector2(600, 440), col);
                if(state == PLAY)
                    spriteBatch.DrawString(Font1, "Disco Tank", new Vector2(340, 425), col);
                else if (state == WIN)
                {
                    spriteBatch.DrawString(Font1, "You Won!!!", new Vector2(340, 425), col);
                }
                else if (state == LOSE)
                {
                    spriteBatch.DrawString(Font1, "You Lose.....", new Vector2(340, 425), col);
                }
            }
            else if (camera.CamState == 0)
            {
                spriteBatch.Draw(hud, new Rectangle(0, 0, 200, 150), Color.White);
                spriteBatch.DrawString(Font1, "Lives: " + player1.Lives, new Vector2(0, 40), col);
                spriteBatch.DrawString(Font1, "Health: " + player1.Health + "%", new Vector2(0, 60), col);
                spriteBatch.DrawString(Font1, "Score: " + player1.Score, new Vector2(0, 80), col);
                spriteBatch.DrawString(Font1, "Enemies killed: " + player1.EnemiesKilled, new Vector2(0, 100), col);
                if (state == PLAY)
                    spriteBatch.DrawString(Font1, "Disco Tank", new Vector2(0, 20), col);

                else if (state == WIN)
                {
                    spriteBatch.DrawString(Font1, "You Won!!!", new Vector2(0, 20), col);
                }
                else if (state == LOSE)
                {
                    spriteBatch.DrawString(Font1, "You Lose.....", new Vector2(0, 20), col);
                }
            }
            spriteBatch.End();
            testEmitter.Draw(gameTime, camera, GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}
