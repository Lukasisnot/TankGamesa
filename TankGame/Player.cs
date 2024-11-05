using System;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankGame;

public class Player : Tank
{
    public Player(Game1 game, float driveSpeed = 100, float rotSpeed = 2, float turretRotSpeed = 1.5f, float health = 100, int reloadTime = 750, float? collRadius = null, Texture2D? texture = null, Texture2D? turretTexture = null, Vector2? position = null, Vector2? size = null, float rotation = 0, float depth = 0) : base(game, driveSpeed, rotSpeed, turretRotSpeed, health, reloadTime, collRadius, texture, turretTexture, position, size, rotation, depth)
    {
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        
        KeyboardState state = Keyboard.GetState();
        if (state.IsKeyDown(Keys.D))
        {
            Turn(1);
        }
        if (state.IsKeyDown(Keys.A))
        {
            Turn(-1);
        }
        if (state.IsKeyDown(Keys.W))
        {
            Drive(1);
        }
        if (state.IsKeyDown(Keys.S))
        {
            Drive(-1, 0.5f);
        }

        // Turret.Rotation = Single.Atan2(Mouse.GetState().Position.Y - Position.Y + Turret.Position.Y, Mouse.GetState().Position.X - Position.X + Turret.Position.X) + Single.Pi / 2;

        Vector2 mousePos = new(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
        
        Vector2 destinationDirTurret = Vector2.Normalize(mousePos - (Position + Turret.Position));
        if (Vector2.Dot(Turret.Forward, destinationDirTurret) < 0.999f)
        {
            if (Vector3.Cross(new(destinationDirTurret.X, destinationDirTurret.Y, 0), new(Turret.Forward.X, Turret.Forward.Y, 0)).Z > 0)
                TurnTurret(-1);
            else
                TurnTurret(1);
        }
        
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            Fire();
        }
    }
}