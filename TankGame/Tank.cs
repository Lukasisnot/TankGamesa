using System;
using System.ComponentModel;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankGame;

public class Tank : Sprite, IDamageable
{
    public float DriveSpeed { get; set; }
    public float RotSpeed { get; set; }
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public event EventHandler<EventArgs> Death;
    public bool IsAlive => Health > 0;
    public int ReloadTime { get; set; }
    private long _lastTimeFired;
    public bool IsReloading
    {
        get => (ReloadTime + _lastTimeFired) > DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
    public Sprite Turret { get; set; }

    public Tank(Game1 game, float driveSpeed = 100f, float rotSpeed = 3f, float health = 100f, int reloadTime = 750, Texture2D? texture = null, Vector2? position = null, Vector2? size = null, float rotation = 0, float depth = 0) : base(game, texture, position, size, rotation, depth)
    {
        DriveSpeed = driveSpeed;
        RotSpeed = rotSpeed;
        Health = health;
        MaxHealth = health;
        ReloadTime = reloadTime;
        Turret = new Sprite(game, game.Content.Load<Texture2D>("Tank_Turret"), Vector2.Zero, size, rotation, depth);
        Turret.Origin = new Vector2(Turret.Texture.Width / 2f, Turret.Texture.Height * 0.75f);
        _lastTimeFired = 0;
    }

    public void Damage(float damage)
    {
        Health = float.Max(0, Health - damage);
        if (Health == 0) OnDeath();
    }

    protected virtual void OnDeath()
    {
        Death?.Invoke(this, new EventArgs());
    }

    public void Drive(int direction)
    {
        Position += DriveSpeed * DeltaTime* Single.Sign(direction) * Forward;
        Position.X = Single.Clamp(Position.X, Size.X / 2, game.GraphicsDevice.Viewport.Width - Size.X / 2);
        Position.Y = Single.Clamp(Position.Y, Size.Y / 2, game.GraphicsDevice.Viewport.Height - Size.Y / 2);
    }
    
    public void Turn(int direction)
    {
        Rotation += RotSpeed * Single.Sign(direction) * DeltaTime;
    }

    public void Fire()
    {
        if (IsReloading) return;
        Projectile projectile = new Projectile(game, this, position: Position + Turret.Forward * 115, direction: Turret.Forward, size: Vector2.One * 3);
        _lastTimeFired = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        game.Components.Add(projectile);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        Turret.DestinationRectangle.X = (int)(Turret.Position + Position).X;
        Turret.DestinationRectangle.Y = (int)(Turret.Position + Position).Y;
        game.SpriteBatch.Draw(Turret.Texture, Turret.DestinationRectangle, null, Color.White, Turret.Rotation, Turret.Origin, SpriteEffects.None, Depth);
    }
}