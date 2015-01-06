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
    public class Projectile
    {
        int speed;
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        Vector2 location;
        public Vector2 Location
        {
            get { return location; }
            set { location = value; }
        }
        public ProjectileType type;
        Vector2 velocity;
        public Vector2 Velocity 
        {
            get { return velocity; }
            set { velocity = value; }
        }
        bool isVisible;
        public bool Visible 
        {
            get { return isVisible; }
            set { isVisible = value; }
        }
        Texture2D texture;
        public Texture2D Texture 
        {
            get { return texture; }
            set {texture = value;}
        }
        public Projectile(int s, Vector2 loc, Vector2 vel, ProjectileType t, Texture2D text) 
        {
            speed = s;
            location = loc;
            type = t;
            velocity = vel;
            isVisible = true;
            texture = text;
        }
        
        public void Draw(SpriteBatch spriteBatch) 
        {

                     
            if (type == ProjectileType.Fireball) { spriteBatch.Draw(texture, new Rectangle((int)location.X + 100, (int)location.Y + 200, 67, 67), Color.White); }
            if (type == ProjectileType.Enemy) { spriteBatch.Draw(texture, new Rectangle((int)location.X + 100, (int)location.Y + 200, 16, 16), Color.White); }
            if (type == ProjectileType.Mudball) { spriteBatch.Draw(texture, new Rectangle((int)location.X + 100, (int)location.Y + 200, 67, 67), Color.White); }
            if (type == ProjectileType.Wave) { spriteBatch.Draw(texture, new Rectangle((int)location.X + 100, (int)location.Y + 200, 67, 67), Color.White); }
            
        }
        
    }
    public enum ProjectileType { Fireball, Mudball, Wave, Enemy }
}
