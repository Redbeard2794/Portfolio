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

namespace zookeeper
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D animalsTexture, highLightTexture, elephantTexture, hourglassTexture1, hourglassTexture2, hourglassTexture3, hourglassTexture4, gameoverTexture, insertCoinTexture1, insertCoinTexture2, insertCoinTexture3, insertCoinTexture4, insertCoinTexture5, levelUpTexture;
        
        SoundEffect backgroundMusic, beepSound, mainMusic, beeping, levelPlus;
        SoundEffectInstance backgroundMusicInstance, beepSoundInstance, mainMusicInstance, beepingInstance, levelPlusInstance;

        MouseState mouseState, previousMouseState;
        SpriteFont font;
        Player playerScore;
        Animal[,] theBoard;
        const int  MaxRows = 8;
        const int MaxCols = 8;
        Random random = new Random();

        const byte SPLASH = 0, GAME = 1, PAUSE = 2, END = 3, Instructions = 4, levelUp = 5; // used as gameMode values
        const byte MONKEY = 0, HIPPO = 1, LION =2, PANDA = 3, ELEPHANT=4, GIRAFE=5,CROC=6,BUNNY=7; // used as animal type values
        const byte OK = 0, SLEEPY = 1, ANGRY = 2, SHOCK = 3; // used as animal mood values and offset in animal sprite sheet
        const byte NONE = 0, RIGHT = 1, DOWN = 2, LEFT = 3, UP = 4, FALLING = 5; // used to control animation for moving animals
        const int XOFFSET = 2, YOFFSET = 2, SPRITEOFFSET = 44, SPRITEWIDTH = 42, XMARGIN =6; // used to control the layout the grid and drawing of animals

        bool selected = false;

        int noOfAnimals = 6;
        int currentX=-2, currentY=-2; // -2 used so that next valid animal won't be adjacent and trigger swap
        int gameMode = SPLASH;          // used in switch in update and draw      
        int splashTiming = 0;   // used to control animation of elephant
        int lastX1, lastX2, lastY1, lastY2; // store last animals swapped in case of no match
        int swapAnimationTime = 0; // used to  control the swapping animation

        int timing = 0;
        string aMessage = "";
        string bMessage = "";
        KeyboardState currentKeyboardState, previousKeyBoardState;
        bool match = false;
        int hourglassCounter = 0;
        int coinTimer = 0;
        int coinTiming = 0;
        int levelTracker = 1;
        bool noMatch = false;//used to stop a move if there is no possible match
        

        public Game1()
        {            
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 360;
            graphics.PreferredBackBufferWidth = 512;
            Content.RootDirectory = "Content";
        }
        
 /// <summary>
 /// create instances of the main structures
 /// set the initial score, lives and time
 /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            theBoard = new Animal[MaxRows, MaxCols];
            playerScore = new Player();
            playerScore.Score = 0;
            playerScore.Timer = 100;
            playerScore.Hints = 3;            
            base.Initialize();
        }
        /// <summary>
        /// Load the resource used in the game
        /// fonts, textures (sprites) and sounds.
        /// populate the table a 8x8 array of animal
        /// using a random animal for each entry
        /// </summary>
        protected override void LoadContent()
        {            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("scorefont");

            animalsTexture = Content.Load<Texture2D>("ZooAnimals_strip42");
            highLightTexture = Content.Load<Texture2D>("highlight");
            elephantTexture = Content.Load<Texture2D>("elephantstage");
            hourglassTexture1 = Content.Load<Texture2D>("hourglass1");
            hourglassTexture2 = Content.Load<Texture2D>("hourglass2");
            hourglassTexture3 = Content.Load<Texture2D>("hourglass3");
            hourglassTexture4 = Content.Load<Texture2D>("hourglass4");
            gameoverTexture = Content.Load<Texture2D>("gameover");
            insertCoinTexture1 = Content.Load<Texture2D>("coin1");
            insertCoinTexture2 = Content.Load<Texture2D>("coin2");
            insertCoinTexture3 = Content.Load<Texture2D>("coin3");
            insertCoinTexture4 = Content.Load<Texture2D>("coin4");
            insertCoinTexture5 = Content.Load<Texture2D>("coin5");
            levelUpTexture = Content.Load<Texture2D>("levelup");
            
            backgroundMusic = Content.Load<SoundEffect>("wahwah");
            backgroundMusicInstance = backgroundMusic.CreateInstance();
            beeping = Content.Load<SoundEffect>("sfx_beep");
            beepingInstance = beeping.CreateInstance();
            levelPlus = Content.Load<SoundEffect>("Wave2");
            levelPlusInstance = levelPlus.CreateInstance();
            //mainMusic = Content.Load<SoundEffect>("mainMusic");
            //mainMusicInstance = mainMusic.CreateInstance();
            //mainMusicInstance.IsLooped = true;
            //mainMusicInstance.Play();

            beepSound = Content.Load<SoundEffect>("beep");
            beepSoundInstance = beepSound.CreateInstance();
            backgroundMusicInstance.IsLooped = true;
            backgroundMusicInstance.Play();

            for (int row = 0; row < MaxRows; row++)
            {
                for (int col = 0; col < MaxCols; col++)
                {
                    theBoard[row, col] = new Animal(animalsTexture,(byte) random.Next(noOfAnimals));
                }     // end inner for             
            } // end outer for      
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
/// <summary>
/// switch statment reflects current gamemode
/// splash mode waits for mouse click and increments splashTiming used to control elephant animation
/// 
/// game mode checks for mouse input to select animals for swapping
/// checks for back button to exit
/// </summary>
/// <param name="gameTime"></param>
                
        protected override void Update(GameTime gameTime)
        {
            switch(gameMode)
            {
                    //shows the instructions for the game
                case Instructions:
                    previousMouseState = mouseState;
                    mouseState = Mouse.GetState();
                    if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                    {
                        gameMode = GAME;
                    }

                    break;

                    //if game is running
                case GAME:
                    checkMouseInput();
                    if (swapAnimationTime > 0)
                    {
                        swapAnimationTime--;
                    }
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    {
                        this.Exit();
                    }
                    //calls the method to check if there is a match of 3
                    Match3();
                    //for keeping track of the timer in the game
                    timing++;
                    if (timing == 50)
                    {
                        playerScore.DecreaseTimer();
                        hourglassCounter++;
                        timing = 0;
                        if (hourglassCounter == 4)
                        {
                            hourglassCounter = 0;
                        }
                    }
                    //controls a message that tells the player that time is running out
                    if (playerScore.Timer == 15)
                        aMessage = "Time";
                    if (playerScore.Timer == 14)
                        aMessage = "is";
                    if (playerScore.Timer == 13)
                        aMessage = "running";
                    if (playerScore.Timer == 12)
                        aMessage = "out!!!";
                    if (playerScore.Timer == 11)
                        aMessage = "";
                    
                    if (playerScore.Timer == 0)
                    {
                        gameMode = END;
                        playerScore.Timer = 100;
                        playerScore.Score = 0;
                        beepSoundInstance.Play();
                    }
                    //calls the method to keep track of what level the player is on
                    LevelTracking();

                    //to pause the game
                    previousKeyBoardState = currentKeyboardState;
                    currentKeyboardState = Keyboard.GetState();
                    if (previousKeyBoardState.IsKeyDown(Keys.P) && currentKeyboardState.IsKeyUp(Keys.P))
                    {
                        gameMode = PAUSE;
                        beeping.Play();
                    }
                    if (previousKeyBoardState.IsKeyDown(Keys.H) && currentKeyboardState.IsKeyUp(Keys.H))
                    {
                        playerScore.Hints -= 1;
                        HorizontalHints();
                        VerticalHints();
                    }
                    break;

                    //when you complete a level
                case levelUp:
                    previousMouseState = mouseState;
                    mouseState = Mouse.GetState();
                    if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                    {
                        gameMode = GAME;
                        playerScore.Timer = 100;
                        playerScore.Score = 0;
                        playerScore.Hints = 3;
                        levelTracker = levelTracker + 1;
                        for (int row = 0; row < MaxRows; row++)
                        {
                            for (int col = 0; col < MaxCols; col++)
                            {
                                theBoard[row, col] = new Animal(animalsTexture, (byte)random.Next(noOfAnimals));
                            }     // end inner for             
                        } // end outer for      
                    }
                    break;

                    //if game is over
                case END:
                    previousMouseState = mouseState;
                    mouseState = Mouse.GetState();
                    
                    coinTiming++;
                    if (coinTiming == 30)
                    {
                        coinTimer++;
                        coinTiming = 0;
                    }
                    if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                    {
                        gameMode = SPLASH;
                        for (int row = 0; row < MaxRows; row++)
                        {
                            for (int col = 0; col < MaxCols; col++)
                            {
                                theBoard[row, col] = new Animal(animalsTexture, (byte)random.Next(noOfAnimals));
                            }     // end inner for             
                        } // end outer for      
                        levelTracker = 1;
                    }
                    break;

                    //if elephant animation is running
                case SPLASH:
                    splashTiming++;
                    previousMouseState = mouseState;
                    mouseState = Mouse.GetState();
                    if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                        gameMode = Instructions;

                    break;

                    //if game is paused
                case PAUSE:
                    previousKeyBoardState = currentKeyboardState;
                    currentKeyboardState = Keyboard.GetState();
                    int pauseTimer = playerScore.Timer;
                    int pauseScore = playerScore.Score;
                    int pauseHints = playerScore.Hints;
                    if (previousKeyBoardState.IsKeyDown(Keys.P) && currentKeyboardState.IsKeyUp(Keys.P))
                    {
                        beeping.Play();
                        gameMode = GAME;
                        playerScore.Timer = pauseTimer;
                        playerScore.Score = pauseScore;
                        playerScore.Hints = pauseHints;
                    }
                    break;


            }
            base.Update(gameTime);
        }
        /// <summary>
        /// not used yet to undo last swap if no matches occur
        /// play sound and swap using data stored during swap lastX1, lastY1, lastX2, lastY2
        /// </summary>
        private void reverse()//int rows, int cols, int rows1, int cols2)
        {
            beepSoundInstance.Play();
            swap(lastX1, lastY1, lastX2, lastY2);
        } 
     
        /// <summary>
        /// check once per click by making sure last time the button was up
        /// workout which animal was clicked on
        /// if re clicked on selected unselect it
        /// if clicked on adjacent animal to selected then swap
        /// else make current animal the selected
        /// 
        /// </summary>
        private void checkMouseInput()
        {
            int newX=-1, newY=-1;
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                newY = (mouseState.X - XOFFSET) / SPRITEOFFSET;
                newX = (mouseState.Y - YOFFSET) / SPRITEOFFSET;
                if (newX >= 0 && newX < 8)
                    if (newY >= 0 && newY < 8)
                    {
                        if (selected && newX == currentX && newY == currentY)
                        {
                            selected = false;
                            currentX = -2;
                            currentY = -2;
                        }
                        else
                        {
                            if ((Math.Abs(currentX - newX) == 1 && currentY == newY) || (Math.Abs(currentY - newY) == 1 && currentX == newX))
                            {
                                swap(currentX, currentY, newX, newY);
                                selected = false;
                                currentX = -2;
                                currentY = -2;// Make sure that next square clicked on is not adjacent to current  based on if statement above                             
                            }
                            else
                            {
                                if (selected)
                                {
                                    theBoard[currentX, currentY].Mood = OK;
                                }
                                currentX = newX;
                                currentY = newY;
                                theBoard[currentX, currentY].Mood = SLEEPY;
                                selected = true;
                            } // end else
                        } // end else
                    } // end if
            } // end if
        }
        /// <summary>
        /// store animal location in lastX! etc. variabes in case of reverse
        /// use temporary animal to perform swap
        /// change mood to OK (after select changed first animal)
        /// </summary>
        /// <param name="currentX"></param>
        /// <param name="currentY"></param>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        private void swap(int currentX, int currentY, int newX, int newY)
        {
            Animal tempAnimal;

            //used later on to reverse last swap if no match occours
            lastX1 = currentX;
            lastX2 = newX;
            lastY1 = currentY;
            lastY2 = newY;
            theBoard[newX, newY].Mood = OK;
            theBoard[currentX, currentY].Mood = OK;
            if (newX < currentX)
            {
                theBoard[newX, newY].Direction = UP;
                theBoard[currentX, currentY].Direction = DOWN;

            }
            if (newX > currentX)
            {
                theBoard[newX, newY].Direction = DOWN;
                theBoard[currentX, currentY].Direction = UP;
            }
            if (newY > currentY)
            {
                theBoard[newX, newY].Direction = LEFT;
                theBoard[currentX, currentY].Direction = RIGHT;
            }
            if (newY <  currentY)
            {
                theBoard[newX, newY].Direction = RIGHT;
                theBoard[currentX, currentY].Direction = LEFT;
            }
            tempAnimal = theBoard[currentX, currentY];
            theBoard[currentX, currentY].Displacement = 22; // 22 update ticks used to animate the swap movement.
            theBoard[currentX, currentY] = theBoard[newX, newY];
            theBoard[currentX, currentY].Displacement = 22;
            theBoard[newX, newY] = tempAnimal;            
        }
        private void Match3()
        {
            //when it selects an animal it checks the next 2 animals, hence the for loops are allowed to run 6 times as there are 8 animals in each row
            //checks horizontal matches
            for (int rows = 0; rows < MaxRows; rows++)
            {
                for (int cols = 0; cols < 6; cols++)
                {
                    if (theBoard[rows,cols].AnimalType == theBoard[rows,cols+1].AnimalType && theBoard[rows,cols].AnimalType == theBoard[rows,cols + 2].AnimalType)
                    {
                        playerScore.IncreaseScore();
                        match = true;
                        if (match == true)
                        {//in these gaps put code to change the tiles to something different
                            theBoard[rows, cols].AnimalType = (byte)random.Next(noOfAnimals); ;
                            RemoveTilesHorizontal(rows, cols);
                            theBoard[rows, cols + 1].AnimalType = (byte)random.Next(noOfAnimals);
                            RemoveTilesHorizontal(rows, cols + 1);
                            theBoard[rows, cols + 2].AnimalType = (byte)random.Next(noOfAnimals);
                            RemoveTilesHorizontal(rows, cols + 2);
                        }//end inner if
                    }//end outer if
                    else match = false;
                    if (match == false)
                    {
                        noMatch = true;
                        //reverse(rows,cols,rows,cols+1);
                    }
                }//end inner for
            }//end outer for
            //checks vertical matches
            for (int cols = 0; cols < MaxCols; cols++)
            {
                for (int rows = 0; rows < 6; rows++)
                {
                    if (theBoard[rows,cols].AnimalType == theBoard[rows+1,cols].AnimalType && theBoard[rows,cols].AnimalType == theBoard[rows+2,cols].AnimalType)
                    {
                        playerScore.IncreaseScore();
                        match = true;
                        if (match == true)
                        {//in these gaps put code to change the tiles to something different
                            theBoard[rows, cols].AnimalType = (byte)random.Next(noOfAnimals);
                            RemoveTilesVertical(rows, cols);
                            theBoard[rows + 1, cols].AnimalType = (byte)random.Next(noOfAnimals);
                            RemoveTilesVertical(rows + 1, cols);
                            theBoard[rows + 2, cols].AnimalType = (byte)random.Next(noOfAnimals);
                            RemoveTilesVertical(rows + 2, cols);
                        }//end inner if
                    }//end outer if
                    else match = false;
                    if (match == false)
                        noMatch = true;
                }//end inner for
            }//end outer for
        }
        //used to remove tiles on the horizontal
        private void RemoveTilesHorizontal(int row, int col)
        {
            for (int row1 = row; row > 0; row--)
            {
                swap(row, col, row - 1, col);
            }
        }
        //used to remove tiles on the vertical
        private void RemoveTilesVertical(int row, int col)
        {
            for (int row1 = row; row > 0; row--)
            {
                swap(row, col, row - 1, col);
            }
        }
        private void HorizontalHints()
        {
            //first possibilty = [rows,cols] [rows,cols+1] [rows-1,cols+2]
            //second possibilty =
            //third possibilty =
            //fourth possibilty =
            //fifth possibilty =
            //sixth possibilty =
            //seventh possibilty =
            //eight possibilty =
            for (int rows = 0; rows < MaxRows; rows++)
            {
                for (int cols = 0; cols < 6; cols++)
                {
                    if (rows > 0 && cols < 6)
                    {
                        //first possibility
                        if (theBoard[rows, cols].AnimalType == theBoard[rows, cols + 1].AnimalType && theBoard[rows, cols].AnimalType == theBoard[rows - 1, cols + 2].AnimalType)
                        {
                            theBoard[rows, cols].Mood = SHOCK;
                            theBoard[rows, cols + 1].Mood = SHOCK;
                            theBoard[rows - 1, cols + 2].Mood = SHOCK;
                        }
                        //third possibility
                        if (theBoard[rows, cols].AnimalType == theBoard[rows - 1, cols + 1].AnimalType && theBoard[rows, cols].AnimalType == theBoard[rows, cols + 2].AnimalType)
                          {
                             theBoard[rows, cols].Mood = SHOCK;
                            theBoard[rows - 1, cols + 1].Mood = SHOCK;
                            theBoard[rows, cols + 2].Mood = SHOCK;
                         }
                        //sixth possibility
                        if (theBoard[rows, cols].AnimalType == theBoard[rows - 1, cols + 1].AnimalType && theBoard[rows, cols].AnimalType == theBoard[rows - 1, cols + 2].AnimalType)
                        {
                            theBoard[rows, cols].Mood = SHOCK;
                            theBoard[rows - 1, cols + 1].Mood = SHOCK;
                            theBoard[rows - 1, cols + 2].Mood = SHOCK;
                        }
                    }
                    if (rows < 7 && cols < 7)
                    {
                        //second possibility
                        if (theBoard[rows, cols].AnimalType == theBoard[rows, cols + 1].AnimalType && theBoard[rows, cols].AnimalType == theBoard[rows + 1, cols + 2].AnimalType)
                        {
                            theBoard[rows, cols].Mood = SHOCK;
                            theBoard[rows, cols + 1].Mood = SHOCK;
                            theBoard[rows + 1, cols + 2].Mood = SHOCK;
                        }
                        //fourth possibility
                        if (theBoard[rows, cols].AnimalType == theBoard[rows + 1, cols + 1].AnimalType && theBoard[rows, cols].AnimalType == theBoard[rows, cols + 2].AnimalType)
                        {
                            theBoard[rows, cols].Mood = SHOCK;
                            theBoard[rows + 1, cols + 1].Mood = SHOCK;
                            theBoard[rows, cols + 2].Mood = SHOCK;
                        }
                        //fifth possibility
                        if (theBoard[rows, cols].AnimalType == theBoard[rows + 1, cols + 1].AnimalType && theBoard[rows, cols].AnimalType == theBoard[rows + 1, cols + 2].AnimalType)
                        {
                            theBoard[rows, cols].Mood = SHOCK;
                            theBoard[rows + 1, cols + 1].Mood = SHOCK;
                            theBoard[rows + 1, cols + 2].Mood = SHOCK;
                        }
                    }
                    if (cols < 5)
                    {
                        //seventh possibility
                        if (theBoard[rows, cols].AnimalType == theBoard[rows, cols + 2].AnimalType && theBoard[rows, cols].AnimalType == theBoard[rows, cols + 3].AnimalType)
                        {
                            theBoard[rows, cols].Mood = SHOCK;
                            theBoard[rows, cols + 2].Mood = SHOCK;
                            theBoard[rows, cols + 3].Mood = SHOCK;
                        }
                        //eight possibility
                        if (theBoard[rows, cols].AnimalType == theBoard[rows, cols + 1].AnimalType && theBoard[rows, cols].AnimalType == theBoard[rows, cols + 3].AnimalType)
                        {
                            theBoard[rows, cols].Mood = SHOCK;
                            theBoard[rows, cols + 1].Mood = SHOCK;
                            theBoard[rows, cols + 3].Mood = SHOCK;
                        }
                    }
                }
            }
        }
        private void VerticalHints()
        {

        }
        private void LevelTracking()
        {
            if (levelTracker == 1 && playerScore.Score >= 100)
            {
                levelPlusInstance.Play();
                gameMode = levelUp;
            }
            if (levelTracker == 2 && playerScore.Score >= 120)
            {
                levelPlusInstance.Play();
                gameMode = levelUp;
            }
            if (levelTracker == 3 && playerScore.Score >= 140)
            {
                levelPlusInstance.Play();
                gameMode = levelUp;
            }
            if (levelTracker == 4 && playerScore.Score >= 160)
            {
                levelPlusInstance.Play();
                gameMode = levelUp;
            }
            if (levelTracker == 5 && playerScore.Score >= 180)
            {
                levelPlusInstance.Play();
                gameMode = levelUp;
            }
        }
        /// <summary>
        /// main draw from game loop
        /// in game mode draw array of animals
        /// and is one is selected highlight it
        /// 
        /// splash draw background picture and overlay elephant
        /// change elephant sub-sprite location based on splashTiming value
        /// when splashTiming exceeds 100 reset it to zero
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LimeGreen);
            //to change the colour of the background when time is running out
           // if (playerScore.Timer <= 15)
               // GraphicsDevice.Clear(Color.Red);
            if (levelTracker == 2)
                GraphicsDevice.Clear(Color.Coral);
            if (levelTracker == 3)
                GraphicsDevice.Clear(Color.DodgerBlue);
            if (levelTracker == 4)
                GraphicsDevice.Clear(Color.IndianRed);
            if (levelTracker == 5)
                GraphicsDevice.Clear(Color.Ivory);

            spriteBatch.Begin();
            switch (gameMode)
            {
                    
                case Instructions:
                    //draws the instrcutions on the screen so the player can see what they have to do
                    spriteBatch.DrawString(font, "1) Match 3 animals in a row.", new Vector2(10, 10), Color.Gold);
                    spriteBatch.DrawString(font, "2) you have 100 seconds to play.", new Vector2(10, 40),Color.LemonChiffon);
                    spriteBatch.DrawString(font, "3) Press 'P' to Pause the game.", new Vector2(10, 70), Color.DodgerBlue);
                    spriteBatch.DrawString(font, "4) press it again to un-pause.", new Vector2(10, 100), Color.Pink);
                    spriteBatch.DrawString(font, "5) When you score a set amount,", new Vector2(10, 130), Color.Crimson);
                    spriteBatch.DrawString(font, "you go to the next level.", new Vector2(10, 160), Color.Crimson);
                    spriteBatch.DrawString(font, "6) Enjoy!!", new Vector2(10, 190), Color.Orange);
                    break;

                case GAME:
                        for (byte row = 0; row < MaxRows; row++)
                        {
                            for (byte col = 0; col < MaxCols; col++)
                            {
                                theBoard[row, col].Draw(spriteBatch, row, col);
                            } // end inner for
                        } // end outer for
                        if (selected)
                        {
                            spriteBatch.Draw(highLightTexture, new Rectangle(currentY * SPRITEOFFSET + XOFFSET, currentX * SPRITEOFFSET + YOFFSET, SPRITEOFFSET, SPRITEOFFSET), Color.White);
                        }
                        spriteBatch.DrawString(font, "Your Score:", new Vector2(360, 20), Color.Black);
                        spriteBatch.DrawString(font, "Time left:", new Vector2(370, 100), Color.Black);
                        spriteBatch.DrawString(font, "Hints:", new Vector2(370, 210), Color.Black);
                        spriteBatch.DrawString(font, "Level " + levelTracker, new Vector2(370, 280), Color.Cyan);
                        playerScore.Draw(spriteBatch, font);

                        //spriteBatch.DrawString(font, aMessage, new Vector2(350, 100), Color.RoyalBlue);

                       // spriteBatch.DrawString(font, bMessage, new Vector2(350, 250), Color.Gold);

                        //hourglass timer
                        if (hourglassCounter == 0)
                            spriteBatch.Draw(hourglassTexture1, new Rectangle(400, 160, 30, 40), Color.White);
                        if (hourglassCounter == 1)
                            spriteBatch.Draw(hourglassTexture2, new Rectangle(400, 160, 30, 40), Color.White);
                        if (hourglassCounter == 2)
                            spriteBatch.Draw(hourglassTexture3, new Rectangle(400, 160, 30, 40), Color.White);
                        if (hourglassCounter == 3)
                            spriteBatch.Draw(hourglassTexture4, new Rectangle(400, 160, 30, 40), Color.White);

                        break;

                case levelUp:
                        spriteBatch.Draw(levelUpTexture, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
                        break;

                    //if game is over
                case END:
                    spriteBatch.Draw(gameoverTexture, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
                    if (coinTimer == 1)
                        spriteBatch.Draw(insertCoinTexture1, new Rectangle(22, 120, 40, 50), Color.White);
                    if (coinTimer == 2)
                        spriteBatch.Draw(insertCoinTexture2, new Rectangle(60, 120, 29, 50), Color.White);
                    if (coinTimer == 3)
                        spriteBatch.Draw(insertCoinTexture3, new Rectangle(100, 120, 30, 50), Color.White);
                    if (coinTimer == 4)
                        spriteBatch.Draw(insertCoinTexture4, new Rectangle(140, 120, 25, 50), Color.White);
                    if (coinTimer == 5)
                        spriteBatch.Draw(insertCoinTexture5, new Rectangle(180, 120, 50, 50), Color.White);
                    if (coinTimer == 6)
                        coinTimer = 0;

                        break;

                case PAUSE:
                        spriteBatch.DrawString(font, "PAUSE", new Vector2(200, 150), Color.Red);
                        break;

                    //elephant starting animation
                case SPLASH:
                        spriteBatch.Draw(elephantTexture, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height),new Rectangle(323, 1,256, 160), Color.White);
                        if (splashTiming < 100)
                        {
                            spriteBatch.Draw(elephantTexture, new Rectangle(102, 81, 316, 214), new Rectangle(163, 99, 158, 94), Color.White);
                        }
                        if (splashTiming < 80)
                        {
                            spriteBatch.Draw(elephantTexture, new Rectangle(102, 81, 316, 214), new Rectangle(2, 99, 158, 94), Color.White);
                        }
                        if (splashTiming < 70)
                        {
                            spriteBatch.Draw(elephantTexture, new Rectangle(102, 81, 316, 214), new Rectangle(163, 2, 158, 94), Color.White);
                        }
                        if (splashTiming < 50)
                        {
                            spriteBatch.Draw(elephantTexture, new Rectangle(102, 81, 316, 214), new Rectangle(2, 2, 158, 94), Color.White);
                        }
                        if (splashTiming > 100)
                        {
                            splashTiming = 0;
                        }
                        break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        
    }
}
