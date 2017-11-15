using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using AnimatedSprite;
using Utilities;

namespace Engines
{
    class ChaseAndFireEngine
    {
        PlayerWithWeapon p;
        Sentry s;
        SpriteBatch spriteBatch;
        private CircularChasingEnemy[] chasers;
        private Game _gameOwnedBy;

        public ChaseAndFireEngine(Game game)
        {
            // Chase engine remembers reference to the game
            _gameOwnedBy = game;
            game.IsMouseVisible = true;
            SoundEffect[] _PlayerSounds = new SoundEffect[5];
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            Projectile bullet;
            Projectile fireball;

            p = new PlayerWithWeapon(game, game.Content.Load<Texture2D>(@"Textures/wizard_strip3"), new Vector2(500, 400), 3);
            fireball = new Projectile(game, game.Content.Load<Texture2D>(@"Textures/fireball_strip4"), 
                new Sprite(game, game.Content.Load<Texture2D>(@"Textures/explosion_strip8"),p.position,8) ,p.position, 4);
            p.loadProjectile(fireball);

            s = new Sentry(game, game.Content.Load<Texture2D>(@"Textures/Turret"), new Vector2(200, 200), 1, p);
            bullet = new Projectile(game, game.Content.Load<Texture2D>(@"Textures/Projectile"),
                new Sprite(game, game.Content.Load<Texture2D>(@"Textures\Explosion"), s.position, 16), s.position, 6);
            s.LoadProjectile(bullet);

            chasers = new CircularChasingEnemy[Utility.NextRandom(2, 5)];

            for (int i = 0; i < chasers.Count(); i++)
            {
                chasers[i] = new CircularChasingEnemy(game,
                        game.Content.Load<Texture2D>(@"Textures/Dragon_strip3"),
                            Vector2.Zero,
                         3);
                chasers[i].myVelocity = (float)Utility.NextRandom(2, 5);
                chasers[i].position = new Vector2(Utility.NextRandom(game.GraphicsDevice.Viewport.Width - chasers[i].spriteWidth),
                        Utility.NextRandom(game.GraphicsDevice.Viewport.Height - chasers[i].spriteHeight));
            }

        }


        public void Update(GameTime gameTime)
        {
            p.Update(gameTime);
            s.Update(gameTime);

            foreach (CircularChasingEnemy chaser in chasers)
            {
                if (p.MyProjectile.ProjectileState == Projectile.PROJECTILE_STATE.EXPOLODING && p.MyProjectile.collisionDetect(chaser))
                    chaser.die();
                chaser.follow(p);
                chaser.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            p.Draw(spriteBatch);
            s.Draw(spriteBatch);
            foreach (CircularChasingEnemy chaser in chasers)
                chaser.Draw(spriteBatch);
        }
    }
}
