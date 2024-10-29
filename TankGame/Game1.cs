using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace TankGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    public SpriteBatch SpriteBatch;
    private Player _player;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        Window.AllowUserResizing = true;
        Window.Position = Point.Zero;
        Window.Title = "Tank Game";
        
        // _graphics.IsFullScreen = true;
        // _graphics.PreferredBackBufferWidth = 1920;
        // _graphics.PreferredBackBufferHeight = 1080;
        // _graphics.ApplyChanges();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        
        _player = new Player(this, driveSpeed: 150f, texture: Content.Load<Texture2D>("Tank_Body"), size: Vector2.One * 3);
        _player.Position = new Vector2(1920 / 3, 1080 / 3);
        Components.Add(_player);
        // foreach (object c in Components)
        // {
        //     if (c is IDamageable)
        //     {
        //         Console.WriteLine(_player.Health);
        //         ((IDamageable)c).Damage(1f);
        //         Console.WriteLine(_player.Health);
        //     }
        // }

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkSlateGray);

        // TODO: Add your drawing code here
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        base.Draw(gameTime);
        SpriteBatch.End();
    }
}