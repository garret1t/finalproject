using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Final_Project
{
    /*
     * Element IDs
     * 0 - Fire
     * 1 - Air
     * 2 - Water
     * 3 - Earth
    */
    public class SpellElement
    {

        #region Element Definitions

        public static readonly SpellElement Fire = new SpellElement(0, "Fire", Color.Red);
        public static readonly SpellElement Air = new SpellElement(1, "Air", Color.Green);
        public static readonly SpellElement Water = new SpellElement(2, "Water", Color.Blue);
        public static readonly SpellElement Earth = new SpellElement(3, "Earth", Color.Yellow);
        public static readonly SpellElement Light = new SpellElement(4, "Light", Color.White);
        public static readonly SpellElement None = new SpellElement(5, "None", Color.Black);        

        #endregion

        #region Weakness Maps

        public static Dictionary<int, int> WeakToMap = new Dictionary<int, int>();
        public static Dictionary<int, SpellElement> RegisteredElements = new Dictionary<int, SpellElement>();
        public static void InitializeElements()
        {
            RegisteredElements.Add(Fire.ElementId, Fire);
            RegisteredElements.Add(Air.ElementId, Air);
            RegisteredElements.Add(Water.ElementId, Water);
            RegisteredElements.Add(Earth.ElementId, Earth);
            RegisteredElements.Add(Light.ElementId, Light);
            RegisteredElements.Add(None.ElementId, None);

            WeakToMap.Add(0, 2);
            WeakToMap.Add(1, 3);
            WeakToMap.Add(2, 0);
            WeakToMap.Add(3, 1);
        }

        #endregion

        #region SpellElement Class

        private int id;
        private string name;
        private Color spellColor;
        public SpellElement(int id, string name, Color color)
        {
            this.id = id;
            this.name = name;
            this.spellColor = color;
        }
        public int ElementId
        {
            get { return id; }
        }
        public string Name
        {
            get { return name; }
        }

        public Color SpellColor { get { return spellColor; } }

        public bool IsWeakTo(SpellElement target)
        {
            if (target == SpellElement.None) return false;
            if (!SpellElement.WeakToMap.ContainsKey(target.ElementId)) return false;
            return SpellElement.WeakToMap[target.ElementId] == id;
        }
        public bool IsStrongTo(SpellElement target)
        {
            if (target == SpellElement.None) return false;            
            return target.ElementId == ElementId;
        }

        public static bool operator ==(SpellElement one, SpellElement binary)
        {
            return (one.ElementId == binary.ElementId);
        }

        public static bool operator !=(SpellElement one, SpellElement binary)
        {
            return (one.ElementId != binary.ElementId);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SpellElement)) return false;
            SpellElement compare = (SpellElement)obj;
            return compare.ElementId == ElementId;
        }

        #endregion
    }
}
