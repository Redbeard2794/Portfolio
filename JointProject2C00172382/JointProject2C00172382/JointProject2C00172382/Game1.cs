//Joint project 2
//Name: Jason Murphy
//I.D C00172382
//Description of task
//We were basically meant to make an xna version of robotron. We were required to do:
//•	Use class inheritance 
//•	Make use of arrays, for example use an array of bullets or enemies etc.
//•	Read the players’ name from the screen.
//•	Save a high score table  to a text file or array of something else like an array of enemies
//•	Provide the player with game instructions.
//•	Start a new game when current game is over.
//•	A high score table showing the scores of the games sorted in descending order. 
//*have collision detection.
//* have sprites face the direction they are moving in
//fire bullets with an array
//•	entities that move in a straight line, then change direction.
//•	entities move up/down/left/right by themselves towards another entity
//•	entities die when hit by player weapon 
//•	Use an array for each entity of the same type.
//•	show lives left 
//•	show score
//have rules and balance

//description of my project
//in my project the player can move left,right,up and down.
//they can shoot with space(bullets are in the array
//all sprites are animated(i.e change as they move)
//arrays of bike enemies, turrets, and patrol enemy
//3 levels that get harder
//playermust collide with the hostage to move to the next level
// i have a simple menu. you can make a new game, load a game, see instructions and a leaderboard
//the game is loaded from a textfile
//the bike enemy follows the player, the patrol moves back and forth and the turret satys still
//the patrol and turret shoot when the player is near them
//the player has three live and a certain amount of health
//the player can enter a name at the start
//all the players info is shown on the hus in game
//i added sounds for shooting, a start up sound, and a death sound


//known bugs
//players bullet does not move right or down consistently. I have absoutely no idea what is causing this as it was working a few days ago with the 
//exact same code.
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
using System.IO;

