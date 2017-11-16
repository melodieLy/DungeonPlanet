﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using DungeonPlanet.Library;

namespace DungeonPlanet
{
    public class Enemy : Sprite
    {
        public EnemyLib EnemyLib { get; set; }
        public PlayerLib PlayerLib { get; set; }

        int _time;
        Player _player;

        public Enemy(Player player,Texture2D texture, Vector2 position, SpriteBatch spritebatch)
            : base(texture, position, spritebatch)
        {
            EnemyLib = new EnemyLib(new System.Numerics.Vector2(position.X, position.Y), texture.Width, texture.Height);
            _player = player;
            PlayerLib = _player.PlayerLib;
            _time = 0;
        }

        public void Update(GameTime gameTime)
        {
            CheckKeyboardAndUpdateMovement();
            EnemyLib.AffectWithGravity();
            EnemyLib.SimulateFriction();
            EnemyLib.MoveAsFarAsPossible((float)gameTime.ElapsedGameTime.TotalMilliseconds / 40);
            EnemyLib.StopMovingIfBlocked();
            Position = new Vector2(EnemyLib.Position.X, EnemyLib.Position.Y);
        }

        private void CheckKeyboardAndUpdateMovement()
        {
            if (EnemyLib.Vision.IntersectsWith(PlayerLib.Bounds))
            {
                if (EnemyLib.GetDistanceTo(PlayerLib.Position).X < 1) { EnemyLib.Left(); }
                if (EnemyLib.GetDistanceTo(PlayerLib.Position).X > 1) { EnemyLib.Right(); }
                /*if (EnemyLib.GetDistanceTo(PlayerLib.Position).X == 0) { EnemyLib.Position = PlayerLib.Position; };*/
                if (EnemyLib.GetDistanceTo(PlayerLib.Position).X == 0) { EnemyLib.Position = PlayerLib.Position; };
            }
        }
    }
}