//////////////////////////////////////////////////
//               Delegates.cs                   //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace RogueBT.Helpers
{
    public delegate uint PoisonDelegate(object context);
    public delegate WoWPoint WoWPointDelegate(object context);
    public delegate WoWItem WoWItemDelegate(object context);
    public delegate WoWUnit WoWUnitDelegate(object context);
}
