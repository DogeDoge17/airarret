using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airarreT.Tiles
{
    public class PlantTile : Tile
    {

        public int plantType = 0;

        public override void SetDefaults()
        {
            plantType = Main.rand.Next(0, 45);

            sourceRec = new Rectangle(plantType * 18, 0, 18,22);
            id = 3;

            placeOver = true;
            passthrough = true;
            ignoredNeighbor = true;

            base.SetDefaults();
        }



        public override void ChangeSource()
        {
            if (!bN)
                Main.RemoveTile(this);


            //base.ChangeSource();
        }
    }
}
