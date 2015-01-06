using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Final_Project
{
    public class LivingEntity : ISpellTargetable, ISpellCaster
    {
        private int health;
        private bool dead;

        protected Vector2 posv = new Vector2();

        public Vector2 PositionV { get { return posv; } set { posv = value; } }

        public int Health
        {
            get { return Health; }
            set {
                if (Dead) { return; }
                if (health - value <= 0)
                {
                    dead = true;
                    health = 0;
                }
                else
                {
                    health -= value;
                }
            }
        }

        public bool Dead
        {
            get
            {
                return dead || health <= 0;
            }
            set
            {
                dead = true;
                health = 0;
            }
        }

        protected SpellElement element;
        public SpellElement Element { get { return element; } }

        public virtual void Damage(int power, SpellElement type)
        {
            int p = power;
            if (Element.IsStrongTo(type))
            {
                Heal(power, type);
                return;
            }
            if (Element.IsWeakTo(type)) p *= 2;
            Health -= p;
        }

        public virtual void Heal(int power, SpellElement type)
        {
            int p = power;
            if (Element.IsWeakTo(type))
            {
                Damage(p, SpellElement.None);
                return;
            }
            Health += p;
        }

        public void OnHit(Spell spell, ISpellCaster caster)
        {
            if (spell is LivingTargetSpell)
            {
                LivingTargetSpell ltp = (LivingTargetSpell)spell;
                ltp.OnHit(this);
            }
        }
    }
}
