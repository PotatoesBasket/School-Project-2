using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.ViewportAdapters;

namespace Platformer
{
    class Intro : AIE.State
    {
        Game1 game = null;
        bool isLoaded = false;
        float timer = 0;

        SpriteFont font = null;
        Texture2D bg = null;
        Camera2D camera = null;
        TiledMap map;
        TiledMap map_effect;

        public Intro(Game1 game) : base()
        {
            this.game = game;
        }

        public override void Update(ContentManager content, GameTime gameTime)
        {
            if (isLoaded == false)
            {
                isLoaded = true;
                bg = content.Load<Texture2D>("moon");
                map = content.Load<TiledMap>("intro_main");
                map_effect = content.Load<TiledMap>("intro_effect");
                font = content.Load<SpriteFont>("arial");

                var viewportAdapter = new BoxingViewportAdapter(game.Window, game.GraphicsDevice, game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);
                camera = new Camera2D(viewportAdapter);
                camera.Position = new Vector2(0, game.GraphicsDevice.Viewport.Height);
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer += deltaTime;
            camera.Position = new Vector2(460, 570);

            if (Keyboard.GetState().IsKeyDown(Keys.Space) == true)
                AIE.StateManager.ChangeState("GameState");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(bg, new Vector2(0, -200), Color.White);
            spriteBatch.End();

            var transformMatrix = camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: transformMatrix);
            map.Draw(spriteBatch);
            map_effect.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(font, "press space to skip...", new Vector2(game.GraphicsDevice.Viewport.Width - 150, game.GraphicsDevice.Viewport.Height - 25), Color.White);
            spriteBatch.End();
        }

        public override void CleanUp()
        {
            isLoaded = false;
        }
    }
}