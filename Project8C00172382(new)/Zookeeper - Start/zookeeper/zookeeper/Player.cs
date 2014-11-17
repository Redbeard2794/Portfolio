using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace zookeeper
{
    class Player
    {
        int score;
        int timer;
        int hints;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }
        

        public int Timer
        {
            get { return timer; }
            set { timer = value; }
        }
        
  
        public int Hints
        {
            get { return hints; }
            set { hints = value; }
        }
        public Player()
        {
           
            

        }
        public void IncreaseScore()
        {
            score = score + 5;
        }
        public void DecreaseTimer()
        {
            timer = timer - 1;
        }
        public void Draw(SpriteBatch spriteBacth,      SpriteFont font)
        {
            spriteBacth.DrawString(font, score.ToString(), new Vector2(400, 40), Color.Blue);
            spriteBacth.DrawString(font, timer.ToString() + " secs", new Vector2(395, 120), Color.FloralWhite);
            spriteBacth.DrawString(font, hints.ToString(), new Vector2(400, 230), Color.Red);
        }


    }
}
