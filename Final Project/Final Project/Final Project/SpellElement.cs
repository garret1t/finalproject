using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public static readonly SpellElement Fire = new SpellElement(0, "Fire");
        public static readonly SpellElement Air = new SpellElement(1, "Air");
        public static readonly SpellElement Water = new SpellElement(2, "Water");
        public static readonly SpellElement Earth = new SpellElement(3, "Earth");
        public static readonly SpellElement Light = new SpellElement(4, "Light");
        public static readonly SpellElement None = new SpellElement(5, "None");

        #endregion

        #region Weakness Maps

        public static Dictionary<int, int> WeakToMap = new Dictionary<int, int>();
        public static void InitializeWeaknessMaps()
        {
            WeakToMap.Add(0, 2);
            WeakToMap.Add(1, 3);
            WeakToMap.Add(2, 0);
            WeakToMap.Add(3, 1);
        }

        #endregion

        #region SpellElement Class

        private int id;
        private string name;
        public SpellElement(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
        public int ElementId
        {
            get { return id; }
        }
        public string Name
        {
            get { return name; }
        }
        public bool IsWeakTo(SpellElement target)
        {
            return WeakToMap[target.ElementId] == id;
        }
        public bool IsStrongTo(SpellElement target)
        {
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
