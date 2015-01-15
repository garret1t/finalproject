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
    public class Enemy : LivingEntity, ISpellCaster
    {
        List<Projectile> projectiles = new List<Projectile>();
        Texture2D projectileTexture;
        int reloadTime;
        int counter;

        public Enemy(int hp, int speed, int range,int reload, Rectangle position, float rotation, Texture2D texture, Texture2D bulletTexture, Game1 game) : base(game, SpellElement.Fire)

        {
            health = hp;
            maxHealth = hp;            
            Speed = speed;
            Range = range;
            Position = position;
            
            Rotation = rotation;
            reloadTime = reload;
            Texture = texture;
            projectileTexture = bulletTexture;

            OnDeath += new DeathHandler(Enemy_OnDeath);
        }

        void Enemy_OnDeath()
        {
            Game1.Instance.TriggerEnemyDeath(EnemyType.Standard, this);
        }


        
        Random random = new Random();
        public void Update(GameTime gameTime, Vector2 playerPosition, Player player)
        {
            Vector2 direction = new Vector2( playerPosition.X + 33 - Position.X, playerPosition.Y + 33 - Position.Y);
            direction.Normalize();
            if (!Dead)
            {
                Rotation = (float)Math.Atan2(direction.Y, direction.X) + MathHelper.Pi;
            }
            
           
            if (Vector2.Distance(new Vector2(Position.X, Position.Y), playerPosition ) < (Range * 100))
            {
                if (counter == 0)
                {
                    if (!Dead)
                    {
                        //Console.WriteLine("Shooting");
                        //Console.WriteLine("Mouse: " + Mouse.GetState().X + "," + Mouse.GetState().Y);
                        //Console.WriteLine("Enemy: " + Position);
                        //Console.WriteLine("Player: " + playerPosition);
                        if (Range > 1)
                        {
                            Shoot(new Vector2(Position.X, Position.Y), direction);
                            
                        }
                        if (Collision.Contains(new Rectangle((int)(playerPosition.X - 67/2), (int)(playerPosition.Y - 67/2), 67, 67)))
                        {
                            player.Damage(10, SpellElement.None);
                        }
                        counter = reloadTime;
                    }
                }

            }
            else 
            {
                if (!Dead)
                {
                    //Console.WriteLine("Moving");
                    Console.WriteLine("Old Position: " + Position);
                    Position = new Rectangle(Position.X + (int)(direction.X * Speed), Position.Y + (int)(direction.Y * Speed), Position.Width, Position.Height);
                    Console.WriteLine("New Position: " + Position);
                }
                
            }

            if (counter > 0)
            {
                counter--;
            }
            
            UpdateProjectiles();

            Vector2 rotatedp = Utils.RotateAboutOrigin(new Vector2(Position.X, Position.Y), new Vector2(Position.X, Position.Y), (float)Rotation);
            rotatedp.X -= 33;
            rotatedp.Y -= 33;

            float x = rotatedp.X;
            float y = rotatedp.Y;
            int ix = (int)x, iy = (int)y;

            collisionBox = new Rectangle(ix, iy, Position.Width, Position.Height);


            base.Update();
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
                
                p.Location += (p.Velocity * p.Speed);
                p.CheckCollision();
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
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, new Vector2(Position.Width/4, Position.Height/4), SpriteEffects.None, 0);
            //spriteBatch.Draw(Game1.Instance.blank, collisionBox, Color.Red*0.5f);
            foreach (Projectile p in projectiles) 
            {
                p.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
            
        }
       
    }
    public enum EnemyType
    {
        Standard,
        Boss
    }
}
