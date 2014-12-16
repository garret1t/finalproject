using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Final_Project.Animations;

namespace Final_Project
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Game1 Instance;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        int[,] tileTypes = new int[9, 9];
        String[] lines = System.IO.File.ReadAllLines("screen1.txt");
        String[] template = new String[5];
        public Grid screen = new Grid();
        public Player wizard;
        Texture2D wizardup, wizarddown, wizardleft, wizardright;
        public Texture2D gui, heart, heartEmpty;
        public Texture2D air, water, earth, fire;
        GamePadState pad1, oldpad1;
        public Texture2D fireballleft, fireballright, fireballup, fireballdown;
        public Texture2D mudballleft, mudballright, mudballup, mudballdown;
        public Dictionary<string, Texture2D> TextureDictionary = new Dictionary<string, Texture2D>();
        public List<PrefabAnimation> Animations = new List<PrefabAnimation>();
        Map map = new Map();
        int mapr = 0;
        int mapc = 0;
        public Game1()
        {
            Game1.Instance = this;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 800;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            SpellElement.InitializeWeaknessMaps();
            SpellRegistry.Initialize();

            
            // TODO: Add your initialization logic here
            for (int i = 0; i < 5; i++) 
            {
                template[i] = "screen" + i + ".txt";
            }

            
            map.Load(template, this);
           
            for(int i= 0; i<5; i++)
            {
                for(int j= 0; j<5; j++)
                {
                if (i == 0) 
                {
                    if (map.map[i, j].grid[0, 4].material == Tile.Material.Grass)
                    { 
                        map.map[i, j].grid[0, 4] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("RockTile")); 
                    }
                    if (map.map[i, j].grid[0, 4].material == Tile.Material.Sand)
                    {
                        if (map.map[i, j].grid[1, 3].material == Tile.Material.Sand)
                        { map.map[i, j].grid[0, 4] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("SandRockTile")); }
                        if (map.map[i, j].grid[1, 3].material == Tile.Material.Water)
                        { map.map[i, j].grid[0, 4] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("WaterRockTile")); }
                    }
                    if (map.map[i, j].grid[0, 4].material == Tile.Material.Water)
                    {
                        map.map[i, j].grid[0, 4] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("WaterRockTile"));
                    }
                }
                if (i == 4) 
                { 
                   
                    if (map.map[i, j].grid[8, 4].material == Tile.Material.Grass)
                    {
                        map.map[i, j].grid[8, 4] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("RockTile"));
                    }
                    if (map.map[i, j].grid[8, 4].material == Tile.Material.Sand)
                    {
                        if (map.map[i, j].grid[1, 3].material == Tile.Material.Sand)
                        {
                            map.map[i, j].grid[8, 4] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("SandRockTile"));
                        }
                        if (map.map[i, j].grid[1, 3].material == Tile.Material.Water)
                        {
                            map.map[i, j].grid[8, 4] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("WaterRockTile")); 
                        }
                    }
                    if (map.map[i, j].grid[8, 4].material == Tile.Material.Water)
                    {
                        map.map[i, j].grid[8, 4] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("WaterRockTile"));
                    }
                }
                if (j == 0) 
                { 
                    
                    if (map.map[i, j].grid[4, 0].material == Tile.Material.Grass)
                    {
                        map.map[i, j].grid[4, 0] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("RockTile"));
                    }
                    if (map.map[i, j].grid[4, 0].material == Tile.Material.Sand)
                    {
                        
                        if (map.map[i, j].grid[1, 3].material == Tile.Material.Sand)
                        {
                            map.map[i, j].grid[4, 0] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("SandRockTile"));
                        }
                        if (map.map[i, j].grid[1, 3].material == Tile.Material.Water)
                        {
                            map.map[i, j].grid[4, 0] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("WaterRockTile"));
                        }
                    }
                    if (map.map[i, j].grid[4, 0].material == Tile.Material.Water)
                    {
                        map.map[i, j].grid[4, 0] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("WaterRockTile"));
                    }
                }
                if (j == 4) 
                { 
                    
                    if (map.map[i, j].grid[4, 8].material == Tile.Material.Grass)
                    {
                        map.map[i, j].grid[4, 8] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("RockTile"));
                    }
                    if (map.map[i, j].grid[4, 8].material == Tile.Material.Sand)
                    {
                        if (map.map[i, j].grid[1, 3].material == Tile.Material.Sand)
                        {
                            map.map[i, j].grid[4, 8] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("SandRockTile"));
                        }
                        if (map.map[i, j].grid[1, 3].material == Tile.Material.Water)
                        {
                            map.map[i, j].grid[4, 8] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("WaterRockTile"));
                        }
                    }
                    if (map.map[i, j].grid[4, 8].material == Tile.Material.Water)
                    {
                        map.map[i, j].grid[4, 8] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("WaterRockTile"));
                    }
                }
                }
            }
            screen = map.map[mapr, mapc];
            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            wizarddown = Content.Load<Texture2D>("WizardDown");
            wizardup = Content.Load<Texture2D>("WizardUp");
            wizardleft = Content.Load<Texture2D>("WizardLeft");
            wizardright = Content.Load<Texture2D>("WizardRight");
            fireballleft = Content.Load<Texture2D>("FireballLeft");
            fireballright = Content.Load<Texture2D>("FireballRight");
            fireballup = Content.Load<Texture2D>("FireballUp");
            fireballdown = Content.Load<Texture2D>("FireballDown");
            air = Content.Load<Texture2D>("air");
            fire = Content.Load<Texture2D>("fire");
            water = Content.Load<Texture2D>("water");
            earth = Content.Load<Texture2D>("earth");
            gui = Content.Load<Texture2D>("gui");
            heart = Content.Load<Texture2D>("heart");
            heartEmpty = Content.Load<Texture2D>("heartEmpty");
            wizard = new Player(4, 4, wizardup, wizarddown, wizardleft, wizardright, spriteBatch);

            TextureDictionary.Add("wizard.down", wizarddown);
            TextureDictionary.Add("wizard.up", wizardup);
            TextureDictionary.Add("wizard.left", wizardleft);
            TextureDictionary.Add("wizard.right", wizardright);

            TextureDictionary.Add("symbols.fire", Content.Load<Texture2D>("Symbols/fire"));
            TextureDictionary.Add("symbols.water", Content.Load<Texture2D>("Symbols/water"));

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            pad1 = GamePad.GetState(PlayerIndex.One);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            wizard.UpdateProjectiles(screen);
            wizard.PollInput();
            map.map[mapr, mapc].visible = true;
            #region Input > Move
            if (pad1.ThumbSticks.Left.X > 0 && !(oldpad1.ThumbSticks.Left.X > 0)) 
            {
                if (wizard.row != 8)
                {
                    wizard.Move(wizard.row + 1, wizard.col, screen); wizard.texture = wizard.textureRight;
                }
                else 
                {
                    
                    if (mapr < 4)
                    {
                        mapr += 1;
                        screen = map.map[mapr, mapc];
                        wizard.row = 0;
                    }
                    
                }

            }
            if (pad1.ThumbSticks.Left.X < 0 && !(oldpad1.ThumbSticks.Left.X < 0)) 
            {
                if (wizard.row != 0)
                {
                    wizard.Move(wizard.row - 1, wizard.col, screen); wizard.texture = wizard.textureLeft;
                }
                else 
                {
                    
                    if (mapr > 0)
                    {
                        mapr -= 1;
                        screen = map.map[mapr, mapc];
                        wizard.row = 8;
                    }
                    
                }
            }
            if (pad1.ThumbSticks.Left.Y > 0 && !(oldpad1.ThumbSticks.Left.Y > 0)) 
            {
                if (wizard.col != 0)
                {
                    wizard.Move(wizard.row, wizard.col - 1, screen); wizard.texture = wizard.textureUp;
                }
                else
                {
                    
                    if (mapc > 0)
                    {
                        mapc -= 1;
                        screen = map.map[mapr, mapc];
                        wizard.col = 8;
                    }
                    
                }
            }
            if (pad1.ThumbSticks.Left.Y < 0 && !(oldpad1.ThumbSticks.Left.Y < 0)) 
            {
                if (wizard.col != 8)
                {
                    wizard.Move(wizard.row, wizard.col + 1, screen); wizard.texture = wizard.textureDown;
                }
                else
                {
                    
                    if (mapc < 4)
                    {
                        mapc += 1;
                        screen = map.map[mapr, mapc];
                        wizard.col = 0;

                    }
                    
                }
            }
            #endregion
            #region FireBall
            if (pad1.ThumbSticks.Right.X > 0 && oldpad1.ThumbSticks.Right.X == 0) { wizard.Shoot(ProjectileType.Fireball, new Vector2(1, 0), fireballright); }
            if (pad1.ThumbSticks.Right.X < 0 && oldpad1.ThumbSticks.Right.X == 0) { wizard.Shoot(ProjectileType.Fireball, new Vector2(-1, 0), fireballleft); }
            if (pad1.ThumbSticks.Right.Y > 0 && oldpad1.ThumbSticks.Right.Y == 0) { wizard.Shoot(ProjectileType.Fireball, new Vector2(0, -1), fireballup); }
            if (pad1.ThumbSticks.Right.Y < 0 && oldpad1.ThumbSticks.Right.Y == 0) { wizard.Shoot(ProjectileType.Fireball, new Vector2(0, 1), fireballdown); }
            #endregion
            for (int i = 0; i < Animations.Count; i++) if (Animations[i].NeedsRemove) Animations.RemoveAt(i);
            foreach (PrefabAnimation pa in Animations) pa.Update(gameTime);
            oldpad1 = pad1;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);           

            spriteBatch.Draw(gui, new Rectangle(0, 0, 800, 800), Color.White);
            
            int renders = 0;
            int vertOffsets = 0;
            int horzOffset = 0;
            int dim = 4;
            
            foreach (Grid g in map.map)
            {
                if (renders % 5 == 0 && renders != 0)
                {
                    horzOffset++;
                    vertOffsets = 0;
                }
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (g.visible == true)
                        { spriteBatch.Draw(g.GetTile(i, j).tileTexture, new Rectangle(i * dim + 5 + (horzOffset * dim * 9), j * dim + 5 + (vertOffsets * dim * 9), dim, dim), Color.White); }
                    }
                }
                vertOffsets++;
                renders++;
            }
            spriteBatch.Draw(this.Content.Load<Texture2D>("playerMarker"), new Rectangle((mapr * dim *9) + 1 + (dim *9 / 2), (mapc * dim * 9) + 1 + (dim *9 / 2), dim *2, dim * 2), Color.White);
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    spriteBatch.Draw(screen.GetTile(i, j).tileTexture, new Rectangle(i * 67 + 100, j * 67 + 200, 67, 67), Color.White);
                }
            }
            foreach (Projectile p in wizard.projectiles) 
            {
                p.Draw(spriteBatch,this);
            }
            wizard.Draw(spriteBatch);
            foreach (PrefabAnimation pa in Animations) pa.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
