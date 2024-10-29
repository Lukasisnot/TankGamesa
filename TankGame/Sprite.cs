#nullable enable
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankGame;

public class Sprite : DrawableGameComponent
{
    public static List<Sprite> AllSprites = new List<Sprite>();
    public Texture2D Texture;
    public Vector2 Position;
    private Vector2 _size;
    public Vector2 Size
    {
        set => _size = new Vector2(Texture.Width * value.X, Texture.Height * value.Y);
        get => _size;
    }
    public Vector2 Origin;
    public float Rotation;
    public float Depth;
    protected Game1 game;
    public Rectangle DestinationRectangle;
    public float DeltaTime;
    public Vector2 Forward => new (float.Sin(Rotation), -float.Cos(Rotation));

    public Sprite(Game1 game, Texture2D? texture = null, Vector2? position = null, Vector2? size = null, float rotation = 0f, float depth = 0f) : base(game)
    {
        this.game = game;
        Texture = texture ?? game.Content.Load<Texture2D>("Arrow");
        Position = position ?? Vector2.Zero;
        Size = size ?? Vector2.One;
        Rotation = rotation;
        Depth = depth;
        DestinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
        Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
        AllSprites.Add(this);
    }

    ~Sprite()
    {
        AllSprites.Remove(this);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public override void Draw(GameTime gameTime)
    {
        DestinationRectangle.X = (int)Position.X;
        DestinationRectangle.Y = (int)Position.Y;
        game.SpriteBatch.Draw(Texture, DestinationRectangle, null, Color.White, Rotation, Origin, SpriteEffects.None, Depth);
    }
}