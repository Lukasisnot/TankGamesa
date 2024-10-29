using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankGame;

public class Projectile : Sprite
{
    public float Speed { get; set; }
    public Vector2 Direction { get; set; }
    public Tank Owner { get; set; }
    
    public Projectile(Game1 game, Tank owner, Vector2? direction = null, float speed = 500f, Texture2D texture = null, Vector2? position = null, Vector2? size = null, float rotation = 0, float depth = 0) : base(game, texture, position, size, rotation, depth)
    {
        Owner = owner;
        Speed = speed;
        Direction = direction ?? owner.Forward;
        Texture = texture ?? game.Content.Load<Texture2D>("Projectile");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Position += Speed * DeltaTime * Direction;
    }
}