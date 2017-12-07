﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DungeonPlanet.Library;

namespace DungeonPlanet
{
    public class Player : Sprite
    {
        public PlayerLib PlayerLib { get; set; }
        public  Weapon Weapon { get; set; }
        public int Life { get; set; }
        public static Player CurrentPlayer { get; private set; }
        public Shield Shield { get; set; }

        KeyboardState _previousKey;

        public Player(Texture2D texturePlayer, Texture2D textureWeapon, Texture2D textureBullet, DungeonPlanetGame ctx, Vector2 position, SpriteBatch spritebatch, List<Enemy> enemys, List<Boss> bosses)
            : base(texturePlayer, position, spritebatch)
        {
            PlayerLib = new PlayerLib(new System.Numerics.Vector2(position.X,position.Y), texturePlayer.Width, texturePlayer.Height);
            Weapon = new Weapon(textureWeapon, textureBullet, ctx, position, spritebatch, bosses);
            Life = 70;
            CurrentPlayer = this;
        }
        
        public void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();
            PlayerLib.AffectWithGravity();
            PlayerLib.SimulateFriction();
            PlayerLib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 15);
            PlayerLib.StopMovingIfBlocked();
            PlayerLib.IsDead(Life);
            position = new Vector2(PlayerLib.Position.X, PlayerLib.Position.Y);
            Weapon.Update(gameTime);
            Life = MathHelper.Clamp(Life, 0, 100);
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            _previousKey = keyboardState;

            if (keyboardState.IsKeyDown(Keys.Q))
            {
                PlayerLib.Left();
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                PlayerLib.Right();
            }
            if (keyboardState.IsKeyDown(Keys.Z) && PlayerLib.IsOnFirmGround())
            { 
                PlayerLib.Jump();
            }
        }

        public override void Draw()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            _previousKey = keyboardState;
            base.Draw();
            Weapon.Draw();

            if (keyboardState.IsKeyDown(Keys.A) && !(Shield.Activate))
            {
                Shield.Activate = true;
                Shield.Draw();
            }
            else if (keyboardState.IsKeyUp(Keys.A) && Shield.Activate == true) { Shield.Draw(); }
            else if (keyboardState.IsKeyDown(Keys.A) && Shield.Activate == true)
            {
                Shield.Activate = false;
            }
        }

    }
}
