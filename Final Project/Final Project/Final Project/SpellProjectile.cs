using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Final_Project
{
    public class SpellProjectile
    {
        protected float speed = 5f;
        protected float rotation = 0f;
        protected Vector2 direction;
        protected ISpellTargetable target;
        protected Spell spell;
        protected Texture2D texture;
        protected bool needsRemove = false;
        protected Vector2 position;
        protected Rectangle collisionBox;        

        public float Speed { get { return speed; } set { speed = value; } }
        public Vector2 Direction { get { return direction; } }
        public ISpellTargetable Target { get { return target; } }
        public Spell Spell { get { return spell; } }
        public bool NeedsRemove { get { return needsRemove; } set { needsRemove = value; } }
        public Vector2 PositionV { get { return position; } set { position = value; } }

        protected SpellProjectile(Spell spell)
        {
            this.spell = spell;
            texture = Game1.Instance.TextureDictionary["projectile"];            
            
        }

        public virtual void Update()
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }

    public class LivingTargetProjectile : SpellProjectile
    {
        Vector2 endp;
        ISpellCaster caster;

        Color spellColor = Color.White;

        public LivingTargetProjectile(ISpellCaster entity, Vector2 endpoint, LivingTargetSpell spell)
            : base(spell)
        {
            endp = endpoint;
            position = entity.PositionV;
            caster = entity;
            Vector2 en = new Vector2(entity.PositionV.X + 100, entity.PositionV.Y + 200);
            Console.WriteLine("end: " + endpoint + "; start: " + entity.PositionV);
            direction = Vector2.Normalize(endpoint - en);

            rotation = (float)(Math.Atan2(direction.Y, direction.X));

            for (int i = 0; i < Spell.Combination.Length; i++)
            {
                if (Spell.DominantType == SpellElement.Fire)
                {
                    spellColor.G -= 85;
                    spellColor.B -= 85;
                }
                if (Spell.DominantType == SpellElement.Air)
                {
                    spellColor.R -= 85;
                    spellColor.B -= 85;
                }
                if (Spell.DominantType == SpellElement.Water)
                {
                    spellColor.G -= 85;
                    spellColor.R -= 85;
                }
                if (Spell.DominantType == SpellElement.Earth)
                {
                    spellColor.B -= 85;                    
                }
            }            
        }

        public override void Update()
        {
            //rotation += MathHelper.ToRadians(1f);
            
            position += direction * speed;

            Vector2 rotatedp = Utils.RotateAboutOrigin(new Vector2(position.X - 8, position.Y - 1), new Vector2(position.X, position.Y), (float)rotation);

            rotatedp.X -= 8;
            rotatedp.Y -= 8;

            float x = rotatedp.X;
            float y = rotatedp.Y;
            int ix = (int)x, iy = (int)y;

            collisionBox = new Rectangle(ix, iy, 16, 16);

            foreach (Enemy e in Game1.Instance.enemies)
            {
                if (e.Collision.Intersects(new Rectangle(collisionBox.X + 100, collisionBox.Y + 200, collisionBox.Width, collisionBox.Height)) && !NeedsRemove)
                {
                    
                    LivingTargetSpell ltp = (LivingTargetSpell)Spell;
                    e.OnHit(ltp, caster);
                    Console.WriteLine("HIT: " + this.GetHashCode());
                    //ltp.OnHit(e);
                    needsRemove = true;
                }
            }
            
            
            
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 drawCoord = new Vector2(position.X + 100, position.Y + 200);            
            //float rotation = 0f;
            spriteBatch.Draw(Game1.Instance.TextureDictionary["projectile"], drawCoord, null, spellColor, rotation, new Vector2(32,16), 1f, SpriteEffects.None, 0.8f);
            //Rectangle r = new Rectangle(collisionBox.X + 100, collisionBox.Y + 200, collisionBox.Width, collisionBox.Height);
            //spriteBatch.Draw(Game1.Instance.blank, r, Color.Red*0.5f);
            base.Draw(spriteBatch);
        }
    }
}
