using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airarreT
{
    public class Input
    {
        public static KeyboardState currentKeyState;
        static KeyboardState previousKeyState;

        public static MouseState currentMouseState;
        static MouseState previousMouseState;

        public static Rectangle mouseRect;
        public static Vector2 mousePosition;

        public static void SetState()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            var mousePos = Main.camera.ScreenToWorld(Mouse.GetState().Position.ToVector2());

            mouseRect = new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1);
            mousePosition = new Vector2((int)Mouse.GetState().Position.X, (int)Mouse.GetState().Position.Y);

        }

        public static bool GetKey(Keys key)
        {
            return currentKeyState.IsKeyDown(key);
        }

        public static bool GetKeyUp(Keys key)
        { 
            return currentKeyState.IsKeyUp(key) && !previousKeyState.IsKeyUp(key);
        }

        public static int ScrollWheel()
        {
            return currentMouseState.ScrollWheelValue;
        }

        public static int GetScrollWheel()
        {
            if (currentMouseState.ScrollWheelValue == previousMouseState.ScrollWheelValue)
                return 0;
            if (currentMouseState.ScrollWheelValue > previousMouseState.ScrollWheelValue)
                return 1;
            else
                return -1;
        }

        public static int HorizontalScrollWheel()
        {
            return currentMouseState.HorizontalScrollWheelValue;
        }

        public static int GetHorizontalScrollWheel()
        {
            if (currentMouseState.HorizontalScrollWheelValue == previousMouseState.HorizontalScrollWheelValue)
                return 0;
            if (currentMouseState.HorizontalScrollWheelValue > previousMouseState.HorizontalScrollWheelValue)
                return 1;
            else
                return -1;
        }

        public static bool GetKeyDown(Keys key)
        {
            return currentKeyState.IsKeyDown(key) && !previousKeyState.IsKeyDown(key);
        }
    }

    public enum Mouses
    {
        left,
        right,
        middle
    }
}
