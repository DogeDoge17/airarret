using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airarreT.Walls
{
    public class WoodWallWall : Wall
    {
        public override void SetDefaults()
        {
            sourceRec = new Rectangle(18, 0, 16, 16);
            id = 4;

            base.SetDefaults();
        }

    }
}
