using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project.Animations
{
    public class PrefabAnimation
    {
        private int ticks = 0;
        private bool needsStop = false;
        public int TicksAlive { get { return ticks; } }
        public bool NeedsRemove { get { return needsStop; } }
        public virtual void Update(GameTime gameTime)
        {
            ticks++;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }
        public void Stop()
        {
            needsStop = true;
        }
    }
}
