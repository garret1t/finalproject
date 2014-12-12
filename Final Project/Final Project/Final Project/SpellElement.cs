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

        public static readonly SpellElement Fire = new SpellElement(0);
        public static readonly SpellElement Air = new SpellElement(1);
        public static readonly SpellElement Water = new SpellElement(2);
        public static readonly SpellElement Earth = new SpellElement(3);

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
        public SpellElement(int id)
        {
            this.id = id;
        }
        public int ElementId
        {
            get { return id; }
        }
        public bool IsWeakTo(SpellElement target)
        {
            return WeakToMap[target.ElementId] == id;
        }
        public bool IsStrongTo(SpellElement target)
        {
            return target.ElementId == ElementId;
        }

        #endregion
    }
}
