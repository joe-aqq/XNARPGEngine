using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.DataTypes
{
    class RPGStats
    {
        public string characterName; //Character's name
        public int characterHealth; //Character's current health
        public int characterHealthMax; //Character's max health
        public int characterMP; //Character's current magic points
        public int characterMPMax; //Character's max magic points
        public int characterEXP; //Character's current EXP
        public int characterLevel; //Character's level

        /// <summary>
        /// Creates a new RPGStats datatype.
        /// </summary>
        /// <param name="name">Character's name.</param>
        /// <param name="health">Character's current health.</param>
        /// <param name="healthMax">Character's max health.</param>
        /// <param name="mp">Character's current magic points.</param>
        /// <param name="mpMax">Character's max magic points.</param>
        /// <param name="exp">Character's current EXP.</param>
        /// <param name="level">Character's level.</param>
        public RPGStats(string name, int health, int healthMax, int mp, int mpMax, int exp, int level)
        {
            this.characterName = name;
            this.characterHealth = health;
            this.characterHealthMax = healthMax;
            this.characterMP = mp;
            this.characterMPMax = mpMax;
            this.characterEXP = exp;
            this.characterLevel = level;
        }

        /// <summary>
        /// Returns the name of the character.
        /// </summary>
        public string Name
        {
            get { return characterName; }
            set { characterName = value; }
        }

        /// <summary>
        /// Returns the health of the character.
        /// </summary>
        public int Health
        {
            get { return characterHealth; }
            set { characterHealth = value; }
        }

        /// <summary>
        /// Returns the max amount of health a character has.
        /// </summary>
        public int HealthMax
        {
            get { return characterHealthMax; }
            set { characterHealthMax = value; }
        }

        /// <summary>
        /// Returns the amount of magic points a character has.
        /// </summary>
        public int MP
        {
            get { return characterMP; }
            set { characterMP = value; }
        }

        /// <summary>
        /// Returns the max amount of MP a character has.
        /// </summary>
        public int MPMax
        {
            get { return characterMPMax; }
            set { characterMPMax = value; }
        }

        /// <summary>
        /// Returns the amount of EXP the character has.
        /// </summary>
        public int EXP
        {
            get { return characterEXP; }
            set { characterEXP = value; }
        }

        /// <summary>
        /// Returns the max EXP of a character.
        /// </summary>
        public int EXPMax
        {
            get { return (int)((characterLevel * .8) * (40)); }
        }

        /// <summary>
        /// Returns the level of the character.
        /// </summary>
        public int Level
        {
            get { return characterLevel; }
            set { characterLevel = value; }
        }
    }
}
