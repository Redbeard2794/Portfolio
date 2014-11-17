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

namespace CityShooter
{
    class npcManager
    {
        public static String npcSymbol = "E";
        public static String npcOne = "E";
        static Model enemyTank;
        static Texture2D detectSquare;

        static Rectangle blockSize;
        

        public static GroundEnemy makeGroundEnemy(char c, Vector2 position)
        {
            //creates an enemy and sets its model, position and start position
            GroundEnemy enemy = new GroundEnemy(theGame);
            if (npcOne.Contains(c))
            {
                int i = npcOne.IndexOf(c);
                enemy.Model = enemyTank;
                enemy.DetectionSquare = detectSquare;
            }
            enemy.Pos = new Vector3(position.X * blockSize.Width +7.5f, 0, position.Y * blockSize.Height+7.5f);
            enemy.StartPos = enemy.Pos;
            enemy.Init();
            return enemy;
        }
        static Game theGame;
        public static void Init(Game game, Rectangle bSize)//, Rectangle bS)
        {
            theGame = game;
            LoadNpcModel();
            blockSize = bSize;
        }
        static void LoadNpcModel()
        {
            //loads the tank model
            ContentManager contentManger = (ContentManager)theGame.Services.GetService(typeof(ContentManager));
            enemyTank = contentManger.Load<Model>("tank");
            detectSquare = contentManger.Load<Texture2D>("detectSquare");
            
        }
        public String NpcSymbol
        {
            get { return npcSymbol; }
            set { npcSymbol = value; }
        }
    }
}
