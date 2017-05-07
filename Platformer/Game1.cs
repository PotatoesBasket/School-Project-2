using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;

namespace Platformer
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont arialFont;
        SpriteFont ventureFont;
        Texture2D heart = null;
        Goal goal = null;        Player player = null;        List<Enemy> enemies = new List<Enemy>();        Camera2D camera = null;
        TiledMap map = null;
        TiledTileLayer collisionLayer;
        TiledTileLayer hazardLayer;

        float score = 0;
        float timer = 500;
        int lives = 3;
        public static int tile = 32;
        public static float meter = tile;
        public static float gravity = meter * 9.8f * 4.0f;
        public static Vector2 maxVelocity = new Vector2(meter * 10, meter * 15);
        public static float acceleration = maxVelocity.X * 2;
        public static float friction = maxVelocity.X * 6;
        public static float jumpImpulse = meter * 750;

        public int ScreenWidth
        {
            get { return graphics.GraphicsDevice.Viewport.Width; }
        }

        public int ScreenHeight
        {
            get { return graphics.GraphicsDevice.Viewport.Height; }
        }


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ventureFont = Content.Load<SpriteFont>("3Dventure");
            arialFont = Content.Load<SpriteFont>("arial");
            heart = Content.Load<Texture2D>("heart_x16");
            map = Content.Load<TiledMap>("level1");

            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, ScreenWidth, ScreenHeight);
            camera = new Camera2D(viewportAdapter);
            camera.Position = new Vector2(0, ScreenHeight);

            foreach (TiledTileLayer layer in map.TileLayers)
            {
                if (layer.Name == "collision")
                    collisionLayer = layer;

                if (layer.Name == "hazard")
                    hazardLayer = layer;
            }

            foreach (TiledObjectGroup group in map.ObjectGroups)
            {
                if (group.Name == "player_spawn")
                {
                    foreach (TiledObject obj in group.Objects)
                    {
                        player = new Player(this);
                        player.Load(Content);
                        player.Posistion = new Vector2(obj.X, obj.Y);
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
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            camera.Position = player.Posistion - new Vector2(ScreenWidth / 2, ScreenHeight / 1.5f);
            CameraMechanics();
            
            timer -= deltaTime;

            if (lives == 0)
            {
                lives = 3;
            }

            CheckCollisions();
            goal.Update(deltaTime);
            player.Update(deltaTime);
            foreach (Enemy e in enemies)
            {
                e.Update(deltaTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var transformMatrix = camera.GetViewMatrix();

            spriteBatch.Begin(transformMatrix: transformMatrix);
            map.Draw(spriteBatch);
            goal.Draw(spriteBatch);
            player.Draw(spriteBatch);
            foreach (Enemy e in enemies)
            {
                e.Draw(spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(ventureFont, "Score : " + score.ToString("f0"), new Vector2(20, 10), Color.White);
            spriteBatch.DrawString(ventureFont, timer.ToString("f0"), new Vector2((ScreenWidth / 2), 10), Color.White);
            for (int i = 0; i < lives; i++)
            {
                spriteBatch.Draw(heart, new Vector2(ScreenWidth - 30 - i * 20, 10), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
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
            foreach (Enemy e in enemies)
            {
                if (IsColliding(player.Bounds, e.Bounds) == true)
                {
                    if (player.IsJumping && player.Velocity.Y > 0)
                    {
                        score += 10;
                        player.JumpOnCollision();
                        enemies.Remove(e);
                        break;
                    }
                    else
                    {
                        lives -= 1;
                        Respawn();
                    }
                }
            }

            if (IsColliding(player.Bounds, goal.Bounds) == true)
            {
                lives = 3;
                score += timer;
                timer = 500;
                Respawn();
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
            var pX = player.Posistion.X - ScreenWidth / 2;
            var pY = player.Posistion.Y - ScreenHeight / 1.5f;

            if (cameraLeft < 0 && cameraTop > 0 && cameraBottom < map.HeightInPixels)
            {
                camera.Position = new Vector2(0, pY);
            }
            if (cameraRight > map.WidthInPixels && cameraTop > 0 && cameraBottom < map.HeightInPixels)
            {
                camera.Position = new Vector2(map.WidthInPixels - ScreenWidth, pY);
            }
            if (cameraTop < 0 && cameraLeft > 0 && cameraRight < map.WidthInPixels)
            {
                camera.Position = new Vector2(pX, 0);
            }
            if (cameraBottom > map.HeightInPixels && cameraLeft > 0 && cameraRight < map.WidthInPixels)
            {
                camera.Position = new Vector2(pX, map.HeightInPixels - ScreenHeight);
            }

            if (cameraLeft < 0 && cameraTop < 0)
            {
                camera.Position = new Vector2(0, 0);
            }
            if (cameraLeft < 0 && cameraBottom > map.HeightInPixels)
            {
                camera.Position = new Vector2(0, map.HeightInPixels - ScreenHeight);
            }
            if (cameraRight > map.WidthInPixels && cameraTop < 0)
            {
                camera.Position = new Vector2(map.WidthInPixels - ScreenWidth, 0);
            }
            if (cameraRight > map.WidthInPixels && cameraBottom > map.HeightInPixels)
            {
                camera.Position = new Vector2(map.WidthInPixels - ScreenWidth, map.HeightInPixels - ScreenHeight);
            }
        }

        public void Respawn()
        {
            foreach (TiledObjectGroup group in map.ObjectGroups)
            {
                if (group.Name == "player_spawn")
                {
                    foreach (TiledObject obj in group.Objects)
                    {
                        player.Posistion = new Vector2(obj.X, obj.Y);
                    }
                }
            }

            if (lives == 0)
            {
                lives = 3;
                score = 0;
                timer = 500;
            }
        }
    }
}
