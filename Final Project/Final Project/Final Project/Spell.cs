﻿using System;
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
            SpellWindBlast windBlast = new SpellWindBlast();
            SpellHealOne healone = new SpellHealOne();
            SpellTimeStop timeStop = new SpellTimeStop();
            SpellShield shield = new SpellShield();

            registeredSpells.Add(fireball);
            registeredSpells.Add(waterbullet);
            registeredSpells.Add(healone);
            registeredSpells.Add(windBlast);
            registeredSpells.Add(timeStop);
            registeredSpells.Add(shield);
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
        private Dictionary<string, object> specialProps = new Dictionary<string, object>();
        protected SpellElement dominantType;
        protected SpellElement[] uniqueCombo;
        protected string name;
        public SpellElement[] Combination { get { return uniqueCombo; } }
        public SpellElement DominantType { get { return dominantType; } }
        public string Name { get { return name; } }
        public virtual void OnCast(ISpellCaster caster) { }
        public void SetSpecialProperty<T>(string k, T v)
        {
            if (!specialProps.ContainsKey(k)) specialProps.Add(k, v);
            else specialProps[k] = v;
        }
        public T GetSpecialProperty<T>(string key)
        {
            if (!specialProps.ContainsKey(key)) return default(T);
            else
            {
                if (specialProps[key] is T)
                    return (T)specialProps[key];
                else
                {
                    Console.Error.WriteLine("Cannot cast " + specialProps[key].GetType().ToString() + " to " + typeof(T).ToString() + ". Return default value of " + typeof(T).ToString() + ".");
                    Console.Error.WriteLine("Error encountered while attempting to retrieve Special Spell Property. Key: " + key);
                    return default(T);
                }
            }
        }
        public T GetSpecialProperty<T>(string key, T defaultReturn)
        {
            if (!HasSpecialProperty(key)) return defaultReturn;
            else return GetSpecialProperty<T>(key);
        }
        public bool HasSpecialProperty(string k)
        {
            return specialProps.ContainsKey(k);
        }
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
            entity.Damage(8, dominantType);
            base.OnHit(entity);
        }
        public override void OnCast(ISpellCaster caster)
        {
            Game1.Instance.waterSound.Play(1, 0, 0); 
            base.OnCast(caster);
        }
    }

    public class SpellWindBlast : LivingTargetSpell
    {
        public SpellWindBlast()
        {
            name = "Wind Blast";
            dominantType = SpellElement.Air;
            uniqueCombo = new SpellElement[] { SpellElement.Air, SpellElement.Air };
            SetSpecialProperty<bool>("RemoveProjectileOnHit", false);
        }
        public override void OnHit(LivingEntity entity)
        {
            entity.Damage(6, dominantType);
            base.OnHit(entity);
        }
        public override void OnCast(ISpellCaster caster)
        {
            Game1.Instance.fireSound.Play(0.85f, 0, 0);
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

    public class SpellTimeStop : SelfTargetSpell
    {
        public SpellTimeStop()
        {
            name = "Time Stop";
            dominantType = SpellElement.Light;
            uniqueCombo = new SpellElement[] { SpellElement.Earth, SpellElement.Light, SpellElement.Earth };
        }
        public override void OnCast(ISpellCaster caster)
        {
            Game1.Instance.timeStopped = true;
            Game1.Instance.timeStopTimer += 360;            
            base.OnCast(caster);
        }
    }

    public class SpellShield : SelfTargetSpell
    {
        public SpellShield()
        {
            name = "Shield";
            dominantType = SpellElement.Light;
            uniqueCombo = new SpellElement[] { SpellElement.Light, SpellElement.Light };
        }
        public override void OnCast(ISpellCaster caster)
        {
            if (caster is LivingEntity)
            {
                ((LivingEntity)caster).DamageAbsorbs += 3;
            }
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
    #endregion
}
