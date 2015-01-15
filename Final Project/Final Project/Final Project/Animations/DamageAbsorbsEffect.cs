using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project.Animations
{
    public class DamageAbsorbsEffect : PrefabAnimation
    {
        LivingEntity orbiter;
        float offset = 0f;
        public DamageAbsorbsEffect(LivingEntity target)
        {
            orbiter = target;           
        }

        List<Rectangle> Objects = new List<Rectangle>();

        public override void Update(GameTime gameTime)
        {
            offset += 1f;
            if (offset == 361f) offset = 0f;

            float offsetRadian = MathHelper.ToRadians(offset);

            if (orbiter.DamageAbsorbs > Objects.Count)
            {
                Objects.Add(new Rectangle());
            }
            if (orbiter.DamageAbsorbs < Objects.Count)
            {
                Objects.RemoveAt(Objects.Count - 1);
            }

            for (int i = 0; i < Objects.Count; i++)
            {
                Rectangle updatePos = new Rectangle();

                updatePos.X = orbiter.Collision.Center.X;
                updatePos.Y = orbiter.Collision.Center.Y;

                updatePos.X += orbiter.Collision.Width / 2;

                Vector2 objPVec = new Vector2(updatePos.X, updatePos.Y);

                float rotAmt = ((((float)Math.PI) * 2) / Objects.Count) * (i + 1);
                Vector2 rotVec = Utils.RotateAboutOrigin(objPVec, new Vector2(orbiter.Collision.Center.X, orbiter.Collision.Center.Y), rotAmt + offsetRadian);

                updatePos.X = (int)rotVec.X;
                updatePos.Y = (int)rotVec.Y;

                Objects[i] = updatePos;
            }

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (Rectangle r in Objects)
            {
                spriteBatch.Draw(Game1.Instance.blank, new Rectangle(r.X, r.Y, 8, 8), Color.Teal * 0.5f);
            }
            base.Draw(spriteBatch);
        }
    }
}
