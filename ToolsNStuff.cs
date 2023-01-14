using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airarreT
{
    public static class ToolsNStuffS
    {
        public static Vector2 Center(this Vector2 ob, Vector2 size)
        {
            return new Vector2(ob.X + (size.X / 2), ob.Y + (size.Y / 2));
        }

        public static Vector2 MoveTowards(this Vector2 v0, Vector2 v1, float t)
        {
            if (Vector2.Distance(v0, v1) < 0.4)
                return v1;

            return new Vector2(t) * (v1 - v0);
        }

        public static float MoveTowards(this float v0, float v1, float t)
        {
            return  t*(v1 - v0);
        }

        public static float Lerp(this float v0, float v1, float t)
        {
            return v0 + t * (v1 - v0);
        }
        public static Vector2 Lerp(this Vector2 v0, Vector2 v1, float t, Vector2 snap)
        {
            return v0 + new Vector2(t) * (v1 - v0);
        }
    }

    public class ToolsNStuff
    {
        public static Vector2 Center(Vector2 ob, Vector2 size)
        {
            return new Vector2(ob.X + (size.X / 2), ob.Y + (size.Y / 2));
        }

        public static Vector2 TopLeft(Vector2 ob, Vector2 size)
        {

           return new Vector2(ob.X + (size.X / 2), ob.Y + (size.Y / 2));
        }

        public static float Lerp(float v0, float v1, float t)
        {
            return v0 + t * (v1 - v0);
        }
        public static Vector2 Lerp(Vector2 v0, Vector2 v1, float t)
        {
            //if()

            return v0 + new Vector2(t) * (v1 - v0);
        }

        public static Vector2 MoveTowards(Vector2 v0, Vector2 v1, float t)
        {
            return new Vector2(t) * (v1 - v0);
        }

        public static float MoveTowards(float v0, float v1, float t)
        {
            return t * (v1 - v0);
        }


        public static int ClosestMultiple(int n, int x)
        {
            if (x > n)
                return x;
            n = n + x / 2;
            n = n - (n % x);
            return n;
        }

        public static int FloorMultiple(int value, int factor)
        {
            return (int)Math.Floor( (value / (double)factor)) * factor;
        }

    }
}
