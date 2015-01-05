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
    class Enemy : LivingEntity
    {

        public Enemy(int hp, int attack, int speed, int range, Rectangle position, float rotation, Texture2D texture, Game game) : base(game)
        {
            Hitpoints = hp;
            Attack = attack;
            Speed = speed;
            Range = range;
            Position = position;
            Rotation = rotation;
            Texture = texture;
        }
       
    }
}
