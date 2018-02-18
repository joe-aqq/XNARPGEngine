using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Characters
{
    public abstract class Character
    {
        #region Variables
        public string name; //The name of the character
        public bool visible; //Whether the character is visible or not
        public Vector2 position; //The position of the character in the game world
        public Texture2D textureSheet; //The texture sheet of the character
        public State state; //The state of the character
        public Direction direction; //The direction that the character is facing

        //Stats
        public int level = 1;
        public int hp = 30;
        public int mp = 30;
        public int exp = 0;

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
        /// Load content for the character.
        /// </summary>
        public abstract void LoadContent(ContentManager Content);

        /// <summary>
        /// Update the character.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Drawing for the character and their actions.
        /// </summary>
        public abstract void Draw(SpriteBatch spriteBatch);

        #region Properties
        /// <summary>
        /// The visible state of the character; whether it's being drawn to the scene or not.
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        /// <summary>
        /// The position of the character. Can be changed remotely, but not reccomended. 
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Texture sheet of the character.
        /// </summary>
        public Texture2D TextureSheet
        {
            get { return textureSheet; }
            set { textureSheet = value; }
        }

        /// <summary>
        /// State of the character. Walking, Idle, etc.
        /// </summary>
        public State CharacterState
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// The direction the character is facing.
        /// </summary>
        public Direction CharacterDirection
        {
            get { return direction; }
            set { direction = value; }
        }

        /// <summary>
        /// The name of the character.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion
    }
}
