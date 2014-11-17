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

namespace Battleship
{
    class Board
    {
        string size;
        string theme;
        Square[,] squares;//2d array of squares
        MouseState mouseState, previousMouseState;
        Vector2 currentSquarePos;//used to pass the players ship its new position



        public Board(string boardSize, string boardTheme, ContentManager c)
        {
            size = boardSize;
            theme = boardTheme;

            squares = new Square[10, 10];
            //based on board size it creates enough squares to fill the board
            if (boardSize == "8*8")
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        squares[i, j] = new Square();
                        squares[i, j].LoadContent(c);
                    }
                }
            }
            else if (boardSize == "10*10")
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        squares[i, j] = new Square();
                        squares[i, j].LoadContent(c);
                    }
                }
            }
        }
        //used for setting a square on the AI's board to be hit
        public void setHit()
        {
            if (size == "10*10")
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (squares[i, j].Col.R == 255 && squares[i,j].Col.G == 255 && squares[i,j].Col.B ==0)
                        {
                            squares[i, j].Hit = true;
                            if (squares[i, j].Occupied == true)
                                squares[i, j].Occupied = false;
                            //currentSquarePos = squares[i, j].Position;
                        }
                    }
                }
            }
            else if (size == "8*8")
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (squares[i, j].Col.R == 255 && squares[i, j].Col.G == 255 && squares[i, j].Col.B == 0)
                        {
                            squares[i, j].Hit = true;
                            if (squares[i, j].Occupied == true)
                                squares[i, j].Occupied = false;
                            //currentSquarePos = squares[i, j].Position;
                        }
                    }
                }
            }
        }
        //used to check if the ai has any 'ships' left
        public bool checkIfAiHasShipsLeft()
        {
            int maxI = 0; 
            if (size == "10*10")
            {
                maxI = 10;
            }
            else if (size == "8*8")
            {
                maxI = 8;
            }
            for (int i = 0; i < maxI; i++)
              {
                   for (int j = 0; j < maxI; j++)
                   {
                       if (squares[i, j].Occupied == true)
                          return true;
                   }
              }
            return false;
        }
        //used to get the current square the player has their mouse over 
        public void getSquare(Vector2 mPos)
        {
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();
            if (size == "10*10")
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (mPos.X >= squares[i, j].Position.X && mPos.X <= squares[i, j].Position.X + 35 && mPos.Y >= squares[i, j].Position.Y && mPos.Y <= squares[i, j].Position.Y + 35)
                        {
                            squares[i, j].Col = Color.Yellow;
                        }
                        else squares[i, j].Col = Color.White;
                    }
                }
            }
            else if (size == "8*8")
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (mPos.X >= squares[i, j].Position.X && mPos.X <= squares[i, j].Position.X + 35 && mPos.Y >= squares[i, j].Position.Y && mPos.Y <= squares[i, j].Position.Y + 35)
                        {
                            squares[i, j].Col = Color.Yellow;
                        }
                        else squares[i, j].Col = Color.White;
                    }
                }
            }
        }
        //used to place the players ship on the corresponding squares based on its x,y position, size and rotation
        public void placeShipOnSquares(Vector2 mPos, Vector2 currentShipPOs, int currentShipSize, int currentShipRotation)
        {
            MouseState mouseState, previousMouseState;
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;
            if (size == "10*10")
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        if (mPos.X >= squares[i, j].Position.X && mPos.X <= squares[i, j].Position.X + 35 && mPos.Y >= squares[i, j].Position.Y && mPos.Y <= squares[i, j].Position.Y + 35)
                        {
                            currentSquarePos = squares[i, j].Position;
                                if (currentShipRotation == 90 || currentShipRotation == 270)
                                {
                                    if (currentShipSize == 70)
                                    {
                                        squares[i, j].Occupied = true;
                                        squares[i + 1, j].Occupied = true;
                                    }
                                    else if (currentShipSize == 105)
                                    {
                                        squares[i, j].Occupied = true;
                                        squares[i + 1, j].Occupied = true;
                                        squares[i + 2, j].Occupied = true;
                                    }
                                    else if (currentShipSize == 140)
                                    {
                                        squares[i, j].Occupied = true;
                                        squares[i + 1, j].Occupied = true;
                                        squares[i + 2, j].Occupied = true;
                                        squares[i + 3, j].Occupied = true;
                                    }
                                    else if (currentShipSize == 175)
                                    {
                                        squares[i, j].Occupied = true;
                                        squares[i + 1, j].Occupied = true;
                                        squares[i + 2, j].Occupied = true;
                                        squares[i + 3, j].Occupied = true;
                                        squares[i + 4, j].Occupied = true;
                                    }
                                }
                                else if (currentShipRotation == 0 || currentShipRotation == 180)
                                {
                                    if (currentShipSize == 70)
                                    {
                                        squares[i, j].Occupied = true;
                                        squares[i, j + 1].Occupied = true;
                                    }
                                    else if (currentShipSize == 105)
                                    {
                                        squares[i, j].Occupied = true;
                                        squares[i, j + 1].Occupied = true;
                                        squares[i, j + 2].Occupied = true;
                                    }
                                    else if (currentShipSize == 140)
                                    {
                                        squares[i, j].Occupied = true;
                                        squares[i, j + 1].Occupied = true;
                                        squares[i, j + 2].Occupied = true;
                                        squares[i, j + 3].Occupied = true;
                                    }
                                    else if (currentShipSize == 175)
                                    {
                                        squares[i, j].Occupied = true;
                                        squares[i, j + 1].Occupied = true;
                                        squares[i, j + 2].Occupied = true;
                                        squares[i, j + 3].Occupied = true;
                                        squares[i, j + 4].Occupied = true;
                                    }
                                }
                        }
                        
                    }
                }
            }
            else if (size == "8*8")
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (mPos.X >= squares[i, j].Position.X && mPos.X <= squares[i, j].Position.X + 35 && mPos.Y >= squares[i, j].Position.Y && mPos.Y <= squares[i, j].Position.Y + 35)
                        {
                            currentSquarePos = squares[i, j].Position;
                            if (currentShipRotation == 90 || currentShipRotation == 270)
                            {
                                if (currentShipSize == 70)
                                {
                                    squares[i, j].Occupied = true;
                                    squares[i + 1, j].Occupied = true;
                                }
                                else if (currentShipSize == 105)
                                {
                                    squares[i, j].Occupied = true;
                                    squares[i + 1, j].Occupied = true;
                                    squares[i + 2, j].Occupied = true;
                                }
                                else if (currentShipSize == 140)
                                {
                                    squares[i, j].Occupied = true;
                                    squares[i + 1, j].Occupied = true;
                                    squares[i + 2, j].Occupied = true;
                                    squares[i + 3, j].Occupied = true;
                                }
                                else if (currentShipSize == 175)
                                {
                                    squares[i, j].Occupied = true;
                                    squares[i + 1, j].Occupied = true;
                                    squares[i + 2, j].Occupied = true;
                                    squares[i + 3, j].Occupied = true;
                                    squares[i + 4, j].Occupied = true;
                                }
                            }
                            else if (currentShipRotation == 0 || currentShipRotation == 180)
                            {
                                if (currentShipSize == 70)
                                {
                                    squares[i, j].Occupied = true;
                                    squares[i, j + 1].Occupied = true;
                                }
                                else if (currentShipSize == 105)
                                {
                                    squares[i, j].Occupied = true;
                                    squares[i, j + 1].Occupied = true;
                                    squares[i, j + 2].Occupied = true;
                                }
                                else if (currentShipSize == 140)
                                {
                                    squares[i, j].Occupied = true;
                                    squares[i, j + 1].Occupied = true;
                                    squares[i, j + 2].Occupied = true;
                                    squares[i, j + 3].Occupied = true;
                                }
                                else if (currentShipSize == 175)
                                {
                                    squares[i, j].Occupied = true;
                                    squares[i, j + 1].Occupied = true;
                                    squares[i, j + 2].Occupied = true;
                                    squares[i, j + 3].Occupied = true;
                                    squares[i, j + 4].Occupied = true;
                                }
                            }
                        }
                        
                    }
                }
            }
        }
        //used to check if a square on the players board is occupied when the ai shoots it
        public bool CheckIfPlayerBoardOccupied(Vector2 AiPickPos)
        {
            if (squares[(int)AiPickPos.X, (int)AiPickPos.Y].Occupied == true)
                return true;
            else return false;
        }
        //used to set the square selected by the ai to be hit
        public void AiHitsSquare(Vector2 pickedSquare)
        {
            squares[(int)pickedSquare.X, (int)pickedSquare.Y].Hit = true;
        }
        //used for AI placing its ships on the corresponding squares
        public void AiPlaceShipsOnSquare(Vector2 s1Pos, Vector2 s2Pos, Vector2 s3Pos, Vector2 s4Pos, Vector2 s5Pos, Vector2 s6Pos)
        {
            //2(these denote the size of the ship)
            squares[(int)s1Pos.X, (int)s1Pos.Y].Occupied = true;
            squares[(int)s1Pos.X+1, (int)s1Pos.Y].Occupied = true;
            //3
            squares[(int)s2Pos.X, (int)s2Pos.Y].Occupied = true;
            squares[(int)s2Pos.X+1, (int)s2Pos.Y].Occupied = true;
            squares[(int)s2Pos.X+2, (int)s2Pos.Y].Occupied = true;
            //3 (ii)
            squares[(int)s3Pos.X, (int)s2Pos.Y].Occupied = true;
            squares[(int)s3Pos.X + 1, (int)s2Pos.Y].Occupied = true;
            squares[(int)s3Pos.X + 2, (int)s2Pos.Y].Occupied = true;
            //4
            squares[(int)s4Pos.X, (int)s4Pos.Y].Occupied = true;
            squares[(int)s4Pos.X+1, (int)s4Pos.Y].Occupied = true;
            squares[(int)s4Pos.X+2, (int)s4Pos.Y].Occupied = true;
            squares[(int)s4Pos.X+3, (int)s4Pos.Y].Occupied = true;
            //4 (ii)
            squares[(int)s5Pos.X, (int)s5Pos.Y].Occupied = true;
            squares[(int)s5Pos.X+1, (int)s5Pos.Y].Occupied = true;
            squares[(int)s5Pos.X+2, (int)s5Pos.Y].Occupied = true;
            squares[(int)s5Pos.X+3, (int)s5Pos.Y].Occupied = true;
            //5
            squares[(int)s6Pos.X, (int)s6Pos.Y].Occupied = true;
            squares[(int)s6Pos.X+1, (int)s6Pos.Y].Occupied = true;
            squares[(int)s6Pos.X+2, (int)s6Pos.Y].Occupied = true;
            squares[(int)s6Pos.X+3, (int)s6Pos.Y].Occupied = true;
            squares[(int)s6Pos.X+4, (int)s6Pos.Y].Occupied = true;

        }
        //used to draw the squares that make up the board
        //the bool passed in tells the board whether it is on the left
        //of the screen or the right
        public void DrawSquares(SpriteBatch sBatch, bool left)
        {
            if (left == true)
            {
                if (size == "8*8")
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (i == 0 && j == 0)
                                squares[i, j].Position = new Vector2(i, j);
                            else if (i == 0 && j != 0)
                                squares[i, j].Position = new Vector2(i, j * 35);
                            else if (i != 0 & j == 0)
                                squares[i, j].Position = new Vector2(i * 35, j);
                            else if (i != 0 && j != 0)
                                squares[i, j].Position = new Vector2(i * 35, j * 35);
                            squares[i, j].Draw(sBatch);
                        }
                    }
                }
                else if (size == "10*10")
                {
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (i == 0 && j == 0)
                                squares[i, j].Position = new Vector2(i, j);
                            else if (i == 0 && j != 0)
                                squares[i, j].Position = new Vector2(i, j * 35);
                            else if (i != 0 & j == 0)
                                squares[i, j].Position = new Vector2(i * 35, j);
                            else if (i != 0 && j != 0)
                                squares[i, j].Position = new Vector2(i * 35, j * 35);
                            squares[i, j].Draw(sBatch);
                        }
                    }
                }
            }
            if (left == false)
            {
                if (size == "8*8")
                {
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (i == 0 && j == 0)
                                squares[i, j].Position = new Vector2(i+520, j);
                            else if (i == 0 && j != 0)
                                squares[i, j].Position = new Vector2(i+520, j * 35);
                            else if (i != 0 & j == 0)
                                squares[i, j].Position = new Vector2((i * 35) +520, j);
                            else if (i != 0 && j != 0)
                                squares[i, j].Position = new Vector2((i * 35)+520, j * 35);
                            squares[i, j].Draw(sBatch);
                        }
                    }
                }
                else if (size == "10*10")
                {
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (i == 0 && j == 0)
                                squares[i, j].Position = new Vector2(i+450, j);
                            else if (i == 0 && j != 0)
                                squares[i, j].Position = new Vector2(i+450, j * 35);
                            else if (i != 0 & j == 0)
                                squares[i, j].Position = new Vector2((i * 35) +450, j);
                            else if (i != 0 && j != 0)
                                squares[i, j].Position = new Vector2((i * 35)+450, j * 35);
                            squares[i, j].Draw(sBatch);
                        }
                    }
                }
            }
            
        }
        //properties
        public Vector2 CurrentSquarePos
        {
            get { return currentSquarePos; }
            set { currentSquarePos = value; }
        }
    }
}
