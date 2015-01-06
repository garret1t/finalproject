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
        List<Projectile> projectiles = new List<Projectile>();
        Texture2D projectileTexture;
        public Enemy(int hp, int attack, int speed, int range, Rectangle position, float rotation, Texture2D texture, Texture2D bulletTexture, Game game) : base(game)
        {
            Hitpoints = hp;
            Attack = attack;
            Speed = speed;
            Range = range;
            Position = position;
            Rotation = rotation;
            Texture = texture;
            projectileTexture = bulletTexture;
        }
        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            Vector2 direction = new Vector2( playerPosition.X + 100 - Position.X, playerPosition.Y + 200 - Position.Y);
            direction.Normalize();
            Rotation = (float)Math.Atan2(direction.Y, direction.X) + MathHelper.Pi;
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
            {
                Shoot(new Vector2(PositionV.X + 125 + 67 / 2, PositionV.Y + 150 + 67 / 2), direction);
            }
            UpdateProjectiles();
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

                if (Vector2.Distance(p.Location, new Vector2(PositionV.X + 100, PositionV.Y + 200)) > 300) { p.Visible = false; }
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
            foreach (Projectile p in projectiles) 
            {
                p.Draw(spriteBatch);
            }
        }
       
    }
}
