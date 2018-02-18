using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RPGEngine.Managers.GlobalVariables;

namespace RPGEngine.Managers
{
    static class TextManager
    {
        #region Variables
        public static List<DataTypes.RPGString> textCache; //The queue of text being rendered on the screen
        public static Texture2D mainFont; //The main font of the game
        public static Texture2D textboxBackground; //The texture of the text background
        public static Texture2D textboxNametag; //The texture of the nametag background
        public static bool textUIReady; //Whether the background texture is in the correct position
        public static bool textUILowering; //Whether the background texture is lowering back
        public static Vector2 textboxPosition; //Where the textbox is on the viewport (camera perspective)
        public static Vector2 nametagPosition; //Where the nametag is on the Viewport
        public static float textboxAnimationSpeed = 5; //The speed of the textbox going up and down
        public static int textboxRaisedY = (RPGEngineMain.WindowHeight / 2) - 60; //The Y position of the textbox if it is raised
        public static int textboxLoweredY = (RPGEngineMain.WindowHeight / 2); //The Y position of the textbox if it is lowered
        public static TextboxState state; //The state that the textbox is in
        public static int defaultTextSpeed = 4; //The default text speed of the text engine
        public static int speechLength = 280; //The maximum length (in pixels) that a string in the textbox can be
        public static string currentSpeech; //The string of letters that is currently being printed 
        public static bool stringDone; //Whether the string is done printing or not
        public static int textTimer; //The timer for determining when to print the next letter
        public static int textSpacing = 0; //The spacing at which each letter is displayed

        #region CharInfo
        public static Dictionary<char, Vector2> charPosition; //The position of each character on the sprite font
        public static int charPositionSize = 16; //The size of each character position - X and Y (a square)
        public static int verticalSpacing = -16; //The spacing between each line of text vertically

        public const char wavyChar = '^'; //The character determining when wavy text should start and stop.
        public const char jitterChar = '_'; //The character determining when jittery text should start and stop.
        public const char phaseChar = 'Æ'; //The character determining when phasey text should start and stop.
        public const char screamChar = '▲'; //The character determining when screaming text should start and stop.
        public const char colorChar = '©'; //The character determining when colored text should start and stop.

        public static bool waving; //Whether the current line is waving or not
        public static bool jittering; //Whether the current line is jittering or not
        public static bool phasing; //Whether the current line is phasing or not
        public static bool screaming; //Whether the current line is screaming or not
        public static bool coloring; //Whether the current line is coloring or not

        public static TextColor currentTextColorProperty = TextColor.white; //The color of the text being written on the screen
        public static Color currentTextColorColor = Color.White; //The actual color value being typed
        #endregion 

        public static float animationIncrement; //Increments +1 every frame, for animation purposes.

        public enum TextboxState //The state of the textbox
        {
            lowering,
            raising, 
            printing,
            lowered
        }

        public enum TextColor //The color of which the text is being printed
        {
            white,
            red,
            blue,
            yellow,
            green,
            gray
        }

        public static Color whiteTextColor = new Color(255, 255, 255); //The color white for text color changes
        public static Color redTextColor = new Color(255, 0, 0); //The color red for text color changes
        public static Color greenTextColor = new Color(0, 255, 0); //The color green for text color changes
        public static Color blueTextColor = new Color(0, 0, 255); //The color blue for text color changes
        public static Color yellowTextColor = new Color(255, 255, 0); //The color yellow for text color changes
        public static Color cyanTextColor = new Color(0, 255, 255); //The color cyan for text color changes
        public static Color pinkTextColor = new Color(255, 0, 255); //The color pink for text color changes
        public static Color grayTextColor = new Color(128, 128, 128); //The color gray for text color changes
        #endregion

        /// <summary>
        /// Instantiates a new TextManager static class.
        /// </summary>
        static TextManager() { }

        /// <summary>
        /// Initializes variables for the Text Manager.
        /// </summary>
        public static void Init()
        {
            textCache = new List<DataTypes.RPGString>();
            InitCharDictionary();
            currentSpeech = "";
            textUIReady = false;
            textUILowering = false;
            stringDone = false;
            textTimer = 0;
            state = TextboxState.lowered;
            textboxPosition = new Vector2(0, textboxLoweredY);

            DisablePreferences();
        }

        /// <summary>
        /// Sets all text rendering preferences to false.
        /// </summary>
        public static void DisablePreferences()
        {
            waving = false;
            jittering = false;
            phasing = false;
            screaming = false;
            coloring = false;
        }

