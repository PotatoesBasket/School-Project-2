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
        bool allowInput = false;
        bool allowMenu = true;
        float inputTimer = 0f;
        float finalScore = 0f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public bool AllowInput
        {
            get { return allowInput; }
            set { allowInput = value; }
        }

        public bool AllowMenu
        {
            get { return allowMenu; }
            set { allowMenu = value; }
        }

        public float FinalScore
        {
            get { return finalScore; }
            set { finalScore = value; }
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
            AIE.StateManager.CreateState("LevelSelect", new LevelSelect(this));
            AIE.StateManager.CreateState("GameState", new GameState(this));
            AIE.StateManager.CreateState("GameOver", new GameOver(this));
            AIE.StateManager.CreateState("PauseState", new PauseState(this));
            AIE.StateManager.CreateState("OptionsState", new OptionsState(this));
            AIE.StateManager.CreateState("GameWon", new GameWon(this));

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
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            inputTimer += deltaTime;

            if (inputTimer > 0.15f)
                allowInput = true;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            AIE.StateManager.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        public void ResetInputTimer()
        {
            inputTimer = 0f;
            allowInput = false;
        }
    }
}