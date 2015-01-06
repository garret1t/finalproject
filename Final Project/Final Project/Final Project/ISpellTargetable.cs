using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Final_Project
{
    public interface ISpellTargetable
    {
        void OnHit(Spell spell, ISpellCaster caster);
        Vector2 PositionV {get; set;}
    }
}
