using System;
using System.IO;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankGame;

public class Enemy : Tank
{
    private Random rand;
    public Vector2 Destination;
    public int DestinationToleration = 50;
    public float RotationToleration = .01f;
    public int RotDirection = 1;
    public bool RotDirChosen = false;
    public Player Player;
    
    protected System.Timers.Timer FireTimer;
    
    public Enemy(Game1 game, Player player, float driveSpeed = 100, float rotSpeed = 2, float turretRotSpeed = 1.5f, float health = 100, int reloadTime = 750, float? collRadius = null, Texture2D? texture = null, Texture2D? turretTexture = null, Vector2? position = null, Vector2? size = null, float rotation = 0, float depth = 0) : base(game, driveSpeed, rotSpeed, turretRotSpeed, health, reloadTime, collRadius, texture, turretTexture, position, size, rotation, depth)
    {
        Player = player;
        rand = new Random();
        GenDestination();
        FireTimer = new System.Timers.Timer(rand.Next(750, 1250));
        FireTimer.Elapsed += (Object source, ElapsedEventArgs e) =>
        {
            Fire();
            FireTimer.Interval = rand.Next(750, 2000);
        };
        FireTimer.Enabled = true;
        FireTimer.AutoReset = true;
    }

    public override void Update(GameTime gameTime)
    {
        if (Player == null) return;
        base.Update(gameTime);
        Move();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        FireTimer.Enabled = false;
    }

    private void Move()
    {
        Drive(1);

        Vector2 destinationDir = Vector2.Normalize(Destination - Position);
        if (Vector2.Dot(Forward, destinationDir) < 1 - RotationToleration)
        {
            if (Vector3.Cross(new(destinationDir.X, destinationDir.Y, 0), new(Forward.X, Forward.Y, 0)).Z > 0)
                Turn(-1);
            else
                Turn(1);
        }

        Vector2 destinationDirTurret = Vector2.Normalize(Player.Position - (Position + Turret.Position));
        if (Vector2.Dot(Turret.Forward, destinationDirTurret) < 1 - RotationToleration)
        {
            if (Vector3.Cross(new(destinationDirTurret.X, destinationDirTurret.Y, 0), new(Turret.Forward.X, Turret.Forward.Y, 0)).Z > 0)
                TurnTurret(-1);
            else
                TurnTurret(1);
        }
        
        if ((Destination - Position).Length() < DestinationToleration)
            GenDestination();
    }

    private void GenDestination()
    {
        Destination = new Vector2(rand.Next(50, game.GraphicsDevice.Viewport.Width - 50), rand.Next(50, game.GraphicsDevice.Viewport.Height - 50));
        RotDirChosen = false;
    }
}