using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

    }
}
