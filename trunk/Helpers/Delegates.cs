//////////////////////////////////////////////////
//               Delegates.cs                   //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace RogueRaidBT.Helpers
{
    public delegate uint PoisonDelegate(object context);
    public delegate WoWPoint WoWPointDelegate(object context);
    public delegate WoWItem WoWItemDelegate(object context);
    public delegate WoWUnit WoWUnitDelegate(object context);
}
