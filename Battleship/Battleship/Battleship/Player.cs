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
    class Player
    {
        int numberOfShips;
        List<Ship> ships;
        //these are used to pass into the board to check which squares are needed based on 
        //the selected ships position, size and rotation
        Vector2 currentShipPos;
        int currentShipSize;
        int currentShipRotation;
        bool shipSelected;

        public Player(int maxShips, string theme, ContentManager c)
        {
            shipSelected = false;
            numberOfShips = maxShips / 2;
            ships = new List<Ship>();
            //creates different ships based on the index of the loop 
            for (int i = 0; i < numberOfShips; i++)
            {
                if (i == 0)
                {
                    Ship s = new Ship(theme, "2", i + 100);
                    s.LoadContent(c);
                    ships.Add(s);
                }
                else if (i == 1 || i == 2)
                {
                    Ship s = new Ship(theme, "3", i + 100);
                    s.LoadContent(c);
                    ships.Add(s);
                }
                else if (i == 3 || i == 4)
                {
                    Ship s = new Ship(theme, "4", i + 100);
                    s.LoadContent(c);
                    ships.Add(s);
                }
                else if (i == 5)
                {
                    Ship s = new Ship(theme, "5", i + 100);
                    s.LoadContent(c);
                    ships.Add(s);
                }
            } 
        }
        //used to get the current ship and select it
        public void getShip(Vector2 mousePos)
        {
            for (int i = 0; i < numberOfShips; i++)
            {
                if (mousePos.X >= ships[i].Position.X && mousePos.X <= (ships[i].Position.X + 20) && mousePos.Y >= ships[i].Position.Y && mousePos.Y <= (ships[i].Position.Y + ships[i].Size))
                {
                    ships[i].Selected = true;
                    currentShipPos = ships[i].Position;
                    currentShipSize = ships[i].Size;
                    break;
                }
            }
        }
        //used to rotate the currently selected ship
        public void RotateShip()
        {
            for (int i = 0; i < numberOfShips; i++)
            {
                if (ships[i].Selected == true)
                {
                    ships[i].Rotate();
                    shipSelected = true;
                    currentShipRotation = ships[i].Rotation;
                    break;
                }
            }
        }
        //used to place the ship
        public void setPlaced(Vector2 currentSquarePos)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                if (ships[i].Selected == true)
                {
                    ships[i].Position = currentSquarePos;
                    ships[i].Selected = false;
                    shipSelected = false;
                }
            }
        }
        //used to apply damage to a ship if it is. does malfunction a bit
        public void WasHit(Vector2 AiPickPos)
        {
            Vector2 AiSelectPos = AiPickPos * 20;
            for (int i = 0; i < ships.Count; i++)
            {
                if (ships[i].Rotation == 90 || ships[i].Rotation == 270)
                {
                    if (AiSelectPos.X >= ships[i].Position.X && AiSelectPos.X <= ships[i].Position.X + ships[i].Size && AiSelectPos.Y >= ships[i].Position.Y && AiSelectPos.Y <= ships[i].Position.Y + 35)
                    {
                        if (ships[i].Health != 0)
                            ships[i].Health -= 20;
                        else if (ships[i].Health == 0)
                            ships[i].Alive = false;
                    }
                }
                else if (ships[i].Rotation == 0 || ships[i].Rotation == 180)
                {
                    if (AiSelectPos.X >= ships[i].Position.X && AiSelectPos.X <= ships[i].Position.X + 35 && AiSelectPos.Y >= ships[i].Position.Y && AiSelectPos.Y <= ships[i].Position.Y + ships[i].Size)
                    {
                        if (ships[i].Health != 0)
                            ships[i].Health -= 20;
                        else if (ships[i].Health == 0)
                            ships[i].Alive = false;
                    }
                }
            }
        }
        //used to draw the ships
        public void DrawShips(SpriteBatch sBatch)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                ships[i].Draw(sBatch);
            }
        }
        //properties
        public Vector2 CurrentShipPOs
        {
            get { return currentShipPos; }
            set { currentShipPos = value; }
        }
        public int CurrentShipSize
        {
            get { return currentShipSize; }
            set { currentShipSize = value; }
        }
        public int CurrentShipRotation
        {
            get { return currentShipRotation; }
            set { currentShipRotation = value; }
        }
        public bool ShipSelected
        {
            get { return shipSelected; }
            set { shipSelected = value; }
        }
    }
}
