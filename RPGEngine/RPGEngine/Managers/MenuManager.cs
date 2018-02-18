using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Managers
{
    static class MenuManager
    {
        #region Variables
        public static Texture2D mainMenu; //The texture for the main menu
        public static Texture2D mainMenuFade; //The fading BG for the main menu
        public static Texture2D mainMenuArrow; //The arrow for the main menu
        public static Texture2D mainMenuBGBar; //The background bar texture
        public static Texture2D mainMenuHealthBar; //The health bar texture
        public static Texture2D mainMenuMPBar; //The MP bar texture
        public static Texture2D mainMenuEXPBar; //The EXP bar texture
        public static Vector2 mainMenuPosition; //The position of the main menu
        public static float menuSpeed = 20f; //The speed of the main menu
        public static float menuSpeedDefault = 20f; //The default speed of the main menu
        public static float menuSpeedMin = .1f; //The minimum speed of the main menu
        public static float menuSpeedMultiplier = .9177f; //The multiplier of the speed of the main menu
        public static MainMenuState mainMenuState; //The variable state of the main menu
        public static Vector2 mapNamePosition; //The position of the text of the current map's name
        public static Vector2[] partyNamePositions; //The position of the text of the name of the party's first member
        public static Vector2[] menuArrowMainPositions; //The regular positions of the major main menu cursor
        public static Vector2[] menuArrowMainPositionsAnimated; //The animation positions of the major main menu cursor
        public static int arrowAnimationTimer = 0; //The tracked timer of the animation state
        public static int arrowAnimationTimeout = 20; //The max amount of time an animation frame can go for the menu cursor
        public static int arrowPixelDifference = 5; //How far on the X axis the hand animates
        public static int menuArrowState; //The numerical state of what box is selected

        #region Stats
        public static Vector2 statsNamePos; //The position of the name on the menu
        public static Vector2 statsLevelPos; //The position of the level on the menu
        public static Vector2 statsHealthPos; //The position of the health on the menu
        public static Vector2 statsHealthBarPos; //The position of the health bar on the menu
        public static Vector2 statsMPPos; //The position of the mp on the menu
        public static Vector2 statsMPBarPos; //The position of the mp bar on the menu
        public static Vector2 statsEXPPos; //The position of the exp on the menu
        public static Vector2 statsEXPBarPos; //The position of the exp bar on the menu
        public static float statBarScale = .7f; //The X scale of the menu health bars
        #endregion

        public enum MainMenuState //The state of the main menu
        {
            raised,
            lowering,
            lowered,
            raising
        }
        #endregion

        /// <summary>
        /// Instantiates a new MenuManager class.
        /// </summary>
        static MenuManager() { }

        /// <summary>
        /// Initializes the menu manager.
        /// </summary>
        public static void Init()
        {
            menuArrowState = 0;
            menuArrowMainPositions = new Vector2[4];
            menuArrowMainPositionsAnimated = new Vector2[4];
            mainMenuState = MainMenuState.raised;

            //Positions of the major main menu arrow
            menuArrowMainPositions[0] = new Vector2(53, 45);
            menuArrowMainPositions[1] = new Vector2(53, 115);
            menuArrowMainPositions[2] = new Vector2(266, 45);
            menuArrowMainPositions[3] = new Vector2(266, 115);

            //Positions of the major main menu arrow, animated by 1 tick
            menuArrowMainPositionsAnimated[0] = new Vector2(menuArrowMainPositions[0].X + arrowPixelDifference, menuArrowMainPositions[0].Y);
            menuArrowMainPositionsAnimated[1] = new Vector2(menuArrowMainPositions[1].X + arrowPixelDifference, menuArrowMainPositions[1].Y);
            menuArrowMainPositionsAnimated[2] = new Vector2(menuArrowMainPositions[2].X - arrowPixelDifference, menuArrowMainPositions[2].Y);
            menuArrowMainPositionsAnimated[3] = new Vector2(menuArrowMainPositions[3].X - arrowPixelDifference, menuArrowMainPositions[3].Y);

            //Main menu text positions
            mapNamePosition = new Vector2(62, 185);

            //Menu party name positions
            partyNamePositions = new Vector2[4];
            partyNamePositions[0] = new Vector2(12, 45);
            partyNamePositions[1] = new Vector2(12, 115);
            partyNamePositions[2] = new Vector2(266, 45);
            partyNamePositions[3] = new Vector2(266, 115);


            /////////STATS/////////
            statsNamePos = new Vector2(86, 76);
            statsLevelPos = new Vector2(233, 76);
            statsHealthPos = new Vector2(86, 88);
            statsHealthBarPos = new Vector2(86, 102);
            statsMPPos = new Vector2(86, 112);
            statsMPBarPos = new Vector2(86, 126);
            statsEXPPos = new Vector2(86, 136);
            statsEXPBarPos = new Vector2(86, 150);
            ///////////////////////
        }

        /// <summary>
        /// Loads the content for the menu manager.
        /// </summary>
        /// <param name="Content">The content parameter for MonoGame.</param>
        public static void LoadContent(ContentManager Content)
        {
            mainMenu = Content.Load<Texture2D>("UI/MenuUIMain");
            mainMenuFade = Content.Load<Texture2D>("UI/MenuUIFade");
            mainMenuArrow = Content.Load<Texture2D>("UI/MenuUIArrow");
            mainMenuBGBar = Content.Load<Texture2D>("UI/MenuUIBGBar");
            mainMenuHealthBar = Content.Load<Texture2D>("UI/MenuUIHealthBar");
            mainMenuMPBar = Content.Load<Texture2D>("UI/MenuUIMPBar");
            mainMenuEXPBar = Content.Load<Texture2D>("UI/MenuUIEXPBar");
            mainMenuPosition = new Vector2(0, -mainMenu.Bounds.Height);
        }

        /// <summary>
        /// Updates the menu manager.
        /// </summary>
        public static void Update()
        {
            CheckMainMenu();
        }

        /// <summary>
        /// Moves the arrow position for an animation effect.
        /// </summary>
        public static void TickArrowAnimation()
        {
            Vector2[] slot = new Vector2[4];
            slot = menuArrowMainPositions;
            menuArrowMainPositions = menuArrowMainPositionsAnimated;
            menuArrowMainPositionsAnimated = slot;
        }

        /// <summary>
        /// Checks the main menu commands through the update method.
        /// </summary>
        public static void CheckMainMenu()
        {
            //If the menu is not hidden, tick the hand for an animation effect
            if(mainMenuState != MainMenuState.raised)
            {
                arrowAnimationTimer++;
                if (arrowAnimationTimer == arrowAnimationTimeout)
                {
                    arrowAnimationTimer = 0;
                    TickArrowAnimation();
                }
            }

            //Check the state of the menu
            switch(mainMenuState)
            {
                //If the main menu is raised...
                case (MainMenuState.raised):
                    //If you click the menu key and the game's state is in gameplay
                    if (InputManager.Clicked(InputManager.MenuKey) &&
                        GlobalVariables.gameState == GlobalVariables.GameState.gamePlay)
                    {
                        //Change the game's state and start lowering the menu
                        GlobalVariables.gameState = GlobalVariables.GameState.menuDisplay;
                        mainMenuState = MainMenuState.lowering;
                    }
                    break;
                //If the menu is lowering...
                case (MainMenuState.lowering):
                    //If the menu position not yet at the lowered position...
                    if(mainMenuPosition.Y < 0)
                    {
                        //Move the menu position down
                        menuArrowState = 0;
                        mainMenuPosition = new Vector2(0, mainMenuPosition.Y + menuSpeed);
                        if(menuSpeed > menuSpeedMin)
                            menuSpeed *= menuSpeedMultiplier;

                        //If there's any overhang, set it back to zero
                        if (mainMenuPosition.Y > 0)
                            mainMenuPosition = Vector2.Zero;

                        //If the menu is at zero, declare it to be lowered
                        if (mainMenuPosition == Vector2.Zero)
                        {
                            menuSpeed = menuSpeedDefault;
                            mainMenuState = MainMenuState.lowered;
                        }
                            
                    }
                    break;
                //If the menu is lowered...
                case (MainMenuState.lowered): 
                    //If the down button is pressed, move the state & cursor down one
                    if (InputManager.Clicked(Keys.Down))
                    {
                        if (menuArrowState < menuArrowMainPositions.Length - 1 && menuArrowState + 1 < PartyManager.PartyCount) { menuArrowState++; }
                        else
                            menuArrowState = 0;
                    }

                    //If the up button is pressed, move the state & the cursor up one
                    if (InputManager.Clicked(Keys.Up))
                    {
                        if (menuArrowState > 0){ menuArrowState--;}
                        else 
                            menuArrowState = PartyManager.PartyCount - 1;
                    }

                    //If left or right is pressed, wrap around the screen
                    if(InputManager.Clicked(Keys.Right) || InputManager.Clicked(Keys.Left))
                    {
                        if (menuArrowState == 0 && PartyManager.PartyCount >= 3){ menuArrowState = 2; break; }
                        if (menuArrowState == 2 && PartyManager.PartyCount >= 1) { menuArrowState = 0; break; }
                        if (menuArrowState == 1 && PartyManager.PartyCount >= 4) { menuArrowState = 3; break; }
                        if (menuArrowState == 3 && PartyManager.PartyCount >= 2) { menuArrowState = 1; break; }
                    }

                    switch (menuArrowState)
                    {
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                    }

                    //If the back key is pressed, exit the menu
                    if (InputManager.Clicked(InputManager.BackKey) &&
                        GlobalVariables.gameState == GlobalVariables.GameState.menuDisplay)
                    {
                        mainMenuState = MainMenuState.raising;
                    }
                    break;
                //If the menu is raising...
                case (MainMenuState.raising):
                    if(mainMenuPosition.Y > -mainMenu.Bounds.Height)
                    {
                        mainMenuPosition = new Vector2(0, mainMenuPosition.Y - menuSpeed);
                        if (menuSpeed > menuSpeedMin)
                            menuSpeed *= menuSpeedMultiplier;

                        if(mainMenuPosition.Y < (-mainMenu.Bounds.Height * .8f))
                            GlobalVariables.gameState = GlobalVariables.GameState.gamePlay;

                        if (mainMenuPosition.Y < -mainMenu.Bounds.Height)
                            mainMenuPosition = new Vector2(0, -mainMenu.Bounds.Height);

                        if (mainMenuPosition.Y == -mainMenu.Bounds.Height)
                        {
                            menuSpeed = menuSpeedDefault;
                            mainMenuState = MainMenuState.raised;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Draws the menus and text to the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void Draw(SpriteBatch spriteBatch)
        {
            #pragma warning disable CS0618 // Type or member is obsolete
            //If the menu is not raised
            if (mainMenuState != MainMenuState.raised)
            {
                //Draw the main menu UI
                spriteBatch.Draw
                (
                    texture: mainMenu,
                    position: Cameras.Camera.ProjectToScreen(mainMenuPosition),
                    color: Color.White
                );

                TextManager.DrawString(
                    MapManager.CurrentMapName, 
                    Cameras.Camera.ProjectToScreen(mainMenuPosition + mapNamePosition),
                    spriteBatch);

                //Draw the characters names
                for(int i = 0; i < PartyManager.PartyCount; i++)
                {
                    TextManager.DrawString(
                        PartyManager.GetStatsFor(i).Name, 
                        Cameras.Camera.ProjectToScreen(mainMenuPosition + partyNamePositions[i]), 
                        spriteBatch
                        );
                }

                ///////////////////////////
                //Draw the characters stats
                ///////////////////////////

                //Draw the character name
                TextManager.DrawString(
                    PartyManager.GetStatsFor(menuArrowState).Name,
                    Cameras.Camera.ProjectToScreen(mainMenuPosition + statsNamePos), 
                    spriteBatch);

                var levelText = "Level " + PartyManager.GetStatsFor(menuArrowState).Level;

                //Draw the character level
                TextManager.DrawString(
                    levelText,
                    Cameras.Camera.ProjectToScreen(
                        mainMenuPosition + new Vector2
                        (
                            statsLevelPos.X - TextManager.MeasureString(levelText),
                            statsLevelPos.Y
                        )
                    ),
                    spriteBatch
                );
                    
                //Draw the character health
                TextManager.DrawString(
                    "HP: " + PartyManager.GetStatsFor(menuArrowState).Health + "/" + PartyManager.GetStatsFor(menuArrowState).HealthMax,
                    Cameras.Camera.ProjectToScreen(mainMenuPosition + statsHealthPos),
                    spriteBatch
                    );

                //Draw the healthbar BG
                spriteBatch.Draw
                (
                    texture: mainMenuBGBar,
                    position: Cameras.Camera.ProjectToScreen(mainMenuPosition + statsHealthBarPos),
                    scale: new Vector2(statBarScale, 1f),
                    color: Color.White
                );

                //Draw the healthbar
                spriteBatch.Draw
                (
                    texture: mainMenuHealthBar,
                    position: Cameras.Camera.ProjectToScreen(mainMenuPosition + statsHealthBarPos),
                    scale: new Vector2((float)PartyManager.GetStatsFor(menuArrowState).Health / (float)PartyManager.GetStatsFor(menuArrowState).HealthMax, 1f) * new Vector2(statBarScale, 1f),
                    color: Color.White 
                );
                
                //Draw the character MP
                TextManager.DrawString(
                    "MP: " + PartyManager.GetStatsFor(menuArrowState).MP + "/" + PartyManager.GetStatsFor(menuArrowState).MPMax,
                    Cameras.Camera.ProjectToScreen(mainMenuPosition + statsMPPos),
                    spriteBatch
                );

                //Draw the MP bar BG
                spriteBatch.Draw
                (
                    texture: mainMenuBGBar,
                    position: Cameras.Camera.ProjectToScreen(mainMenuPosition + statsMPBarPos),
                    scale: new Vector2(statBarScale, 1f),
                    color: Color.White
                );

                //Draw the MP bar
                spriteBatch.Draw
                (
                    texture: mainMenuMPBar,
                    position: Cameras.Camera.ProjectToScreen(mainMenuPosition + statsMPBarPos),
                    scale: new Vector2((float)PartyManager.GetStatsFor(menuArrowState).MP / (float)PartyManager.GetStatsFor(menuArrowState).MPMax, 1f) * new Vector2(statBarScale, 1f),
                    color: Color.White
                );

                //Draw the character EXP
                TextManager.DrawString(
                    "EXP: " + PartyManager.GetStatsFor(menuArrowState).EXP + " - " + PartyManager.GetStatsFor(menuArrowState).EXPMax,
                    Cameras.Camera.ProjectToScreen(mainMenuPosition + statsEXPPos),
                    spriteBatch
                );

                //Draw the EXP bar BG
                spriteBatch.Draw
                (
                    texture: mainMenuBGBar,
                    position: Cameras.Camera.ProjectToScreen(mainMenuPosition + statsEXPBarPos),
                    scale: new Vector2(statBarScale, 1f),
                    color: Color.White
                );


                //Draw the EXP bar
                spriteBatch.Draw
                (
                    texture: mainMenuEXPBar,
                    position: Cameras.Camera.ProjectToScreen(mainMenuPosition + statsEXPBarPos),
                    scale: new Vector2(((float)PartyManager.GetStatsFor(menuArrowState).EXP / (float)PartyManager.GetStatsFor(menuArrowState).EXPMax), 1f) * new Vector2(statBarScale, 1f),
                    color: Color.White
                );

                ///////////////////////////
                //////////// End //////////
                ///////////////////////////

                //If the arrow is on the left side of the screen...
                if (menuArrowState == 0 || menuArrowState == 1)
                {
                    //Draw it in it's correct position, unflipped
                    spriteBatch.Draw
                    (
                        texture: mainMenuArrow,
                        position: Cameras.Camera.ProjectToScreen(mainMenuPosition + menuArrowMainPositions[menuArrowState]),
                        sourceRectangle: new Rectangle(0,0,22,16),
                        color: Color.White
                    );
                }
                //Else, if it is on the right side of the screen
                else if (menuArrowState == 2 || menuArrowState == 3)
                {
                    //Draw it flipped
                    spriteBatch.Draw
                    (
                        texture: mainMenuArrow,
                        position: Cameras.Camera.ProjectToScreen(mainMenuPosition + menuArrowMainPositions[menuArrowState] + new Vector2(-mainMenuArrow.Bounds.Width / 2, 0)),
                        sourceRectangle: new Rectangle(22, 0, 22, 16),
                        color: Color.White
                    );
                }
            }
            #pragma warning restore CS0618 // Type or member is obsolete
        }
    }
}
