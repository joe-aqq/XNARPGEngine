using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.DataTypes
{
    public class RPGString
    {
        #region Variables
        public string text; //The text that the NPC/Scene is saying
        public string name; //The name of the character speaking
        public int textSpeed; //The speed of the text being run
        public int waveSpeed; //The speed of the text waving
        public int jitterSpeed; //the speed of the text jittering
        #endregion

        /// <summary>
        /// Create a new RPGString class, for the TextManager.
        /// </summary>
        /// <param name="text">The string of text that goes into the dialogue box.</param>
        /// <param name="name">The name of the character speaking.</param>
        /// <param name="textSpeed">The speed of the text being rendered onto the screen.</param>
        public RPGString(string text, string name, int textSpeed, int waveSpeed = 0, int jitterSpeed = 0)      
        {
            this.text = text;
            this.name = name;
            this.textSpeed = textSpeed;
            this.waveSpeed = waveSpeed;
            this.jitterSpeed = jitterSpeed;
        }

        /// <summary>
        /// The text that goes into the dialogue box.
        /// </summary>
        public string Text
        {
            get { return text; }
        }

        /// <summary>
        /// The name of the character speaking.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// The speed of the text being rendered.
        /// </summary>
        public int TextSpeed
        {
            get { return textSpeed; }
        }

        /// <summary>
        /// The speed at which the text waves.
        /// </summary>
        public int WaveSpeed
        {
            get { return waveSpeed; }
        }

        /// <summary>
        /// The speed at which the text jitters.
        /// </summary>
        public int JitterSpeed
        {
            get { return jitterSpeed; }
        }
    }
}
