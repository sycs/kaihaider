﻿//////////////////////////////////////////////////
//                  Arena/Holy.cs               //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using TreeSharp;
namespace PallyRaidBT.Composites.Context.Arena
{
    class Holy
    {
        static public Composite BuildCombatBehavior()
        {
            return Raid.Retribution.BuildCombatBehavior();
        }

        static public Composite BuildPullBehavior()
        {
            return Raid.Retribution.BuildPullBehavior();
        }

        static public Composite BuildBuffBehavior()
        {
            return Raid.Retribution.BuildBuffBehavior();
        }
    }
}