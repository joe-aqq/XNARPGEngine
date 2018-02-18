using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGEngine.Managers;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace RPGEngine.Cameras
{
    public static class Camera
    {
        #region Variables
        private static Matrix transform; //The transform matrix of the camera
        private static Vector2 center, center_p; //The center and previous center of the camera
        private static Viewport screen; //The viewport that the camera is projecting to, through MonoGame
        private static float zoom; //The zoom amount of the camera
        private static float max_zoom, min_zoom; //The max and min zoom of the camera
        private static float rotation; //The rotation of the camera
        private static Random r; //Random value for the shake effect
        private static int shakeWeight = 2; //The power of the shaking for the camera
        private static CameraState state;

        private enum CameraState
        {
            normal,
            shaking
        }


        #endregion

        /// <summary>
        /// Main camera class. Controls all viewport actions
        /// </summary>
        /// <param name="v">Viewport of the game.</param>
        static Camera() { }

        /// <summary>
        /// Initialization of the camera.
        /// </summary>
        /// <param name="v"></param>
        public static void Init(Viewport v)
        {
            zoom = 1f;
            max_zoom = .1f;
            min_zoom = 5f;
            rotation = 0f;
            screen = v;
            r = new Random();
            state = CameraState.normal;
        }

        /// <summary>
        /// Updates the camera based on the position of the focus.
        /// </summary>
        /// <param name="position">The position of the focus.</param>
        public static void Update(Vector2 position)
        {
            center_p = center;
            center = position;
            Vector2 pos = Vector2.Lerp(center_p, center, .002f);

            if(InputManager.Clicked(Keys.L))
            {
                switch (state) { case CameraState.normal: state = CameraState.shaking; break; case CameraState.shaking: state = CameraState.normal; break; }
                Debug.WriteLine("Toggled Shake, state: " + state);
            }

            switch (state)
            {
                case (CameraState.normal):
                    transform = Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0f)) *
                        Matrix.CreateRotationZ(rotation) *
                        Matrix.CreateScale(zoom) *
                        Matrix.CreateTranslation(new Vector3(screen.Width / 2f, screen.Height / 2f, 0f));
                    break;
                case (CameraState.shaking):
                    transform = Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0f) + new Vector3((float)r.Next(0, shakeWeight), (float)r.Next(0, shakeWeight), 0f)) *
                        Matrix.CreateRotationZ(rotation) *
                        Matrix.CreateScale(zoom) *
                        Matrix.CreateTranslation(new Vector3(screen.Width / 2f, screen.Height / 2f, 0f));
                    break;
            }

        }

        /// <summary>
        /// Projects a vector to the screen position.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Vector2 ProjectToScreen(Vector2 v)
        {
            return new Vector2(
                center.X - (float)Math.Round(RPGEngineMain.WindowWidth/4.0) + v.X,
                center.Y - (float)Math.Round(RPGEngineMain.WindowHeight/4.0) + v.Y);
        }

        /// <summary>
        /// Returns the transform matrix of the camera.
        /// </summary>
        public static Matrix Transform
        {
            get { return transform; }
        }

        /// <summary>
        /// Returns the center X of the camera.
        /// </summary>
        public static float X
        {
            get { return center.X; }
            set { center.X = value; }
        }

        /// <summary>
        /// Returns the center Y of the camera.
        /// </summary>
        public static float Y
        {
            get { return center.Y; }
            set { center.Y = value; }
        }

        public static Vector2 ScreenPosition
        {
            get { return new Vector2(center.X - (RPGEngineMain.WindowWidth / 2), center.Y - (RPGEngineMain.WindowHeight / 2)); }
        }

        /// <summary>
        /// Returns and sets the zoom of the camera. Keep at 1 most of the time, gamers.
        /// </summary>
        public static float Zoom
        {
            get { return Zoom; }
            set
            {
                zoom = value;
                limit(zoom, max_zoom, min_zoom);
            }
        }

        /// <summary>
        /// Returns and sets the cameras rotation. Don't touch thpublic staticis buddy.
        /// </summary>
        public static float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        /// <summary>
        /// Limit a float value for convience. 
        /// </summary>
        /// <param name="a">The passed in float.</param>
        /// <param name="mx">the max it limits to.</param>
        /// <param name="mn">The minimum it limits to.</param>
        /// <returns></returns>
        public static float limit(float a, float mx, float mn)
        {
            if (a > mx) a = mx;
            if (a < mn) a = mn;
            return a;
        }
    }
}
