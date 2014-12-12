using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project
{
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

    #region Abstract Definitions
    public abstract partial class Spell
    {        
        private SpellElement domtype;
        private SpellElement[] uniqueCombo;
        public SpellElement[] Combination { get { return uniqueCombo; } }
        public SpellElement DominantType { get { return domtype; } }
    }
    public abstract class LivingTargetSpell : Spell
    {
        public abstract void OnHit(LivingEntity entity);
    }
    public abstract class TileTargetSpell : Spell
    {
        public abstract void OnHit(Tile tile);
    }
    #endregion

    #region Spell Definitions
    public class SpellFireball : LivingTargetSpell
    {
        public override void OnHit(LivingEntity entity)
        {
            
        }
    }
    public class FailSpell : LivingTargetSpell
    {
        public override void OnHit(LivingEntity entity)
        {            
        }
    }
    #endregion
}
