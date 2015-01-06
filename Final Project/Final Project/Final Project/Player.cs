﻿using System;
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
    public class Player : LivingEntity
    {
        // Need floats for positioning not ints.
        public int GridX { get { return (int)((PositionV.X + 67 / 2) / 67); } set { PositionV = new Vector2(value * 67, PositionV.Y); } }
        public int GridY { get { return (int)((PositionV.Y + 67 / 2) / 67); } set { PositionV = new Vector2(PositionV.X, value * 67); } }
        float speed = 3;        
        public Rectangle Position;
        public Texture2D texture;
        public Texture2D textureUp;
        public Texture2D textureDown;
        public Texture2D textureLeft;
        public Texture2D textureRight;
        public SpriteBatch spriteBatch;
        public List<Projectile> projectiles = new List<Projectile>();
        public event RecordStatusEvent OnRecordStatusChanged;
        SpellComboList currentCombo = new SpellComboList();
        private bool recordingStatus = false;

        public bool RecordStatus
        {
            get { return recordingStatus; }
            set 
            {
                bool evnt = recordingStatus != value;                
                recordingStatus = value;
                if (evnt) OnRecordStatusChanged(value);
            }
        }
        public int GetGridPosFromFloat(float val)
        {
            return (int)((val+(Math.Sign(val))*(67/2)) / 67);
        }
        Texture2D blank;

        public Player(int playerRow, int playerColumn, Texture2D tup, Texture2D tdown, Texture2D tleft, Texture2D tright, SpriteBatch game1spriteBatch, Game1 game) : base(game)
        {
            element = SpellElement.Light;
            Position = new Rectangle();
            GridX = playerRow;
            GridY = playerColumn;
            textureUp = tup;
            textureDown = tdown;
            textureLeft = tleft;
            textureRight = tright;
            texture = tdown;
            spriteBatch = game1spriteBatch;
            OnRecordStatusChanged += new RecordStatusEvent(Player_OnRecordStatusChanged);
            currentCombo.OnSpellAdded += new SpellComboList.SpellAdded(currentCombo_OnSpellAdded);
            blank = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1);
            blank.SetData(new Color[] { Color.White });
        }

        void currentCombo_OnSpellAdded(SpellElement type)
        {
            Console.WriteLine("Element: " + type.Name);
            Game1.Instance.Animations.Add(new Animations.SpellFlash(type));
        }
        Spell current;
        void Player_OnRecordStatusChanged(bool on)
        {
            Console.WriteLine("Record Combo: " + (on ? "on" : "off"));
            if (!on)
            {
                Spell c = currentCombo.Complete();                
                currentCombo.Clear();

                Console.WriteLine(c.Name);

                if (c is LivingTargetSpell)
                {
                    current = c;
                    Game1.Instance.showingOmniSelector = true;
                    Game1.Instance.OmniSelectionMade += new Game1.OmniSelectionHandler(Instance_OmniSelectionMade);
                }
            }            
        }

        void Instance_OmniSelectionMade(Vector2 vec)
        {
            LivingTargetSpell ltp = (LivingTargetSpell)current;
            Game1.Instance.ActiveProjectiles.Add(new LivingTargetProjectile(this, vec, ltp));
        }

        public void MoveOld(int r, int c, Grid tiles)
        {
            if (Math.Abs(GridX - r) <= 1 && Math.Abs(GridY - c) <= 1 && tiles.GetTile(r, c).canWalk) { GridX = r; GridY = c; }
        }
        public void Move(float x, float y, Grid tiles)
        {
            float xi = PositionV.X + x;
            float yi = PositionV.Y - y;
            int xr = GetGridPosFromFloat(xi);
            int yr = GetGridPosFromFloat(yi);
            if (xr > 8 && Game1.Instance.mapr < 4)
            {
                Game1.Instance.mapr++;
                Game1.Instance.screen = Game1.Instance.map.map[Game1.Instance.mapr, Game1.Instance.mapc];                
                GridX = 0;
                return;
            }
            if (xr < 0 && Game1.Instance.mapr > 0)
            {
                Game1.Instance.mapr--;
                Game1.Instance.screen = Game1.Instance.map.map[Game1.Instance.mapr, Game1.Instance.mapc];
                GridX = 8;
                return;
            }
            if (yr > 8 && Game1.Instance.mapc < 4)
            {
                Game1.Instance.mapc++;
                Game1.Instance.screen = Game1.Instance.map.map[Game1.Instance.mapr, Game1.Instance.mapc];
                GridY = 0;
                return;
            }
            if (yr < 0 && Game1.Instance.mapc > 0)
            {
                Game1.Instance.mapc--;
                Game1.Instance.screen = Game1.Instance.map.map[Game1.Instance.mapr, Game1.Instance.mapc];
                GridY = 8;
                return;
            }

            if (tiles.GetTile(xr, GridY).canWalk)
            {
                PositionV = new Vector2(PositionV.X + x, PositionV.Y);                
            }
            if (tiles.GetTile(GridX, yr).canWalk)
            {
                PositionV = new Vector2(PositionV.X, PositionV.Y - y);
            }

            if (x != 0 && x > 0) texture = textureRight;
            if (x != 0 && x < 0) texture = textureLeft;
            if (y != 0 && y > 0) texture = textureUp;
            if (y != 0 && y < 0) texture = textureDown;
        }
        public void Shoot(ProjectileType type, Vector2 vel, Texture2D projtexture) 
        {
            Projectile temp = new Projectile(5,new Vector2(GridX * 67, GridY * 67), vel, type, projtexture);
            if (projectiles.Count() < 3) { projectiles.Add(temp);  }
        }
        GamePadState oldState, curState;
        public void PollInput()
        {
            curState = GamePad.GetState(PlayerIndex.One);
            if (oldState == null) oldState = curState;

            if (Keyboard.GetState().IsKeyDown(Keys.F1)) Console.WriteLine("Row: " + GridX + "; Col: " + GridY);
            if (curState.IsButtonDown(Buttons.Start)) { Game1.Instance.screen = Game1.Instance.map.map[0, 0]; GridX = 2; GridY = 2; Game1.Instance.mapr = 0; Game1.Instance.mapc = 0; }

            /*if (!Game1.Instance.screen.GetTile(Row, Col).canWalk)
            {
                Row--;
                Col--;
            }/*/

            if (curState.Triggers.Left >= 0.75f)
            {
                RecordStatus = true;
            }
            else { RecordStatus = false; }

            if (RecordStatus && currentCombo.Count < 3)
            {
                if (oldState.Buttons.B == ButtonState.Released && curState.Buttons.B == ButtonState.Pressed) currentCombo.Add(SpellElement.Fire);
                else if (oldState.Buttons.A == ButtonState.Released && curState.Buttons.A == ButtonState.Pressed) currentCombo.Add(SpellElement.Air);
                else if (oldState.Buttons.X == ButtonState.Released && curState.Buttons.X == ButtonState.Pressed) currentCombo.Add(SpellElement.Water);
                else if (oldState.Buttons.Y == ButtonState.Released && curState.Buttons.Y == ButtonState.Pressed) currentCombo.Add(SpellElement.Earth);
                else if (oldState.Buttons.RightShoulder == ButtonState.Released && curState.Buttons.RightShoulder == ButtonState.Pressed) currentCombo.Add(SpellElement.Light);
            }
            
            #region Input > Move
            float x = curState.ThumbSticks.Left.X * speed;
            float y = curState.ThumbSticks.Left.Y * speed;
            Move(x, y, Game1.Instance.screen);
            //Console.WriteLine("X: " + x + "; Y: " + y);
            #endregion

            oldState = curState;
            
        }

        public void UpdateProjectiles(Grid tiles) 
        {
            
            foreach (Projectile p in projectiles) 
            {
                
                p.Location += (p.Velocity *p.Speed);

                if (Vector2.Distance(p.Location, new Vector2(GridX * 67, GridY * 67)) > 300 || tiles.GetTile(new Rectangle((int)p.Location.X, (int)p.Location.Y, 1,1)).canWalk != true) { p.Visible = false; }
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
            int leftMargin = 100;
            int topMargin = 200;
            //spriteBatch.Draw(texture, new Rectangle(row * 67 + 100, col * 67 + 200, 67, 67), Color.White);
            //spriteBatch.Draw(blank, new Rectangle(Row * 67 + leftMargin, Col * 67 + topMargin, 67, 67), Color.Red);
            spriteBatch.Draw(texture, new Rectangle((int)PositionV.X + leftMargin, (int)PositionV.Y + topMargin, 67, 67), Color.White);
            
        }
    }
}
