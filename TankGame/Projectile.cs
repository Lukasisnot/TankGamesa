using System;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankGame;

public class Projectile : Sprite, IDisposable
{
    public float Speed { get; set; }
    public Vector2 Direction { get; set; }
    public Tank Owner { get; set; }
    
    protected System.Timers.Timer LifeTimer;
    
    public Projectile(Game1 game, Tank owner, Vector2? direction = null, float speed = 500f, int ttl = 750, Texture2D texture = null, Vector2? position = null, Vector2? size = null, float rotation = 0, float depth = 0) : base(game, texture, position, size, rotation, depth)
    {
        Owner = owner;
        Speed = speed;
        Direction = direction ?? owner.Forward;
        Texture = texture ?? game.Content.Load<Texture2D>("Projectile");
        LifeTimer = new System.Timers.Timer(ttl);
        LifeTimer.Elapsed += (Object source, ElapsedEventArgs e) => { game.Components.Remove(this); };
        LifeTimer.Enabled = true;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Position += Speed * DeltaTime * Direction;
    }
}