using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project
{
    #region Spell Registry
    public static class SpellRegistry
    {
        private static List<Spell> registeredSpells;
        public static FailSpell FailSpell;
        public static void Initialize()
        {
            registeredSpells = new List<Spell>();
            FailSpell = new FailSpell();

            SpellFireball fireball = new SpellFireball();
            SpellWaterBullet waterbullet = new SpellWaterBullet();
            SpellHealOne healone = new SpellHealOne();

            registeredSpells.Add(fireball);
            registeredSpells.Add(waterbullet);
            registeredSpells.Add(healone);
        }

        public static List<Spell> Registry { get { return registeredSpells; } }

        public static Spell GetSpellFromCombo(SpellElement[] combo)
        {
            foreach (Spell sp in Registry)
            {
                if (Utils.ElementArraysEqual(sp.Combination, combo)) return sp;
            }
            return FailSpell;
        }
    }
    #endregion

    #region Abstract Definitions
    public class Spell
    {        
        protected SpellElement dominantType;
        protected SpellElement[] uniqueCombo;
        protected string name;
        public SpellElement[] Combination { get { return uniqueCombo; } }
        public SpellElement DominantType { get { return dominantType; } }
        public string Name { get { return name; } }
        public virtual void OnCast(ISpellCaster caster) { }

    }
    public class LivingTargetSpell : Spell
    {
        public virtual void OnHit(LivingEntity entity) { }
    }
    public class TileTargetSpell : Spell
    {
        public virtual void OnHit(Tile tile) { }
    }
    public class SelfTargetSpell : Spell 
    {
    }
    #endregion

    #region Spell Definitions
    public class SpellFireball : LivingTargetSpell
    {
        public SpellFireball()
        {
            name = "Fireball";
            dominantType = SpellElement.Fire;
            uniqueCombo = new SpellElement[] { SpellElement.Fire };
        }
        public override void OnHit(LivingEntity entity)
        {
            entity.Damage(10, dominantType);
            base.OnHit(entity);
        }
        public override void OnCast(ISpellCaster caster)
        {
            Game1.Instance.fireSound.Play(1, 0, 0); 
            base.OnCast(caster);
        }
    }
    public class SpellWaterBullet : LivingTargetSpell
    {
        public SpellWaterBullet()
        {
            name = "Water Bullet";
            dominantType = SpellElement.Water;
            uniqueCombo = new SpellElement[] { SpellElement.Water };
        }
        public override void OnHit(LivingEntity entity)
        {
            entity.Damage(6, dominantType);
            base.OnHit(entity);
        }
        public override void OnCast(ISpellCaster caster)
        {
            Game1.Instance.waterSound.Play(1, 0, 0); 
            base.OnCast(caster);
        }
    }
    public class FailSpell : LivingTargetSpell
    {
        public FailSpell()
        {
            name = "Failure";
            dominantType = SpellElement.None;
        }
        public override void OnCast(ISpellCaster caster)
        {
            Game1.Instance.buzzerSound.Play(1, 0, 0);
            base.OnCast(caster);
        }
    }
    public class SpellHealOne : SelfTargetSpell
    {
        public SpellHealOne()
        {
            name = "Heal I";
            dominantType = SpellElement.Light;
            uniqueCombo = new SpellElement[] { SpellElement.Light };
        }
        public override void OnCast(ISpellCaster caster)
        {
            Game1.Instance.dingSound.Play(1, 0, 0); 
            caster.Heal(15, DominantType);
            base.OnCast(caster);
        }
    }
    #endregion
}
