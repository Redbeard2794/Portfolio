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
    class Square
    {
        Vector2 position;
        bool occupied;
        bool hit;
        Texture2D texture;
        //used to denote if it is highlighted or hit
        Color col = new Color(255, 255, 255);

        public Square()
        {
            occupied = false;
            hit = false;
        }
        //load the texture
        public void LoadContent(ContentManager c)
        {
            texture = c.Load<Texture2D>("square");
        }
        //draw the square
        public void Draw(SpriteBatch sBatch)
        {
            if(hit == false)
            sBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 35, 35), col);
            else if (hit == true)
                sBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 35, 35), Color.Crimson);
        }
        //properties
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Color Col
        {
            get { return col; }
            set { col = value; }
        }
        public bool Hit
        {
            get { return hit; }
            set { hit = value; }
        }
        public bool Occupied
        {
            get { return occupied; }
            set { occupied = value; }
        }
    }
}
