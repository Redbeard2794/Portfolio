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

namespace RobotTank
{
    class Camera
    {
        Matrix cameraTransform;//cameras transform
        Vector2 centre;//center of camera
        float zPos;
        Viewport view;//for viewport
        float zoom;//used for the scaling matrix
        Vector2 worldMinLimit;
        Vector2 worldMaxLimit;
        //start properties
        public Matrix CameraTransform
        {
            get { return cameraTransform;  }
            set { cameraTransform = value; }
        }
        //end properties

        //constructor
        public Camera(Viewport viewport, Texture2D back)
        {
            zoom = 1.5f;//final value = 2.0f
            view = viewport;
            zPos = 0;

            //for stopping the camera going over the edge of the map

            float width = back.Width;//gets the width
            float height = back.Height;//gets the height
            worldMinLimit = new Vector2(back.Bounds.Left - 140, back.Bounds.Top - 170);//stops the camera going off the top and left edge of the map
            worldMaxLimit = new Vector2(back.Bounds.Right-720, back.Bounds.Bottom-500);//stops the camera going off the bottom and right edge of the map
        }
        //update method. sets up the camera to zoom in and focus on the player
        public void Update(KeyboardState kInput,  Vector2 playerPos)
        {
            //centres the camera on the player
            centre = new Vector2(playerPos.X, playerPos.Y);

            //clamp the camera to the world limits
            centre = Vector2.Clamp(centre, worldMinLimit, worldMaxLimit);

            //transform for the camera. translates to its centre and scales everything it is apllied to
            cameraTransform = Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0)) * Matrix.CreateScale(new Vector3(zoom, zoom, zPos)) * Matrix.CreateTranslation(new Vector3(view.Width / 2, view.Height / 2, 0));
        }
        
    }
}
