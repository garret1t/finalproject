using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
        public static Color ColorMixer(Color c1, Color c2)
        {

            int _r = Math.Min((c1.R + c2.R), 255);
            int _g = Math.Min((c1.G + c2.G), 255);
            int _b = Math.Min((c1.B + c2.B), 255);

            return new Color(Convert.ToByte(_r),
                             Convert.ToByte(_g),
                             Convert.ToByte(_b));
        }
        public static Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin, float rotation)
        {
            return Vector2.Transform(point - origin, Matrix.CreateRotationZ(rotation)) + origin;
        } 
    }
}
