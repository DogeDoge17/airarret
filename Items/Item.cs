using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace airarreT.Items
{
    public class Item : Loop
    {
        public string displayName = "";

        public int width = 16;
        public int height = 16;

        public bool consumable = false;
        public bool useTurn = true;
        public bool autoReuse = false;

        public int useAnimation = 0;
        public int useTime = 0;
        public int useStyle = 0;

        public int maxStack = 9999;

        public int createTile = -1;

        public Item()
        {
            displayName = GetType().Name;
            SetDefaults();
        }

        public virtual void SetDefaults()
        {
           
        }

        public override void Update()
        {

        }
    }
}
