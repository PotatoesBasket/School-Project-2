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
    class Intro : AIE.State
    {
        Game1 game = null;
        bool isLoaded = false;
        SpriteFont font = null;
        SpriteFont font2 = null;
        Texture2D kitty = null;
        Texture2D grass = null;

        public Intro(Game1 game) : base()
        {
            this.game = game;
        }

        public override void Update(ContentManager content, GameTime gameTime)
        {
            if (isLoaded == false)
            {
                isLoaded = true;
                font = content.Load<SpriteFont>("visitor");
                font2 = content.Load<SpriteFont>("ps2p");
                kitty = content.Load<Texture2D>("kitty");
                grass = content.Load<Texture2D>("grass");
            }
            if (game.AllowInput == true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    AIE.StateManager.ChangeState("GameState");
                    game.ResetInputTimer();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            game.GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            spriteBatch.Draw(grass, new Vector2(-300, -100), Color.White);
            spriteBatch.Draw(kitty, new Vector2(375, 150), Color.White);
            spriteBatch.DrawString(font, "Oh no!", new Vector2(140, 190), Color.Black);
            spriteBatch.DrawString(font, "This adorable, precious kitty\n   just wandered into a\n     creepy factory.", new Vector2(40, 225), Color.Black);
            spriteBatch.DrawString(font2, "press space to go save that floof-ball", new Vector2(100, 450), Color.Black);
            spriteBatch.End();
        }

        public override void CleanUp()
        {
            isLoaded = false;
        }
    }
}