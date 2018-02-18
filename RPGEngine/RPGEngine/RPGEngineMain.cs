using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RPGEngine
{
    public class RPGEngineMain : Game
    {
        #region InstVariables
        static int SCREEN_WIDTH_BUFF = 640;
        static int SCREEN_HEIGHT_BUFF = 480;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        #endregion

        /// <summary>
        /// Main instantiation for the game.
        /// </summary>
        public RPGEngineMain()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = SCREEN_WIDTH_BUFF;
            this.graphics.PreferredBackBufferHeight = SCREEN_HEIGHT_BUFF;
            //ENABLE 30 FPS IN RENDER//
            //graphics.PreparingDeviceSettings += (sender, e) =>
            //{
            //    e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.Two;
            //};
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Initialize method for the game.
        /// </summary>
        protected override void Initialize()
        {
            //Manager initialization
            Managers.GlobalVariables.Init();
            Managers.CollisionManager.Init();
            Managers.TextManager.Init();
            Managers.InputManager.Init();
            Managers.MenuManager.Init();
            Managers.MapManager.Init();
            //Managers.SoundManager.Init();
            //Initialize main character
            Characters.MainCharacter.Init(new Vector2(20, 20));
            
            base.Initialize();
        }

        /// <summary>
        /// Controls the loaded content at startup.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Characters.MainCharacter.LoadContent(Content);
            Managers.TextManager.LoadContent(Content);
            Managers.MenuManager.LoadContent(Content);
            Managers.MapManager.LoadContent(Content);
            //Managers.SoundManager.LoadContent(Content);
            Cameras.Camera.Init(GraphicsDevice.Viewport);
        }

        /// <summary>
        /// Unloads content on request. 
        /// </summary>
        protected override void UnloadContent()
        {
            //Unloads content???
            Content.Unload();
        }

        /// <summary>
        /// Updates the game.
        /// </summary>
        /// <param name="gameTime">The gametime variable from windows.</param>
        protected override void Update(GameTime gameTime)
        {
            Managers.InputManager.Update();
            Managers.TextManager.Update();
            Managers.MenuManager.Update();
            Managers.MapManager.Update();
            base.Update(gameTime);
            Characters.MainCharacter.Update();
            Cameras.Camera.Update(Characters.MainCharacter.Center);
        }

        /// <summary>
        /// Draws to the game's viewport.
        /// </summary>
        /// <param name="gameTime">The gametime variable from windows.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                Cameras.Camera.Transform * Matrix.CreateScale(2.0f, 2.0f, 1f) * Matrix.CreateTranslation(-SCREEN_WIDTH_BUFF / 2, -SCREEN_HEIGHT_BUFF / 2, 0f)
            );
            ////////////////////////////
            //////SPRITEBATCH BEGIN/////
            ////////////////////////////

            //Layer 1//
            Managers.MapManager.DrawLayer1(spriteBatch);
            //Layer 2//

            Characters.MainCharacter.Draw(spriteBatch);

            //Layer 2
            Managers.MapManager.DrawLayer2(spriteBatch);
            Managers.TextManager.Draw(spriteBatch);
            Managers.MenuManager.Draw(spriteBatch);
            //Layer 2

            ////////////////////////////
            ///////SPRITEBATCH END//////
            ////////////////////////////
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// The width of the window, accessible to other classes
        /// </summary>
        public static int WindowWidth
        {
            get { return SCREEN_WIDTH_BUFF; }
        }

        /// <summary>
        /// The height of the window, accessible to other classes
        /// </summary>
        public static int WindowHeight
        {
            get { return SCREEN_HEIGHT_BUFF; }
        }
    }
}
