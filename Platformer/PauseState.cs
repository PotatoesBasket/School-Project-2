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
    class PauseState : AIE.State
    {
        Game1 game = null;
        bool isLoaded = false;
        bool showControls = false;
        SpriteFont font = null;

        enum menuCursor
        {
            _continue,
            _options,
            _controls,
            _exit
        }
        menuCursor mc = menuCursor._continue;

        public PauseState(Game1 game) : base()
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
                if (showControls == false)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    {
                        switch (mc)
                        {
                            case menuCursor._continue:
                                mc = menuCursor._options;
                                break;
                            case menuCursor._options:
                                mc = menuCursor._controls;
                                break;
                            case menuCursor._controls:
                                mc = menuCursor._exit;
                                break;
                            case menuCursor._exit:
                                mc = menuCursor._continue;
                                break;
                        }
                        game.ResetInputTimer();
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    {
                        switch (mc)
                        {
                            case menuCursor._continue:
                                mc = menuCursor._exit;
                                break;
                            case menuCursor._options:
                                mc = menuCursor._continue;
                                break;
                            case menuCursor._controls:
                                mc = menuCursor._options;
                                break;
                            case menuCursor._exit:
                                mc = menuCursor._controls;
                                break;
                        }
                        game.ResetInputTimer();
                    }

                    if (mc == menuCursor._continue && Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        game.ResetInputTimer();
                        AIE.StateManager.PopState();
                    }

                    if (mc == menuCursor._options && Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        game.ResetInputTimer();
                        AIE.StateManager.ChangeState("OptionsState");
                    }

                    if (mc == menuCursor._exit && Keyboard.GetState().IsKeyDown(Keys.Enter))
                        game.Exit();
                }

                if (mc == menuCursor._controls && Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    if (showControls == false)
                    {
                        showControls = true;
                        game.ResetInputTimer();
                    }
                    else
                    {
                        showControls = false;
                        game.ResetInputTimer();
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            if (showControls == false)
            {
                spriteBatch.DrawString(font, "PAUSED", new Vector2(95, 300), Color.Red);

                if (mc == menuCursor._continue)
                    spriteBatch.DrawString(font, "continue", new Vector2(55, 330), Color.White);
                else
                    spriteBatch.DrawString(font, "continue", new Vector2(55, 330), Color.DimGray);

                if (mc == menuCursor._options)
                    spriteBatch.DrawString(font, "options", new Vector2(55, 350), Color.White);
                else
                    spriteBatch.DrawString(font, "options", new Vector2(55, 350), Color.DimGray);

                if (mc == menuCursor._controls)
                    spriteBatch.DrawString(font, "controls", new Vector2(55, 370), Color.White);
                else
                    spriteBatch.DrawString(font, "controls", new Vector2(55, 370), Color.DimGray);

                if (mc == menuCursor._exit)
                    spriteBatch.DrawString(font, "exit", new Vector2(55, 390), Color.White);
                else
                    spriteBatch.DrawString(font, "exit", new Vector2(55, 390), Color.DimGray);
            }
            else
            {
                spriteBatch.DrawString(font, "CONTROLS", new Vector2(350, 160), Color.Red);
                spriteBatch.DrawString(font, "left/right", new Vector2(220, 190), Color.White);
                spriteBatch.DrawString(font, "MOVE", new Vector2(450, 190), Color.White);
                spriteBatch.DrawString(font, "up/spacebar", new Vector2(220, 210), Color.White);
                spriteBatch.DrawString(font, "JUMP", new Vector2(450, 210), Color.White);
                spriteBatch.DrawString(font, "enter", new Vector2(220, 230), Color.White);
                spriteBatch.DrawString(font, "PAUSE/SELECT", new Vector2(450, 230), Color.White);
            }

            spriteBatch.End();
        }

        public override void CleanUp()
        {
            isLoaded = false;
        }
    }
}
