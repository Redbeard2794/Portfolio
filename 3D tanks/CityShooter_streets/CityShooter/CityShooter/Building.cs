using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace CityShooter
{
    class BuildingFactory
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


        static Texture2D oneStoreyBuild;
        static Texture2D twoStoreyBuild;
        static Texture2D threeStoreyBuild;
        static Texture2D fourStoreyBuild;
        static Texture2D fiveStoreyBuild;
        static Texture2D sixStoreyBuild;
        static Texture2D sevenStoreyBuild;
        static Texture2D nineStoreyBuild;

        static Rectangle blockSize;



        public static Building makeBuilding(char c, Vector2 position)
        {
            Building b = new Building(theGame);
            int buildingType = buildingSymbols.IndexOf(c);
            //determines which building is required and sets
            //its texture and height
            if (oneStorey.Contains(c))
            {
                int i = oneStorey.IndexOf(c);
                b.Texture = oneStoreyBuild;
                b.BuildingHeight = 10;
            }
            if (twoStorey.Contains(c))
            {
                int i = twoStorey.IndexOf(c);
                b.Texture = twoStoreyBuild;
                b.BuildingHeight = 20;
            }
            if (threeStorey.Contains(c))
            {
                int i = threeStorey.IndexOf(c);
                b.Texture = threeStoreyBuild;
                b.BuildingHeight = 30;
            }
            if (fourStorey.Contains(c))
            {
                int i = fourStorey.IndexOf(c);
                b.Texture = fourStoreyBuild;
                b.BuildingHeight = 40;
            }
            if (fiveStorey.Contains(c))
            {
                int i = fiveStorey.IndexOf(c);
                b.Texture = fiveStoreyBuild;
                b.BuildingHeight = 50;
            }
            if (sixStorey.Contains(c))
            {
                int i = sixStorey.IndexOf(c);
                b.Texture = sixStoreyBuild;
                b.BuildingHeight = 60;
            }
            if (sevenStorey.Contains(c))
            {
                int i = sevenStorey.IndexOf(c);
                b.Texture = sevenStoreyBuild;
                b.BuildingHeight = 70;
            }
            if (nineStorey.Contains(c))
            {
                int i = nineStorey.IndexOf(c);
                b.Texture = nineStoreyBuild;
                b.BuildingHeight = 90;
            }
            //set the position of each building
            b.Position = new Vector3(position.X * blockSize.Width, 0, position.Y * blockSize.Height);
            b.Size = blockSize;
            b.Init();
            return b;
        }
        static Game theGame;

        public static void Init(Game game, Rectangle bS)
        {
            theGame = game;
            LoadBuildingTextures();
            blockSize = bS;
        }
        //loads the textures used for the buildings
        static void LoadBuildingTextures()
        {
            ContentManager contentManger = (ContentManager)theGame.Services.GetService(typeof(ContentManager));

            oneStoreyBuild = contentManger.Load<Texture2D>("building1");
            twoStoreyBuild = contentManger.Load<Texture2D>("building2");
            threeStoreyBuild = contentManger.Load<Texture2D>("building3");
            fourStoreyBuild = contentManger.Load<Texture2D>("building8");
            fiveStoreyBuild = contentManger.Load<Texture2D>("building7");
            sixStoreyBuild = contentManger.Load<Texture2D>("building6");
            sevenStoreyBuild = contentManger.Load<Texture2D>("building7");
            nineStoreyBuild = contentManger.Load<Texture2D>("building8");
        }

    }





    class Building: GameObject
    {

        float rotation;
        Texture2D texture;
        Matrix rotationMatrix;
        float buildingHeight;

        Vector3 col = new Vector3(1, 1, 1);

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
        public Vector3 Col
        {
            get { return col; }
            set { col = value; }
        }

        public Building(Game game)
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

            numVertices = 16;
            vertices = new VertexPositionNormalTexture[numVertices];

            //sets the position of each vertex and also sets its texture coordinate

            //for left side of building
            vertices[0].Position = new Vector3(Position.X, 0, Position.Z + size.Height);
            vertices[0].TextureCoordinate = new Vector2(1, 1);

            vertices[1].Position = new Vector3(Position.X, 0, Position.Z);
            vertices[1].TextureCoordinate = new Vector2(0, 1);

            vertices[2].Position = new Vector3(Position.X, buildingHeight, Position.Z);
            vertices[2].TextureCoordinate = new Vector2(0, 0);

            vertices[3].Position = new Vector3(Position.X, buildingHeight, Position.Z + size.Height);
            vertices[3].TextureCoordinate = new Vector2(1, 0);

            //for right of building
            vertices[4].Position = new Vector3(Position.X + size.Width, 0, Position.Z + size.Height);
            vertices[4].TextureCoordinate = new Vector2(0, 1);

            vertices[5].Position = new Vector3(Position.X + size.Width, 0, Position.Z);
            vertices[5].TextureCoordinate = new Vector2(1, 1);

            vertices[6].Position = new Vector3(Position.X + size.Width, buildingHeight, Position.Z);
            vertices[6].TextureCoordinate = new Vector2(1, 0);

            vertices[7].Position = new Vector3(Position.X + size.Width, buildingHeight, Position.Z + size.Height);
            vertices[7].TextureCoordinate = new Vector2(0, 0);

            //for front
            vertices[8].Position = new Vector3(Position.X, 0, Position.Z);
            vertices[8].TextureCoordinate = new Vector2(1,1);

            vertices[9].Position = new Vector3(Position.X + size.Width, 0, Position.Z);
            vertices[9].TextureCoordinate = new Vector2(0, 1);

            vertices[10].Position = new Vector3(Position.X + size.Width, buildingHeight, Position.Z);
            vertices[10].TextureCoordinate = new Vector2(0, 0);

            vertices[11].Position = new Vector3(Position.X, buildingHeight, Position.Z);
            vertices[11].TextureCoordinate = new Vector2(1, 0);

            //for back
            vertices[12].Position = new Vector3(Position.X, 0, Position.Z + size.Height);
            vertices[12].TextureCoordinate = new Vector2(0, 1);

            vertices[13].Position = new Vector3(Position.X + size.Width, 0, Position.Z + size.Height);
            vertices[13].TextureCoordinate = new Vector2(1, 1);

            vertices[14].Position = new Vector3(Position.X + size.Width, buildingHeight, Position.Z + size.Height);
            vertices[14].TextureCoordinate = new Vector2(1, 0);

            vertices[15].Position = new Vector3(Position.X, buildingHeight, Position.Z + size.Height);
            vertices[15].TextureCoordinate = new Vector2(0, 0);


            indices = new short[24];//36
            //left
            indices[0] = 0;//0
            indices[1] = 1;//1
            indices[2] = 2;//2
            indices[3] = 2;//0
            indices[4] = 3;//2
            indices[5] = 0;//3
            //right
            indices[6] = 4;
            indices[7] = 5;
            indices[8] = 6;
            indices[9] = 6;
            indices[10] = 7;
            indices[11] = 4;
            //front
            indices[12] = 8;
            indices[13] = 9;
            indices[14] = 10;
            indices[15] = 10;
            indices[16] = 11;
            indices[17] = 8;
            //back
            indices[18] = 12;
            indices[19] = 13;
            indices[20] = 14;
            indices[21] = 14;
            indices[22] = 15;
            indices[23] = 12;

            Vector3 centreDisplace = position + new Vector3(size.Width / 2.0f, 0, size.Height / 2.0f);

            rotationMatrix = Matrix.CreateTranslation(-centreDisplace) * Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(centreDisplace);


        }

        public override void Draw(GameTime gametime, Camera camera)
        {
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.World = rotationMatrix;
            effect.Texture = texture;



            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                effect.LightingEnabled = true;

                effect.EmissiveColor = col;
                pass.Apply();
                graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, vertices, 0, numVertices, indices, 0, 8);
            }
        }
    }
   }
