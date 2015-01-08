using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project
{
    public class LivingEntity : DrawableGameComponent, ISpellTargetable
    {
        protected int health;
        private bool dead;

        protected Vector2 posv = new Vector2();

        public delegate void HealthHandler(int oldHp, int newHp);

        public event HealthHandler OnDamageTaken;
        public event HealthHandler OnHealthTaken;

        public Vector2 PositionV { get { return posv; } set { posv = value; } }
        public LivingEntity(Game associatedGame, SpellElement assigned) : base(associatedGame) 
        {
            element = assigned;
            OnDamageTaken += (int old, int ne) => { };
            OnHealthTaken += (int old, int ne) => { };
        }

        int hitpoints;
        int attack;
        int speed;
        int range;
        protected int maxHealth;
        protected Rectangle position = new Rectangle();
        protected Rectangle collisionBox;
        float rotation;
        Texture2D texture;
        bool alive;

        public Rectangle Collision { get { return collisionBox; } }
        public int Hitpoints
        {
            get { return hitpoints; }
            set { hitpoints = value; }
        }
        public int Attack
        {
            get { return attack; }
            set { attack = value; }
        }
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public int Range
        {
            get { return range; }
            set { range = value; }
        }
        public Rectangle Position
        {
            get { return position; }
            set { position = value; }
        }
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        public int Health
        {
            get { return health; }
            set
            {
                if (Dead) { return; }
                if (value <= 0)
                {
                    dead = true;
                    health = 0;
                }
                else if (value > maxHealth)
                {
                    health = maxHealth;
                }
                else
                {
                    health = value;
                }
            }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
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
            int oldHp = Health;
            Console.WriteLine("Damage: " + power + "; " + type.Name);
            int p = power;
            if (Element.IsStrongTo(type))
            {
                Heal(power, type);
                return;
            }
            if (Element.IsWeakTo(type)) p *= 2;            
            Health -= p;
            int newHp = Health;
            if (oldHp != newHp)
                OnDamageTaken(oldHp, newHp);
        }

        public virtual void Heal(int power, SpellElement type)
        {
            int p = power;
            int oldHp = Health;
            if (Element.IsWeakTo(type))
            {
                Damage(p, SpellElement.None);
                return;
            }
            Health += p;
            int newHp = Health;
            if (oldHp != newHp)
                OnHealthTaken(oldHp, newHp);
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