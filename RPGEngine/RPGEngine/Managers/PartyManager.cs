using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RPGEngine.DataTypes;

namespace RPGEngine.Managers
{
    static class PartyManager
    {
        #region Variables
        public static int partyCount = 1; //The amount of people in the party
        public static RPGStats memberOne; //The first member of the party
        public static RPGStats memberTwo; //The second member of the party
        public static RPGStats memberThree; //The third member of the party
        public static RPGStats memberFour; //The fourth member of the party
        #endregion --Variables

        /// <summary>
        /// Initializes the party manager.
        /// </summary>
        /// <param name="member1"></param>
        /// <param name="member2"></param>
        /// <param name="member3"></param>
        /// <param name="member4"></param>
        public static void Init(RPGStats member1, RPGStats member2 = null, RPGStats member3 = null, RPGStats member4 = null)
        {
            memberOne = member1;
            memberTwo = member2;
            memberThree = member3;
            memberFour = member4;
        }

        /// <summary>
        /// Gets or sets the number of characters in the current party.
        /// </summary>
        public static int PartyCount
        {
            get { return partyCount; }
            set { partyCount = value; }
        }

        /// <summary>
        /// Returns the stats for a particular character.
        /// </summary>
        /// <param name="memberNumber"></param>
        /// <returns></returns>
        public static RPGStats GetStatsFor(int memberNumber)
        {
            switch(memberNumber % 4)
            {
                case 0:
                    return memberOne;
                case 1:
                    return memberTwo;
                case 2:
                    return memberThree;
                case 3:
                    return memberFour;
            }
            return null;
        }
    }
}