        /// <summary>
        /// Initialize the char dictionary.
        /// </summary>
        public static void InitCharDictionary()
        {
            charPosition = new Dictionary<char, Vector2>();
            string chars = " !\"■$%¢'()*+,-./0123456789:;“=”?•ABCDEFGHIJKLMNOPQRSTUVWXYZαβγΣΩπabcdefghijklmnopqrstuvwxyz[♩]~◯";
            char[] charsArray = chars.ToCharArray();
            int pos = 0;
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 12; j++)
                {
                    charPosition.Add(charsArray[pos], new Vector2(charPositionSize * j, charPositionSize * i));
                    pos++;
                }
            }
        }

        /// <summary>
        /// Draws a string onto the screen in a typewriter fashion.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pos"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="maxWidth"></param>
        public static void DrawString(string s, Vector2 startPos, SpriteBatch spriteBatch, int maxWidth = 300)
        {
            Vector2 currentPosition = startPos;
            char[] chars = s.ToCharArray();
            int length = 0;
            int currentLine = 1;
            float ylock = startPos.Y;
            for(int i = 0; i < s.Length; i++)
            {
                if(chars[i] == wavyChar)
                {
                    waving = !waving;
                }
                else if(chars[i] == jitterChar)
                {
                    jittering = !jittering;
                }
                else if(chars[i] == phaseChar)
                {
                    phasing = !phasing;
                }
                else if(chars[i] == screamChar)
                {
                    screaming = !screaming;
                }
                else if(chars[i] == colorChar)
                {
                    coloring = !coloring;
                }
                else
                {
                    int incr;
                    if (i != 0)
                        incr = MeasureChar(chars[i - 1]);
                    else
                        incr = 0;

                    if (waving)
                    {
                        //no lerp/rough
                        //currentPosition += new Vector2(incr, (float)Math.Floor(Math.Cos((animationIncrement / 7) + (i / 2)) * 2.2));

                        //lerp/smooth
                        currentPosition += new Vector2(incr, (float)(Math.Cos((animationIncrement / 7) + (i / 2)) * 2.2));
                    }
                    else if (jittering)
                    {
                        currentPosition += new Vector2(incr, (float)Math.Cos((animationIncrement + (i * 1.2f)) * .9f));
                    }
                    else if (phasing)
                    {
                        currentPosition += new Vector2(incr, (float)Math.Cos((animationIncrement) * 3));
                    }
                    else if (screaming)
                    {
                        currentPosition += new Vector2(incr + (float)Math.Cos((animationIncrement) * 2f), (float)Math.Cos((animationIncrement + (i / 2))));
                    }
                    else
                    {
                        currentPosition += new Vector2(incr, 0);
                    }

                    length += incr;
                    Vector2 dictPos = charPosition[chars[i]];
                    Rectangle charSource = new Rectangle((int)dictPos.X, (int)dictPos.Y, charPositionSize, charPositionSize);

                    if (chars[i] == ' ' && i != s.Length - 1)
                    {
                        if (length + LengthOfNextWord(s.Substring(i)) > maxWidth)
                        {
                            currentLine++;
                            currentPosition = startPos + new Vector2(-MeasureChar(' '), (currentLine * charPositionSize) + verticalSpacing);
                            ylock = currentPosition.Y;
                            length = 0;
                        }
                    }

                    if(coloring)
                    {
                        switch(currentTextColorProperty)
                        {
                            case (TextColor.white): currentTextColorColor = whiteTextColor; break;
                            case (TextColor.red): currentTextColorColor = redTextColor; break;
                            case (TextColor.green): currentTextColorColor = greenTextColor; break;
                            case (TextColor.blue): currentTextColorColor = blueTextColor; break;
                            case (TextColor.yellow): currentTextColorColor = yellowTextColor; break;
                            case (TextColor.gray): currentTextColorColor = grayTextColor; break;
                        }
                    }
                    else
                    {
                        currentTextColorColor = whiteTextColor;
                    }

                    spriteBatch.Draw
                    (
                        texture: mainFont,
                        sourceRectangle: charSource,
                        position: currentPosition,
                        color: currentTextColorColor
                    );
                }
                currentPosition = new Vector2(currentPosition.X, ylock);
            }
            DisablePreferences();
        }

        /// <summary>
        /// Returns the length of the next word in pixels.
        /// </summary>
        /// <param name="s">The string to be passed in.</param>
        /// <returns></returns>
        public static int LengthOfNextWord(string s)
        {
            string sub = s.Substring(0, s.IndexOf(' '));
            return MeasureString(sub);
        }

        /// <summary>
        /// Returns the length of a given string in pixels.
        /// </summary>
        /// <param name="s">The string's length to be determined.</param>
        /// <returns></returns>
        public static int MeasureString(string s)
        {
            int spacingLength = s.Length * textSpacing;
            int l = 0;
            for(int i = 0; i < s.Length; i++)
            {
                l += MeasureChar(s.ToCharArray()[i]);
            }

            return l - spacingLength;
        }


        #region CharLengths
        /// <summary>
        /// Measures the length of a char in pixels.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int MeasureChar(char c)
        {
            switch (c)
            {
                case (':'): 
                case ('I'): 
                case ('i'): 
                case ('l'): return 2;

                case ('■'):
                case ('\''):
                case ('!'):
                case (','): 
                case ('-'): 
                case ('.'): 
                case (';'): 
                case ('j'): 
                case ('['): 
                case (']'): return 3;

                case ('"'): 
                case ('('): 
                case (')'): 
                case ('*'):
                case ('•'): 
                case ('f'): 
                case ('r'): 
                case ('t'): return 4;

                case (' '):
                case ('/'):
                case ('0'):
                case ('1'):
                case ('2'):
                case ('3'):
                case ('4'):
                case ('6'):
                case ('7'):
                case ('8'):
                case ('9'): 
                case ('“'): 
                case ('”'): 
                case ('?'): 
                case ('E'): 
                case ('F'): 
                case ('J'): 
                case ('L'): 
                case ('Z'):
                case ('β'): 
                case ('b'): 
                case ('c'): 
                case ('d'): 
                case ('e'): 
                case ('g'): 
                case ('h'): 
                case ('Σ'):
                case ('k'): 
                case ('n'): 
                case ('o'): 
                case ('p'): 
                case ('s'):
                case ('u'):
                case ('x'): 
                case ('y'): 
                case ('z'): return 5;

                case ('$'):
                case ('+'):
                case ('='): 
                case ('B'): 
                case ('C'): 
                case ('D'): 
                case ('G'): 
                case ('H'):
                case ('X'):
                case ('Y'): 
                case ('N'):
                case ('K'):
                case ('O'): 
                case ('P'): 
                case ('Q'):
                case ('R'): 
                case ('S'):
                case ('5'):
                case ('T'): 
                case ('U'):
                case ('Ω'):
                case ('π'): 
                case ('a'): 
                case ('α'): 
                case ('v'):
                case ('♩'): return 6;

                case ('A'): 
                case ('V'): 
                case ('γ'): 
                case ('~'): return 7;

                case ('¢'): 
                case ('M'): 
                case ('W'): 
                case ('m'):
                case ('w'):
                case ('◯'): return 8;

                case ('%'): return 10;

                default: return 3;
            }
        }

        #endregion

        /// <summary>
        /// Loads the content for the text manager.
        /// </summary>
        /// <param name="Content"></param>
        public static void LoadContent(ContentManager Content)
        {
            mainFont = Content.Load<Texture2D>("UI/MainFont");
            textboxBackground = Content.Load<Texture2D>("UI/TextUIBackground");
            textboxNametag = Content.Load<Texture2D>("UI/TextUINametag");
            nametagPosition = new Vector2(-textboxNametag.Width, textboxPosition.Y - textboxNametag.Height);
        }

        /// <summary>
        /// Turns an RPG String datatype into a string and sets parameters accordingly for rendering, then adds to cache.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="name"></param>
        /// <param name="textSpeed"></param>
        public static void Say(string text, string name = "???", int textSpeed = 4)
        {
            DataTypes.RPGString speech = new DataTypes.RPGString(FilterString(text), name, textSpeed);
            AddTextToCache(speech);
        }

        /// <summary>
        /// Adds a RPGString to the text manager cache.
        /// </summary>
        /// <param name="input"></param>
        public static void AddTextToCache(DataTypes.RPGString input)
        {
            textCache.Insert(0, input);
        }

        /// <summary>
        /// Pops the last (and finished) RPG String off the 
        /// </summary>
        public static void CachePop()
        {
            if(textCache.Count != 0)
                textCache.RemoveAt(textCache.Count - 1);
        }

        public static string FilterString(string s)
        {
            return s;
        }

        /// <summary>
        /// Updates the text manager.
        /// </summary>
        public static void Update()
        {
            if (animationIncrement < int.MaxValue)
                animationIncrement++;
            else
                animationIncrement = 0;
            
            //Checks the state of the text manager
            switch(state)
            {
                //If the textbox is lowered...
                case (TextboxState.lowered):
                    if (nametagPosition.Y < RPGEngineMain.WindowHeight + textboxNametag.Height)
                        nametagPosition = new Vector2(nametagPosition.X, nametagPosition.Y + textboxAnimationSpeed);

                    if (textCache.Count > 0)
                        state = TextboxState.raising;
                    break;

                //If the textbox is raising...
                case (TextboxState.raising):
                    if (textboxPosition.Y > textboxRaisedY)
                        textboxPosition = new Vector2(textboxPosition.X, textboxPosition.Y - textboxAnimationSpeed);

                    if (textboxPosition.Y < textboxRaisedY)
                        textboxPosition = new Vector2(textboxPosition.X, textboxRaisedY);

                    nametagPosition = new Vector2(0, textboxPosition.Y - textboxNametag.Height);

                    if (textboxPosition.Y == textboxRaisedY)
                        state = TextboxState.printing;
                    break;

                //If the textbox is printing...
                case (TextboxState.printing):
                    //If you're holding down the key, make it type faster 
                    if(InputManager.State.IsKeyDown(Keys.Z))
                    {
                        textTimer += 4;
                    }
                    else
                    {
                        textTimer++;
                    }
                    
                    //If there's still text left...
                    if(textCache.Count > 0 && currentSpeech.Length < textCache[textCache.Count - 1].text.Length)
                    {
                        //And the timer reaches its limit...
                        if(textTimer >= textCache[textCache.Count - 1].textSpeed)
                        {
                            textTimer = 0;
                            //And the current speech length isin't greater than what's in the cache...
                            if(currentSpeech.Length < textCache[textCache.Count - 1].text.Length)
                            {
                                //Add a letter to the current text
                                currentSpeech = textCache[textCache.Count - 1].text.Substring(0, currentSpeech.Length + 1);
                            }

                            //If the next character equals these, make it longer to print out the next character
                            char newChar = currentSpeech.ToCharArray()[currentSpeech.Length - 1];
                            if (newChar == '.' || newChar == ',' || newChar == '!' || newChar == '?' || newChar == ';' || newChar == ':')
                            {
                                textTimer = (int)Math.Round(-textCache[textCache.Count - 1].textSpeed * 3.0);
                            }
                        }
                    }
                    else
                    {
                        //If the text is done, and you hit the key...
                        if(InputManager.Clicked(Keys.Z))
                        {
                            //Clear the current speech, and pop the cache
                            currentSpeech = "";
                            CachePop();
                            //But if there's no text left in the cache...
                            if (textCache.Count == 0)
                            {
                                //Reset the timer and lower the textbox
                                textTimer = 0;
                                state = TextboxState.lowering;
                            }
                        }
                    }
                    break;

                //If the textbox is lowering...
                case (TextboxState.lowering):
                    if (textboxPosition.Y < textboxLoweredY)
                        textboxPosition = new Vector2(textboxPosition.X, textboxPosition.Y + textboxAnimationSpeed);

                    if (textboxPosition.Y > textboxLoweredY)
                        textboxPosition = new Vector2(textboxPosition.X, textboxLoweredY);

                    nametagPosition = new Vector2(0, textboxPosition.Y - textboxNametag.Height);

                    if (textboxPosition.Y == textboxLoweredY)
                    {
                        state = TextboxState.lowered;
                        Managers.GlobalVariables.gameState = GameState.gamePlay;
                    } 
                    break;          
            }
        }

        /// <summary>
        /// Draws text to the screen if text is in the cache.
        /// </summary>
        public static void Draw(SpriteBatch spriteBatch)
        {
            //If the textbox nametag is visible...
            if (nametagPosition.Y < RPGEngineMain.WindowHeight + textboxNametag.Height)
            {
                //Draw it to the screen
                spriteBatch.Draw
                (
                    texture: textboxNametag,
                    position: Cameras.Camera.ProjectToScreen(nametagPosition),
                    color: Color.White
                );
            }

            //If the textbox is visible...
            if (textboxPosition.Y != textboxLoweredY)
            {
                //Draw it to the screen
                spriteBatch.Draw
                (
                    texture: textboxBackground,
                    position: Cameras.Camera.ProjectToScreen(textboxPosition),
                    color: Color.White
                );

                //If the textbox state is printing...
                if(state == TextboxState.printing)
                {
                    DrawString(currentSpeech, Cameras.Camera.ProjectToScreen(textboxPosition) + new Vector2(8, 8), spriteBatch, speechLength);
                    DrawString(textCache[textCache.Count - 1].Name, Cameras.Camera.ProjectToScreen(nametagPosition) + new Vector2(5), spriteBatch, speechLength);
                }
            }
        }

        public static int DefaultTextSpeed
        {
            get { return defaultTextSpeed; }
        }

        public static Texture2D MainFont
        {
            get { return mainFont; }
        }
    }
}
