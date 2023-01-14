using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airarreT
{
    public class Time
    {
        public static float deltaTime { get; private set; }
        public static float gameDelta { get; private set; }
        public static float worldDelta { get; private set; }
        public static float framerate { get; private set; }

        public static float worldTimeModifier = 1;
        public static float gameTimeModifier = 1;


        private static int frameCounter = 0;
        private static float timeCounter = 0.0f;
        private static float refreshTime = 0.5f;

        public static void SetValues(GameTime gameTime)
        {
            #region Delta Times
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            worldDelta = (float)gameTime.ElapsedGameTime.TotalSeconds * worldTimeModifier;
            gameDelta = (float)gameTime.ElapsedGameTime.TotalSeconds * gameTimeModifier;
            #endregion

            #region Framerate
            if (timeCounter < refreshTime)
            {
                timeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
                frameCounter++;
            }
            else
            {
                //This code will break if you set your refreshTime to 0, which makes no sense.
                framerate = (float)frameCounter / timeCounter;
                frameCounter = 0;
                timeCounter = 0.0f;
            }
            #endregion
        }
    }
}
