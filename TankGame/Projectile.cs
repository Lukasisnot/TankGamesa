using System;
using System.Linq;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankGame;

public class Projectile : Sprite
{
    public float Speed { get; set; }
    public Vector2 Direction { get; set; }
    public Tank Owner { get; set; }
    public float Damage { get; set; }
    public float CollRadius { get; set; }

    public Sprite Explosion;
    
    protected System.Timers.Timer LifeTimer;
    protected System.Timers.Timer ExplosionTimer;
    
    public Projectile(Game1 game, Tank owner, float? collRadius = null, float damage = 10f, Vector2? direction = null, float speed = 600f, int ttl = 1500, Texture2D texture = null, Vector2? position = null, Vector2? size = null, float rotation = 0, float depth = 0) : base(game, texture, position, size, rotation, depth)
    {
        Owner = owner;
        CollRadius = collRadius ?? 10f;
        Damage = damage;
        Speed = speed;
        Direction = direction ?? owner.Forward;
        Texture = texture ?? game.Content.Load<Texture2D>("Tank_Projectile");
        Rotation = Single.Atan2(Direction.Y, Direction.X) + Single.Pi/2;
        
        LifeTimer = new System.Timers.Timer(ttl);
        LifeTimer.Elapsed += (Object source, ElapsedEventArgs e) =>
        {
            game.Components.Remove(this);
        };
        LifeTimer.Enabled = true;
        
        ExplosionTimer = new System.Timers.Timer(500);
        ExplosionTimer.Elapsed += (Object source, ElapsedEventArgs e) =>
        {
            game.Components.Remove(Explosion);
        };
        ExplosionTimer.Enabled = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Position += Speed * DeltaTime * Direction;
    }

    public void OnHit(IDamageable damageable)
    {
        damageable.Damage(Damage);
        Explosion = new Sprite(game, game.Content.Load<Texture2D>("Explosion"), Position);
        game.Components.Add(Explosion);
        ExplosionTimer.Start();
        game.Components.Remove(this);
    }
}