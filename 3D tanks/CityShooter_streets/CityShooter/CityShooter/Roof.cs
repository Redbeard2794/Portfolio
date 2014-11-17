using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace CityShooter
{
    class RoofFactory
    {
        public static String buildingSymbols = "1, 2, 3, 4, 5, 6, 7, 9";
        public static String oneStorey = "1";
        public static String twoStorey = "2";
        public static String threeStorey = "3";
        public static String fourStorey = "4";
        public static String fiveStorey = "5";
        public static String sixStorey = "6";
        public static String sevenStorey = "7";
        public static String nineStorey = "9";

        static Texture2D roof1;
        static Texture2D roof2;
        static Texture2D roof3;
        static Texture2D roof4;

        static Rectangle blockSize;

        public static Roof makeBuilding(char c, Vector2 position)
        {
            Roof r = new Roof(theGame);
            int buildingType = buildingSymbols.IndexOf(c);

            //determines which type of roof is needed and 
            //where it is positioned and how high up it is
            if (oneStorey.Contains(c))
            {
                int i = oneStorey.IndexOf(c);
                r.Texture = roof1;
                r.BuildingHeight = 10;
            }
            if (twoStorey.Contains(c))
            {
                int i = twoStorey.IndexOf(c);
                r.Texture = roof1;
                r.BuildingHeight = 20;
            }
            if (threeStorey.Contains(c))
            {
                int i = threeStorey.IndexOf(c);
                r.Texture = roof3;
                r.BuildingHeight = 30;
            }
            if (fourStorey.Contains(c))
            {
                int i = fourStorey.IndexOf(c);
                r.Texture = roof4;
                r.BuildingHeight = 40;
            }
            if (fiveStorey.Contains(c))
            {
                int i = fiveStorey.IndexOf(c);
                r.Texture = roof4;
                r.BuildingHeight = 50;
            }
            if (sixStorey.Contains(c))
            {
                int i = sixStorey.IndexOf(c);
                r.Texture = roof1;
                r.BuildingHeight = 60;
            }
            if (sevenStorey.Contains(c))
            {
                int i = sevenStorey.IndexOf(c);
                r.Texture = roof2;
                r.BuildingHeight = 70;
            }
            if (nineStorey.Contains(c))
            {
                int i = nineStorey.IndexOf(c);
                r.Texture = roof3;
                r.BuildingHeight = 90;
            }

            r.Position = new Vector3(position.X * blockSize.Width, 0, position.Y * blockSize.Height);
            r.Size = blockSize;
            r.Init();
            return r;
        }

        static Game theGame;

        public static void Init(Game game, Rectangle bS)
        {
            theGame = game;
            LoadBuildingTextures();
            blockSize = bS;
        }

        static void LoadBuildingTextures()
        {
            //load the textures for the roof
            ContentManager contentManger = (ContentManager)theGame.Services.GetService(typeof(ContentManager));
            roof1 = contentManger.Load<Texture2D>("roof1");
            roof2 = contentManger.Load<Texture2D>("roof2");
            roof3 = contentManger.Load<Texture2D>("roof3");
            roof4 = contentManger.Load<Texture2D>("roof4");
        }

    }

    class Roof : GameObject
    {
        float rotation;
        Texture2D texture;
        Matrix rotationMatrix;
        Texture2D roofTexture;
        float buildingHeight;

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public Texture2D RoofTexture
        {
            get { return roofTexture; }
            set { roofTexture = value; }
        }
        public float BuildingHeight
        {
            get { return buildingHeight; }
            set { buildingHeight = value; }
        }

        Rectangle size;
        public Rectangle Size
        {
            get { return size; }
            set { size = value; }
        }

        public Roof(Game game)
            : base(game)
        {

        }

        VertexPositionNormalTexture[] vertices;
        short[] indices;
        int numVertices;

        public override void Init()
        {
            effect = new BasicEffect(graphicsDevice);
            effect.VertexColorEnabled = false;
            effect.TextureEnabled = true;

            numVertices = 4;
            vertices = new VertexPositionNormalTexture[numVertices];
            //gives each vertex its position and texture coordinate

            vertices[0].Position = new Vector3(Position.X, buildingHeight, Position.Z);
            vertices[0].TextureCoordinate = new Vector2(0, 0);

            vertices[1].Position = new Vector3(Position.X + size.Width, buildingHeight, Position.Z);
            vertices[1].TextureCoordinate = new Vector2(1, 0);

            vertices[2].Position = new Vector3(Position.X + size.Width, buildingHeight, Position.Z + size.Height);
            vertices[2].TextureCoordinate = new Vector2(1, 1);

            vertices[3].Position = new Vector3(Position.X, buildingHeight, Position.Z + size.Height);
            vertices[3].TextureCoordinate = new Vector2(0, 1);

            //joins the vertices up to make a square
            indices = new short[6];
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 2;
            indices[4] = 3;
            indices[5] = 0;

            Vector3 centreDisplace = position + new Vector3(size.Width / 2.0f, 0, size.Height / 2.0f);

            rotationMatrix = Matrix.CreateTranslation(-centreDisplace) * Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(centreDisplace);
        }
        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gametime, Camera camera)
        {
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.World = rotationMatrix;
            effect.Texture = texture;


            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, numVertices, indices, 0, 2);
            }
        }
    }
}
