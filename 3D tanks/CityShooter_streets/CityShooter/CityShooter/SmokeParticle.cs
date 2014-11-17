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

namespace CityShooter
{
    class SmokeParticle
    {
        Texture2D texture;
        float angle;
        Vector3 position;
        Vector3 velocity;
        int lifetime;

        Matrix world;
        VertexPositionTexture[] boardVerts = new VertexPositionTexture[4];

        BasicEffect basicEffect;

        public SmokeParticle(Vector3 emitterPos, Vector3 initVelocity, int life, ContentManager c, GraphicsDevice graphics)
        {
            position = emitterPos;
            velocity = initVelocity;
            lifetime = life;
            angle = 0;
            LoadContent(c, graphics);
        }
        public void LoadContent(ContentManager c, GraphicsDevice graphics)
        {
            texture = c.Load<Texture2D>("particles_smoke");
            basicEffect = new BasicEffect(graphics);
            basicEffect.VertexColorEnabled = true;

             //create our quad
            //give each vertex texture coordinates

            boardVerts[0].Position = new Vector3(position.X, 5, position.Z);
            boardVerts[0].TextureCoordinate = new Vector2(0, 0);

            boardVerts[1].Position = new Vector3(position.X, 0, position.Z);
            boardVerts[1].TextureCoordinate = new Vector2(0, 1);

            boardVerts[2].Position = new Vector3(position.X + 5, 5, position.Z);
            boardVerts[2].TextureCoordinate = new Vector2(1, 0);

            boardVerts[3].Position = new Vector3(position.X + 5, 0, position.Z);
            boardVerts[3].TextureCoordinate = new Vector2(1, 1);
        }
        public void update()
        {
            Random rand = new Random();
            lifetime--;
            //if (lifetime != 0)
            //{
                position += velocity;
                if (lifetime % 2 == 0)
                {
                    //velocity.X = rand.Next(0, 2);
                    //if (velocity.X == 0)
                    //    velocity.Z = rand.Next(0, 2);
                }
            //}
            angle += 0.1f;
        }
        public void Draw(GameTime gametime, Camera camera, GraphicsDevice graphics)
        {
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            graphics.RasterizerState = rs;

            basicEffect.Projection = camera.Projection;
            basicEffect.View = camera.View;
            basicEffect.World = Matrix.Identity;


            basicEffect.TextureEnabled = true;
            basicEffect.VertexColorEnabled = false;
                                                                                                //prblem is here
            basicEffect.World = Matrix.CreateBillboard(new Vector3(position.X, 3, position.Z), camera.Position, camera.Up, camera.Forward);


            
            basicEffect.Texture = texture;
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphics.DrawUserPrimitives(PrimitiveType.TriangleStrip, boardVerts, 0, 2);
            }
            
        }
        public int Lifetime
        {
            get { return lifetime; }
            set { lifetime = value; }
        }
    }
}
