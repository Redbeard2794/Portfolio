using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CityShooter
{
    public class Camera
    {
        Vector3 pos;
        Vector3 dir;
        Vector3 up;

        float fieldOfView;
        float aspectRatio;
        float near;
        float far;

        Matrix view;

        //for keeping track of which state the camera is in
        const byte orthographic = 0, firstPerson = 1, thirdPerson = 2;
        int camState = 2;

        Vector3 camPos;

        public Matrix View
        {
            get { return view; }
            set { view = value; }
        }

        Matrix projection;
        public int CamState
        {
            get { return camState; }
            set { camState = value; }
        }
        public Matrix Projection
        {
            get { return projection; }
            set { projection = value; }
        }
        public Vector3 Position
        {
            get { return camPos; }
            set { camPos = value; }
        }
        public Vector3 Up
        {
            get { return up; }
            set { up = value; }
        }
        public Vector3 Forward
        {
            get { return Vector3.Forward; }
            
        }
        float speed = 0.050f;
        float angVel = 0.0025f; // set max angular velocity for rotating camera

        public void Init(Vector3 p, Vector3 lookat, Vector3 u,float FOV,float ar,float n, float f)
        {
            pos = p;
            dir = (lookat - p);
            dir.Normalize();
            up = u;

            fieldOfView = FOV;
            aspectRatio = ar;
            near = n;
            far = f;
            UpdateView();
            UpdateProj();
        }

        void UpdateView()
        {
            view= Matrix.CreateLookAt(pos, pos + dir, up);
        }
        void UpdateProj()
        {
            projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio,near, far);

        }
        public void updateCameraState(Vector3 playerPos, Vector3 playerDirection, Vector3 forwardVect, Vector3 turretDirection, Matrix turretRotation, Matrix tankRotation)
        {
            switch (camState)
            {
                case orthographic:
                    view = Matrix.CreateLookAt(camPos, new Vector3(75, 0, 75), Vector3.Forward);
                    projection = Matrix.CreateOrthographic(300, 150, near, far);

                    break;
                case firstPerson:
                    Matrix fullRotation;                                               //x is the depth

                    Vector3 direction = new Vector3(0, 0, 1);
                    //works out the tanks rotation * the turret rotation so the entire
                    //rotation is taken into account for positioning the camera
                    fullRotation = turretRotation * tankRotation;
                    //gives the camera the way it sould be pointing
                    Vector3 tankDir = Vector3.Transform(direction, fullRotation);
                                //position, target, up
                    camPos = new Vector3(playerPos.X - tankDir.X, playerPos.Y + 2, playerPos.Z - tankDir.Z);
                    view = Matrix.CreateLookAt(camPos, new Vector3(playerPos.X + tankDir.X, playerPos.Y + 2, playerPos.Z + tankDir.Z), Vector3.Up);
                    UpdateProj();
                    break;
                case thirdPerson:
                    view = Matrix.CreateLookAt(camPos, new Vector3(playerPos.X, playerPos.Y, playerPos.Z), up);
                    UpdateProj();
                    break;
            }
        }


        public void Update(GameTime gameTime, Vector3 playerPos, Vector3 tankDirection, Vector3 turretDirection, Matrix turretTransform)
        {
            KeyboardState kb = Keyboard.GetState();

            float distanceTravelled = speed * gameTime.ElapsedGameTime.Milliseconds;
            
            Vector3 right = Vector3.Cross(dir, up);
            //updates the position of the camera in thirdPerson and orthographic
            //first person does this itself
            if (camState == thirdPerson)
            {
                camPos = new Vector3(playerPos.X, playerPos.Y + 3.5f, playerPos.Z) - (-tankDirection * 10);//fiddle with 4. maybe dir = negative?
            }
            if (camState == orthographic)
            {
                camPos = new Vector3(75, 250, 75);
            }

            if (kb.IsKeyDown(Keys.O))
                camState = orthographic;
            if (kb.IsKeyDown(Keys.D1))
                camState = firstPerson;
            if (kb.IsKeyDown(Keys.D3))
                camState = thirdPerson;

            UpdateView();
            UpdateProj();
        }




    }
}
