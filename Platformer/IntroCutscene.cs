using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Maps.Tiled;
using MonoGame.Extended.ViewportAdapters;

namespace Platformer
{
    class IntroCutscene : AIE.State
    {
        Game1 game = null;
        bool isLoaded = false;
        bool allowSkip = false;
        bool skipActive = false;
        float timer = 0f;

        SpriteFont font = null;
        Texture2D moon = null;
        Player_Cutscene player = null;
        Kitty_Cutscene kitty = null;
        Effect_FadeIn fadein = null;
        Effect_FadeOut fadeout = null;
        Camera2D camera = null;
        TiledMap map;
        TiledMap map2;

        public int ScreenWidth
        {
            get { return game.GraphicsDevice.Viewport.Width; }
        }

        public int ScreenHeight
        {
            get { return game.GraphicsDevice.Viewport.Height; }
        }


        public IntroCutscene(Game1 game) : base()
        {
            this.game = game;
        }

        public override void Update(ContentManager content, GameTime gameTime)
        {
            if (isLoaded == false)
            {
                isLoaded = true;
                font = content.Load<SpriteFont>("3Dventure");
                moon = content.Load<Texture2D>("moon");
                map = content.Load<TiledMap>("intro_main");
                map2 = content.Load<TiledMap>("intro_effect");
                var viewportAdapter = new BoxingViewportAdapter(game.Window, game.GraphicsDevice, ScreenWidth / 2, ScreenHeight / 2);
                camera = new Camera2D(viewportAdapter);
                camera.Position = new Vector2(100, 560);

                foreach (TiledObjectGroup group in map.ObjectGroups)
                {
                    if (group.Name == "player_spawn")
                    {
                        foreach (TiledObject obj in group.Objects)
                        {
                            player = new Player_Cutscene(this);
                            player.Load(content);
                            player.Position = new Vector2(obj.X + 80, obj.Y);
                        }
                    }

                    if (group.Name == "kitty_spawn")
                    {
                        foreach (TiledObject obj in group.Objects)
                        {
                            kitty = new Kitty_Cutscene(this);
                            kitty.Load(content);
                            kitty.Position = new Vector2(obj.X + 70, obj.Y);
                        }
                    }
                }

                fadein = new Effect_FadeIn(this);
                fadein.Load(content);
                fadein.Position = new Vector2(0, 300);

                fadeout = new Effect_FadeOut(this);
                fadeout.Load(content);
                fadeout.Position = new Vector2(500, 300);
                fadeout.Pause();
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            player.Update(deltaTime);
            kitty.Update(deltaTime);
            fadein.Update(deltaTime);
            fadeout.Update(deltaTime);

            if (game.AllowInput == true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    allowSkip = true;
                    game.ResetInputTimer();
                }
                if (allowSkip == true && Keyboard.GetState().IsKeyUp(Keys.Enter))
                {
                    skipActive = true;
                }
                if (skipActive == true && Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    CleanUp();
                    AIE.StateManager.ChangeState("GameState");
                    game.ResetInputTimer();
                }
            }

            /*if (Keyboard.GetState().IsKeyDown(Keys.Right))
                camera.Move(new Vector2(300, 0) * deltaTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                camera.Move(new Vector2(-300, 0) * deltaTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                camera.Move(new Vector2(0, 300) * deltaTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                camera.Move(new Vector2(0, -300) * deltaTime);*/

            Cutscene(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var transformMatrix = camera.GetViewMatrix();

            spriteBatch.Begin();
            spriteBatch.Draw(moon, new Vector2(0, -150), Color.White);
            spriteBatch.End();

            spriteBatch.Begin(transformMatrix: transformMatrix);
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            kitty.Draw(spriteBatch);
            map2.Draw(spriteBatch);
            fadein.Draw(spriteBatch);
            fadeout.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            if (skipActive == true)
                spriteBatch.DrawString(font, "press enter to skip", new Vector2(600, 450), Color.White);
            //spriteBatch.DrawString(font, camera.Position.ToString(), new Vector2(30, 30), Color.White);
            //spriteBatch.DrawString(font, timer.ToString(), new Vector2(30, 50), Color.White);
            spriteBatch.End();
        }

        public override void CleanUp()
        {
            isLoaded = false;
            timer = 0;
        }

        public void Cutscene(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer += deltaTime;
            player.Position += player.Velocity;
            kitty.Position += kitty.Velocity;

            if (timer > 0.95)
                fadein.Pause();

            //camera movement
            if (timer < 13)
                camera.Move(new Vector2(10, 0) * deltaTime);
            if (timer > 13 && timer < 17.5)
                camera.Move(new Vector2(90, 0) * deltaTime);

            //spaghetti movement
            if (timer < 3)
                kitty.Velocity = new Vector2(20, 0) * deltaTime;
            if (timer > 3 && timer < 3.5)
            {
                kitty.Velocity = Vector2.Zero;
                kitty.Pause();
            }
            if (timer > 3.5 && timer < 4.5)
                kitty.Flip();
            if (timer > 4.5 && timer < 5.5)
                kitty.Unflip();
            if (timer > 5.5 && timer < 7)
                kitty.Flip();
            if (timer > 7 && timer < 8.5)
            {
                kitty.Velocity = new Vector2(-30, 0) * deltaTime;
                kitty.Play();
            }
            if (timer > 8.5 && timer < 9.5)
            {
                kitty.Velocity = Vector2.Zero;
                kitty.Pause();
            }
            if (timer > 9.5 && timer < 11)
                kitty.Unflip();
            if (timer > 11 && timer < 16)
            {
                kitty.Velocity = new Vector2(130, 0) * deltaTime;
                kitty.Play();
            }

            //player movement
            if (timer < 12)
            player.Velocity = new Vector2(10, 0) * deltaTime;
            if (timer > 12 && timer < 13)
            {
                player.Velocity = Vector2.Zero;
                player.Pause();
            }
            if (timer > 13 && timer < 18)
            {
                player.Velocity = new Vector2(70, 0) * deltaTime;
                player.Play();
            }
            if (timer > 18 && timer < 19)
                player.Velocity = new Vector2(20, 0) * deltaTime;
            if (timer > 19 && timer < 21)
            {
                player.Velocity = Vector2.Zero;
                player.Pause();
            }
            if (timer > 21)
            {
                player.Velocity = new Vector2(10, 0) * deltaTime;
                player.Play();
            }

            if (timer > 21.5)
                fadeout.Play();
            if (timer > 22.45f)
            {
                fadeout.Pause();
            }
            if (timer > 23)
            {
                CleanUp();
                AIE.StateManager.ChangeState("GameState");
            }
        }
    }
}