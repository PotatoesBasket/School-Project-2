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
    class OptionsState : AIE.State
    {
        Game1 game = null;
        bool isLoaded = false;
        bool soundOn = true;
        SpriteFont font = null;

        enum optionsCursor
        {
            _sounds,
            _levelselect,
            _back
        }
        optionsCursor oc = optionsCursor._sounds;

        public OptionsState(Game1 game) : base()
        {
            this.game = game;
        }

        public override void Update(ContentManager content, GameTime gameTime)
        {
            if (isLoaded == false)
            {
                isLoaded = true;
                font = content.Load<SpriteFont>("ps2p");
            }

            if (game.AllowInput == true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    switch (oc)
                    {
                        case optionsCursor._sounds:
                            oc = optionsCursor._levelselect;
                            break;
                        case optionsCursor._levelselect:
                            oc = optionsCursor._back;
                            break;
                        case optionsCursor._back:
                            oc = optionsCursor._sounds;
                            break;
                    }
                    game.ResetInputTimer();
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    switch (oc)
                    {
                        case optionsCursor._sounds:
                            oc = optionsCursor._back;
                            break;
                        case optionsCursor._levelselect:
                            oc = optionsCursor._sounds;
                            break;
                        case optionsCursor._back:
                            oc = optionsCursor._levelselect;
                            break;
                    }
                    game.ResetInputTimer();
                }
                if ((oc == optionsCursor._sounds) && (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.Left)))
                {
                    if (soundOn == false)
                    {
                        soundOn = true;
                        game.ResetInputTimer();
                    }
                    else
                    {
                        soundOn = false;
                        game.ResetInputTimer();
                    }
                }

                if (oc == optionsCursor._back && Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    AIE.StateManager.ChangeState("PauseState");
                    game.ResetInputTimer();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.DrawString(font, "OPTIONS", new Vector2(95, 300), Color.Red);

            if (oc == optionsCursor._sounds)
                spriteBatch.DrawString(font, "sounds", new Vector2(55, 330), Color.White);
            else
                spriteBatch.DrawString(font, "sounds", new Vector2(55, 330), Color.DimGray);
                         if (oc == optionsCursor._levelselect)
                spriteBatch.DrawString(font, "level select", new Vector2(55, 350), Color.White);
            else
                spriteBatch.DrawString(font, "level select", new Vector2(55, 350), Color.DimGray);

            if (oc == optionsCursor._back)
                spriteBatch.DrawString(font, "back", new Vector2(55, 370), Color.White);
            else
                spriteBatch.DrawString(font, "back", new Vector2(55, 370), Color.DimGray);

            if (soundOn == true)
            {
                spriteBatch.DrawString(font, "ON", new Vector2(170, 330), Color.Orange);
                spriteBatch.DrawString(font, "OFF", new Vector2(220, 330), Color.DimGray);
            }
            else
            {
                spriteBatch.DrawString(font, "ON", new Vector2(170, 330), Color.DimGray);
                spriteBatch.DrawString(font, "OFF", new Vector2(220, 330), Color.Orange);
            }

            spriteBatch.End();
        }

        public override void CleanUp()
        {
            isLoaded = false;
        }
    }
}