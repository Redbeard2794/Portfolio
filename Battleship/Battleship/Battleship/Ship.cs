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
    class Ship
    {
        int size;
        string theme;
        string type;
        int health;
        int rotation;
        bool alive;
        int shipID;

        //textures
        Texture2D textureUp;
        Texture2D textureDown;
        Texture2D textureRight;
        Texture2D textureLeft;
        SpriteFont font;

        bool selected;
        bool placed;

        Vector2 position;

        public Ship(string themeForShip, string shipType, int id)
        {
            theme = themeForShip;
            type = shipType;
            health = 100;

            rotation = 0;
            alive = true;
            shipID = id;

            selected = false;
            placed = false;
            //gives size, start position and health based on ship type
            if (type == "2")
            {
                size = 70;
                position = new Vector2(50, 400);
                health = 40;
            }
            else if (type == "3")
            {
                size = 105;
                position = new Vector2(80, 370);
                health = 60;
            }
            else if (type == "4")
            {
                size = 140;
                position = new Vector2(110, 350);
                health = 80;
            }
            else if (type == "5")
            {
                size = 175;
                position = new Vector2(140, 350);
                health = 100;
            }
        }
        //loads the textures based on the ship type
        public void LoadContent(ContentManager c)
        {
            if (type == "2")
            {
                textureUp = c.Load<Texture2D>("ModernShips/2SquareUp");
                textureDown = c.Load<Texture2D>("ModernShips/2SquareDown");
                textureRight = c.Load<Texture2D>("ModernShips/2SquareRight");
                textureLeft = c.Load<Texture2D>("ModernShips/2SquareLeft");
            }
            else if (type == "3")
            {
                textureUp = c.Load<Texture2D>("ModernShips/3SquareUp");
                textureDown = c.Load<Texture2D>("ModernShips/3SquareDown");
                textureRight = c.Load<Texture2D>("ModernShips/3SquareRight");
                textureLeft = c.Load<Texture2D>("ModernShips/3SquareLeft");
            }
            else if (type == "4")
            {
                textureUp = c.Load<Texture2D>("ModernShips/4SquareUp");
                textureDown = c.Load<Texture2D>("ModernShips/4SquareDown");
                textureRight = c.Load<Texture2D>("ModernShips/4SquareRight");
                textureLeft = c.Load<Texture2D>("ModernShips/4SquareLeft");
            }
            else if (type == "5")
            {
                textureUp = c.Load<Texture2D>("ModernShips/biggestShipUp");
                textureDown = c.Load<Texture2D>("ModernShips/biggestShipDown");
                textureRight = c.Load<Texture2D>("ModernShips/biggestShipRight");
                textureLeft = c.Load<Texture2D>("ModernShips/biggestShipLeft");
            }

            font = c.Load<SpriteFont>("font");
        }
        //rotates the ship if this is callled
        public void Rotate()
        {
                rotation += 90;
            if (rotation == 360)
                rotation = 0;
        }
        //draws the ship based on if it is selected or not
        public void Draw(SpriteBatch sBatch)
        {
            if (alive == true)
            {
                if (selected == false)
                {
                    if (rotation == 0)
                        sBatch.Draw(textureUp, new Rectangle((int)position.X, (int)position.Y, 20, size), Color.White);
                    else if (rotation == 90)
                        sBatch.Draw(textureRight, new Rectangle((int)position.X, (int)position.Y, size, 20), Color.White);
                    else if (rotation == 180)
                        sBatch.Draw(textureDown, new Rectangle((int)position.X, (int)position.Y, 20, size), Color.White);
                    else if (rotation == 270)
                        sBatch.Draw(textureLeft, new Rectangle((int)position.X, (int)position.Y, size, 20), Color.White);
                }
                else if (selected == true)
                {
                    if (rotation == 0)
                        sBatch.Draw(textureUp, new Rectangle((int)position.X, (int)position.Y, 20, size), Color.Yellow);
                    else if (rotation == 90)
                        sBatch.Draw(textureRight, new Rectangle((int)position.X, (int)position.Y, size, 20), Color.Yellow);
                    else if (rotation == 180)
                        sBatch.Draw(textureDown, new Rectangle((int)position.X, (int)position.Y, 20, size), Color.Yellow);
                    else if (rotation == 270)
                        sBatch.Draw(textureLeft, new Rectangle((int)position.X, (int)position.Y, size, 20), Color.Yellow);
                }
            }
            //sBatch.DrawString(font, "ship rotation" + rotation + "\n", new Vector2(300, 310), Color.Red);
        }
        //properties
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public int Size
        {
            get { return size; }
            set { size = value; }
        }
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }
        public int Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public int Health
        {
            get { return health; }
            set { health = value; }
        }
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }
    }
}
