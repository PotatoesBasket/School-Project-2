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
        SpriteFont arial = null;
        SpriteFont funsize = null;
        bool isLoaded = false;
        bool start = true;
        float buttonPressTimer = 0;
        float buttonPressSpeed = 0.15f;

        public TitleScreen(Game1 game) : base()
        {
            this.game = game;
        }

        public override void Update(ContentManager content, GameTime gameTime)
        {
            if (isLoaded == false)
            {
                isLoaded = true;
                arial = content.Load<SpriteFont>("arial");
                funsize = content.Load<SpriteFont>("funsize");
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            buttonPressTimer += deltaTime;
            if (start == true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true)
                    AIE.StateManager.ChangeState("GameState");
                if ((Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.Up)) && (buttonPressTimer > buttonPressSpeed))
                {
                    buttonPressTimer = 0;
                    start = false;
                }
            }
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true)
                    game.Exit();
                if ((Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.Up)) && (buttonPressTimer > buttonPressSpeed))
                {
                    buttonPressTimer = 0;
                    start = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            game.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            spriteBatch.DrawString(funsize, "Unfinished:", new Vector2(300, 150), Color.Black);
            spriteBatch.DrawString(funsize, "The Game", new Vector2(300, 190), Color.Black);

            if (start == true)
            {
                spriteBatch.DrawString(arial, "start", new Vector2(350, 250), Color.Black);
                spriteBatch.DrawString(arial, "exit", new Vector2(350, 275), Color.DimGray);
            }
            else
            {
                spriteBatch.DrawString(arial, "start", new Vector2(350, 250), Color.DimGray);
                spriteBatch.DrawString(arial, "exit", new Vector2(350, 275), Color.Black);
            }
            spriteBatch.End();
        }

        public override void CleanUp()
        {
            isLoaded = false;
        }
    }
}