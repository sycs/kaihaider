//////////////////////////////////////////////////
//                 Retribution.cs               //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx;
using TreeSharp;

namespace PallyRaidBT.Composites.Context
{
    class Retribution
    {
        static public Composite BuildCombatBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                new Switch<Helpers.Area.LocationContext>(ret => Helpers.Area.mLocation,
                    Raid.Retribution.BuildCombatBehavior(),

                    new SwitchArgument<Helpers.Area.LocationContext>(Helpers.Area.LocationContext.Battleground,
                        Battleground.Retribution.BuildCombatBehavior()
                    ),


                    new SwitchArgument<Helpers.Area.LocationContext>(Helpers.Area.LocationContext.Undefined,
                        Battleground.Retribution.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Area.LocationContext>(Helpers.Area.LocationContext.World,
                        Level.Retribution.BuildCombatBehavior()
                    )
                )
            );
        }

        static public Composite BuildPullBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                new Switch<Helpers.Area.LocationContext>(ret => Helpers.Area.mLocation,
                    Raid.Retribution.BuildPullBehavior(),

                    new SwitchArgument<Helpers.Area.LocationContext>(Helpers.Area.LocationContext.Battleground,
                        Battleground.Retribution.BuildPullBehavior()
                    ),


                    new SwitchArgument<Helpers.Area.LocationContext>(Helpers.Area.LocationContext.Undefined,
                        Battleground.Retribution.BuildPullBehavior()
                    ),


                    new SwitchArgument<Helpers.Area.LocationContext>(Helpers.Area.LocationContext.World,
                        Level.Retribution.BuildPullBehavior()
                    )
                )
            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Switch<Helpers.Area.LocationContext>(ret => Helpers.Area.mLocation,
                Raid.Retribution.BuildBuffBehavior(),

                new SwitchArgument<Helpers.Area.LocationContext>(Helpers.Area.LocationContext.Battleground,
                    Battleground.Retribution.BuildBuffBehavior()
                ),


                    new SwitchArgument<Helpers.Area.LocationContext>(Helpers.Area.LocationContext.Undefined,
                        Battleground.Retribution.BuildBuffBehavior()
                    ),


                new SwitchArgument<Helpers.Area.LocationContext>(Helpers.Area.LocationContext.World,
                    Level.Retribution.BuildBuffBehavior()
                )
            );
        }
    }
}
