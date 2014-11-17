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

namespace Battleship
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BattleShipGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D button;
        Texture2D label;

        const byte Menu = 0, Options = 1, PlaceShips = 2, Game = 3,End = 4;
        int gameState = Menu;

        SpriteFont font;

        MouseState mouseState, previousMouseState;

        const byte English = 0, Irish = 1, French = 2;
        int language = 0;

        int maxNumberOfShips;
        string theme;
        string boardSize;

        Player player;
        Board b1;
        Board b2;
        bool isAiTurn;
        AI ai;
        int timer = 0;
        public BattleShipGame()
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
            this.IsMouseVisible = true;
            isAiTurn = false;
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

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>("font");

            button = this.Content.Load<Texture2D>("button");
            label = this.Content.Load<Texture2D>("label");
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
            KeyboardState ks = new KeyboardState();
            if (ks.IsKeyDown(Keys.Escape))
                this.Exit();

            //checkMouseInput();

            switch (gameState)
            {
                case Menu:
                    checkMouseInput();
                    break;
                case Options:
                    checkMouseInput();
                    break;
                case PlaceShips:
                    checkMouseInput();
                    break;
                case Game:
                    checkMouseInput();
                    if (isAiTurn == true)
                    {
                        b1.AiHitsSquare(ai.pickSquareToShoot(boardSize));
                        isAiTurn = false;
                    }
                    playerBoardCheckOccupied();
                    if (b2.checkIfAiHasShipsLeft() == false)
                        gameState = End;
                    break;
                case End:
                    checkMouseInput();
                    break;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        //this method detects if a square on the players board is hit and if it contains a ship
        public void playerBoardCheckOccupied()
        {
            if (b1.CheckIfPlayerBoardOccupied(ai.SquarePos) == true)
            {
                player.WasHit(ai.SquarePos);
            }
        }

        //to get a particular ship(belonging to the player) while in the place ships state.
        public void getShip(Vector2 pos)
        {
            //position of mouse
            Vector2 mousePos = pos;
            player.getShip(mousePos);
        }
        //player places their ship
        public void PlaceShip(Vector2 mousePos)
        {
            b1.placeShipOnSquares(mousePos, player.CurrentShipPOs, player.CurrentShipSize, player.CurrentShipRotation);
        }
        //for rotating the players currently selected ship
        public void RotateShip()
        {
                player.RotateShip();
        }
        //player selecting a square to place a ship on or shoot depending on game state
        public void pickSquare(Vector2 mousePos)
        {
           switch (gameState)
            {
                case PlaceShips:
                b1.getSquare(mousePos);
                break;
                case Game:
                b2.getSquare(mousePos);
                break;
            }
        }
        //logs the hit on the ai's board
        public void logHit()
        {
            b2.setHit();
        }
        //checks for mouse input and calls the appropriate methods
        private void checkMouseInput()
        {
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();
            switch(gameState)
            {
                case Menu:
                if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    if (mouseState.X > 350 && mouseState.X < 450 && mouseState.Y > 150 && mouseState.Y < 200)
                        gameState = Options;
                    if (mouseState.X > 350 && mouseState.X < 450 && mouseState.Y > 250 && mouseState.Y < 300)
                        this.Exit();
                }
                break;
                case Options:
                if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    if (mouseState.X > 50 && mouseState.X < 100 && mouseState.Y > 80 && mouseState.Y < 130)
                        maxNumberOfShips = 8;
                    else if (mouseState.X > 150 && mouseState.X < 200 && mouseState.Y > 80 && mouseState.Y < 130)
                        maxNumberOfShips = 10;
                    else if (mouseState.X > 250 && mouseState.X < 300 && mouseState.Y > 80 && mouseState.Y < 130)
                        maxNumberOfShips = 12;

                    if (mouseState.X > 400 && mouseState.X < 500 && mouseState.Y > 80 && mouseState.Y < 130)
                        theme = "modern";
                    //this is not available due to time constraints
                    //else if (mouseState.X > 600 && mouseState.X < 700 && mouseState.Y > 80 && mouseState.Y < 130)
                    //    theme = "pirate";

                    if (mouseState.X > 250 && mouseState.X < 350 && mouseState.Y > 260 && mouseState.Y < 310)
                        boardSize = "8*8";
                    else if (mouseState.X > 450 && mouseState.X < 550 && mouseState.Y > 260 && mouseState.Y < 310)
                        boardSize = "10*10";

                    if (mouseState.X > 350 && mouseState.X < 450 && mouseState.Y > 400 && mouseState.Y < 500)
                    {
                        player = new Player(maxNumberOfShips, theme, Content);
                        b1 = new Board(boardSize, theme, Content);
                        b2 = new Board(boardSize, theme, Content);
                        gameState = PlaceShips;
                    }
                }
                break;
                case PlaceShips:
                if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    //if(player.CurrentShip.Selected == false)
                    getShip(new Vector2(mouseState.X, mouseState.Y));
                    //if(player.CurrentShip.Selected == true)
                    if (player.ShipSelected == true)
                    {
                        PlaceShip(new Vector2(mouseState.X, mouseState.Y));
                        player.setPlaced(b1.CurrentSquarePos);
                    }

                    //logHit();
                    if (mouseState.X > 350 && mouseState.X < 450 && mouseState.Y > 400 && mouseState.Y < 500)
                    {
                        gameState = Game;
                        ai = new AI(maxNumberOfShips/2);
                        ai.placeShips(boardSize);
                        b2.AiPlaceShipsOnSquare(ai.Ship1Pos, ai.Ship2Pos, ai.Ship3Pos, ai.Ship4Pos, ai.Ship5Pos, ai.Ship6Pos);
                    }
                }
                if (mouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton == ButtonState.Released)
                    RotateShip();
                //getShip(new Vector2(mouseState.X, mouseState.Y));
                pickSquare(new Vector2(mouseState.X, mouseState.Y));
                //PlaceShip(new Vector2(mouseState.X, mouseState.Y));
                break;
                case Game:
                if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    logHit();
                    //player.WasHit(b1.CurrentSquarePos);
                    isAiTurn = true;
                }
                pickSquare(new Vector2(mouseState.X, mouseState.Y));
                break;
                case End:
                if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                    this.Exit();
                break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// Drawing of the GUI and ships, boards etc
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.PaleTurquoise);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            switch(gameState)
            {
                case Menu:
                    GraphicsDevice.Clear(Color.PaleTurquoise);

                    spriteBatch.Draw(button, new Rectangle(350, 150, 100, 50), Color.White);
                    spriteBatch.DrawString(font, "Play", new Vector2(370, 160), Color.DodgerBlue);
                    spriteBatch.Draw(button, new Rectangle(350, 250, 100, 50), Color.White);
                    spriteBatch.DrawString(font, "Quit", new Vector2(370, 260), Color.DodgerBlue);
                break;

                case Options:
                    GraphicsDevice.Clear(Color.PaleGreen);
                    //pick max number of ships
                    spriteBatch.Draw(label, new Rectangle(50, 20, 300, 50), Color.White);
                    spriteBatch.DrawString(font, "Pick Max number of ships", new Vector2(70, 30), Color.DodgerBlue);
                    spriteBatch.Draw(button, new Rectangle(50, 80, 50, 50), Color.White);
                    spriteBatch.DrawString(font, "4", new Vector2(70, 90), Color.DodgerBlue);
                    spriteBatch.Draw(button, new Rectangle(150, 80, 50, 50), Color.White);
                    spriteBatch.DrawString(font, "5", new Vector2(170, 90), Color.DodgerBlue);
                    spriteBatch.Draw(button, new Rectangle(250, 80, 50, 50), Color.White);
                    spriteBatch.DrawString(font, "6", new Vector2(270, 90), Color.DodgerBlue);
                    //pick theme
                    spriteBatch.Draw(label, new Rectangle(400, 20, 300, 50), Color.White);
                    spriteBatch.DrawString(font, "Pick the theme", new Vector2(420, 30), Color.DodgerBlue);
                    spriteBatch.Draw(button, new Rectangle(400, 80, 100, 50), Color.White);
                    spriteBatch.DrawString(font, "Modern", new Vector2(420, 100), Color.DodgerBlue);
                    spriteBatch.Draw(button, new Rectangle(600, 80, 100, 50), Color.White);
                    spriteBatch.DrawString(font, "Pirate", new Vector2(620, 100), Color.DodgerBlue);
                    spriteBatch.DrawString(font, "Not available", new Vector2(620, 130), Color.DodgerBlue);
                    //pick board size
                    spriteBatch.Draw(label, new Rectangle(250, 200, 300, 50), Color.White);
                    spriteBatch.DrawString(font, "Pick the board's size", new Vector2(270, 220), Color.DodgerBlue);
                    spriteBatch.Draw(button, new Rectangle(250, 260, 100, 50), Color.White);
                    spriteBatch.DrawString(font, "8*8", new Vector2(270, 280), Color.DodgerBlue);
                    spriteBatch.Draw(button, new Rectangle(450, 260, 100, 50), Color.White);
                    spriteBatch.DrawString(font, "10*10", new Vector2(470, 280), Color.DodgerBlue);
                    //Ready
                    spriteBatch.Draw(button, new Rectangle(350, 400, 100, 50), Color.White);
                    spriteBatch.DrawString(font, "Ready", new Vector2(370,420),Color.DodgerBlue);

                    spriteBatch.DrawString(font, "Each player has " + maxNumberOfShips/2 + " ships", new Vector2(300, 310), Color.Black);
                    spriteBatch.DrawString(font, "Theme:" + theme, new Vector2(300, 330), Color.Black);
                    spriteBatch.DrawString(font, "Board size:" + boardSize, new Vector2(300, 350), Color.Black);

                break;
                case PlaceShips:
                GraphicsDevice.Clear(Color.Cyan);
                b1.DrawSquares(spriteBatch, true);
                b2.DrawSquares(spriteBatch, false);
                player.DrawShips(spriteBatch);
                spriteBatch.Draw(button, new Rectangle(350, 400, 100, 50), Color.White);
                spriteBatch.DrawString(font, "Ready", new Vector2(370,420),Color.DodgerBlue);
                spriteBatch.DrawString(font, "Left click a ship to select it, \n Right click to rotate it \n (this must be done), \n left click on a square to \n place it", new Vector2(450, 350), Color.Black);
                break;
                case Game:
                GraphicsDevice.Clear(Color.DarkCyan);
                b1.DrawSquares(spriteBatch, true);
                b2.DrawSquares(spriteBatch, false);
                player.DrawShips(spriteBatch);
                break;
                case End:
                GraphicsDevice.Clear(Color.PaleTurquoise);
                timer++;
                if (timer <= 30)
                    GraphicsDevice.Clear(Color.PaleGreen);
                else if (timer <= 60)
                    GraphicsDevice.Clear(Color.Cyan);
                else if (timer <= 90)
                    GraphicsDevice.Clear(Color.DarkCyan);
                else if (timer == 120)
                    timer = 0;
                spriteBatch.DrawString(font, "GAME OVER", new Vector2(350, 150), Color.Black);
                break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
