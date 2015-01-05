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
    public class LivingEntity : DrawableGameComponent
    {
        public LivingEntity(Game associatedGame) : base(associatedGame) { }
             
        int hitpoints;
        int attack;
        int speed;
        int range;
        Rectangle position;
        float rotation;
        Texture2D texture;
        bool alive;
        
        public int Hitpoints 
        {
            get { return hitpoints; }
            set { hitpoints = value; }
        }
        public int Attack
        {
            get { return attack; }
            set { attack = value; }
        }
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public int Range
        {
            get { return range; }
            set { range = value; }
        }
        public Rectangle Position 
        {
            get { return position; }
            set { position = value;}
        }
        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }
        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }
        
        public virtual void Damage(int power, SpellElement type)
        {
            hitpoints -= power;
        }
        public override void Update(GameTime gameTime) 
        {
            if (hitpoints < 0) 
            {
                alive = false;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, rotation, Vector2.Zero, SpriteEffects.None, 0);
           
        }
        
    }
}
