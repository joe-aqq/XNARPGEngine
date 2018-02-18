using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Managers
{
    static class MapManager
    {
        #region TESTMAP_VARIABLES
        private static string currentMapName; //The name of the map
        private static Texture2D baseTexture; //The base texture of the map - the "whole map"
        private static Texture2D prop1; //An example prop for the map
        private static Vector2 prop1Position; //The position of the example prop
        private static Characters.Character npc; //The test NPC
        private static Vector2 npcPosition; //The position of the NPC
        #endregion

        /// <summary>
        /// Initializes variables for the map manager.
        /// </summary>
        public static void Init()
        {
            switch (GlobalVariables.currentMap)
            {
                case (GlobalVariables.Maps.testMap):
                    InitTestMap();
                    break;
            }
        }

        #region TESTMAP_INIT
        /// <summary>
        /// Initializes the test map.
        /// </summary>
        public static void InitTestMap()
        {
            currentMapName = "????????";
            npcPosition = new Vector2(200, 100);
            prop1Position = new Vector2(200, 200);
            npc = new Characters.NPCs.TestNPC(npcPosition);
        }
        #endregion

        /// <summary>
        /// Updates the map manager.
        /// </summary>
        public static void Update()
        {
            switch (GlobalVariables.currentMap)
            {
                case (GlobalVariables.Maps.testMap):
                    UpdateTestMap();
                    break;
            }
        }

        /// <summary>
        /// Updates the test map.
        /// </summary>
        public static void UpdateTestMap()
        {
            npc.Update();
        }

        /// <summary>
        /// The stem method for loading all the games maps.
        /// </summary>
        /// <param name="Content"></param>
        public static void LoadContent(ContentManager Content)
        {
            switch(GlobalVariables.currentMap)
            {
                case(GlobalVariables.Maps.testMap):
                    LoadTestMap(Content);
                    break;
            }
        }

        /// <summary>
        /// Loads the correct map assets for the current map.
        /// </summary>
        /// <param name="Content"></param>
        public static void LoadTestMap(ContentManager Content)
        {
            baseTexture = Content.Load<Texture2D>("Maps/TestMap");
            prop1 = Content.Load<Texture2D>("Props/TestProp");
            npc.LoadContent(Content);

            Managers.CollisionManager.AddToCollisionList(
                "testprop",
                prop1Position,
                new Vector2(prop1.Bounds.Width, prop1.Bounds.Height)
            );
        }

        //-------------------------------------------------------------------------//
        //---------------------------------DRAWING---------------------------------//
        //-------------------------------------------------------------------------//

        /// <summary>
        /// Draws the map base and all props/NPCs within the map.
        /// </summary>
        /// <param name="spriteBatch">Monogame's spritebatch reference.</param>
        public static void DrawLayer1(SpriteBatch spriteBatch)
        {
            switch(GlobalVariables.currentMap)
            {
                case (GlobalVariables.Maps.testMap):
                    DrawLayer1TestMap(spriteBatch);
                    break;
            }
            
        }

        /// <summary>
        /// Draws what is on top of the player, and things that should go above the bottom layer
        /// </summary>
        /// <param name="spriteBatch"></param>
        public static void DrawLayer2(SpriteBatch spriteBatch)
        {
            switch (GlobalVariables.currentMap)
            {
                case (GlobalVariables.Maps.testMap):
                    DrawLayer2TestMap(spriteBatch);
                    break;
            }
        }

        #region TESTMAP_DRAWING
        public static void DrawLayer1TestMap(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw
            (
                texture: baseTexture,
                position: Vector2.Zero,
                color: Color.White
            );

            spriteBatch.Draw
            (
                texture: prop1,
                position: prop1Position,
                color: Color.White
            );

            npc.Draw(spriteBatch);
        }

        public static void DrawLayer2TestMap(SpriteBatch spriteBatch)
        {

        }
        #endregion

        //-------------------------------------------------------------------------//
        //-------------------------------END DRAWING-------------------------------//
        //-------------------------------------------------------------------------//

        public static string CurrentMapName
        {
            get { return currentMapName; }
        }
    }
}
