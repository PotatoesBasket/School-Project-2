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

        public TitleScreen(Game1 game) : base()
        {
            this.game = game;
        }

        public override void Update(ContentManager content, GameTime gameTime)
        {
            if (font == null)
            {
                font = content.Load<SpriteFont>("arial");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) == true)
            {
                AIE.StateManager.ChangeState("GameState");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "TitleScreen", new Vector2(200, 200), Color.White);
            spriteBatch.End();
        }

        public override void CleanUp()
        {
            font = null;
        }
    }
}