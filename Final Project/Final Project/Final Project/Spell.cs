using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Project
{
    class Spell
    {
        public enum Elements 
        {Air, Earth, Fire, Water }
        Elements element1;
        Elements element2;
        Player player;
        Vector2 velocity;
        Game1 game1;
        public Spell(Elements elm1, Player play, Vector2 vel, Game1 game) 
        {
            element1 = elm1;
            player = play;
            velocity = vel;
            game1 = game;
        }
        public void AddElement(Elements elm2)
        {
            element2 = elm2;
        }
        public void CreateSpell()
        {
            if (element1 == Elements.Earth && element2 == Elements.Water){player.Shoot(ProjectileType.Mudball, velocity, game1.mudballdown);}
 
        }
        public void Draw(SpriteBatch spriteBatch) 
        {
            
            if (element1 == Elements.Air) { spriteBatch.Draw(game1.air, new Rectangle(500, 32, 64, 64), Color.White); }
            if (element1 == Elements.Earth) { spriteBatch.Draw(game1.earth, new Rectangle(500, 32, 64, 64), Color.White); }
            if (element1 == Elements.Fire) { spriteBatch.Draw(game1.fire, new Rectangle(500, 32, 64, 64), Color.White); }
            if (element1 == Elements.Water) { spriteBatch.Draw(game1.water, new Rectangle(500, 32, 64, 64), Color.White); }
            if (element2 == Elements.Air) { spriteBatch.Draw(game1.air, new Rectangle(600, 32, 64, 64), Color.White); }
            if (element2 == Elements.Earth) { spriteBatch.Draw(game1.earth, new Rectangle(600, 32, 64, 64), Color.White); }
            if (element2 == Elements.Fire) { spriteBatch.Draw(game1.fire, new Rectangle(600, 32, 64, 64), Color.White); }
            if (element2 == Elements.Water) { spriteBatch.Draw(game1.water, new Rectangle(600, 32, 64, 64), Color.White); }
            
        }
    }
}
