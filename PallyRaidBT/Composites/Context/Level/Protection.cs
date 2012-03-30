//////////////////////////////////////////////////
//              Level/Protection.cs             //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using CommonBehaviors.Actions;
using Styx;
using TreeSharp;
namespace PallyRaidBT.Composites.Context.Level
{
    class Protection
    {
        static public Composite BuildCombatBehavior()
        {
            return Raid.Protection.BuildCombatBehavior();
        }

        static public Composite BuildPullBehavior()
        {
            return Raid.Protection.BuildPullBehavior();
        }

        static public Composite BuildBuffBehavior()
        {
            return Raid.Protection.BuildBuffBehavior();
        }
    }
}
