using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGEngine.Managers
{
    public static class CollisionManager
    {
        #region Variables
        public static List<Vector2>[] collisionList; //CollisionList[0] = positions, collisionList[1] = sizes
        public static List<string> collisionNameList; //Names of collisions, so they can be easily added and removed
        #endregion

        /// <summary>
        /// Instantiates a new collision manager.
        /// </summary>
        static CollisionManager(){}

        /// <summary>
        /// Initializes the collision manager variables.
        /// </summary>
        public static void Init()
        {
            collisionList = new List<Vector2>[2];
            collisionNameList = new List<string>();
            collisionList[0] = new List<Vector2>();
            collisionList[1] = new List<Vector2>();
        }


        /// <summary>
        /// Adds a collision object to the current collision map's information.
        /// </summary>
        /// <param name="name">The name of the new collision object.</param>
        /// <param name="position">The position of the new collision object.</param>
        /// <param name="size">The size of the new collision object.</param>
        public static void AddToCollisionList(string name, Vector2 position, Vector2 size)
        {
            collisionList[0].Add(position);
            collisionList[1].Add(size);
            collisionNameList.Add(name);
        }

        /// <summary>
        /// Removes a collision object from the current collision map's information.
        /// </summary>
        /// <param name="name">The name of the collision object being removed.</param>
        public static void RemoveFromCollisionList(string name)
        {
            //Set position to "not found"
            int pos = -1;
            for (int i = 0; i < collisionNameList.Count; i++)
            {
                //If the collision name is found, save the position
                if (collisionNameList[i] == name)
                    pos = i;
            }

            //If the position isn't invalid, remove the collision from all lists
            if (pos != -1)
            {
                collisionList[0].RemoveAt(pos);
                collisionList[1].RemoveAt(pos);
                collisionNameList.RemoveAt(pos);
            }
        }

        /// <summary>
        /// Checks to see if the input position and size collides with any existing objects in the colision list. 
        /// </summary>
        /// <param name="position">Position of the object's collision getting checked.</param>
        /// <param name="size">Size of the object's collision getting checked.</param>
        /// <returns></returns>
        public static bool CheckCollision(Vector2 position, Vector2 size)
        {
            Rectangle rect1 = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            for(int i = 0; i < collisionList[0].Count; i++)
            {
                Rectangle rect2 = 
                    new Rectangle(
                        (int)collisionList[0][i].X, 
                        (int)collisionList[0][i].Y,
                        (int)collisionList[1][i].X, 
                        (int)collisionList[1][i].Y
                        );
                if (!Rectangle.Intersect(rect1, rect2).IsEmpty)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the double array of collision maps, for positions and sizes. 
        /// </summary>
        public static List<Vector2>[] CollisionList
        {
            get { return collisionList; }
        }

        /// <summary>
        /// Returns the list of collision map names.
        /// </summary>
        public static List<string> CollisionNameList
        {
            get { return collisionNameList; }
        }
    }
}
