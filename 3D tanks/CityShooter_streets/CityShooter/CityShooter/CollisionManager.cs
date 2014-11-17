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
    class CollisionManager
    {
        //collision method for 2 models
        //done with bounding spheres
        public bool ModelCollision(Model model1, Matrix world1, Model model2, Matrix world2)
        {
            for (int meshIndex1 = 0; meshIndex1 < model1.Meshes.Count; meshIndex1++)
            {
                BoundingSphere sphere1 = model1.Meshes[meshIndex1].BoundingSphere;
                sphere1 = sphere1.Transform(world1);

                for (int meshIndex2 = 0; meshIndex2 < model2.Meshes.Count; meshIndex2++)
                {
                    BoundingSphere sphere2 = model2.Meshes[meshIndex2].BoundingSphere;
                    sphere2 = sphere2.Transform(world2);

                    if (sphere1.Intersects(sphere2))
                        return true;
                }
            }
            return false;
        }
        //collision between model and building
        //done with a bounding sphere for model and bounding box for building
        public bool ModelBuildingCollision(Model model1, Matrix world1, Vector3 buildPos, float buildWidth, float buildHeight)
        {
            for (int meshIndex1 = 0; meshIndex1 < model1.Meshes.Count; meshIndex1++)
            {
                BoundingSphere sphere1 = model1.Meshes[meshIndex1].BoundingSphere;
                sphere1 = sphere1.Transform(world1);
                float Width = buildWidth;
                float Height = buildHeight;
                Vector3 CenterOfBox = new Vector3(buildPos.X + (buildWidth/2), 0, buildPos.Z +(buildWidth/2));//new Vector3(Width/2,0,Width/2);
                BoundingBox box1 = new BoundingBox(CenterOfBox - new Vector3(Width / 2, Height / 2, Width / 2), CenterOfBox + new Vector3(Width / 2, Height / 2, Width / 2));


                if (sphere1.Intersects(box1))
                {
                    return true;
                }
            }
            return false;
        }



    }
}
