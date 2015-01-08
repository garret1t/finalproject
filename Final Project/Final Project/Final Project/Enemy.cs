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
    public class Enemy : LivingEntity
    {
        List<Projectile> projectiles = new List<Projectile>();
        Texture2D projectileTexture;
        int reloadTime;
        int counter;
        public Enemy(int hp, int attack, int speed, int range,int reload, Rectangle position, float rotation, Texture2D texture, Texture2D bulletTexture, Game game) : base(game, SpellElement.Fire)
        {
            health = hp;
            maxHealth = hp;
            Attack = attack;
            Speed = speed;
            Range = range;
            Position = position;
            
            Rotation = rotation;
            reloadTime = reload;
            Texture = texture;
            projectileTexture = bulletTexture;
        }
        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            Vector2 direction = new Vector2( playerPosition.X + 33 - Position.X, playerPosition.Y + 33 - Position.Y);
            direction.Normalize();
            Rotation = (float)Math.Atan2(direction.Y, direction.X) + MathHelper.Pi;

            //Console.WriteLine(Vector2.Distance(new Vector2(Position.X, Position.Y), playerPosition));
            if (Vector2.Distance(new Vector2(Position.X, Position.Y), playerPosition ) < (Range * 100))
            {
                if (counter == 0)
                {

                    Shoot(new Vector2(Position.X, Position.Y), direction);
                    counter = reloadTime;
                }

            }
            else 
            {
                //Console.WriteLine("Moving");
                Position = new Rectangle(Position.X + (int)direction.X, Position.Y + (int)direction.Y, Position.Width, Position.Height);
                
            }

            if (counter > 0)
            {
                counter--;
            }
            //Console.WriteLine(Position);
            //Console.WriteLine(playerPosition);
            UpdateProjectiles();

            Vector2 rotatedp = Utils.RotateAboutOrigin(new Vector2(Position.X, Position.Y), new Vector2(Position.X, Position.Y), (float)Rotation);
            rotatedp.X -= 33;
            rotatedp.Y -= 33;

            float x = rotatedp.X;
            float y = rotatedp.Y;
            int ix = (int)x, iy = (int)y;

            collisionBox = new Rectangle(ix, iy, Position.Width, Position.Height);

            base.Update(gameTime);
        }
        public void Shoot(Vector2 initialPosition, Vector2 direction) 
        {
            if (projectiles.Count < 3)
            {
                projectiles.Add(new Projectile(5, initialPosition, direction, ProjectileType.Enemy, projectileTexture));
            }
        }
        public void UpdateProjectiles()
        {

            foreach (Projectile p in projectiles)
            {
                Console.WriteLine(p.Location);
                p.Location += (p.Velocity * p.Speed);
               
                if (Vector2.Distance(p.Location, new Vector2(Position.X + 100, Position.Y + 200)) > Range * 100) { p.Visible = false; }
            }
            for (int i = 0; i < projectiles.Count(); i++)
            {
                if (projectiles[i].Visible == false)
                {
                    projectiles.RemoveAt(i);
                    i--;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, new Vector2(Position.Width/4, Position.Height/4), SpriteEffects.None, 0);
            //spriteBatch.Draw(Game1.Instance.blank, collisionBox, Color.Red*0.5f);
            foreach (Projectile p in projectiles) 
            {
                p.Draw(spriteBatch);
            }

            if (!Dead)
            {
                spriteBatch.Draw(Game1.Instance.blank, new Rectangle(Collision.X, Collision.Y - 5, Collision.Width, 3), Color.Red);
                spriteBatch.Draw(Game1.Instance.blank, new Rectangle(Collision.X, Collision.Y - 5, (Health / MaxHealth) * Collision.Width, 3), Color.Green);
            }
            else
            {
                spriteBatch.Draw(Game1.Instance.blank, new Rectangle(Collision.X, Collision.Y - 5, Collision.Width, 3), Color.Black);
            }
        }
       
    }
}
