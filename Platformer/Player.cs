using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    class Player
    {
        GameState game = null;
        Sprite walk = new Sprite();
        bool isFalling = true;
        bool isJumping = false;
        bool autoJump = true;
        bool allowMovement = true;

        Vector2 velocity = Vector2.Zero;
        Vector2 position = Vector2.Zero;

        public Vector2 Posistion
        {
            get { return walk.position; }
            set { walk.position = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
        }

        public Rectangle Bounds
        {
            get { return walk.Bounds; }
        }

        public bool IsJumping
        {
            get { return isJumping; }
        }


        public Player(GameState game)
        {
            this.game = game;
            isFalling = true;
            isJumping = false;
            velocity = Vector2.Zero;
            position = Vector2.Zero;
        }

        public void Load(ContentManager content)
        {
            AnimatedTexture walkAni = new AnimatedTexture(Vector2.Zero, 0, 1, 1);
            walkAni.Load(content, "player_walk_x48", 4, 10);
            walk.Add(walkAni, 4, -10);
        }

        public void Update(float deltaTime)
        {
            UpdateInput(deltaTime);
            walk.Update(deltaTime);
            walk.Pause();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            walk.Draw(spriteBatch);
        }


        public void JumpOnCollision()
        {
            autoJump = true;
        }

        public void UpdateInput(float deltaTime)
        {
            bool wasMovingLeft = velocity.X < 0;
            bool wasMovingRight = velocity.X > 0;
            bool falling = isFalling;
            Vector2 acceleration = new Vector2(0, GameState.gravity);

            if (Keyboard.GetState().IsKeyDown(Keys.Left) && allowMovement == true)
            {
                acceleration.X -= GameState.acceleration;
                walk.SetFlipped(true);
                walk.Play();
                if (isJumping == true)
                {
                    walk.Reset();
                }
            }
            else if (wasMovingLeft == true)
            {
                acceleration.X += GameState.friction;
                walk.Reset();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && allowMovement == true)
            {
                acceleration.X += GameState.acceleration;
                walk.SetFlipped(false);
                walk.Play();
                if (isJumping == true)
                {
                    walk.Reset();
                }
            }
            else if (wasMovingRight == true)
            {
                acceleration.X -= GameState.friction;
                walk.Reset();
            }

            if ((Keyboard.GetState().IsKeyDown(Keys.Up) && allowMovement == true && this.isJumping == false && falling == false) || autoJump == true)
            {
                autoJump = false;
                acceleration.Y -= GameState.jumpImpulse;
                this.isJumping = true;
            }

            
            velocity += acceleration * deltaTime;
            velocity.X = MathHelper.Clamp(velocity.X, -GameState.maxVelocity.X, GameState.maxVelocity.X);
            velocity.Y = MathHelper.Clamp(velocity.Y, -GameState.maxVelocity.Y, GameState.maxVelocity.Y);
            walk.position += velocity * deltaTime;

            if ((wasMovingLeft && (velocity.X > 0)) || (wasMovingRight && (velocity.X < 0)))
            {
                velocity.X = 0;
            }


            int tx = game.PixelToTile(walk.position.X);
            int ty = game.PixelToTile(walk.position.Y);
            bool nx = (walk.position.X) % GameState.tile != 0;
            bool ny = (walk.position.Y) % GameState.tile != 0;
            bool cell = game.CellAtTileCoord(tx, ty) != 0;
            bool cellright = game.CellAtTileCoord(tx + 1, ty) != 0;
            bool celldown = game.CellAtTileCoord(tx, ty + 1) != 0;
            bool celldiag = game.CellAtTileCoord(tx + 1, ty + 1) != 0;

            if (this.velocity.Y > 0)
            {
                if ((celldown && !cell) || (celldiag && !cellright && nx))
                {
                    walk.position.Y = game.TileToPixel(ty);
                    this.velocity.Y = 0;
                    this.isFalling = false;
                    this.isJumping = false;
                    ny = false;
                }
            }
            else if (this.velocity.Y < 0)
            {
                if ((cell && !celldown) || (cellright && !celldiag && nx))
                {
                    walk.position.Y = game.TileToPixel(ty + 1);
                    this.velocity.Y = 0;
                    cell = celldown;
                    cellright = celldiag;
                    ny = false;
                }
            }

            if (this.velocity.X > 0)
            {
                if ((cellright && !cell) || (celldiag && !celldown && ny))
                {
                    walk.position.X = game.TileToPixel(tx);
                    this.velocity.X = 0;
                }
            }
            else if (this.velocity.X < 0)
            {
                if ((cell && !cellright) || (celldown && !celldiag && ny))
                {
                    walk.position.X = game.TileToPixel(tx + 1);
                    this.velocity.X = 0;
                }
            }

            this.isFalling = !(celldown || (nx && celldiag));
        }

        public void Stop()
        {
            if (velocity.X > 0)
                velocity.X -= GameState.maxVelocity.X;
        }
    }
}