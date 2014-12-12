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
