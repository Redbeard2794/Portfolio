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
    class AI
    {
        Vector2 squarePos;
        Random rand = new Random();
        int numShips;
        Vector2 ship1Pos;
        Vector2 ship2Pos;
        Vector2 ship3Pos;
        Vector2 ship4Pos;
        Vector2 ship5Pos;
        Vector2 ship6Pos;
        public AI(int nShips)
        {
            numShips = nShips;
        }
        //for picking a square on the players board to shoot
        public Vector2 pickSquareToShoot(String boardSize)
        {
            if (boardSize == "10*10")
            {
                squarePos.X = rand.Next(0, 10);
                squarePos.Y = rand.Next(0, 10);
            }
            else if (boardSize == "8*8")
            {
                squarePos.X = rand.Next(0, 8);
                squarePos.Y = rand.Next(0, 8);
            }
            return squarePos;
        }
        //for placing its ships
        public void placeShips(String boardSize)
        {
            if (boardSize == "10*10")
            {
                ship1Pos.X = rand.Next(2,6);
                ship1Pos.Y = rand.Next(2, 6);

                ship2Pos.X = rand.Next(2, 6);
                ship2Pos.Y = rand.Next(2, 6);

                ship3Pos.X = rand.Next(2, 6);
                ship3Pos.Y = rand.Next(2, 6);

                ship4Pos.X = rand.Next(2, 6);
                ship4Pos.Y = rand.Next(2, 6);
                if (numShips >= 5)
                {
                    ship5Pos.X = rand.Next(2, 6);
                    ship5Pos.Y = rand.Next(2, 6);
                }
                if (numShips == 6)
                {
                    ship6Pos.X = rand.Next(2, 6);
                    ship6Pos.Y = rand.Next(2, 6);
                }
            }
            else if (boardSize == "8*8")
            {
                ship1Pos.X = rand.Next(0, 8);
                ship1Pos.Y = rand.Next(0, 8);

                ship2Pos.X = rand.Next(0, 8);
                ship2Pos.Y = rand.Next(0, 8);

                ship3Pos.X = rand.Next(0, 8);
                ship3Pos.Y = rand.Next(0, 8);

                ship4Pos.X = rand.Next(0, 8);
                ship4Pos.Y = rand.Next(0, 8);
                if (numShips >= 5)
                {
                    ship5Pos.X = rand.Next(0, 8);
                    ship5Pos.Y = rand.Next(0, 8);
                }
                if (numShips == 6)
                {
                    ship6Pos.X = rand.Next(0, 8);
                    ship6Pos.Y = rand.Next(0, 8);
                }
            }
        }
        //properties
        public Vector2 SquarePos
        {
            get { return squarePos; }
            set { squarePos = value; }
        }
        public Vector2 Ship1Pos
        {
            get { return ship1Pos; }
            set { ship1Pos = value; }
        }
        public Vector2 Ship2Pos
        {
            get { return ship2Pos; }
            set { ship2Pos = value; }
        }
        public Vector2 Ship3Pos
        {
            get { return ship3Pos; }
            set { ship3Pos = value; }
        }
        public Vector2 Ship4Pos
        {
            get { return ship4Pos; }
            set { ship4Pos = value; }
        }
        public Vector2 Ship5Pos
        {
            get { return ship5Pos; }
            set { ship5Pos = value; }
        }
        public Vector2 Ship6Pos
        {
            get { return ship6Pos; }
            set { ship6Pos = value; }

        }
    }
}
