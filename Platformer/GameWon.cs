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
    class GameWon : AIE.State
    {
        Game1 game = null;
        bool isLoaded = false;
        SpriteFont font = null;
        Texture2D spaghettiboi = null;

        public GameWon(Game1 game) : base()
        {
            this.game = game;
        }

        public override void Update(ContentManager content, GameTime gameTime)
        {
            if (isLoaded == false)
            {
                isLoaded = true;
                font = content.Load<SpriteFont>("ps2p");
                spaghettiboi = content.Load<Texture2D>("gamewon");
            }

            game.AllowMenu = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true)
            {
                AIE.StateManager.ChangeState("GameState");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(spaghettiboi, new Vector2(-50, -100), Color.White);
            spriteBatch.DrawString(font, "You did it! Kitty is safe <3", new Vector2(325, 405), Color.White);
            spriteBatch.DrawString(font, "Final Score: " + game.FinalScore.ToString("f0"), new Vector2(350, 430), Color.White);
            spriteBatch.End();
        }

        public override void CleanUp()
        {
            isLoaded = false;
        }
    }
}