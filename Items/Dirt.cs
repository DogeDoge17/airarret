using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airarreT.Items
{
    public class Dirt : Item
    {
        public override void SetDefaults()
        {
            displayName = "Dirt";

            width = 48;
            height = 34;

            consumable = true;

            useAnimation = 15;
            useTime = 15;
            useStyle = 1;
            useTurn = true;
            autoReuse = true;

            createTile = 0;
        }

       
    }
}