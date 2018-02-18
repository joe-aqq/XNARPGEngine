using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RPGEngine.Managers;
using static RPGEngine.Managers.GlobalVariables;

namespace RPGEngine.Characters.NPCs
{
    public class TestNPC : Character
    {
        #region Variables
        private Vector2 size; //Size of one frame of the character
        private float talkDist; //The distance at which the character needs to be to talk to the character
        private int talkTimes; //The amount of times you've talked to this NPC
        private talkingState talkState; //The state of text that the NPC displays when talked to

        private enum talkingState //The state of the the NPC's text
        {
            regular,
            pissed,
            reallyPissed
        }
        #endregion

        /// <summary>
        /// Create a new TestNPC.
        /// </summary>
        /// <param name="position"></param>
        public TestNPC(Vector2 position)
        {
            this.position = position;
            this.name = "NPC";
            this.size = new Vector2(18, 30);
            this.state = State.still;
            this.visible = true;
            this.talkDist = 35f;
            this.talkState = talkingState.regular;
        }

        /// <summary>
        /// Load the NPC's content.
        /// </summary>
        /// <param name="Content">MonoGame's content reference.</param>
        public override void LoadContent(ContentManager Content)
        {
            textureSheet = Content.Load<Texture2D>("Characters/TestNPC");
            Managers.CollisionManager.AddToCollisionList(
                "testnpc",
                position,
                new Vector2(size.X, (float)Math.Round(size.Y / 2))
            );
        }

        /// <summary>
        /// Update the NPC.
        /// </summary>
        public override void Update()
        {
            CheckForInteraction();
        }

        public void CheckForInteraction()
        {
            //If the NPC is within the talking distance and text is already not on the screen...
            if (Vector2.Distance(this.position, MainCharacter.Pos) < talkDist && GlobalVariables.gameState == GameState.gamePlay)
            {
                //And the player presses the "Z" key...
                if (InputManager.Clicked(InputManager.UseKey))
                {
                    //Set showing text to true, and send an RPG string to the Text Manager.
                    GlobalVariables.gameState = GameState.showingText;
                    this.talkTimes++;
                    switch (talkState)
                    {
                        case (talkingState.regular):
                            TextManager.currentTextColorProperty = TextManager.TextColor.gray;
                            TextManager.Say("Now that the text engine works properly, I can do anything I want with it.... ©^(how mischevious!)^©", name);
                            talkState = talkingState.pissed;
                            break;

                        case (talkingState.pissed):
                            TextManager.Say("What are ^you^ looking at you son of a bitch?", name + " (pissed)");
                            if(talkTimes >= 5)
                                talkState = talkingState.reallyPissed;
                            break;

                        case (talkingState.reallyPissed):
                            TextManager.Say("You're really going to go ahead and talk to me _" + talkTimes + "_ times in a row?", name + " (really pissed)");
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Draw the NPC to the scene.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                spriteBatch.Draw
                (
                    texture: this.textureSheet,
                    sourceRectangle:new Rectangle(0,0,(int)size.X, (int)size.Y),
                    position: this.position,
                    color: Color.White
                );
            }
        }

        /// <summary>
        /// The center of the NPC; the average of it's pixel data.
        /// </summary>
        public Vector2 Center
        {
            get { return new Vector2(position.X + (float)Math.Round(size.X / 2), position.Y + (float)Math.Round(size.Y / 2)); }
        }
    }
}
