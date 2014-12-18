using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Final_Project
{
    public class Map
    {
        public Grid[,] map = new Grid[5, 5];
        String[] lines = new String[9];
        int[,] tileTypes = new int[9,9];

        Random rand = new System.Random();
        public void Load(String[] templates, Game1 game) 
        {
            
            for (int i = 0; i < 5; i++) 
            {
                
                for (int j = 0; j < 5; j++)
                {
                    lines = System.IO.File.ReadAllLines(templates[rand.Next(0, 5)]);
                    for (int k = 0; k < 9; k++)
                    {
                        for (int l = 0; l < 9; l++)
                        {
                            tileTypes[k, l] = Convert.ToInt32(new String(lines[l].ToCharArray()[k], 1));
                        }
                    }
                    map[i, j] = new Grid();
                    map[i, j].Load(tileTypes, game);

                }
            }
           
        }
    }
}
