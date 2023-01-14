using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace airarreT
{
    public abstract class Loop
    {
        public bool ingoreCamera;

        public Loop()
        {
            Main.loops.Add(this);
        }

        public virtual void Update()
        {
          
        }

        public virtual void Draw()
        {

        }

        public virtual void FixedUpdate()
        {

        }

    }
}
