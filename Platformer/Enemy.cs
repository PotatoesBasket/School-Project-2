﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParticleEffects;

namespace Platformer
{
    class Enemy
    {
        GameState game = null;
        Sprite sprite = new Sprite();
        Vector2 velocity = Vector2.Zero;
        Emitter fireEmitter = null;
        Texture2D fireTexture = null;

        float pause = 0;
        bool moveRight = true;
        static float ghostAcceleration = GameState.acceleration / 5;
        static Vector2 ghostMaxVelocity = GameState.maxVelocity / 5;

        public Vector2 Position
        {
            get { return sprite.position; }
            set { sprite.position = value; }
        }

        public Rectangle Bounds
        {
            get { return sprite.Bounds; }
        }

        public Enemy(GameState game)
        {
            this.game = game;
            velocity = Vector2.Zero;
        }

        public void Load(ContentManager content)
        {
            AnimatedTexture animation = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            animation.Load(content, "ghost_walk_x48", 4, 5);
            sprite.Add(animation, -8, -10);

            fireTexture = content.Load<Texture2D>("spark");
            fireEmitter = new Emitter(fireTexture, Position);
        }

        public void Update(float deltaTime)
        {
            sprite.Update(deltaTime);

            if (pause > 0)
            {
                pause -= deltaTime;
            }
            else
            {
                float ddx = 0;
                int tx = game.PixelToTile(Position.X);
                int ty = game.PixelToTile(Position.Y);
                bool nx = (Position.X) % GameState.tile != 0;
                bool ny = (Position.Y) % GameState.tile != 0;
                bool cell = game.CellAtTileCoord(tx, ty) != 0;
                bool cellright = game.CellAtTileCoord(tx + 1, ty) != 0;
                bool celldown = game.CellAtTileCoord(tx, ty + 1) != 0;
                bool celldiag = game.CellAtTileCoord(tx + 1, ty + 1) != 0;

                if (moveRight)
                {
                    sprite.SetFlipped(true);
                    if (celldiag && !cellright)
                    {
                        ddx = ddx + ghostAcceleration;
                    }
                    else
                    {
                        this.velocity.X = 0;
                        this.moveRight = false;
                        this.pause = 0.5f;
                    }
                }
                else
                {
                    sprite.SetFlipped(false);
                    if (celldown && !cell)
                    {
                        ddx = ddx - ghostAcceleration;
                    }
                    else
                    {
                        this.velocity.X = 0;
                        this.moveRight = true;
                        this.pause = 0.5f;
                    }
                }

                Position = new Vector2((float)Math.Floor(Position.X + (deltaTime * velocity.X)), Position.Y);
                velocity.X = MathHelper.Clamp(velocity.X + (deltaTime * ddx), -ghostMaxVelocity.X, ghostMaxVelocity.X);
            }
        }

        public void ParticleUpdate(GameTime gameTime)
        {
            fireEmitter.position = Position + new Vector2(22, 18);
            fireEmitter.minVelocity = new Vector2(2, 2);
            fireEmitter.maxVelocity = new Vector2(8, 8);
            fireEmitter.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            fireEmitter.Draw(spriteBatch);
            sprite.Draw(spriteBatch);
        }
    }
}