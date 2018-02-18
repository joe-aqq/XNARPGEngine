using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RPGEngine.Managers.GlobalVariables;

namespace RPGEngine.Characters
{
    public static class MainCharacter
    {
        #region Variables
        public static string name; //The name of the character
        public static bool visible; //Whether the character is visible or not
        public static Vector2 position; //The position of the character in the game world
        public static Texture2D textureSheet; //The texture sheet of the character
        public static State state; //The state of the character
        public static Direction direction; //The direction that the character is facing
        private static Vector2 positionp; //previous position of character
        private static Vector2 size; //size of the character's texture
        private static float moveSpeed = 1.35f; //Move speed of the character
        private static float runMultiplier; //Multiplies the movespeed up runspeed if the run button is pressed
        private static float runSpeed = 1f; //The speed increase from holding down the run button
        private static int frameCounter; //What frame the counter is at, ranging from 0 -> framesInAnimation
        private static int framesInAnimation = 15; //How many frames there are in each animation
        private static int frameInAnimation = 0; //The current frame of animation, ranging from 0 -> maxFrames
        private static int maxFrames = 4; //How many frames there are in each animation
        private static Vector2 pos; //Static position, for sharing between classes

        public enum State //The state of the character
        {
            still, //0
            walking //1
        }

        public enum Direction //The direction of the character
        {
            down, //0
            up,   //1
            left, //2
            right //3
        }
        #endregion

        /// <summary>
        /// Creates a new main character object.
        /// </summary>
        /// <param name="position">Starting position of the character.</param>
        /// <param name="textureSheet">Texture sheet of the character.</param>
        public static void Init(Vector2 position_s)
        {
            position = position_s;
            name = "King";
            size = new Vector2(18, 30);
            visible = true;
            runMultiplier = 0f;

            Managers.PartyManager.memberOne =
                new DataTypes.RPGStats(
                    name: name,
                    health: 15,
                    healthMax: 30,
                    mp: 15,
                    mpMax: 30,
                    exp: 5,
                    level: 1
                );
        }

        /// <summary>
        /// Loads the main character's textures.
        /// </summary>
        /// /// <param name="Content">The content reference for monogame.</param>
        public static void LoadContent(ContentManager Content)
        {
            textureSheet = Content.Load<Texture2D>("Characters/TestCharacter");
        }

        /// <summary>
        /// Updates the main chatacter's information.
        /// </summary>
        public static void Update()
        {
            //If text is being shown on the screen...
            if (Managers.GlobalVariables.gameState == GameState.gamePlay)
            {
                //Update static position
                pos = position;

                //Set previous position to the position from last update
                positionp = position;

                //Check if the run key is being pressed; if it is, set the run multiplier to the run speed
                if (Managers.InputManager.State.IsKeyDown(Managers.InputManager.RunKey))
                {
                    runMultiplier = runSpeed;
                    framesInAnimation = 7;
                }
                else
                {
                    runMultiplier = 0;
                    framesInAnimation = 15;
                }

                bool anyKeyIsPressed = false;
                //Checking keys and applying motion to the character. Arrow keys + WSAD
                if ((Managers.InputManager.State.IsKeyDown(Keys.W) || Managers.InputManager.State.IsKeyDown(Keys.Up)) && !Managers.InputManager.State.IsKeyDown(Keys.Down))
                {
                    position = new Vector2(position.X, (float)Math.Round(position.Y - (moveSpeed + runMultiplier)));
                    direction = Direction.down;
                    anyKeyIsPressed = true;
                }
                if (Managers.InputManager.State.IsKeyDown(Keys.S) || Managers.InputManager.State.IsKeyDown(Keys.Down) && !Managers.InputManager.State.IsKeyDown(Keys.Up))
                {
                    position = new Vector2(position.X, (float)Math.Round(position.Y + (moveSpeed + runMultiplier)));
                    direction = Direction.up;
                    anyKeyIsPressed = true;
                }
                if (Managers.InputManager.State.IsKeyDown(Keys.A) || Managers.InputManager.State.IsKeyDown(Keys.Left) && !Managers.InputManager.State.IsKeyDown(Keys.Right))
                {
                    position = new Vector2((float)Math.Round(position.X - (moveSpeed + runMultiplier)), position.Y);
                    direction = Direction.left;
                    anyKeyIsPressed = true;
                }
                if (Managers.InputManager.State.IsKeyDown(Keys.D) || Managers.InputManager.State.IsKeyDown(Keys.Right) && !Managers.InputManager.State.IsKeyDown(Keys.Left))
                {
                    position = new Vector2((float)Math.Round(position.X + (moveSpeed + runMultiplier)), position.Y);
                    direction = Direction.right;
                    anyKeyIsPressed = true;
                }

                //If the new position is colliding, replace it with the old position and don't progress animation
                if (Managers.CollisionManager.CheckCollision(position, size))
                {
                    position = positionp;
                    anyKeyIsPressed = false;
                }

                //Update animation frame
                if (anyKeyIsPressed)
                {
                    state = State.walking;
                    if (frameCounter < framesInAnimation)
                    {
                        frameCounter++;
                    }
                    else if (frameCounter >= framesInAnimation)
                    {
                        frameCounter = 0;
                        frameInAnimation++;
                    }
                    if (frameInAnimation >= maxFrames)
                    {
                        frameInAnimation = 0;
                    }
                }
                else
                {
                    state = State.still;
                    frameInAnimation = 0;
                }
            }
        }

        /// <summary>
        /// Draws the main character on the scene.
        /// </summary>
        /// <param name="spriteBatch">Monogame's spritebatch reference.</param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            if(visible)
            {
                spriteBatch.Draw
                (
                    texture: textureSheet,
                    sourceRectangle:
                        new Rectangle(
                            new Point((int)size.X * frameInAnimation, (int)size.Y * (int)direction),
                            new Point((int)size.X, (int)size.Y)
                        ),
                    position: position,
                    color: Color.White
                );
            }
        }

        /// <summary>
        /// Returns the characters move speed.
        /// </summary>
        public static float MoveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }

        /// <summary>
        /// Returns the size of a single frame/texture in the texture sheet.
        /// </summary>
        public static Vector2 Size
        {
            get { return size; }
        }

        /// <summary>
        /// Returns the center of the character.
        /// </summary>
        public static Vector2 Center
        {
            get
            {
                return new Vector2(
                    (float)Math.Round(position.X + (size.X / 2)),
                    (float)Math.Round(position.Y + (size.Y / 2))
                );
            }
        }

        /// <summary>
        /// Returns the static position of the player, for class sharing.
        /// </summary>
        public static Vector2 Pos
        {
            get { return pos; }
        }
    }
}
