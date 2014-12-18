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
    public class Player
    {
        public int row;
        public int col;
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

        public Player(int playerRow, int playerColumn, Texture2D tup, Texture2D tdown, Texture2D tleft, Texture2D tright, SpriteBatch game1spriteBatch)
        {
            row = playerRow;
            col = playerColumn;
            textureUp = tup;
            textureDown = tdown;
            textureLeft = tleft;
            textureRight = tright;
            texture = tdown;
            spriteBatch = game1spriteBatch;
            OnRecordStatusChanged += new RecordStatusEvent(Player_OnRecordStatusChanged);
            currentCombo.OnSpellAdded += new SpellComboList.SpellAdded(currentCombo_OnSpellAdded);
        }

        void currentCombo_OnSpellAdded(SpellElement type)
        {
            Console.WriteLine("Element: " + type.Name);
            Game1.Instance.Animations.Add(new Animations.SpellFlash(type));
        }

        void Player_OnRecordStatusChanged(bool on)
        {
            Console.WriteLine("Record Combo: " + (on ? "on" : "off"));
            if (!on)
            {
                Spell c = currentCombo.Complete();
                Console.WriteLine(c.Name);
                currentCombo.Clear();
            }            
        }
        public void Move(int r, int c, Grid tiles)
        {
            if (Math.Abs(row - r) <= 1 && Math.Abs(col - c) <= 1 && tiles.GetTile(r, c).canWalk) { row = r; col = c; }
        }
        public void Shoot(ProjectileType type, Vector2 vel, Texture2D projtexture) 
        {
            Projectile temp = new Projectile(5,new Vector2(row * 67, col * 67), vel, type, projtexture);
            if (projectiles.Count() < 3) { projectiles.Add(temp);  }
        }
        GamePadState oldState, curState;
        public void PollInput()
        {
            curState = GamePad.GetState(PlayerIndex.One);
            if (oldState == null) oldState = curState;

            if (curState.Triggers.Left >= 0.75f)
            {
                RecordStatus = true;
            }
            else { RecordStatus = false; }

            if (RecordStatus && currentCombo.Count < 3)
            {
                if      (oldState.Buttons.B == ButtonState.Released && curState.Buttons.B == ButtonState.Pressed) currentCombo.Add(SpellElement.Fire);
                else if (oldState.Buttons.A == ButtonState.Released && curState.Buttons.A == ButtonState.Pressed) currentCombo.Add(SpellElement.Air);
                else if (oldState.Buttons.X == ButtonState.Released && curState.Buttons.X == ButtonState.Pressed) currentCombo.Add(SpellElement.Water);
                else if (oldState.Buttons.Y == ButtonState.Released && curState.Buttons.Y == ButtonState.Pressed) currentCombo.Add(SpellElement.Earth);
            }

            #region Input > Move
            if (curState.ThumbSticks.Left.X > 0 && !(oldState.ThumbSticks.Left.X > 0))
            {
                if (row != 8)
                {
                    Move(row + 1, col, Game1.Instance.screen); texture = textureRight;
                }
                else
                {

                    if (Game1.Instance.mapr < 4)
                    {
                        Game1.Instance.mapr += 1;
                        Game1.Instance.screen = Game1.Instance.map.map[Game1.Instance.mapr, Game1.Instance.mapc];
                        row = 0;
                    }

                }

            }
            if (curState.ThumbSticks.Left.X < 0 && !(oldState.ThumbSticks.Left.X < 0))
            {
                if (row != 0)
                {
                    Move(row - 1, col, Game1.Instance.screen); texture = textureLeft;
                }
                else
                {

                    if (Game1.Instance.mapr > 0)
                    {
                        Game1.Instance.mapr -= 1;
                        Game1.Instance.screen = Game1.Instance.map.map[Game1.Instance.mapr, Game1.Instance.mapc];
                        row = 8;
                    }

                }
            }
            if (curState.ThumbSticks.Left.Y > 0 && !(oldState.ThumbSticks.Left.Y > 0))
            {
                if (col != 0)
                {
                    Move(row, col - 1, Game1.Instance.screen); texture = textureUp;
                }
                else
                {

                    if (Game1.Instance.mapc > 0)
                    {
                        Game1.Instance.mapc -= 1;
                        Game1.Instance.screen = Game1.Instance.map.map[Game1.Instance.mapr, Game1.Instance.mapc];
                        col = 8;
                    }

                }
            }
            if (curState.ThumbSticks.Left.Y < 0 && !(oldState.ThumbSticks.Left.Y < 0))
            {
                if (col != 8)
                {
                    Move(row, col + 1, Game1.Instance.screen); texture = textureDown;
                }
                else
                {

                    if (Game1.Instance.mapc < 4)
                    {
                        Game1.Instance.mapc += 1;
                        Game1.Instance.screen = Game1.Instance.map.map[Game1.Instance.mapr, Game1.Instance.mapc];
                        col = 0;

                    }

                }
            }
            #endregion

            oldState = curState;
            
        }

        public void UpdateProjectiles(Grid tiles) 
        {
            
            foreach (Projectile p in projectiles) 
            {
                
                p.Location += (p.Velocity *p.Speed);

                if (Vector2.Distance(p.Location, new Vector2(row * 67, col * 67)) > 300 || tiles.GetTile(new Rectangle((int)p.Location.X, (int)p.Location.Y, 1,1)).canWalk != true) { p.Visible = false; }
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
            spriteBatch.Draw(texture, new Rectangle(row * 67 + 100, col * 67 + 200, 67, 67), Color.White);
        }
    }
}
