using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Final_Project
{
    public class Tile
    {
        public enum Material { Water, Sand, Rock, Grass, Null }
        public Material material;
        public bool canWalk;
        public bool causesDamage;
        public Texture2D tileTexture;
        public Tile(Material a, bool b, bool c, Texture2D d)
        {
            material = a;
            canWalk = b;
            causesDamage = c;
            tileTexture = d;
        }
        public Tile() { }
    }
}
