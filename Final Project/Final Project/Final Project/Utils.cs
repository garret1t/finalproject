using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Final_Project
{
    public static class Utils
    {
        public static bool ElementArraysEqual(SpellElement[] a1, SpellElement[] a2)
        {
            if (a1.Length != a2.Length) return false;
            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i].ElementId != a2[i].ElementId) return false;
            }
            return true;
        }
    }
}
