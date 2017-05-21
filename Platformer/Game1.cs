using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;

namespace Platformer //aka "I Wanna Be The Super Meat Boy Ripoff"
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

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

            AIE.StateManager.CreateState("TitleScreen", new TitleScreen(this));
            AIE.StateManager.CreateState("Intro", new Intro(this));
            AIE.StateManager.CreateState("GameState", new GameState(this));
            AIE.StateManager.CreateState("GameOver", new GameOver(this));
            AIE.StateManager.CreateState("WinScreen", new WinScreen(this));

            AIE.StateManager.PushState("TitleScreen");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            AIE.StateManager.Update(Content, gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            AIE.StateManager.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}