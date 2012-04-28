//////////////////////////////////////////////////
//                    Holy.cs                   //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////
using TreeSharp;

namespace PallyRaidBT.Composites.Context
{
    static class Holy
    {
        static public Composite BuildCombatBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                new Switch<Helpers.Enumeration.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Raid,
                        Raid.Holy.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Arena,
                        Arena.Holy.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.HeroicDungeon,
                        Raid.Holy.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Dungeon,
                        Level.Holy.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Battleground,
                        Battleground.Holy.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.World,
                        Level.Holy.BuildCombatBehavior()
                    )
                )
            );
        }

        static public Composite BuildPullBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                new Switch<Helpers.Enumeration.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Raid,
                        Raid.Holy.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Arena,
                        Arena.Holy.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.HeroicDungeon,
                        Raid.Holy.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Dungeon,
                        Level.Holy.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Battleground,
                        Battleground.Holy.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.World,
                        Level.Holy.BuildPullBehavior()
                    )
                )
            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Switch<Helpers.Enumeration.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Raid,
                        Raid.Holy.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Arena,
                        Arena.Holy.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.HeroicDungeon,
                        Raid.Holy.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Dungeon,
                        Level.Holy.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.Battleground,
                        Battleground.Holy.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enumeration.LocationContext>(Helpers.Enumeration.LocationContext.World,
                        Level.Holy.BuildBuffBehavior()
                    )
            );
        }
    }
}
