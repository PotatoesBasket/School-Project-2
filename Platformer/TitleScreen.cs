using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    public class TitleScreen : AIE.State
    {
        Game1 game = null;
        SpriteFont ps2p = null;
        SpriteFont font04b = null;
        Texture2D kitty = null;
        Texture2D grass = null;
        float aniTimer = 0f;
        float runTimer = 0f;
        float rotateTimer = 0f;
        bool isLoaded = false;
        bool start = true;

        public TitleScreen(Game1 game) : base()
        {
            this.game = game;
        }

        public override void Update(ContentManager content, GameTime gameTime)
        {
            if (isLoaded == false)
            {
                isLoaded = true;
                ps2p = content.Load<SpriteFont>("ps2p");
                font04b = content.Load<SpriteFont>("funsize");
                kitty = content.Load<Texture2D>("jett_run_x1000");
                grass = content.Load<Texture2D>("grass");
            }
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            aniTimer += deltaTime;
            runTimer += deltaTime * 4;
            rotateTimer += deltaTime * 1.5f;

            if (aniTimer > 1)
                aniTimer = 0;
            if (runTimer > 1)
                runTimer = 0;
            if (rotateTimer > 360)
                rotateTimer = 0;

            if (game.AllowInput == true)
            {
                if (start == true)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true)
                    {
                        AIE.StateManager.ChangeState("IntroCutscene");
                        game.ResetInputTimer();
                    }
                    if ((Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.Up)))
                    {
                        start = false;
                        game.ResetInputTimer();
                    }
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true)
                        game.Exit();
                    if ((Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.Up)))
                    {
                        start = true;
                        game.ResetInputTimer();
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            game.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            spriteBatch.Draw(grass, new Vector2(-300, -100), Color.White);
            if (runTimer >= 0 && runTimer < 0.5f)
                spriteBatch.Draw(kitty, new Vector2(410, 700), null, new Rectangle(0, 0, 360, 300), new Vector2(200, 800), rotateTimer - 90, new Vector2(.5f, .5f), Color.White, SpriteEffects.None, 0);
            if (runTimer > 0.5f && runTimer <= 1)
                spriteBatch.Draw(kitty, new Vector2(410, 700), null, new Rectangle(360, 0, 360, 300), new Vector2(200, 800), rotateTimer - 90, new Vector2(.5f, .5f), Color.White, SpriteEffects.None, 0);
            if (aniTimer >= 0 && aniTimer < 0.5f)
            {
                spriteBatch.DrawString(font04b, "SUPER KITTY", new Vector2(225, 100), Color.DeepPink);
                spriteBatch.DrawString(font04b, "ADVENTURE", new Vector2(250, 140), Color.DeepPink);
            }
            if (aniTimer > 0.5f && aniTimer <= 1)
            {
                spriteBatch.DrawString(font04b, "SUPER KITTY", new Vector2(225, 103), Color.DeepPink);
                spriteBatch.DrawString(font04b, "ADVENTURE", new Vector2(250, 143), Color.DeepPink);
            }
            if (start == true)
            {
                spriteBatch.DrawString(ps2p, "START!", new Vector2(375, 230), Color.Yellow);
                spriteBatch.DrawString(ps2p, "EXIT", new Vector2(380, 255), Color.DimGray);
            }
            else
            {
                spriteBatch.DrawString(ps2p, "START", new Vector2(375, 230), Color.DimGray);
                spriteBatch.DrawString(ps2p, "EXIT!", new Vector2(380, 255), Color.Yellow);
            }
            spriteBatch.End();
        }

        public override void CleanUp()
        {
            isLoaded = false;
        }
    }
}