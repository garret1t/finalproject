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
        private SpellElement element;
        public SpellElement Element { get { return element; } }
        private Texture2D tex;
        public SpellFlash(SpellElement element)
        {
            tex = Game1.Instance.TextureDictionary["symbols." + element.Name.ToLower()];
            this.element = element;
            color = Element.SpellColor;
            color = new Color(MathHelper.Min(0, color.R - 30), MathHelper.Min(0, color.G - 30), MathHelper.Min(0, color.B - 30), MathHelper.Min(0,color.A-127));
        }
        float scaleFactor = 1f;
        float bgOpac = 0.5f;
        Color color;
        public override void Update(GameTime gameTime)
        {
            if (TicksAlive > 30)
            {
                Stop();
            }
            scaleFactor += 0.05f;

            if (bgOpac > 0)
            {
                bgOpac -= 0.05f;
            }
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            int dims = (int)(200 * scaleFactor);
            spriteBatch.Draw(Game1.Instance.blank, new Rectangle(0, 0, Game1.Instance.GraphicsDevice.Viewport.Width, Game1.Instance.GraphicsDevice.Viewport.Height), Element.SpellColor * bgOpac);
            spriteBatch.Draw(tex, new Rectangle(
                (800 - dims) / 2,
                (800 - dims) / 2,
                dims,
                dims)
                , null, Color.White * (float)((30f - (float)TicksAlive) / 30f));

            base.Draw(spriteBatch);
        }
    }
}
