﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Comora;
using DungeonPlanet.Library;
namespace DungeonPlanet
{

    public class DungeonPlanetGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _tileTexture, _playerTexture, _enemyTexture, _enemyTexture2, _enemyWeaponTexture, _bossTexture, _weaponTexture, _bulletTexture, _bulletETexture, _mediTexture;
        private Player _player;
        private Enemy _enemy;
        private Enemy _enemy2;
        private Boss _boss;
        private Board _board;
        private Random _rnd = new Random();
        private MediPack _mediPack;
        private SpriteFont _debugFont;
        private Camera _camera;
        public static List<Enemy> Enemys { get; private set; }
        public static List<Boss> Bosses { get; private set; }

        public DungeonPlanetGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            Enemys = new List<Enemy>();
            Bosses = new List<Boss>();
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _tileTexture = Content.Load<Texture2D>("tile");
            _playerTexture = Content.Load<Texture2D>("player");
            _enemyTexture = Content.Load<Texture2D>("enemy");
            _enemyTexture2 = Content.Load<Texture2D>("enemy2");
            _bossTexture = Content.Load<Texture2D>("boss");
            _weaponTexture = Content.Load<Texture2D>("player_arm");
            _enemyWeaponTexture = Content.Load<Texture2D>("player_arm");
            _bulletTexture = Content.Load<Texture2D>("bullet");
            _bulletETexture = Content.Load<Texture2D>("bulletE");
            _mediTexture = Content.Load<Texture2D>("Medipack");
            _board = new Board(_spriteBatch, _tileTexture, 2, 2);
            _player = new Player(_playerTexture, _weaponTexture, _bulletTexture, this, new Vector2(80, 80), _spriteBatch, Enemys, Bosses);
            _enemy = new Enemy( _enemyTexture, new Vector2(500, 200), _spriteBatch, "CQC");
            _enemy2 = new Enemy( _enemyTexture2, new Vector2(400, 100), _spriteBatch, "DIST", _weaponTexture, _bulletETexture, this);
            _boss = new Boss(_bossTexture, new Vector2(1360, 200), _spriteBatch);
            _mediPack = new MediPack(_mediTexture, new Vector2(300, 300), _spriteBatch, 45, _player);
            
            _debugFont = Content.Load<SpriteFont>("DebugFont");
            _camera = new Camera(GraphicsDevice);
            _camera.LoadContent(GraphicsDevice);
            
            Enemys.Add(_enemy);
            Enemys.Add(_enemy2);
            Bosses.Add(_boss);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            _camera.Update(gameTime);
            _player.Update(gameTime);
            for (int i = 0; i < Enemys.Count; i++)
            {
                if (Enemys[i].EnemyLib.Life <= 0)
                {
                    Enemys.Remove(Enemys[i]);
                }
                else
                {
                    Enemys[i].Update(gameTime);
                }
            }

            for (int i = 0; i < Bosses.Count; i++)
            {
                if (Bosses[i].BossLib.Life <= 0)
                {
                    Bosses.Remove(Bosses[i]);
                }
                else
                {
                    Bosses[i].Update(gameTime);
                }
            }

            if (_mediPack != null)
            {
                _mediPack.Update();
                if (_mediPack.IsFinished)
                {
                    _mediPack = null;
                }
            }
            if (_player.PlayerLib.IsDead(_player.Life)) RestartGame();
            _camera.Position = _player.position;
            CheckKeyboardAndReact();
        }

        private void CheckKeyboardAndReact()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.F5)) { RestartGame(); }
            if (state.IsKeyDown(Keys.Escape)) { Exit(); }
            _camera.Debug.IsVisible = Keyboard.GetState().IsKeyDown(Keys.F1);

        }

        private void RestartGame()
        {
            /*Board.CurrentBoard.CreateNewBoard();
            PutJumperInTopLeftCorner();*/
            LoadContent();
        }

        private void PutJumperInTopLeftCorner()
        {
            PlayerLib.Position = System.Numerics.Vector2.One * 80;
            _player.PlayerLib.Movement = System.Numerics.Vector2.Zero;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
           
            _spriteBatch.Begin(_camera);
            base.Draw(gameTime);
            _board.Draw();
            if (_mediPack != null) _mediPack.Draw();
            WriteDebugInformation();
            _player.Draw();
            foreach (Enemy enemy in Enemys) enemy.Draw();
            foreach (Boss boss in Bosses) boss.Draw();
            _spriteBatch.End();
            
            _spriteBatch.Draw(gameTime, _camera.Debug);
        }
        
        private void WriteDebugInformation()
        {
            string enemyLifeText;
            string positionInText = string.Format("Position of Jumper: ({0:0.0}, {1:0.0})", PlayerLib.Position.X, _player.position.Y);
            string movementInText = string.Format("Current movement: ({0:0.0}, {1:0.0})", _player.PlayerLib.Movement.X, _player.PlayerLib.Movement.Y);
            string isOnFirmGroundText = string.Format("On firm ground?: {0}", _player.PlayerLib.IsOnFirmGround());
            string playerLifeText = string.Format("PLife: {0}/100", _player.Life);
            if(_enemy != null)
            {
                enemyLifeText = string.Format("ELife: {0}/100", _enemy.EnemyLib.Life);
                DrawWithShadow(enemyLifeText, new Vector2(70, 620));
            }
            if (_enemy2 != null)
            {
                enemyLifeText = string.Format("ELife2: {0}/100", _enemy2.EnemyLib.Life);
                DrawWithShadow(enemyLifeText, new Vector2(70, 640));
            }
            string bossLifeText = string.Format("BLife: {0}/200", _boss.BossLib.Life);

            DrawWithShadow(positionInText, new Vector2(10, 0));
            DrawWithShadow(movementInText, new Vector2(10, 20));
            DrawWithShadow(isOnFirmGroundText, new Vector2(10, 40));
            DrawWithShadow("F5 for random board", new Vector2(70, 580));
            DrawWithShadow(playerLifeText, new Vector2(70, 600));
            DrawWithShadow(bossLifeText, new Vector2(1300, 600));
        }

        private void DrawWithShadow(string text, Vector2 position)
        {
            _spriteBatch.DrawString(_debugFont, text, position + Vector2.One, Color.Black);
            _spriteBatch.DrawString(_debugFont, text, position, Color.LightYellow);
        }
    }
}