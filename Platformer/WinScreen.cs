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
    class WinScreen : AIE.State
    {
        Game1 game = null;
        bool isLoaded = false;
        SpriteFont font = null;

        public WinScreen(Game1 game) : base()
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

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true)
                AIE.StateManager.ChangeState("GameState");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "YOU WIN! :)", new Vector2(350, 200), Color.White);
            spriteBatch.End();
        }

        public override void CleanUp()
        {
            isLoaded = false;
        }
    }
}
