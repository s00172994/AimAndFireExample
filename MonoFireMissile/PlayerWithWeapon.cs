using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AnimatedSprite
{
    class PlayerWithWeapon : Sprite
    {
        protected Game myGame;
        protected float playerVelocity = 6.0f;
        private Projectile myProjectile;
        protected CrossHair Site;

        public Vector2 CentrePos
        {
            get { return position + new Vector2(spriteWidth / 2, spriteHeight / 2); }

        }

        public Projectile MyProjectile
        {
            get
            {
                return myProjectile;
            }

            set
            {
                myProjectile = value;
            }
        }

        public PlayerWithWeapon(Game g, Texture2D texture, Vector2 userPosition, int framecount) : base(g, texture, userPosition, framecount)
        {
            myGame = g;
            Site = new CrossHair(g, g.Content.Load<Texture2D>(@"Textures\CrossHair"), userPosition, 1);

        }

        public void loadProjectile(Projectile r)
        {
            MyProjectile = r;
        }


        public override void Update(GameTime gameTime)
        {

            Viewport gameScreen = myGame.GraphicsDevice.Viewport;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.position += new Vector2(1, 0) * playerVelocity;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.position += new Vector2(-1, 0) * playerVelocity;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                this.position += new Vector2(0, -1) * playerVelocity;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                this.position += new Vector2(0, 1) * playerVelocity;
            }
            // check for site change

            Site.Update(gameTime);
            // Whenever the rocket is still and loaded it follows the player posiion
            if (MyProjectile != null && MyProjectile.ProjectileState == Projectile.PROJECTILE_STATE.STILL)
                MyProjectile.position = this.CentrePos;
            // if a roecket is loaded
            if (MyProjectile != null)
            {
                // fire the rocket and it looks for the target
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && MyProjectile.ProjectileState == Projectile.PROJECTILE_STATE.STILL)
                    MyProjectile.fire(Site.position);
            }

            // Make sure the player stays in the bounds see previous lab for details
            position = Vector2.Clamp(position, Vector2.Zero,
                                            new Vector2(gameScreen.Width - spriteWidth,
                                                        gameScreen.Height - spriteHeight));

            // Update the Camera with respect to the players new position
            //Vector2 delta = cam.Pos - this.position;
            //cam.Pos += delta;

            if (MyProjectile != null)
                MyProjectile.Update(gameTime);
            // Update the players site
            Site.Update(gameTime);
            // call Sprite Update to get it to animated 
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            Site.Draw(spriteBatch);
            if (MyProjectile != null && MyProjectile.ProjectileState != Projectile.PROJECTILE_STATE.STILL)
                MyProjectile.Draw(spriteBatch);

        }

    }
}