namespace JointProject2C00172382
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D playerLeft1Texture, playerLeft2Texture, playerLeft3Texture, playerLeft4Texture, playerRight1Texture, playerRight2Texture, playerRight3Texture, playerRight4Texture, playerFront1Texture, playerFront2Texture, playerFront3Texture, playerBulletTexture, enemTurretLeftTexture, playerBack1Texture, playerBack2Texture, playerBack3Texture, turretBulletTexture, bikeLeft1Texture, bikeRight1Texture, bikeWheelieTexture, bikeUpTexture, bikeDownTexture, bikeWheelieRightTexture, menuIconTexture, backButtonTexture, hudTexture, backgroundTexture, background2Texture, background3Texture, soldierR1Texture, soldierR2Texture, soldierR3Texture, soldierR4Texture, soldierR5Texture, soldierR6Texture, soldierL1Texture, soldierL2Texture, soldierL3Texture, soldierL4Texture, soldierL5Texture, soldierL6Texture, enemyBulletTexture, hostage1Texture, hostage2Texture, hostage3Texture, hostage4Texture, hostage5Texture, hostage6Texture, hostage1BackTexture, hostage2BackTexture, hostage3BackTexture, hostage4BackTexture, hostage5BackTexture, hostage6BackTexture, gameOverTexture, winTexture;
        SpriteFont font;
        SoundEffect deathSound, playerShot, soldierShot, intro;
        SoundEffectInstance deathSoundInstance, playerShotInstance, soldierShotInstance, introInstance;
        //player object
        Player playerOne;
        //turret object array
        static int maxTurrets = 2;
        Turret[] theTurrets = new Turret[maxTurrets];
        //bike object array
        static int maxBikes = 2;
        BikeEnemy[] theBikes = new BikeEnemy[maxBikes];
        //player bullet object array
        static int maxBullets = 3;
        PlayerBullet[] myBullets = new PlayerBullet[maxBullets];
        //patrol enemy object array
        static int maxPatrol = 4;
        PatrolEnemy[] thePatrol = new PatrolEnemy[maxPatrol];
        //hostage object
        Hostage hostage1;
        Random randNum = new Random();//random generator
        KeyboardState currentKeyboardState, previousKeyBoardState;//stores previous and current keyboard state
        const byte None = 0, North = 1, East = 2, South = 3, West = 4;//orientation for playermovement
        int playerOrientation;// = None;//stores player current orientation
        int bulletOrientation = None;//orientation of bullet
        //game modes
        const byte Menu = 0, Instructions = 1, Game = 2, HighScore = 3, EnterName = 4, GameOver = 5, Win = 6;
        int gameMode = EnterName;//sets game mode to enter name at start
        //level
        int level = 1;
        //game state, play,save,load
        const byte gamePlay = 0, fileHandling = 1, fileLoading = 2;
        int gameState = gamePlay;
        //file for save
        string currentFile = "Save.txt";
        //file for leaderboard
        string leaderFile = "LeaderBoard.Txt";
        string line;
        string aMessage;
        //array for keys
        Keys[] oldKeys = new Keys[0];
        //players name
        public string playersName;
        int enterCount = 0;//amount of times enter is hit
        string l1;//letter 1
        string l2;//letter 2
        string l3;//letter 3
        string fullName;
        //mouse state
        MouseState mouseState, previousMouseState;
        //timers for the colours to change in the menu
        int colourTimer1 = 0;
        int colourTimer2 = 0;
        bool score = false;
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
            //makes mouse visible on the screen(maybe only in certain game modes)
            this.IsMouseVisible = true;
            base.Initialize();
            //bulletOrientation = playerOrientation;
           // maxTurrets = 2;
        }
            
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("fonts/SpriteFont1");
            //sound effects
            deathSound = Content.Load<SoundEffect>("sounds/pacman_death");
            deathSoundInstance = deathSound.CreateInstance();
            playerShot = Content.Load<SoundEffect>("sounds/bullet");
            playerShotInstance = playerShot.CreateInstance();
            soldierShot = Content.Load<SoundEffect>("sounds/soldierBullet");
            soldierShotInstance = soldierShot.CreateInstance();
            intro = Content.Load<SoundEffect>("pacman_beginning");
            introInstance = intro.CreateInstance();

            //textures for the player
            playerLeft1Texture = Content.Load<Texture2D>("playerSprites/playerLeft1");
            playerLeft2Texture = Content.Load<Texture2D>("playerSprites/playerLeft2");
            playerLeft3Texture = Content.Load<Texture2D>("playerSprites/playerLeft3");
            playerLeft4Texture = Content.Load<Texture2D>("playerSprites/playerLeft4");
            playerRight1Texture = Content.Load<Texture2D>("playerSprites/playerRight1");
            playerRight2Texture = Content.Load<Texture2D>("playerSprites/playerRight2");
            playerRight3Texture = Content.Load<Texture2D>("playerSprites/playerRight3");
            playerRight4Texture = Content.Load<Texture2D>("playerSprites/playerRight4");
            playerFront1Texture = Content.Load<Texture2D>("playerSprites/playerFront1");
            playerFront2Texture = Content.Load<Texture2D>("playerSprites/playerFront2");
            playerFront3Texture = Content.Load<Texture2D>("playerSprites/playerFront3");
            playerBack1Texture = Content.Load<Texture2D>("playerSprites/playerBack1");
            playerBack2Texture = Content.Load<Texture2D>("playerSprites/playerBack2");
            playerBack3Texture = Content.Load<Texture2D>("playerSprites/playerBack3");
            playerBulletTexture = Content.Load<Texture2D>("playerSprites/playerBullet");
            for (int i = 0; i < 3; i++)
            {
                myBullets[i] = new PlayerBullet(playerBulletTexture, playerOrientation);
            }
            //passes these to player class
            playerOne = new Player(playerLeft1Texture, playerLeft2Texture, playerLeft3Texture, playerLeft4Texture, playerRight1Texture, playerRight2Texture, playerRight3Texture, playerRight4Texture, playerFront1Texture, playerFront2Texture, playerFront3Texture, playerBulletTexture, playerBack1Texture, playerBack2Texture, playerBack3Texture);
            //turret textures
            enemTurretLeftTexture = Content.Load<Texture2D>("turretSprites/enemTurretLeft");
            turretBulletTexture = Content.Load<Texture2D>("turretSprites/turretBullet");
            //passes textures to the turret class for each object
            for (int i = 0; i < maxTurrets; i++)
            {
                theTurrets[i] = new Turret(enemTurretLeftTexture, turretBulletTexture);
            }

            //textures for bike enemy
            bikeLeft1Texture = Content.Load<Texture2D>("bikeEnemySprites/bikeLeft1");
            bikeRight1Texture = Content.Load<Texture2D>("bikeEnemySprites/bikeRight1");
            bikeWheelieTexture = Content.Load<Texture2D>("bikeEnemySprites/bikeWheelie");
            bikeWheelieRightTexture = Content.Load<Texture2D>("bikeEnemySprites/bikeWheelieRight");
            bikeUpTexture = Content.Load<Texture2D>("bikeFront");
            bikeDownTexture = Content.Load<Texture2D>("bikeBack");
            for (int i = 0; i < 2; i++)
            {
                theBikes[i] = new BikeEnemy(bikeLeft1Texture, bikeRight1Texture, bikeWheelieTexture, bikeWheelieRightTexture, bikeUpTexture, bikeDownTexture);
            }

            //menu and hud
            menuIconTexture = Content.Load<Texture2D>("menuSprites/menuIcon");
            backButtonTexture = Content.Load<Texture2D>("menuSprites/backButton");
            hudTexture = Content.Load<Texture2D>("hudAndBackgroundSprites/hud2");
            backgroundTexture = Content.Load<Texture2D>("hudAndBackgroundSprites/background1");
            background2Texture = Content.Load<Texture2D>("background2");
            background3Texture = Content.Load<Texture2D>("background3");
            winTexture = Content.Load<Texture2D>("winner-win");

            //patrol enemy
            soldierR1Texture = Content.Load<Texture2D>("soldierSprites/soldier");
            soldierR2Texture = Content.Load<Texture2D>("soldierSprites/soldier1");
            soldierR3Texture = Content.Load<Texture2D>("soldierSprites/soldier2");
            soldierR4Texture = Content.Load<Texture2D>("soldierSprites/soldier3");
            soldierR5Texture = Content.Load<Texture2D>("soldierSprites/soldier4");
            soldierR6Texture = Content.Load<Texture2D>("soldierSprites/soldier5");
            soldierL1Texture = Content.Load<Texture2D>("soldierSprites/soldier left");
            soldierL2Texture = Content.Load<Texture2D>("soldierSprites/soldier1 left");
            soldierL3Texture = Content.Load<Texture2D>("soldierSprites/soldier2 left");
            soldierL4Texture = Content.Load<Texture2D>("soldierSprites/soldier3 left");
            soldierL5Texture = Content.Load<Texture2D>("soldierSprites/soldier4 left");
            soldierL6Texture = Content.Load<Texture2D>("soldierSprites/soldier5 left");
            enemyBulletTexture = Content.Load<Texture2D>("soldierSprites/enemyBullet");
            for (int i = 0; i < maxPatrol; i++)
            {
                thePatrol[i] = new PatrolEnemy(soldierR1Texture, soldierR2Texture, soldierR3Texture, soldierR4Texture, soldierR5Texture, soldierR6Texture, soldierL1Texture, soldierL2Texture, soldierL3Texture, soldierL4Texture, soldierL5Texture, soldierL6Texture, enemyBulletTexture);
            }
            
            //hostage
            hostage1Texture = Content.Load<Texture2D>("hostageSprites/hostage1");
            hostage2Texture = Content.Load<Texture2D>("hostageSprites/hostage2");
            hostage3Texture = Content.Load<Texture2D>("hostageSprites/hostage3");
            hostage4Texture = Content.Load<Texture2D>("hostageSprites/hostage4");
            hostage5Texture = Content.Load<Texture2D>("hostageSprites/hostage5");
            hostage6Texture = Content.Load<Texture2D>("hostageSprites/hostage6");
            hostage1BackTexture = Content.Load<Texture2D>("hostageSprites/hostage1 back");
            hostage2BackTexture = Content.Load<Texture2D>("hostageSprites/hostage2 back");
            hostage3BackTexture = Content.Load<Texture2D>("hostageSprites/hostage3 back");
            hostage4BackTexture = Content.Load<Texture2D>("hostageSprites/hostage4 back");
            hostage5BackTexture = Content.Load<Texture2D>("hostageSprites/hostage5 back");
            hostage6BackTexture = Content.Load<Texture2D>("hostageSprites/hostage6 back");
            hostage1 = new Hostage(hostage1Texture, hostage2Texture, hostage3Texture, hostage4Texture, hostage5Texture, hostage6Texture, hostage1BackTexture, hostage2BackTexture, hostage3BackTexture, hostage4BackTexture, hostage5BackTexture, hostage6BackTexture);

            gameOverTexture = Content.Load<Texture2D>("hudAndBackgroundSprites/gameOver");
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
            // TODO: Add your update logic here
            switch (gameMode)
            {
                case EnterName:
                    //calls method for enter name
                    EnterNameOfPlayer();
                    //timers for changing colour
                    colourTimer1++;
                    if (colourTimer1 == 50)
                    {
                        colourTimer2++;
                        colourTimer1 = 0;
                    }
                    break;
                case Menu:
                    previousMouseState = mouseState;
                    mouseState = Mouse.GetState();
                    //method for checking mouse input
                    CheckMouseInput();
                    colourTimer1++;
                    if (colourTimer1 == 50)
                    {
                        colourTimer2++;
                        colourTimer1 = 0;
                    }
                    break;
                case Instructions:
                    previousMouseState = mouseState;
                    mouseState = Mouse.GetState();
                    CheckMouseInput();
                    colourTimer1++;
                    if (colourTimer1 == 50)
                    {
                        colourTimer2++;
                        colourTimer1 = 0;
                    }
                    break;
                case HighScore:
                    previousMouseState = mouseState;
                    mouseState = Mouse.GetState();
                    CheckMouseInput();
                    //LoadLeaderBoard();
                    //LoadFromFile();
                    break;
                case Game:
                    //hostage move method
                    hostage1.Move();
                    //patrol enemy shoot method
                    for (int i = 0; i < maxPatrol; i++)
                    {
                        thePatrol[i].Shoot((int)playerOne.playerPos.X, (int)playerOne.playerPos.Y, soldierShot);
                    }
                    //method for tracking players health
                    PlayerHealthTracker();
                    //for moving players bullets
                    for (int index = 0; index < 3; index++)
                    {
                        myBullets[index].Move((int)playerOne.playerPos.X, (int)playerOne.playerPos.Y);//, bulletOrientation);
                    }
                    //controls player bullets movement
                    ControlOfBullets();
                    //method that controls all the collisions
                    Collisions();
                    //calls method for patrol enemy to move
                    for (int i = 0; i < maxPatrol; i++)
                    {
                        thePatrol[i].Move();
                    }
                    //gamestate method
                    GameState();
                    //status = started;
                    for (int i = 0; i < 2; i++)
                    {
                        theBikes[i].Move((int)playerOne.playerPos.X, (int)playerOne.playerPos.Y);
                        theBikes[i].Wheelie((int)playerOne.playerPos.X, (int)playerOne.playerPos.Y);
                    }
                    //calls method for turrets to shoot
                    for (int i = 0; i < maxTurrets; i++)
                    {
                        theTurrets[i].Shoot((int)playerOne.playerPos.X, (int)playerOne.playerPos.Y);
                    }
                    //calls method that checks keyboard input
                    KeyboardInput();
                    //method for moving players bullets
                    for (int index = 0; index < 3; index++)
                    {
                        //myBullets[index].realOrientation = playerOrientation;
                        //myBullets[index].bulletPosition.X = playerOne.playerPos.X;
                        //myBullets[index].bulletPosition.Y = playerOne.playerPos.Y;
                        myBullets[index].Move((int)playerOne.playerPos.X, (int)playerOne.playerPos.Y);//, bulletOrientation);
                    }
                    break;
                case GameOver:
                    previousMouseState = mouseState;
                    mouseState = Mouse.GetState();
                    CheckMouseInput();
                    //SaveLeaderBoard();
                    break;
                case Win:
                    previousMouseState = mouseState;
                    mouseState = Mouse.GetState();
                    if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                    {
                        gameMode = Menu;
                        level = 1;
                        playerOne.Lives = 3;
                        playerOne.Score = 0;
                        playerOne.HostagesSaved = 0;
                    }
                    break;
            }
            base.Update(gameTime);
        }
        //method for detecting player and turret collision
        public void PlayerTurretCollision()
        {
            for (int i = 0; i < maxTurrets; i++)
            {
                playerOne.CollisionTurret((int)theTurrets[i].turretPos.X, (int)theTurrets[i].turretPos.Y, enemTurretLeftTexture.Width, enemTurretLeftTexture.Height);
            }
        }
        //this method controls all the collisions in the game
        public void Collisions()
        {
            //for collision between players bullets and turrrets
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < maxTurrets; j++)
                {
                    if (myBullets[i].Collision((int)theTurrets[j].turretPos.X, (int)theTurrets[j].turretPos.Y, enemTurretLeftTexture.Width, enemTurretLeftTexture.Height) == true)
                    {
                        score = true;
                        ScoreTracker();
                        score = false;
                        theTurrets[j].alive1 = false;
                        myBullets[i].bulletAlive = false;
                    }
                }
            }
            //collision between player and turrets
            for (int i = 0; i < maxTurrets; i++)
            {
                if (theTurrets[i].alive1 == true)
                    PlayerTurretCollision();
            }
            //collision between player and hostage that changes the level
            if (hostage1.Collision((int)playerOne.playerPos.X, (int)playerOne.playerPos.Y, playerRight1Texture.Width, playerRight1Texture.Height) == true)
            {
                //levelComplete = true;
                LevelTracker();
                level += 1;
                playerOne.Score += 50;
                playerOne.playerPos.X = 30;
                for (int i = 0; i < maxBikes; i++)
                {
                    theBikes[i].alive = true;
                }
                for (int i = 0; i < maxTurrets; i++)
                {
                    theTurrets[i].alive1 = true;
                }
                for (int i = 0; i < maxPatrol; i++)
                {
                    thePatrol[i].alive = true;
                }
                if (level == 4)
                {
                    gameMode = Win;
                }
            }
            //collision between turret bullets and player
            for (int i = 0; i < maxTurrets; i++)
            {
                if (theTurrets[i].Bullet1CollisionWithPlayer((int)playerOne.playerPos.X, (int)playerOne.playerPos.Y, playerRight1Texture.Width, playerRight1Texture.Height) == true)
                {
                    playerOne.Health -= 10;
                    theTurrets[i].bullet1Alive = false;
                    theTurrets[i].turretBulletPos.X = theTurrets[i].turretPos.X;
                }
            }
            //collision between patrol enemy and the player
            for (int i = 0; i < maxPatrol; i++)
            {
                if (thePatrol[i].CollisionWithPlayer((int)playerOne.playerPos.X, (int)playerOne.playerPos.Y, playerRight1Texture.Width, playerRight1Texture.Height) == true)
                {
                    playerOne.Health -= 10;
                    playerOne.playerPos.X = 40;
                }
            }
            //collision between player bullets and bike enemies
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < maxBikes; j++)
                {
                    if (myBullets[i].CollisionWithBike((int)theBikes[j].bikePos.X, (int)theBikes[j].bikePos.Y, bikeRight1Texture.Width, bikeRight1Texture.Height) == true)
                    {
                        score = true;
                        ScoreTracker();
                        score = false;
                        myBullets[i].bulletPosition.X = playerOne.playerPos.X;
                        myBullets[i].bulletPosition.Y = playerOne.playerPos.Y;
                        theBikes[j].alive = false;
                        myBullets[i].bulletAlive = false;
                    }
                }
            }
            //collision between players bullets and patrol enemies
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < maxPatrol; j++)
                {
                    if (myBullets[i].CollisionWithPatrol((int)thePatrol[j].bikePos.X, (int)thePatrol[j].bikePos.Y, soldierL1Texture.Width, soldierL1Texture.Height) == true)
                    {
                        score = true;
                        ScoreTracker();
                        score = false;
                        myBullets[i].bulletAlive = false;
                        myBullets[i].bulletPosition.X = playerOne.playerPos.X;
                        myBullets[i].bulletPosition.Y = playerOne.playerPos.Y;
                        thePatrol[j].alive = false;
                    }
                }
            }
            //collision between bike enemies and player
            for (int i = 0; i < maxBikes; i++)
            {
                if (theBikes[i].CollisionWithPlayer((int)playerOne.playerPos.X, (int)playerOne.playerPos.Y, playerRight1Texture.Width, playerRight1Texture.Height) == true)
                {
                    playerOne.Health -= 50;
                    //play sound here
                    theBikes[i].alive = false;
                }
            }
            //collision between player and patrol enemies bullets
            for (int i = 0; i < maxPatrol; i++)
            {
                if (thePatrol[i].CollisionBetweenPlayerAndPatrolBullet((int)playerOne.playerPos.X, (int)playerOne.playerPos.Y, playerRight1Texture.Width, playerRight1Texture.Height) == true)
                {
                    playerOne.Health -= 10;
                    thePatrol[i].BulletAlive = false;
                    thePatrol[i].CanShoot = false;
                    thePatrol[i].bulletPos.X = thePatrol[i].bikePos.X;
                }
            }
        }
        public void ScoreTracker()
        {
            if (score == true)
            {
                playerOne.Score = playerOne.Score + 10;
                score = false;
            }
        }
        //used to detect input from the keyboard. This method is called in the update method
        //used for the movement of the player and for the player to shoot
        public void KeyboardInput()
        {
            previousKeyBoardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            //moves player left
            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                playerOrientation = East;
                playerOne.Move(playerOrientation);
            }
            //moves player right
            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                playerOrientation = West;
                playerOne.Move(playerOrientation);
            }
            //moves player down
            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                playerOrientation = South;
                playerOne.Move(playerOrientation);
            }
            //moves player up
            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                playerOrientation = North;
                playerOne.Move(playerOrientation);
            }
            //exits the game
            if (currentKeyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            //saves the game
            if (currentKeyboardState.IsKeyDown(Keys.Z))
            {
                gameState = fileHandling;
            }
        }
        //this method controls the players bullets. if space is pressed, makes bullet alive true, gives it its direction, calls its move method, plays sound
        public void ControlOfBullets()
        {
                KeyboardState keyState = Keyboard.GetState();
                 //the keys that are currently pressed
                Keys[] pressedKeys;
                pressedKeys = currentKeyboardState.GetPressedKeys();

                // work through each key that is presently pressed
                for (int i = 0; i < pressedKeys.Length; i++)
                {
                    // set a flag to indicate we have not found the key
                    bool foundIt = false;

                    // work through each key in the old keys
                    for (int j = 0; j < oldKeys.Length; j++)
                    {
                        if (pressedKeys[i] == oldKeys[j])
                        {
                            // we found the key in the old keys
                            foundIt = true;
                        }
                    }
                    if (foundIt == false)
                    {
                        if (keyState.IsKeyDown(Keys.Space))
                        {
                            for (int index = 0; index < 3; index++)
                            {
                                if (myBullets[index].bulletAlive == false)
                                {
                                    myBullets[index].bulletAlive = true;
                                    myBullets[index].realOrientation = playerOrientation;
                                    myBullets[index].bulletPosition.X = playerOne.playerPos.X;
                                    myBullets[index].bulletPosition.Y = playerOne.playerPos.Y;
                                    myBullets[index].Move((int)playerOne.playerPos.X, (int)playerOne.playerPos.Y);//, bulletOrientation);
                                    playerShotInstance.Play();
                                    break;
                                }
                            }
                        }
                    }
                    oldKeys = pressedKeys;  
                }
                
        }
        //checks where a mouse click occurs on the menu screen, depending on where the mouse is you will be brought to a different screen
        public void CheckMouseInput()
        {
            switch (gameMode)
            {
                case Menu:
                    //new game
                    if (mouseState.X >= 300 && mouseState.X <= 450 && mouseState.Y >= 0 && mouseState.Y <= 50 && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                    {
                        gameMode = Game;
                        for (int i = 0; i < maxBikes; i++)
                        {
                            theBikes[i].alive = true;
                            theBikes[i].bikePos.X = randNum.Next(300, 400);
                            theBikes[i].bikePos.Y = randNum.Next(100, 300);
                        }
                        for (int i = 0; i < maxPatrol; i++)
                        {
                            thePatrol[i].alive = true;
                            thePatrol[i].bikePos.Y = randNum.Next(100, 300);
                        }
                        for (int i = 0; i < maxTurrets; i++)
                        {
                            theTurrets[i].alive1 = true;
                            theTurrets[i].turretPos.X = randNum.Next(400, 500);
                            theTurrets[i].turretPos.Y = randNum.Next(100, 350);
                        }
                    }
                    //load game
                    if (mouseState.X >= 300 && mouseState.X <= 450 && mouseState.Y >= 100 && mouseState.Y <= 150 && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                    {
                        LoadFromFile();
                    }
                    //instructions
                    if (mouseState.X >= 300 && mouseState.X <= 450 && mouseState.Y >= 200 && mouseState.Y <= 250 && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                        gameMode = Instructions;
                    //highscore
                    if (mouseState.X >= 300 && mouseState.X <= 450 && mouseState.Y >= 300 && mouseState.Y <= 350 && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                    {
                        //LoadFromFile();
                        gameMode = HighScore;
                    }
                    //exit
                    if (mouseState.X >= 300 && mouseState.X <= 450 && mouseState.Y >= 400 && mouseState.Y <= 450 && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                        this.Exit();
                    break;
                case Instructions:
                    //back
                    if (mouseState.X >= 300 && mouseState.X <= 450 && mouseState.Y >= 400 && mouseState.Y <= 450 && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                        gameMode = Menu;
                    break;
                case HighScore:
                    //back
                    if (mouseState.X >= 300 && mouseState.X <= 450 && mouseState.Y >= 400 && mouseState.Y <= 450 && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                        gameMode = Menu;
                    break;
                case GameOver:
                    //brings you back to the menu
                    if (mouseState.X >= 200 && mouseState.X <= 445 && mouseState.Y >= 400 && mouseState.Y <= 450 && mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                    {
                        gameMode = Menu;
                        level = 1;
                        playerOne.Lives = 3;
                        playerOne.Health = 100;
                        playerOne.playerPos.X = 40;
                        playerOne.playerPos.Y = 40;
                    }
                    break;
            }
        }
        //handles whether the game is being saved or loaded
        public void GameState()
        {
            if (gameState == fileHandling)
            {
                SaveToFile();
                gameState = gamePlay;
            }
            if (gameState == fileLoading)
            {
                LoadFromFile();
                gameState = gamePlay;
            }
        }
        //saves the game
        public void SaveToFile()
        {
            try
            {
                StreamWriter outputStream = File.CreateText(currentFile);
                playerOne.WriteToTextFile(outputStream);
                outputStream.WriteLine();
                for (int i = 0; i < maxBikes; i++)
                {
                    theBikes[i].WriteToTextFile(outputStream);
                    outputStream.WriteLine();
                }
                for (int i = 0; i < maxTurrets; i++)
                {
                    theTurrets[i].WriteToTextFile(outputStream);
                    outputStream.WriteLine();
                }
                for (int i = 0; i < maxPatrol; i++)
                {
                    thePatrol[i].WriteToTextFile(outputStream);
                    outputStream.WriteLine();
                }
                outputStream.Close();
            }
            catch (FileNotFoundException problem)
            {
                aMessage = "File not found: " + currentFile + " \n" + problem.Message;
            }
            catch (IOException anException)
            {
                aMessage = "Error occured when trying to write to the file Save.txt";
            }
        }
        //loads the game
        public void LoadFromFile()
        {
            try
            {
                StreamReader inputStream = File.OpenText(currentFile);
                line = inputStream.ReadLine();
                playerOne.LoadFromTextFile(line);
                
                for (int index = 0; index < maxBikes; index++)
                {
                    line = inputStream.ReadLine(); // reads up to the carriage return
                    if (line != null)
                        theBikes[index].LoadFromTextFile(line);
                }
                for (int i = 0; i < maxTurrets; i++)
                {
                    line = inputStream.ReadLine(); // reads up to the carriage return
                    if (line != null)
                        theTurrets[i].LoadFromTextFile(line);
                }
                for (int i = 0; i < maxPatrol; i++)
                {
                    line = inputStream.ReadLine();
                    if (line != null)
                        thePatrol[i].LoadFromTextFile(line);
                }
                inputStream.Close();
                gameMode = Game;
            }
            catch (FileNotFoundException problem)
            {
                aMessage = "File not found: " + currentFile + " \n" + problem.Message;
            }
        }
        //attempt at saving the leaderboard
        //public void SaveLeaderBoard()
        //{
        //    try
        //    {
        //        StreamWriter outputStream = File.CreateText(leaderFile);
        //        playerOne.WriteToLeaderBoardTextFile(outputStream);
        //        outputStream.WriteLine();
        //        outputStream.Close();
        //    }
        //    catch (FileNotFoundException problem)
        //    {
        //        aMessage = "File not found: " + currentFile + " \n" + problem.Message;
        //    }
        //    catch (IOException anException)
        //    {
        //        aMessage = "Error occured when trying to write to the file Save.txt";
        //    }
        //}
        //attempt at loading the leaderboard
        //public void LoadLeaderBoard()
        //{
        //    try
        //    {
        //        StreamReader inputStream = File.OpenText(leaderFile);
        //        line = inputStream.ReadLine();
        //        playerOne.LoadFromLeaderBoardTextFile(line);
        //        inputStream.Close();
        //    }
        //    catch (FileNotFoundException problem)
        //    {
        //        aMessage = "File not found: " + currentFile + " \n" + problem.Message;
        //    }
        //}
        //lets the player enter a 3 letter name, like an arcade game
        public void EnterNameOfPlayer()
        {
            KeyboardState keyState = Keyboard.GetState();
            // the keys that are currently pressed
            Keys[] pressedKeys;
            pressedKeys = keyState.GetPressedKeys();

            // work through each key that is presently pressed
            for (int i = 0; i < pressedKeys.Length; i++)
            {
                // set a flag to indicate we have not found the key
                bool foundIt = false;

                // work through each key in the old keys
                for (int j = 0; j < oldKeys.Length; j++)
                {
                    if (pressedKeys[i] == oldKeys[j])
                    {
                        // we found the key in the old keys
                        foundIt = true;
                    }
                }
                if (foundIt == false)
                {
                    switch (pressedKeys[i])
                    {
                        case Keys.A:
                            playersName = "A";
                            break;
                        case Keys.B:
                            playersName = "B";
                            break;
                        case Keys.C:
                            playersName = "C";
                            break;
                        case Keys.D:
                            playersName = "D";
                            break;
                        case Keys.E:
                            playersName = "E";
                            break;
                        case Keys.F:
                            playersName = "F";
                            break;
                        case Keys.G:
                            playersName = "G";
                            break;
                        case Keys.H:
                            playersName = "H";
                            break;
                        case Keys.I:
                            playersName = "I";
                            break;
                        case Keys.J:
                            playersName = "J";
                            break;
                        case Keys.K:
                            playersName = "K";
                            break;
                        case Keys.L:
                            playersName = "L";
                            break;
                        case Keys.M:
                            playersName = "M";
                            break;
                        case Keys.N:
                            playersName = "N";
                            break;
                        case Keys.O:
                            playersName = "O";
                            break;
                        case Keys.P:
                            playersName = "P";
                            break;
                        case Keys.Q:
                            playersName = "Q";
                            break;
                        case Keys.R:
                            playersName = "R";
                            break;
                        case Keys.S:
                            playersName = "S";
                            break;
                        case Keys.T:
                            playersName = "T";
                            break;
                        case Keys.U:
                            playersName = "U";
                            break;
                        case Keys.V:
                            playersName = "V";
                            break;
                        case Keys.W:
                            playersName = "W";
                            break;
                        case Keys.X:
                            playersName = "X";
                            break;
                        case Keys.Y:
                            playersName = "Y";
                            break;
                        case Keys.Z:
                            playersName = "Z";
                            break;
                    }
                    
                    if (keyState.IsKeyDown(Keys.Enter))
                    {
                        enterCount++;
                        //first letter
                        if (enterCount == 1)
                            l1 = playersName;
                        //second letter
                        if (enterCount == 2)
                            l2 = playersName;
                        //third letter
                        if (enterCount == 3)
                        {
                            l3 = playersName;
                            fullName = l1 + l2 + l3;
                            //combines the 3 letters
                            playerOne.name = fullName;
                            gameMode = Menu;
                            //plays sound
                            introInstance.Play();
                        }
                    }
                }
            }
            oldKeys = pressedKeys;
        }
        //keeps track of players health
        public void PlayerHealthTracker()
        {
            int newLivesLeft;
            if (playerOne.Health <= 0)
            {
                newLivesLeft = playerOne.Lives -= 1;
                playerOne.Lives = newLivesLeft;
                //plays sound
                deathSoundInstance.Play();
                playerOne.Health = 100;
                playerOne.playerPos.X = 40;
                playerOne.playerPos.Y = 40;
            }
            if (playerOne.Lives == 0)
            {
                //SaveToFile();
                gameMode = GameOver;
            }
        }
        //keeps track of the level. when level increases enemies are set back to alive. in level 2 bike enemy speed is increased
        //in level 3 enemy bullet speed is increased
        public void LevelTracker()
        {
            if (level == 2)
            {
                
                for (int i = 0; i < maxBikes; i++)
                {
                    if (theBikes[i].alive == false)
                        theBikes[i].alive = true;
                    theBikes[i].speed = 3;
                }
                for (int i = 0; i < maxPatrol; i++)
                {
                    if (thePatrol[i].alive == false)
                        thePatrol[i].alive = true;
                }
                for (int i = 0;i<maxTurrets;i++)
                {
                    if (theTurrets[i].alive1 == false)
                        theTurrets[i].alive1 = true;
                }
                playerOne.HostagesSaved = 1;
            }
            if (level == 3)
            {
                for (int i = 0; i < maxBikes; i++)
                {
                    if (theBikes[i].alive == false)
                        theBikes[i].alive = true;
                    theBikes[i].speed = 2;
                }
                for (int i = 0; i < maxPatrol; i++)
                {
                    if (thePatrol[i].alive == false)
                        thePatrol[i].alive = true;
                    thePatrol[i].bulletSpeed = 6;
                }
                for (int i = 0; i < maxTurrets; i++)
                {
                    if (theTurrets[i].alive1 == false)
                        theTurrets[i].alive1 = true;
                    thePatrol[i].bulletSpeed = 6;
                }
                playerOne.HostagesSaved = 2;
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //changes colour of the screen in the menu and where the player enters their name
            if (gameMode == Menu ||gameMode == EnterName)
            {
                if (colourTimer2 <=1)
                    GraphicsDevice.Clear(Color.Cyan);
                else if (colourTimer2 <= 2)
                    GraphicsDevice.Clear(Color.Violet);
                else if (colourTimer2<=3)
                    GraphicsDevice.Clear(Color.Gold);
                else if (colourTimer2 <= 4)
                    GraphicsDevice.Clear(Color.Indigo);
                else if (colourTimer2<=5)
                    colourTimer2 = 0;
            }
            //changes colour in the instructions screen
            else if (gameMode == Instructions)
            {
                if (colourTimer2 <= 1)
                    GraphicsDevice.Clear(Color.Black);
                else if (colourTimer2 <= 2)
                    GraphicsDevice.Clear(Color.Red);
                else if (colourTimer2 <= 3)
                    GraphicsDevice.Clear(Color.LimeGreen);
                else if (colourTimer2 <= 4)
                    GraphicsDevice.Clear(Color.MidnightBlue);
                else if (colourTimer2 <= 5)
                    colourTimer2 = 0;
            }
            else GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            switch(gameMode)
            {
                    //draws messages and the letters being entered
                case EnterName:
                    spriteBatch.DrawString(font, "you can enter a name consisting of 3 letters like ", new Vector2(100, 40), Color.Black);
                    spriteBatch.DrawString(font, "old arcade machines", new Vector2(100, 60), Color.Black);
                    spriteBatch.DrawString(font, "type a letter and press 'Enter' after each letter", new Vector2(100, 80), Color.Black);
                    spriteBatch.DrawString(font, "please enter your name : " + "(" + playersName + ")", new Vector2(100, 100), Color.Black);
                    spriteBatch.DrawString(font, "" + l1 + "" + l2 + "" + l3, new Vector2(420, 100), Color.Black);
                    break;
                    //draws the menu buttons and their text
                case Menu:
                    spriteBatch.DrawString(font, "Hello  " + fullName, new Vector2(10, 10), Color.Black);
                    spriteBatch.Draw(menuIconTexture, new Rectangle(300,0,menuIconTexture.Width, menuIconTexture.Height), Color.White);
                    spriteBatch.DrawString(font, "New Game", new Vector2(315, 15), Color.White);
                    spriteBatch.Draw(menuIconTexture, new Rectangle(300, 100, menuIconTexture.Width, menuIconTexture.Height), Color.White);
                    spriteBatch.DrawString(font,"Load Game", new Vector2(315, 115), Color.White);
                    spriteBatch.Draw(menuIconTexture, new Rectangle(300, 200, menuIconTexture.Width, menuIconTexture.Height), Color.White);
                    spriteBatch.DrawString(font, "Instructions", new Vector2(315, 215), Color.White);
                    spriteBatch.Draw(menuIconTexture, new Rectangle(300, 300, menuIconTexture.Width, menuIconTexture.Height), Color.White);
                    spriteBatch.DrawString(font, "Highscore", new Vector2(315, 315), Color.White);
                    spriteBatch.Draw(backButtonTexture, new Rectangle(300, 400, backButtonTexture.Width, backButtonTexture.Height), Color.White);
                    spriteBatch.DrawString(font, "Exit", new Vector2(315, 415), Color.DodgerBlue);
                    break;
                    //draaws the instructions for how to play the game
                case Instructions:
                    spriteBatch.DrawString(font, "1) Press 'W' to go up", new Vector2(250, 10), Color.Snow);
                    spriteBatch.DrawString(font, "2) Press 'S' to go down", new Vector2(250, 30), Color.Snow);
                    spriteBatch.DrawString(font, "3) Press 'A' to go left", new Vector2(250, 50), Color.Snow);
                    spriteBatch.DrawString(font, "4) Press 'D' to go right", new Vector2(250, 70), Color.Snow);
                    spriteBatch.DrawString(font, "5) Press 'Space' to shoot", new Vector2(250, 90), Color.Snow);
                    spriteBatch.DrawString(font, "6) Press 'Z' to save the game", new Vector2(250, 110), Color.Snow);
                    spriteBatch.DrawString(font, "7) You will have 3 lives", new Vector2(250, 130), Color.Snow);
                    spriteBatch.DrawString(font, "8) If you run out of lives the game is over", new Vector2(250, 150), Color.Snow);
                    spriteBatch.DrawString(font, "9) Kill all enemies in each level", new Vector2(250, 170), Color.Snow);
                    spriteBatch.Draw(backButtonTexture, new Rectangle(300, 400, backButtonTexture.Width, backButtonTexture.Height), Color.White);
                    spriteBatch.DrawString(font, "Main menu", new Vector2(315, 415), Color.Red);
                    break;
                    //draws highscore table
                case HighScore:
                    //string name1 = Convert.ToString(playerOne.LeaderBoard[0]);
                    //if (playerOne.LeaderBoard[0] != null)
                        //spriteBatch.DrawString(font, playerOne.LeaderBoard[0], new Vector2(10, 10), Color.Pink);
                    spriteBatch.DrawString(font, "1....." + playerOne.name + "....." + playerOne.Score, new Vector2(10, 10), Color.Pink);
                    spriteBatch.Draw(backButtonTexture, new Rectangle(300, 400, backButtonTexture.Width, backButtonTexture.Height), Color.White);
                    spriteBatch.DrawString(font, "Main menu", new Vector2(315, 415), Color.Red);
                    break;
                    //draws game over screen 
                case GameOver:
                    spriteBatch.Draw(gameOverTexture, new Rectangle(0, 0, gameOverTexture.Width + 550, gameOverTexture.Height + 290), Color.White);
                    spriteBatch.Draw(menuIconTexture, new Rectangle(200, 400, menuIconTexture.Width + 300, menuIconTexture.Height), Color.Transparent);
                    break;
                    //
                case Game:
                    spriteBatch.DrawString(font, "" + playerOrientation, new Vector2(20, 20), Color.Blue);
                    //draws backgrounds for each level
                    if (level == 1)
                        spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, 800, 370), Color.White);
                    else if (level == 2)
                        spriteBatch.Draw(background2Texture, new Rectangle(0, 0, 800, 370), Color.White);
                    else if (level == 3)
                        spriteBatch.Draw(background3Texture, new Rectangle(0, 0, 800, 370), Color.White);
                    //passes spriteBatch and font to hostage draw method
                    hostage1.Draw(spriteBatch, font);
                    //draw current level
                    spriteBatch.DrawString(font, "" + level, new Vector2(10, 10), Color.White);
                    //draw hud
                    spriteBatch.Draw(hudTexture, new Rectangle(0, 375, hudTexture.Width + 383,hudTexture.Height), Color.White);
                    //passes spriteBatch and font to player draw method
                    playerOne.Draw(spriteBatch, playerOrientation, font);
                    //passes spriteBatch and font to turret draw method
                    for (int i = 0; i < maxTurrets; i++)
                    {
                        theTurrets[i].Draw(spriteBatch, font);
                    }
                    //passes spriteBatch and font to patrol method
                    for (int i = 0; i < maxPatrol; i++)
                    {
                        thePatrol[i].Draw(spriteBatch, font);
                    }
                    //passes spriteBatch and font to bike draw method
                    for (int i = 0; i < 2; i++)
                    {
                        theBikes[i].Draw(spriteBatch, font);
                    }
                    //passes spriteBatch to player bullet draw method
                    for (int i = 0; i < 3; i++)
                    {
                        myBullets[i].Draw(spriteBatch);
                    }
                    break;
                case Win:
                    spriteBatch.Draw(winTexture, new Rectangle(0, 0, winTexture.Width + 550, winTexture.Height + 290), Color.White);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
