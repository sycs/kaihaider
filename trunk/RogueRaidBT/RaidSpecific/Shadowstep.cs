//////////////////////////////////////////////////
//                Shadowstep.cs                 //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
// This is a blacklist of mobs which are        //
// considered unsafe to shadowstep to.          //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Collections.Generic;

namespace RogueRaidBT.RaidSpecific
{
    class Shadowstep
    {
        static public HashSet<uint> Blacklist = new HashSet<uint>
        {
        };
    }
}
