using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Final_Project
{
    public interface ISpellCaster
    {
        Vector2 PositionV { get; set; }
        Vector2 OmniSelectionTarget { get; }
        int GridX { get; set; }
        int GridY { get; set; }
        void Heal(int power, SpellElement type);
        void Damage(int power, SpellElement type);  
      
    }
}
