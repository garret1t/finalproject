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
        protected Vector2 direction;
        protected ISpellTargetable target;
        protected Spell spell;
        protected Texture2D texture;
        protected bool needsRemove = false;
        protected Vector2 position;

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
        public LivingTargetProjectile(ISpellCaster entity, Vector2 endpoint, LivingTargetSpell spell)
            : base(spell)
        {
            endp = endpoint;
            position = entity.PositionV;
            caster = entity;
            Vector2 en = new Vector2(entity.PositionV.X + 100, entity.PositionV.Y + 200);
            Console.WriteLine("end: " + endpoint + "; start: " + entity.PositionV);
            direction = Vector2.Normalize(endpoint - en);
        }

        public override void Update()
        {
            position += direction * speed;
            base.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 drawCoord = new Vector2(position.X + 100, position.Y + 200);
            float rotation = (float)(Math.Atan2(direction.Y, direction.X));
            //float rotation = 0f;
            spriteBatch.Draw(Game1.Instance.TextureDictionary["projectile"], drawCoord, null, Color.White, rotation, new Vector2(32,16), 1f, SpriteEffects.None, 0.8f);
            base.Draw(spriteBatch);
        }
    }
}
