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
        SpriteFont font = null;
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
                font = content.Load<SpriteFont>("arial");
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
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Unfinished: The Game", new Vector2(300, 150), Color.White);
            if (start == true)
            {
                spriteBatch.DrawString(font, "start", new Vector2(350, 200), Color.White);
                spriteBatch.DrawString(font, "exit", new Vector2(350, 225), Color.DimGray);
            }
            else
            {
                spriteBatch.DrawString(font, "start", new Vector2(350, 200), Color.DimGray);
                spriteBatch.DrawString(font, "exit", new Vector2(350, 225), Color.White);
            }
            spriteBatch.End();
        }

        public override void CleanUp()
        {
            isLoaded = false;
        }
    }
}