using System;
using System.ComponentModel;
using System.Linq;
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
    private Enemy _enemy;
    public float GameSize = 4f;

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
        
        _player = new(this, driveSpeed: 175f, texture: Content.Load<Texture2D>("Tank_Body"), turretTexture: Content.Load<Texture2D>("Tank_Turret"));
        _player.Position = new(GraphicsDevice.Viewport.Width / 2f, GraphicsDevice.Viewport.Height / 2f);
        Components.Add(_player);
        
        _enemy = new(this, _player, driveSpeed: 150f, texture: Content.Load<Texture2D>("EnemyTank_Body"), turretTexture: Content.Load<Texture2D>("EnemyTank_Turret"));
        _enemy.Position = new(GraphicsDevice.Viewport.Width / 4f, GraphicsDevice.Viewport.Height / 4f);
        Components.Add(_enemy);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        
        foreach (var component in Components.ToList())
        {
            if (component is not Projectile projectile) continue;
            foreach (var comp in Components.ToList())
            {
                if (comp is Tank tank && (projectile.Position - tank.Position).Length() < (tank.CollRadius + projectile.CollRadius) && comp is IDamageable damageable)
                    projectile.OnHit(damageable);
            }
        }

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