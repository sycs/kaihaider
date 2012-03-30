//////////////////////////////////////////////////
//               Raid/Protection.cs             //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////
using TreeSharp;

namespace PallyRaidBT.Composites.Context
{
    static class Protection
    {
        static public Composite BuildCombatBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                new Switch<Helpers.Enumeration.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Raid,
                        Raid.Protection.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.HeroicDungeon,
                        Raid.Protection.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Dungeon,
                        Level.Protection.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Battleground,
                        Battleground.Protection.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.World,
                        Level.Protection.BuildCombatBehavior()
                    )
                )
            );
        }

        static public Composite BuildPullBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                new Switch<Helpers.Enumeration.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Raid,
                        Raid.Protection.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.HeroicDungeon,
                        Raid.Protection.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Dungeon,
                        Level.Protection.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Battleground,
                        Battleground.Protection.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.World,
                        Level.Protection.BuildPullBehavior()
                    )
                )
            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Switch<Helpers.Enumeration.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Raid,
                        Raid.Protection.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.HeroicDungeon,
                        Raid.Protection.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Dungeon,
                        Level.Protection.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Battleground,
                        Battleground.Protection.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.World,
                        Level.Protection.BuildBuffBehavior()
                    )
            );
        }
    }
}
