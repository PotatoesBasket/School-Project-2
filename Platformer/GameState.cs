using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.ViewportAdapters;
using ParticleEffects;

namespace Platformer
{
    public class GameState : AIE.State
    {
        Game1 game = null;
        bool isLoaded = false;

        SpriteFont arialFont;
        SpriteFont ventureFont;
        SpriteFont psFont;
        Song bgm;
        SoundEffect keyS;
        SoundEffect splat;
        SoundEffectInstance keyInst;
        SoundEffectInstance splatInst;
        Texture2D heart = null;
        Texture2D spaghettiboi = null;
        Goal goal = null;        Player player = null;
        Key key = null;        List<Enemy> enemies = new List<Enemy>();        List<LockedWall> lockedWalls = new List<LockedWall>();        Camera2D camera = null;
        TiledMap map;
        TiledTileLayer collisionLayer;

        float score = 0;
        float finalScore = 0;
        float timer = 500;
        int lives = 3;
        bool showKey = true;
        bool allowMovement = true;
        bool endGame = false;
        public static int tile = 32;
        public static float meter = tile;
        public static float gravity = meter * 9.8f * 4.0f;
        public static Vector2 maxVelocity = new Vector2(meter * 10, meter * 15);
        public static float acceleration = maxVelocity.X * 2;
        public static float friction = maxVelocity.X * 6;
        public static float jumpImpulse = meter * 750;

        enum Level
        {
            W1_L1,
            W1_L2,
            W1_L3
        }
        Level level = Level.W1_L1;

        public int ScreenWidth
        {
            get { return game.GraphicsDevice.Viewport.Width; }
        }

        public int ScreenHeight
        {
            get { return game.GraphicsDevice.Viewport.Height; }
        }

        public bool ShowKey
        {
            get { return showKey; }
        }

        public bool AllowMovement
        {
            get { return allowMovement; }
        }


        public GameState(Game1 game) : base()
        {
            this.game = game;
        }

