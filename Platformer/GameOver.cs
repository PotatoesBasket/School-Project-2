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
    class GameOver : AIE.State
    {
        Game1 game = null;
        bool isLoaded = false;
        SpriteFont font = null;
        Texture2D gameOver = null;

        public GameOver(Game1 game) : base()
        {
            this.game = game;
        }

        public override void Update(ContentManager content, GameTime gameTime)
        {
            if (isLoaded == false)
            {
                isLoaded = true;
                font = content.Load<SpriteFont>("ps2p");
                gameOver = content.Load<Texture2D>("gameover");
            }

            game.AllowMenu = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true)
            {
                AIE.StateManager.ChangeState("GameState");
                game.ResetInputTimer();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(gameOver, new Vector2(0, -10), Color.White);
            spriteBatch.DrawString(font, "YOU DIED.", new Vector2(95, 300), Color.Red);
            spriteBatch.DrawString(font, "kitty is disappoint", new Vector2(55, 330), Color.White);
            spriteBatch.End();
        }

        public override void CleanUp()
        {
            isLoaded = false;
        }
    }
}