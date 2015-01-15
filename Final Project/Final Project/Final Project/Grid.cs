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
    public class Grid
    {
        public Tile[,] grid = new Tile[9, 9];
        public List<Enemy> enemyList = new List<Enemy>();
        public bool visible;
        public void Load(int[,] tiles, Game1 game1)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    grid[i, j] = new Tile();
                    if (tiles[i, j] == 0)
                    {
                        grid[i, j].material = Tile.Material.Rock;
                        grid[i, j].tileTexture = game1.Content.Load<Texture2D>("RockTile");
                        grid[i, j].canWalk = false;
                    }
                    if (tiles[i, j] == 1)
                    {
                        grid[i, j].material = Tile.Material.Sand;
                        grid[i, j].tileTexture = game1.Content.Load<Texture2D>("SandTile");
                        grid[i, j].canWalk = true;
                    }
                    if (tiles[i, j] == 2)
                    {
                        grid[i, j].material = Tile.Material.Grass;
                        grid[i, j].tileTexture = game1.Content.Load<Texture2D>("GrassTile");
                        grid[i, j].canWalk = true;
                    }
                    if (tiles[i, j] == 3)
                    {
                        grid[i, j].material = Tile.Material.Water;
                        grid[i, j].tileTexture = game1.Content.Load<Texture2D>("WaterTile");
                        grid[i, j].canWalk = false;
                    }
                    if (tiles[i, j] == 4)
                    {
                        grid[i, j].material = Tile.Material.SandRock;
                        grid[i, j].tileTexture = game1.Content.Load<Texture2D>("SandRockTile");
                        grid[i, j].canWalk = false;
                    }
                    if (tiles[i, j] == 5)
                    {
                        grid[i, j].material = Tile.Material.WaterRock;
                        grid[i, j].tileTexture = game1.Content.Load<Texture2D>("WaterRockTile");
                        grid[i, j].canWalk = false;
                    }
                }
            }
        }
        public void LoadEnemies(int[,] enemies, Game1 game1) 
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (enemies[i, j] == 0) 
                    {
                        
                    }
                    if (enemies[i, j] == 1)
                    {
                        enemyList.Add(new Enemy(30,3, 3, 180, new Rectangle(i * 67 + 100 + 67 / 2, i * 67 + 200 + 67/2, 67, 67), 0, game1.Content.Load<Texture2D>("enemy1"), game1.Content.Load<Texture2D>("bullet"), game1, EnemyTypeAI.Fire, SpellElement.Fire));
                        game1.enemiesRemaining++;
                    }
                    if (enemies[i, j] == 2)
                    {
                        enemyList.Add(new Enemy(20, 4, 1, 120, new Rectangle(i * 67 + 100 + 67 / 2, i * 67 + 200 + 67 / 2, 67, 67), 0, game1.Content.Load<Texture2D>("enemy3"), game1.Content.Load<Texture2D>("bullet"), game1, EnemyTypeAI.Melee, SpellElement.None));
                        game1.enemiesRemaining++;
                    }
                    if (enemies[i, j] == 3)
                    {
                        enemyList.Add(new Enemy(30, 5, 3, 180, new Rectangle(i * 67 + 100 + 67 / 2, i * 67 + 200 + 67 / 2, 67, 67), 0, game1.Content.Load<Texture2D>("enemy2"), game1.Content.Load<Texture2D>("bullet"), game1, EnemyTypeAI.Water, SpellElement.Water));
                        game1.enemiesRemaining++;
                    }
                }
            }
        }
        public Tile GetTile(int i, int j)
        {
            return grid[i, j];
        }
        public Tile GetTile(Rectangle rect)
        {
            if (rect.X > 0 && rect.Y > 0 && rect.X < 600 && rect.Y < 600)
            {
                
                return grid[rect.X / 67, rect.Y / 67];
            }
            else
            {
                return new Tile(Tile.Material.Null, false, false, null);
            }
        }
    }
}
