using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project.Animations
{
    public class SpellFlash : PrefabAnimation
    {
#if DEBUG
        private bool debug = true;
#endif
        private SpellElement element;
        public SpellElement Element { get { return element; } }
        private Texture2D tex;
        public SpellFlash(SpellElement element)
        {
            tex = Game1.Instance.TextureDictionary["symbols." + element.Name.ToLower()];
            this.element = element;
        }
        float scaleFactor = 1f;
        public override void Update(GameTime gameTime)
        {
            if (TicksAlive > 30)
            {
                Stop();
            }
            scaleFactor += 0.05f;
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            int dims = (int)(200 * scaleFactor);
            spriteBatch.Draw(tex, new Rectangle(
                (800-dims)/2,
                (800-dims)/2,
                dims,
                dims)
                , null, Color.White * (float)((30f - (float)TicksAlive) / 30f));
            base.Draw(spriteBatch);
        }
    }
}
