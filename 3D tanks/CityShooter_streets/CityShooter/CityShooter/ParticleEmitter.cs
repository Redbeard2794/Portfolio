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
    class ParticleEmitter
    {
        //https://gist.github.com/kenpower/9328104
        //only take any code that applies to billboards
        int spawnRate;
        Vector3 position;
        Vector3 particleInitialVelocity;
        int particleLifeTime;
        bool spawnParticles;
        List<SmokeParticle> smokeParticles;

        public ParticleEmitter(Vector3 modelPos)
        {
            //sets spawn rate of particles, position, velocity
            spawnRate = 100;
            position = new Vector3(modelPos.X-11, modelPos.Y, modelPos.Z);
            particleInitialVelocity = new Vector3(0,4,0);
            particleLifeTime = 360;
            spawnParticles = false;
            smokeParticles = new List<SmokeParticle>();
        }
        public void Update(Vector3 modelPos, ContentManager c, GraphicsDevice graphics)
        {
            //position = modelPos;
            //trying to get the emitter in the right position
            //does not work!!
            position = new Vector3(modelPos.X-11, modelPos.Y, modelPos.Z);
            if (spawnParticles == true)
            {
                spawnRate--;
                if (spawnRate <= 0)
                {
                    smokeParticles.Add(new SmokeParticle(position, particleInitialVelocity, particleLifeTime, c, graphics));
                    spawnRate = 200;
                }
            }
            if (smokeParticles.Count > 0)
            {
                for (int i = 0; i < smokeParticles.Count; i++)
                {
                    if (smokeParticles[i].Lifetime != 0)
                        smokeParticles[i].update();
                    else smokeParticles.RemoveAt(i);
                }
            }

        }
        public void Draw(GameTime gametime, Camera camera, GraphicsDevice graphics)
        {
            if (spawnParticles == true)
            {
                for (int i = 0; i < smokeParticles.Count; i++)
                {
                    if(smokeParticles[i].Lifetime != 0)
                        smokeParticles[i].Draw(gametime, camera, graphics);
                }
            }
        }
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        public bool SpawnParticles
        {
            get { return spawnParticles; }
            set { spawnParticles = value; }
        }
    }
}
