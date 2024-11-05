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
    public float TurretRotSpeed { get; set; }
    public float CollRadius { get; set; }

    public Tank(Game1 game, float driveSpeed = 100f, float rotSpeed = 2f, float turretRotSpeed = 1.5f, float health = 100f, int reloadTime = 750, float? collRadius = null, Texture2D? texture = null, Texture2D? turretTexture = null, Vector2? position = null, Vector2? size = null, float rotation = 0, float depth = 0) : base(game, texture, position, size, rotation, depth)
    {
        DriveSpeed = driveSpeed * game.GameSize / 3;
        RotSpeed = rotSpeed * game.GameSize / 3;
        Health = health;
        MaxHealth = health;
        ReloadTime = reloadTime;
        CollRadius = collRadius ?? Single.Max(Size.X, Size.Y) / 2;
        Turret = new Sprite(game, turretTexture ?? game.Content.Load<Texture2D>("Tank_Turret"), Vector2.Zero, size, rotation, depth);
        Turret.Origin = new Vector2(Turret.Texture.Width / 2f, Turret.Texture.Height * 0.75f);
        TurretRotSpeed = turretRotSpeed * game.GameSize / 3;
        _lastTimeFired = 0;
    }

    public void Damage(float damage)
    {
        Health = float.Max(0, Health - damage);
        Console.WriteLine($"{Texture.Name} took {damage} damage. Health: {Health}/{MaxHealth}");
        if (Health == 0) OnDeath();
    }

    protected virtual void OnDeath()
    {
        Death?.Invoke(this, new EventArgs());
        game.Components.Remove(this);
    }

    public void Drive(int direction, float multiplier = 1f)
    {
        Position += DriveSpeed * multiplier * DeltaTime * Single.Sign(direction) * Forward;
        Position.X = Single.Clamp(Position.X, Size.X / 2, game.GraphicsDevice.Viewport.Width - Size.X / 2);
        Position.Y = Single.Clamp(Position.Y, Size.Y / 2, game.GraphicsDevice.Viewport.Height - Size.Y / 2);
    }
    
    public void Turn(int direction)
    {
        Rotation += RotSpeed * Single.Sign(direction) * DeltaTime;
        Turret.Rotation += RotSpeed * Single.Sign(direction) * DeltaTime;
    }
    
    public void TurnTurret(int direction)
    {
        Turret.Rotation += TurretRotSpeed * Single.Sign(direction) * DeltaTime;
    }

    public void Fire()
    {
        if (IsReloading) return;
        Projectile projectile = new Projectile(game, this, position: Position + Turret.Position + Turret.Forward * (Turret.Size.Y - Turret.Origin.Y), direction: Turret.Forward);
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