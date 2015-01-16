using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project.Animations
{
    public class FirebombEffect : PrefabAnimation
    {        
        const int ParticleCount = 20;
        const int ParticleWidth = 8;
        const int ParticleHeight = 8;
        const float ParticleSpeed = 3f;

        LivingEntity affected;
        float Radius;
        Vector2 sourcePos;
        
        public FirebombEffect(LivingEntity target, float radius)
        {
            affected = target;
            Radius = radius;
            sourcePos = new Vector2(affected.Collision.Center.X, affected.Collision.Center.Y);

            for (int i = 0; i < ParticleCount; i++)
            {
                Rectangle currentPosition = new Rectangle();
                Vector2 targetPosition = new Vector2();
                FirebombParticle fbp = new FirebombParticle();

                currentPosition.X = affected.Collision.Center.X;
                currentPosition.Y = affected.Collision.Center.Y;                

                targetPosition.X = affected.Collision.Center.X;
                targetPosition.Y = affected.Collision.Center.Y;

                targetPosition.X += radius;

                Vector2 objPVec = new Vector2(targetPosition.X, targetPosition.Y);

                float rotAmt = ((((float)Math.PI) * 2) / ParticleCount) * (i + 1);
                Vector2 rotVec = Utils.RotateAboutOrigin(objPVec, new Vector2(affected.Collision.Center.X, affected.Collision.Center.Y), rotAmt);

                Vector2 direction = new Vector2(rotVec.X - currentPosition.X, rotVec.Y - currentPosition.Y);
                direction.Normalize();
                float rotAmtFinal = (float)Math.Atan2(direction.Y, direction.X);

                fbp.Rectangle = currentPosition;
                fbp.Rotation = rotAmtFinal;
                fbp.Velocity = direction;
                fbp.Removes = false;

                Objects.Add(fbp);
                
            }

        }

        List<FirebombParticle> Objects = new List<FirebombParticle>();        
        public override void Update(GameTime gameTime)
        {

            for (int i = 0; i < Objects.Count; i++)
            {
                FirebombParticle fbp = new FirebombParticle();
                fbp = Objects[i];

                fbp.Rectangle = new Rectangle(fbp.Rectangle.X += (int)(fbp.Velocity.X * ParticleSpeed), fbp.Rectangle.Y += (int)(fbp.Velocity.Y * ParticleSpeed), 0, 0);
                

                if (Vector2.Distance(new Vector2(fbp.Rectangle.X, fbp.Rectangle.Y), sourcePos) >= Radius)
                {
                    fbp.Removes = true;
                }

                Objects[i] = fbp;

            }


            for (int i = 0; i < Objects.Count; i++)
            {
                if (Objects[i].Removes) Objects.RemoveAt(i);
            }

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (FirebombParticle r in Objects)
            {
                spriteBatch.Draw(Game1.Instance.TextureDictionary["projectile"], new Rectangle(r.Rectangle.X, r.Rectangle.Y, ParticleWidth, ParticleHeight), null, Color.Red * 0.75f, r.Rotation, new Vector2(ParticleWidth/2, ParticleHeight/2), SpriteEffects.None, 0.5f);
            }
            base.Draw(spriteBatch);
        }

        struct FirebombParticle
        {
            public Rectangle Rectangle;
            public Vector2 Velocity;
            public float Rotation;
            public bool Removes;
        }

    }
}
