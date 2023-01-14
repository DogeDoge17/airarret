using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace airarreT.Tiles
{
    public class DirtTile : Tile
    {
        public override void SetDefaults()
        {
            sourceRec = new Rectangle(18, 0, 16, 16);
            id = 0;

            base.SetDefaults();

        }


    }
}
