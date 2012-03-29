﻿//////////////////////////////////////////////////
//             Level/Retribution.cs             //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from PallyRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using CommonBehaviors.Actions;
using Styx;
using Styx.WoWInternals;
using TreeSharp;

namespace PallyRaidBT.Composites.Context.Level
{
    class Retribution
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
