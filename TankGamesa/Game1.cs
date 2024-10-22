using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankGamesa;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _tankTexture;
    private Vector2 _tankPosition = new Vector2(100, 100);
    private float _tankSpeed = 100f;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _tankTexture = Content.Load<Texture2D>("MiniTank");

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        KeyboardState state = Keyboard.GetState();
        if (state.IsKeyDown(Keys.D))
        {
            _tankPosition.X += _tankSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        if (state.IsKeyDown(Keys.A))
        {
            _tankPosition.X -= _tankSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        if (state.IsKeyDown(Keys.W))
        {
            _tankPosition.Y -= _tankSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        if (state.IsKeyDown(Keys.S))
        {
            _tankPosition.Y += _tankSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Bisque);

        // TODO: Add your drawing code here

        Rectangle destinationRect = new Rectangle((int)_tankPosition.X, (int)_tankPosition.Y, 100, 100);
        
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_tankTexture, destinationRect, null, Color.White);
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}