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

        public enum GameState
        {
            Game,
            Death
        }



        #region Fields

        public GameState State = GameState.Game;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //public delegate void TileSelectionHandler(int x, int y);
        public delegate void OmniSelectionHandler(Vector2 vec);
        public delegate void EnemyEventHandler(EnemyType type, Enemy e);

        public event OmniSelectionHandler OmniSelectionMade;
        public event EnemyEventHandler EnemyDeath;
        //public event TileSelectionHandler TileSelectionMade;

        public int ManaSelectedRefill = 0;
        public bool ShowingManaRefill = false;
        int[,] tileTypes = new int[9, 9];
        String[] lines = System.IO.File.ReadAllLines("screen1.txt");
        String[] template = new String[5];
        public Song[] themes = new Song[2];
        Song theme1, theme2;
        public Grid screen = new Grid();
        public Player wizard;
        Texture2D wizardup, wizarddown, wizardleft, wizardright;
        public Texture2D gui, heart, heartEmpty;
        public Texture2D air, water, earth, fire;
        GamePadState pad1, oldpad1;
        public Texture2D fireballleft, fireballright, fireballup, fireballdown;
        public Texture2D mudballleft, mudballright, mudballup, mudballdown;
        public SpriteFont Flyover, Hudfont;
        Texture2D omnisel;
        Texture2D tilesel;
        Texture2D enemy1;
        Texture2D bullet;

        public Song currentTheme;

        Effect saturation;
        Effect satlit;

        bool bossSpawned;
        public SoundEffect dingSound, waterSound, fireSound, buzzerSound, bansheeSound;
        public Song death, lose;
        public Dictionary<string, Texture2D> TextureDictionary = new Dictionary<string, Texture2D>();
        public List<PrefabAnimation> Animations = new List<PrefabAnimation>();
        public List<SpellProjectile> ActiveProjectiles = new List<SpellProjectile>();
        public Map map = new Map();
        public int mapr = 0;
        public int mapc = 0;        

        public bool showingTileSelector = false;
        public bool showingOmniSelector = false;
        float selectionSensitivity = 10f;

        public List<Enemy> enemies = new List<Enemy>();
        Vector2 omniSelVector = new Vector2();

        public Texture2D blank;
        public bool timeStopped = false;
        public int timeStopTimer = 0;
        public int enemiesRemaining;

        public DateTime startGame;

        bool gameDone = false;
        #endregion

        public Game1()
        {
            Game1.Instance = this;
            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 800;
            graphics.SynchronizeWithVerticalRetrace = true;
            Content.RootDirectory = "Content";
            OmniSelectionMade += (Vector2 v) => { };
            EnemyDeath += (EnemyType e, Enemy et) => { };
        }

        public void TriggerEnemyDeath(EnemyType e, Enemy et)
        {
            enemiesRemaining--;
            EnemyDeath(e, et);

            if (e == EnemyType.Boss)
            {
                gameDone = true;
            }
        }

        protected override void Initialize()
        {            
            SpellElement.InitializeElements();
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
                        if (map.map[i, j].grid[1, 3].material == Tile.Material.Rock)
                        { map.map[i, j].grid[0, 4] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("RockTile")); }
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
                        if (map.map[i, j].grid[1, 3].material == Tile.Material.Rock)
                        { map.map[i, j].grid[8, 4] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("RockTile")); }
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
                        if (map.map[i, j].grid[1, 3].material == Tile.Material.Rock)
                        { map.map[i, j].grid[4, 0] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("RockTile")); }
                        
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
                        if (map.map[i, j].grid[1, 3].material == Tile.Material.Rock)
                        { map.map[i, j].grid[4, 8] = new Tile(Tile.Material.Rock, false, false, Content.Load<Texture2D>("RockTile")); }
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

        public Texture2D freeze;

        protected override void LoadContent()
        {
            freeze = new Texture2D(GraphicsDevice, 800, 800);
            renderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            blank = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1);
            blank.SetData(new Color[] { Color.White });
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
            enemy1 = Content.Load<Texture2D>("enemy1");
            bullet = Content.Load<Texture2D>("bullet");
           

            wizard = new Player(4, 4, wizardup, wizarddown, wizardleft, wizardright, spriteBatch, this);
            wizard.MaxHealth = 50;
            wizard.Health = 50;
            wizard.Activate();
            
            TextureDictionary.Add("wizard.down", wizarddown);
            TextureDictionary.Add("wizard.up", wizardup);
            TextureDictionary.Add("wizard.left", wizardleft);
            TextureDictionary.Add("wizard.right", wizardright);

            TextureDictionary.Add("symbols.fire", Content.Load<Texture2D>("Symbols/fire"));
            TextureDictionary.Add("symbols.water", Content.Load<Texture2D>("Symbols/water"));
            TextureDictionary.Add("symbols.earth", Content.Load<Texture2D>("Symbols/earth"));
            TextureDictionary.Add("symbols.air", Content.Load<Texture2D>("Symbols/air"));
            TextureDictionary.Add("symbols.light", Content.Load<Texture2D>("Symbols/light"));

            TextureDictionary.Add("projectile", Content.Load<Texture2D>("projectile"));

            saturation = Content.Load<Effect>("Effects/Saturation");
            satlit = Content.Load<Effect>("Effects/Saturation");
            satlit.Parameters["Saturation"].SetValue(1f);

            tilesel = Content.Load<Texture2D>("TileSelector");
            omnisel = Content.Load<Texture2D>("OmniSelector");

            Flyover = Content.Load<SpriteFont>("flyoverfont");
            Hudfont = Content.Load<SpriteFont>("hudfont");

            fireSound = Content.Load<SoundEffect>("fireball");
            waterSound = Content.Load<SoundEffect>("wave");
            dingSound = Content.Load<SoundEffect>("ding");
            buzzerSound = Content.Load<SoundEffect>("buzzer");
            bansheeSound = Content.Load<SoundEffect>("banshee");
            death = Content.Load<Song>("death");
            lose = Content.Load<Song>("lose");
            theme1 = Content.Load<Song>("theme1");
            theme2 = Content.Load<Song>("theme2");
            themes[0] = theme1;
            themes[1] = theme2;
            Random rand = new System.Random();
            Game1.Instance.currentTheme = Game1.Instance.themes[rand.Next(0, 2)];
            MediaPlayer.Stop();
            MediaPlayer.Play(Game1.Instance.currentTheme);

            startGame = DateTime.Now;
        }

        protected override void UnloadContent()
        {
        }

        bool mouseActive = false;
        MouseState oldmouse, mouse;

        int exitTime = 0, maxExitTime = 180;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                exitTime++;
            else exitTime = 0;

            if (exitTime >= maxExitTime) this.Exit();

            if (State == GameState.Game)
                UpdateMainGame(gameTime);
            else if (State == GameState.Death)
            {
                deathAnimTrans -= 0.01f;
                if (new Rectangle(300, 400, 200, 100).Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed) Exit();
                if (Save.GetSavedData<bool>("HasSave", false))
                {
                    if (new Rectangle(250, 600, 300, 100).Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed) LoadGame();
                }
            }
            base.Update(gameTime);
        }

        void LoadGame()
        {
            screen = Save.GetSavedData<Grid>("Screen");
            wizard = Save.GetSavedData<Player>("Player").Copy();
            wizard.Activate();
            map.map = Save.GetSavedData<Grid[,]>("Map");
            mapr = Save.GetSavedData<int>("mapr", mapr);
            mapc = Save.GetSavedData<int>("mapc", mapc);
            timeStopped = Save.GetSavedData<bool>("TimeStopped", timeStopped);
            timeStopTimer = Save.GetSavedData<int>("TimeStopTimer", timeStopTimer);
            State = GameState.Game;
        }

        int timeStopTime = 0;
        float satlitsat = 1f;
        float satlitlit = 0f;
        protected void UpdateMainGame(GameTime gameTime)
        {

            pad1 = GamePad.GetState(PlayerIndex.One);            
            mouse = Mouse.GetState();

            if (timeStopTimer <= 0) { timeStopped = false; }
            else { timeStopTimer--; }

            if (timeStopped)
            {
                timeStopTime++;
                float sat = 1f;
                float mult = MathHelper.Clamp((30 - timeStopTime) / 30f, 0f, 1f);
                sat = MathHelper.Clamp(sat * mult, 0.3f, 1f);                
                saturation.Parameters["Saturation"].SetValue(sat);
            }
            else timeStopTime = 0;
            //gameDone = true;
            if (gameDone)
            {
                //satlitsat -= 0.005f;
                //satlitlit += 0.005f;
                satlit.Parameters["Saturation"].SetValue(MathHelper.Clamp(satlitsat, 0f, 1f));
                satlit.Parameters["Brightness"].SetValue(MathHelper.Clamp(satlitlit, 0f, 1f));
            }

            if (oldmouse == null) oldmouse = mouse;

            if (mouse.X != oldmouse.X || mouse.Y != oldmouse.Y) mouseActive = true;
            else mouseActive = false;

            if (mouseActive)
            {
                omniSelVector = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }

            omniSelVector.X += pad1.ThumbSticks.Right.X * selectionSensitivity;
            omniSelVector.Y -= pad1.ThumbSticks.Right.Y * selectionSensitivity;

            wizard.UpdateProjectiles(screen);
            wizard.PollInput();
            map.map[mapr, mapc].visible = true;

            if (pad1.Buttons.RightShoulder == ButtonState.Pressed && oldpad1.Buttons.RightShoulder == ButtonState.Released)
            {
                if (showingOmniSelector) OmniSelectionMade((omniSelVector));
            }
            if (mouse.LeftButton == ButtonState.Pressed && oldmouse.LeftButton == ButtonState.Released)
            {
                if (showingOmniSelector) OmniSelectionMade((omniSelVector));
            }

            if (!showingOmniSelector)
            {
                omniSelVector = new Vector2(wizard.PositionV.X+137,wizard.PositionV.Y+237);
            }
           
           
            for (int i = 0; i < Animations.Count; i++) if (Animations[i].NeedsRemove) Animations.RemoveAt(i);
            foreach (PrefabAnimation pa in Animations) pa.Update(gameTime);

            for (int i = 0; i < ActiveProjectiles.Count; i++) if (ActiveProjectiles[i].NeedsRemove) ActiveProjectiles.RemoveAt(i);
            foreach (SpellProjectile sp in ActiveProjectiles) sp.Update();
            
            if (!timeStopped)
            foreach (Enemy e in screen.enemyList) e.Update(gameTime, wizard.PositionV + new Vector2(100,200), wizard);
          
            if (enemiesRemaining <= 0 && !bossSpawned)
            {
                map.map[2, 2].enemyList = new List<Enemy>();
                bossSpawned = true;

                map.map[2, 2] = new Grid();
                for (int k = 0; k < 9; k++)
                {
                    for (int l = 0; l < 9; l++)
                    {
                        map.map[2, 2].grid[k, l] = new Tile(Tile.Material.Sand, true, false, this.Content.Load<Texture2D>("SandTile"));
                        map.map[2, 2].grid[k, 0] = new Tile(Tile.Material.Sand, false, false, this.Content.Load<Texture2D>("RockTile"));
                        map.map[2, 2].grid[k, 8] = new Tile(Tile.Material.Sand, false, false, this.Content.Load<Texture2D>("RockTile"));
                        map.map[2, 2].grid[0, l] = new Tile(Tile.Material.Sand, false, false, this.Content.Load<Texture2D>("RockTile"));
                        map.map[2, 2].grid[8, l] = new Tile(Tile.Material.Sand, false, false, this.Content.Load<Texture2D>("RockTile"));
                        map.map[2, 2].grid[4, 0] = new Tile(Tile.Material.Sand, true, false, this.Content.Load<Texture2D>("RockTile"));
                        map.map[2, 2].grid[4, 8] = new Tile(Tile.Material.Sand, true, false, this.Content.Load<Texture2D>("RockTile"));
                        map.map[2, 2].grid[0, 4] = new Tile(Tile.Material.Sand, true, false, this.Content.Load<Texture2D>("RockTile"));
                        map.map[2, 2].grid[8, 4] = new Tile(Tile.Material.Sand, true, false, this.Content.Load<Texture2D>("RockTile"));
                    }
                }
                map.map[2,2].enemyList.Add(new Enemy(100,3,10,10,new Rectangle(400, 500, 134,134), -MathHelper.Pi, this.Content.Load<Texture2D>("boss"),bullet, this, EnemyTypeAI.Boss, SpellElement.None));

                Save.SetSavedData<Grid>("Screen", screen);
                Save.SetSavedData<Player>("Player", wizard.Copy());
                Save.SetSavedData<Grid[,]>("Map", map.map);
                Save.SetSavedData<int>("mapr", mapr);
                Save.SetSavedData<int>("mapc", mapc);
                Save.SetSavedData<bool>("TimeStopped", timeStopped);
                Save.SetSavedData<int>("TimeStopTimer", timeStopTimer);
                Save.SetSavedData<bool>("HasSave", true);
            }

            oldpad1 = pad1;
            oldmouse = mouse;
            base.Update(gameTime);
        }
        public RenderTarget2D renderTarget;
        public bool isDying = false;
        float deathAnimTrans = 1f;
        protected override bool BeginDraw()
        {
            if (isDying)
            {
                MediaPlayer.Stop();
                MediaPlayer.Play(death);
                GraphicsDevice.SetRenderTargets(renderTarget);
            }
            return base.BeginDraw();
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            if (State == GameState.Game)
                DrawMainGame(gameTime);
            else if (State == GameState.Death)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                Vector2 gameoverPos = new Vector2();
                string goText = "Game Over";
                gameoverPos.X = (800 - Hudfont.MeasureString(goText).X) / 2;
                gameoverPos.Y = ((800 - Hudfont.MeasureString(goText).Y)) / 4;
                spriteBatch.DrawString(Hudfont, goText, gameoverPos, Color.LightGray);

                spriteBatch.Draw(blank, new Rectangle(300, 400, 200, 100), Color.White);
                spriteBatch.Draw(blank, new Rectangle(302, 402, 196, 96), Color.Black);

                Vector2 exitPos = new Vector2();
                string exitText = "Exit";
                exitPos.X = (196 - Hudfont.MeasureString(exitText).X) / 2;
                exitPos.Y = ((96 - Hudfont.MeasureString(exitText).Y)) / 2;
                exitPos.X += 302;
                exitPos.Y += 402;
                spriteBatch.DrawString(Hudfont, exitText, exitPos, Color.LightGray);


                if (Save.GetSavedData<bool>("HasSave", false))
                {
                    spriteBatch.Draw(blank, new Rectangle(250, 600, 300, 100), Color.White);
                    spriteBatch.Draw(blank, new Rectangle(252, 602, 296, 96), Color.Black);

                    Vector2 loadSavePos = new Vector2();
                    string loadSaveText = "Load Game";
                    loadSavePos.X = (296 - Hudfont.MeasureString(loadSaveText).X) / 2;
                    loadSavePos.Y = ((96 - Hudfont.MeasureString(loadSaveText).Y)) / 2;
                    loadSavePos.X += 252;
                    loadSavePos.Y += 602;
                    spriteBatch.DrawString(Hudfont, loadSaveText, loadSavePos, Color.LightGray);
                }

                spriteBatch.Draw(freeze, Vector2.Zero, Color.White * deathAnimTrans);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
        protected override void EndDraw()
        {
            if (isDying)
            {
                
                MediaPlayer.Play(lose);
                GraphicsDevice.SetRenderTarget(null);                
                freeze = (Texture2D)renderTarget;
                System.IO.FileStream st = new System.IO.FileStream("latest-death.jpg", System.IO.FileMode.Create);
                freeze.SaveAsJpeg(st, 800, 800);
                st.Flush();
                st.Close();
                isDying = false;
                State = GameState.Death;
            }
            base.EndDraw();
        }
        protected void DrawMainGame(GameTime gameTime)
        {            
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null);

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
                        //if (true)
                        { spriteBatch.Draw(g.GetTile(i, j).tileTexture, new Rectangle(i * dim + 5 + (horzOffset * dim * 9), j * dim + 5 + (vertOffsets * dim * 9), dim, dim), Color.White); }
                    }
                }
                vertOffsets++;
                renders++;
            }            
            spriteBatch.Draw(this.Content.Load<Texture2D>("playerMarker"), new Rectangle((mapr * dim *9) + 1 + (dim *9 / 2), (mapc * dim * 9) + 1 + (dim *9 / 2), dim *2, dim * 2), Color.White);

            if (timeStopped)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, saturation);
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    
                    spriteBatch.Draw(screen.GetTile(i, j).tileTexture, new Rectangle(i * 67 + 100, j * 67 + 200, 67, 67), Color.White);
                    if (showingTileSelector)
                        if (((Mouse.GetState().X - 100) / 67) == i && ((Mouse.GetState().Y - 200) / 67) == j) spriteBatch.Draw(tilesel, new Rectangle(i * 67 + 100, j * 67 + 200, 67, 67), Color.White);
                }
            }
            if (timeStopped)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            }

            if (showingOmniSelector)
                spriteBatch.Draw(omnisel, new Rectangle((int)omniSelVector.X - 33, (int)omniSelVector.Y - 33, 67, 67), Color.White);

            foreach (SpellProjectile sp in ActiveProjectiles) sp.Draw(spriteBatch);
            foreach (Projectile p in wizard.projectiles) 
            {
                
                p.Draw(spriteBatch);
            }
            if (timeStopped)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, saturation);
            }
            foreach (Enemy e in screen.enemyList) 
            {
                e.Draw(spriteBatch);
            }

            if (timeStopped)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            }

            wizard.Draw(spriteBatch);            

            #region Status

            #region Health

            //spriteBatch.DrawString(Hudfont, "Health: ", new Vector2(200, 10), Color.White);

            #endregion

            #region Mana

            Mana m = wizard.mana;
            int[] convVals = new int[5];
            int[] srcVals = new int[5];
            for (int i = 0; i < convVals.Length; i++)
            {
                float mult = (float)m[SpellElement.RegisteredElements[i]] / (float)m.MaximumMana;
                float height = mult * 64;
                float srchgt = mult * 200;
                convVals[i] = (int)height;
                srcVals[i] = (int)srchgt;
            }

            if (ShowingManaRefill)
            spriteBatch.Draw(tilesel, new Rectangle(426 + 72 * ManaSelectedRefill, 112, 64, 64), Color.White);

            spriteBatch.Draw(TextureDictionary["symbols.light"], new Rectangle(426, 112 + (64 - convVals[4]), 64, convVals[4]), new Rectangle(0, 200 - srcVals[4], 200, srcVals[4]), Color.White); //4
            spriteBatch.Draw(TextureDictionary["symbols.air"], new Rectangle(498, 112 + (64 - convVals[1]), 64, convVals[1]), new Rectangle(0, 200 - srcVals[1], 200, srcVals[1]), Color.White);
            spriteBatch.Draw(TextureDictionary["symbols.water"], new Rectangle(570, 112 + (64 - convVals[2]), 64, convVals[2]), new Rectangle(0, 200 - srcVals[2], 200, srcVals[2]), Color.White);
            spriteBatch.Draw(TextureDictionary["symbols.fire"], new Rectangle(642, 112 + (64 - convVals[0]), 64, convVals[0]), new Rectangle(0, 200 - srcVals[0], 200, srcVals[0]), Color.White);
            spriteBatch.Draw(TextureDictionary["symbols.earth"], new Rectangle(714, 112 + (64 - convVals[3]), 64, convVals[3]), new Rectangle(0, 200 - srcVals[3], 200, srcVals[3]), Color.White);            

            spriteBatch.DrawString(Hudfont, "~Mana~", new Vector2(((714+64-426)-Hudfont.MeasureString("~Mana~").X)/2 + 426, 48), Color.White);

            #endregion

            #endregion

            foreach (PrefabAnimation pa in Animations) pa.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
