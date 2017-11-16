using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AnimatedSprite
{
    class Sentry : RotatingSprite
    {
        float collisionRadius = 300;
        float turretSpeed = 10;
        float angleOfRotationPrev;
        private Projectile myProjectile;
        PlayerWithWeapon p;

        public Vector2 CentrePosition
        {
            get
            {
                return position + new Vector2(spriteWidth / 2, spriteHeight / 2);
            }
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


        public Sentry(Game g, Texture2D texture, Vector2 userPosition, int framecount, PlayerWithWeapon p) : base(g, texture, userPosition, framecount)
        {
            this.p = p;
        }

        public void LoadProjectile(Projectile r)
        {
            MyProjectile = r;
        }

        public override void Update(GameTime gameTime)
        {
            angleOfRotationPrev = angleOfRotation;
            Face(p);
            Check(p, gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (MyProjectile != null && MyProjectile.ProjectileState != Projectile.PROJECTILE_STATE.STILL)
                MyProjectile.Draw(spriteBatch);
        }

        public bool IsInRadius(PlayerWithWeapon p)
        {
            float distance = Math.Abs(Vector2.Distance(this.WorldOrigin, p.CentrePos));

            if (distance <= collisionRadius)
                return true;
            else
                return false;
        }

        public void Face(PlayerWithWeapon p)
        {
            if (IsInRadius(p))
            {
                this.angleOfRotation = TurnToFace(position, p.position, angleOfRotation, turretSpeed);
            }
        }

        public void Check(PlayerWithWeapon p, GameTime gameTime)
        {
            if (MyProjectile != null && MyProjectile.ProjectileState == Projectile.PROJECTILE_STATE.STILL)
                MyProjectile.position = this.position;

            if (MyProjectile != null)
            {
                if (IsInRadius(p) && MyProjectile.ProjectileState == Projectile.PROJECTILE_STATE.STILL 
                    && angleOfRotation != 0 && angleOfRotationPrev == angleOfRotation)
                {
                    MyProjectile.fire(p.position);
                }
            }

            if (MyProjectile != null)
                MyProjectile.Update(gameTime);
        }
    }
}
