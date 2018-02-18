using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Enemies
{
    abstract class Enemy
    {
        private string name; //The name of the enemy
        private int health; //The current health of the enemy
        private int maxHealth; //The maximum health of the enemy
        private int mp; //The current magic points of the enemy
        private int maxMP; //The maximum magic points of the enemy
        private int expGiven; //The exp the enemy gives to the player
        private Texture2D worldTexture; //The world texture of the enemy
        private Texture2D battleTexture; //The battle sprite of the enemy
        private Vector2 position; //The position of the enemy on the screen
        private bool visible = true; //Whether the enemy is visible on the scren
        private Direction direction; //The current direction the enemy is facing
        private State state; //The state the enemy is in

        private enum State
        {
            idle,
            walking,
            following
        }

        private enum Direction //The direction the enemy is facing
        {
            down, //0
            up,   //1
            left, //2
            right //3
        }

        /// <summary>
        /// Initializes the enemy class.
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// Updates the enemy class.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Draws the enemy class to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(SpriteBatch spriteBatch);

        #region properties
        /// <summary>
        /// Returns the name of the enemy.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Returns current health the of the enemy.
        /// </summary>
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        /// <summary>
        /// Returns the maximum health of the enemy.
        /// </summary>
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        /// <summary>
        /// Returns the current magic points of the enemy.
        /// </summary>
        public int MP
        {
            get { return mp; }
            set { mp = value; }
        }

        /// <summary>
        /// Returns the maximum magic points of the enemy.
        /// </summary>
        public int MaxMP
        {
            get { return maxMP; }
            set { maxMP = value; }
        }

        /// <summary>
        /// Returns the exp given from the enemy.
        /// </summary>
        public int EXPGiven
        {
            get { return expGiven; }
            set { expGiven = value; }
        }

        /// <summary>
        /// Returns the world texture of the enemy.
        /// </summary>
        public Texture2D WorldTexture
        {
            get { return worldTexture; }
            set { worldTexture = value; }
        }

        /// <summary>
        /// Returns the battle sprite of the enemy.
        /// </summary>
        public Texture2D BattleTexture
        {
            get { return battleTexture; }
            set { battleTexture = value; }
        }

        /// <summary>
        /// Returns the position of the enemy.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Returns whether the enemy is visible or not
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }
        #endregion
    }
}
