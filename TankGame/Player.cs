using System;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankGame;

public class Player : Tank
{
    public Player(Game1 game, float driveSpeed = 100, float rotSpeed = 3, float health = 100, int reloadTime = 750, Texture2D texture = null, Vector2? position = null, Vector2? size = null, float rotation = 0, float depth = 0) : base(game, driveSpeed, rotSpeed, health, reloadTime, texture, position, size, rotation, depth)
    {
        Texture = game.Content.Load<Texture2D>("Tank_Body");
        Turret.Texture = game.Content.Load<Texture2D>("Tank_Turret");
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
            Drive(-1);
        }

        Turret.Rotation = Single.Atan2(Mouse.GetState().Position.Y - Position.Y + Turret.Position.Y, Mouse.GetState().Position.X - Position.X + Turret.Position.X) + Single.Pi / 2;
        
        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            Fire();
        }
    }
}