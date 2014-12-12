using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Final_Project
{
    public class SpellComboList : List<SpellElement>
    {
        public delegate void SpellAdded(SpellElement type);
        public event SpellAdded OnSpellAdded;
        public new void Add(SpellElement item)
        {
            OnSpellAdded(item);
            base.Add(item);
        }
        public Spell Complete()
        {
            return SpellRegistry.GetSpellFromCombo(this.ToArray());
        }
    }
}
