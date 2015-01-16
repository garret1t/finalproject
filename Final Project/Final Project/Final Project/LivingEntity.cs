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
        private int damageAbsorbs = 0;
        public int DamageAbsorbs { get { return damageAbsorbs; } set { damageAbsorbs = value; } }

        protected Vector2 posv = new Vector2();

        public delegate void HealthHandler(int oldHp, int newHp);
        public delegate void DeathHandler();

        public event HealthHandler OnDamageTaken;
        public event HealthHandler OnHealthTaken;
        public event DeathHandler OnDeath;

        public Vector2 PositionV { get { return posv; } set { posv = value; } }
        public int GridX { get { return (int)((PositionV.X + 67 / 2) / 67); } set { PositionV = new Vector2(value * 67, PositionV.Y); } }
        public int GridY { get { return (int)((PositionV.Y + 67 / 2) / 67); } set { PositionV = new Vector2(PositionV.X, value * 67); } }
        public LivingEntity(Game associatedGame, SpellElement assigned) : base(associatedGame) 
        {
            element = assigned;
            OnDamageTaken += (int old, int ne) => { };
            OnHealthTaken += (int old, int ne) => { };

            OnDamageTaken += new HealthHandler(Enemy_OnDamageTaken);
            OnHealthTaken += new HealthHandler(Enemy_OnHealthTaken);
            OnDeath += () => { };
        }

        
        
        int speed;
        int range;
        protected int maxHealth;
        protected Rectangle position = new Rectangle();
        protected Rectangle collisionBox;
        float rotation;
        Texture2D texture;

        int flyoverCounter = -1;
        int flyoverHealthShown = 0;
        bool flyoverTypeisDamage = false;
        string flyoverText = "";
        Vector2 flyoverPos = new Vector2();

        void Enemy_OnHealthTaken(int oldHp, int newHp)
        {
            flyoverCounter = 0;
            flyoverHealthShown = Math.Abs(newHp - oldHp);
            flyoverTypeisDamage = false;
            flyoverText = "" + ((flyoverTypeisDamage) ? "-" : "+") + flyoverHealthShown;
            flyoverPos.X = ((collisionBox.Right - collisionBox.Left - Game1.Instance.Flyover.MeasureString(flyoverText).X) / 2) + collisionBox.Left;
            flyoverPos.Y = collisionBox.Y - 8 - Game1.Instance.Flyover.MeasureString(flyoverText).Y;
        }

        void Enemy_OnDamageTaken(int oldHp, int newHp)
        {
            flyoverCounter = 0;
            flyoverHealthShown = Math.Abs(oldHp - newHp);
            flyoverTypeisDamage = true;
            flyoverText = "" + ((flyoverTypeisDamage) ? "-" : "+") + flyoverHealthShown;
            flyoverPos.X = ((collisionBox.Right - collisionBox.Left - Game1.Instance.Flyover.MeasureString(flyoverText).X) / 2) + collisionBox.Left;
            flyoverPos.Y = collisionBox.Y - 8 - Game1.Instance.Flyover.MeasureString(flyoverText).Y;
        }
        Random random = new Random();
        public virtual void Update()
        {
            if (flyoverCounter != -1)
            {
                flyoverCounter++;
                flyoverPos.Y -= ((random.Next(100) > 80) ? 2 : 1);
                flyoverPos.X -= ((random.Next(100) > 90) ? 1 : 0);
            }

            if (flyoverCounter == 60)
            {
                flyoverCounter = -1;
            }
        }

        public Rectangle Collision { get { return collisionBox; } }
        
       
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
            int p = power;
            if (Element.IsStrongTo(type))
            {
                Heal(power, type);
                return;
            }

            if (DamageAbsorbs > 0)
            {
                DamageAbsorbs--;
                return;
            }

            if (Element.IsWeakTo(type)) p *= 2;            
            Health -= p;
            int newHp = Health;
            if (oldHp != newHp)
                OnDamageTaken(oldHp, newHp);
            if (Dead)
                OnDeath();
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

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!Dead)
            {

                spriteBatch.Draw(Game1.Instance.blank, new Rectangle(Collision.X, Collision.Y - 5, Collision.Width, 3), Color.Red);
                spriteBatch.Draw(Game1.Instance.blank, new Rectangle(Collision.X, Collision.Y - 5, (int)(((float)Health / (float)MaxHealth) * Collision.Width), 3), Color.Green);
            }
            else
            {
                spriteBatch.Draw(Game1.Instance.blank, new Rectangle(Collision.X, Collision.Y - 5, Collision.Width, 3), Color.Black);
            }

            if (flyoverCounter != -1)
            {
                spriteBatch.DrawString(Game1.Instance.Flyover, flyoverText, new Vector2(flyoverPos.X - 2, flyoverPos.Y - 2), Color.Black, 0f, Vector2.Zero, 1.1f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(Game1.Instance.Flyover, flyoverText, flyoverPos, ((flyoverTypeisDamage) ? Color.Red : Color.Green));
            }
        }

    }
}