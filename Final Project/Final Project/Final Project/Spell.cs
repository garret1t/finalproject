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

            registeredSpells.Add(fireball);
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
    }
    public class LivingTargetSpell : Spell
    {
        public virtual void OnHit(LivingEntity entity) { }
    }
    public class TileTargetSpell : Spell
    {
        public virtual void OnHit(Tile tile) { }
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
    }
    public class FailSpell : LivingTargetSpell
    {
        public FailSpell()
        {
            name = "Failure";
            dominantType = SpellElement.None;
        }
        public override void OnHit(LivingEntity entity)
        {
        }
    }
    #endregion
}
