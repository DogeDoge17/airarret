using airarreT.Collision;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airarreT.Dusts
{
    public class Dust : Loop
    {
        public Vector2 velocity = new Vector2();

        public Vector2 position = new Vector2();

        DynamicCollider collider;

        public int width;
        public int height;


        public Dust()
        {
            collider = new DynamicCollider(new DynamicRectangle(position, new Vector2(width, height)));
        }

        public override void Update()
        {
            CollisionManagement.CheckCollision(collider);

            base.Update();
        }

        public static void CreateDust(Dust type )
        {


        }
    }
}
