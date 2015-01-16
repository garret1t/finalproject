using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Final_Project
{
    public class Mana
    {
        private Dictionary<int, int> manavalues = new Dictionary<int, int>();
        int max;
        public Mana(int globalAmtMax, bool startAtMax)
        {
            max = globalAmtMax;
            int amt = 0;
            if (startAtMax) amt = globalAmtMax;
            else amt = 0;
            foreach (SpellElement elements in SpellElement.RegisteredElements.Values)
            {                
                manavalues.Add(elements.ElementId, amt);
            }
        }

        public int GetAmountFromSpellElement(SpellElement type)
        {
            return manavalues[type.ElementId];
        }

        public int SetAmountBySpellElement(SpellElement type, int amt)
        {
            manavalues[type.ElementId] = amt;
            return GetAmountFromSpellElement(type);
        }

        public int MaximumMana { get { return max; } set { max = value; } }

        public Mana Copy()
        {
            Mana m = new Mana(30, true);
            m.manavalues = manavalues;
            return m;
        }

        public int this[SpellElement index]
        {
            get
            {
                return manavalues[index.ElementId];
            }
            set
            {
                manavalues[index.ElementId] = value;
            }
        }
    }
}
