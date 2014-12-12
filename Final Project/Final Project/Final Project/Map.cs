using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Final_Project
{
    class Map
    {
        Grid[,] grid = new Grid[5, 5];
        String[] lines;
        int[,] tileTypes;
        Random rand;
        public void Load(String[] templates, Game1 game) 
        {
            
            for (int i = 0; i < 5; i++) 
            {
                lines = System.IO.File.ReadAllLines(templates[rand.Next(0,4)]);
                for (int j = 0; j < 9; j++)
                {
                    for (int k = 0; k < 9; k++)
                    {
                        tileTypes[i, j] = Convert.ToInt32(new String(lines[j].ToCharArray()[i], 1));
                        grid[i, j].Load(tileTypes, game);
                    }
                }
            }
        }
    }
}