        public override void Update(ContentManager Content, GameTime gameTime)
        {
            switch (level)
            {
                case Level.W1_L1:
                    map = Content.Load<TiledMap>("Level_1");
                    if (endGame == true && Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        endGame = false;
                        allowMovement = true;
                    }
                    break;
                case Level.W1_L2:
                    map = Content.Load<TiledMap>("Level_2");
                    break;
                case Level.W1_L3:
                    map = Content.Load<TiledMap>("Level_3");
                    break;
            }

            if (isLoaded == false)
            {
                isLoaded = true;
                ventureFont = Content.Load<SpriteFont>("3Dventure");
                arialFont = Content.Load<SpriteFont>("arial");
                psFont = Content.Load<SpriteFont>("ps2p");
                bgm = Content.Load<Song>("alley cat");
                heart = Content.Load<Texture2D>("heart_x16");
                spaghettiboi = Content.Load<Texture2D>("gamewon");
                keyS = Content.Load<SoundEffect>("keys");
                splat = Content.Load<SoundEffect>("splat");
                keyInst = keyS.CreateInstance();
                splatInst = splat.CreateInstance();

                var viewportAdapter = new BoxingViewportAdapter(game.Window, game.GraphicsDevice, ScreenWidth, ScreenHeight);
                camera = new Camera2D(viewportAdapter);
                camera.Position = new Vector2(0, ScreenHeight);
                MediaPlayer.Play(bgm);

                foreach (TiledTileLayer layer in map.TileLayers)
                {
                    if (layer.Name == "collision")
                        collisionLayer = layer;
                }

                foreach (TiledObjectGroup group in map.ObjectGroups)
                {
                    if (group.Name == "player_spawn")
                    {
                        foreach (TiledObject obj in group.Objects)
                        {
                            player = new Player(this);
                            player.Load(Content);
                            player.Position = new Vector2(obj.X, obj.Y);
                        }
                    }

                    if (group.Name == "enemy_spawn")
                    {
                        foreach (TiledObject obj in group.Objects)
                        {
                            Enemy enemy = new Enemy(this);
                            enemy.Load(Content);
                            enemy.Position = new Vector2(obj.X, obj.Y);
                            enemies.Add(enemy);
                        }
                    }

                    if (group.Name == "goal")
                    {
                        foreach (TiledObject obj in group.Objects)
                        {
                            goal = new Goal(this);
                            goal.Load(Content);
                            goal.Position = new Vector2(obj.X, obj.Y);
                        }
                    }

                    if (group.Name == "key")
                    {
                        foreach (TiledObject obj in group.Objects)
                        {
                            key = new Key(this);
                            key.Load(Content);
                            key.Position = new Vector2(obj.X, obj.Y);
                        }
                    }

                    if (group.Name == "lock")
                    {
                        foreach (TiledObject obj in group.Objects)
                        {
                            LockedWall lockedWall = new LockedWall(this);
                            lockedWall.Load(Content);
                            lockedWall.Position = new Vector2(obj.X, obj.Y);
                            lockedWalls.Add(lockedWall);
                        }
                    }
                }
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            camera.Position = player.Position - new Vector2(ScreenWidth / 2, ScreenHeight / 1.5f);
            CameraMechanics();

            timer -= deltaTime;

            CheckCollisions();
            goal.Update(deltaTime);
            player.Update(deltaTime);
            key.Update(deltaTime);
            foreach (Enemy e in enemies)
            {
                e.Update(deltaTime);
                e.ParticleUpdate(gameTime);
            }
            foreach (LockedWall lw in lockedWalls)
            {
                lw.Update(deltaTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var transformMatrix = camera.GetViewMatrix();

            spriteBatch.Begin(transformMatrix: transformMatrix);
            map.Draw(spriteBatch);
            goal.Draw(spriteBatch);
            player.Draw(spriteBatch);
            foreach (Enemy e in enemies)
                e.Draw(spriteBatch);
            foreach (LockedWall lw in lockedWalls)
                lw.Draw(spriteBatch);
            if (showKey == true)
                key.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(ventureFont, "Score : " + score.ToString("f0"), new Vector2(20, 10), Color.White);
            spriteBatch.DrawString(ventureFont, timer.ToString("f0"), new Vector2((ScreenWidth / 2), 10), Color.White);
            for (int i = 0; i < lives; i++)
            {
                spriteBatch.Draw(heart, new Vector2(ScreenWidth - 30 - i * 20, 10), Color.White);
            }
            if (endGame == true)
            {
                spriteBatch.Draw(spaghettiboi, new Vector2(-50, -100), Color.White);
                spriteBatch.DrawString(psFont, "You did it! Kitty is safe <3", new Vector2(325, 405), Color.White);
                spriteBatch.DrawString(psFont, "Final Score: " + finalScore.ToString("f0"), new Vector2(350, 430), Color.White);
            }
            spriteBatch.End();
        }

        public override void CleanUp()
        {
            isLoaded = false;
        }


        public int PixelToTile(float pixelCoord)
        {
            return (int)Math.Floor(pixelCoord / tile);
        }

        public int TileToPixel(int tileCoord)
        {
            return tile * tileCoord;
        }

        public int CellAtPixelCoord(Vector2 pixelCoords)
        {
            if (pixelCoords.X < 0 || pixelCoords.X > map.WidthInPixels || pixelCoords.Y < 0)
                return 1;
            if (pixelCoords.Y > map.HeightInPixels)
                return 0;
            return CellAtTileCoord(PixelToTile(pixelCoords.X), PixelToTile(pixelCoords.Y));
        }

        public int CellAtTileCoord(int tx, int ty)
        {
            if (tx < 0 || tx >= map.Width || ty < 0)
                return 1;
            if (ty >= map.Height)
                return 0;
            TiledTile tile = collisionLayer.GetTile(tx, ty);
            return tile.Id;
        }

        public void CheckCollisions()
        {
            GameOver();

            if (player.Position.Y > map.HeightInPixels)
            {
                if (lives > 0)
                {
                    lives -= 1;
                    foreach (TiledObjectGroup group in map.ObjectGroups)
                    {
                        if (group.Name == "player_spawn")
                        {
                            foreach (TiledObject obj in group.Objects)
                            {
                                player.Position = new Vector2(obj.X, obj.Y);
                            }
                        }
                    }
                }
            }
            foreach (LockedWall lw in lockedWalls)
            {
                if (IsColliding(player.Bounds, lw.Bounds) == true)
                {
                    player.Stop();
                }
                if (showKey == false)
                {
                    lockedWalls.Remove(lw);
                    break;
                }
            }

            foreach (Enemy e in enemies)
            {
                if (IsColliding(player.Bounds, e.Bounds) == true)
                {
                    if (player.IsJumping && player.Velocity.Y > 0)
                    {
                        score += 10;
                        splatInst.Play();
                        player.JumpOnCollision();
                        enemies.Remove(e);
                        break;
                    }
                    else
                    {
                        if (lives > 0)
                        {
                            lives -= 1;
                            foreach (TiledObjectGroup group in map.ObjectGroups)
                            {
                                if (group.Name == "player_spawn")
                                {
                                    foreach (TiledObject obj in group.Objects)
                                    {
                                        player.Position = new Vector2(obj.X, obj.Y);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (IsColliding(player.Bounds, key.Bounds) == true)
            {
                keyInst.Play();
                showKey = false;
            }

            if (IsColliding(player.Bounds, goal.Bounds) == true)
            {
                RespawnWin();
                ResetLevel();
                switch (level)
                {
                    case Level.W1_L1:
                        level = Level.W1_L2;
                        break;
                    case Level.W1_L2:
                        level = Level.W1_L3;
                        break;
                    case Level.W1_L3:
                        level = Level.W1_L1;
                        endGame = true;
                        allowMovement = false;
                        finalScore = score;
                        lives = 0;
                        break;
                }
            }
        }

        public bool IsColliding(Rectangle rect1, Rectangle rect2)
        {
            if (rect1.X + rect1.Width < rect2.X || rect1.X > rect2.X + rect2.Width ||
                rect1.Y + rect1.Height < rect2.Y || rect1.Y > rect2.Y + rect2.Height)
            {
                return false;
            }
            return true;
        }

        public void CameraMechanics()
        {
            var cameraLeft = camera.Position.X;
            var cameraTop = camera.Position.Y;
            var cameraRight = camera.Position.X + ScreenWidth;
            var cameraBottom = camera.Position.Y + ScreenHeight;
            var pX = player.Position.X - ScreenWidth / 2;
            var pY = player.Position.Y - ScreenHeight / 1.5f;

            if (cameraLeft < 0 && cameraTop > 0 && cameraBottom < map.HeightInPixels)
                camera.Position = new Vector2(0, pY);
            if (cameraRight > map.WidthInPixels && cameraTop > 0 && cameraBottom < map.HeightInPixels)
                camera.Position = new Vector2(map.WidthInPixels - ScreenWidth, pY);
            if (cameraTop < 0 && cameraLeft > 0 && cameraRight < map.WidthInPixels)
                camera.Position = new Vector2(pX, 0);
            if (cameraBottom > map.HeightInPixels && cameraLeft > 0 && cameraRight < map.WidthInPixels)
                camera.Position = new Vector2(pX, map.HeightInPixels - ScreenHeight);
            if (cameraLeft < 0 && cameraTop < 0)
                camera.Position = new Vector2(0, 0);
            if (cameraLeft < 0 && cameraBottom > map.HeightInPixels)
                camera.Position = new Vector2(0, map.HeightInPixels - ScreenHeight);
            if (cameraRight > map.WidthInPixels && cameraTop < 0)
                camera.Position = new Vector2(map.WidthInPixels - ScreenWidth, 0);
            if (cameraRight > map.WidthInPixels && cameraBottom > map.HeightInPixels)
                camera.Position = new Vector2(map.WidthInPixels - ScreenWidth, map.HeightInPixels - ScreenHeight);
        }

        public void RespawnWin()
        {
            lives = 3;
            score += timer;
            timer = 500;
            ResetLevel();
        }

        public void GameOver()
        {
            if (lives == 0 && endGame == false)
            {
                level = Level.W1_L1;
                lives = 3;
                timer = 500;
                score = 0;
                ResetLevel();
                AIE.StateManager.ChangeState("GameOver");
                MediaPlayer.Stop();
            }
        }

        public void ResetLevel()
        {
            isLoaded = false;
            showKey = true;
            enemies.Clear();
            lockedWalls.Clear();
        }
    }
}