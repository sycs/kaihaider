//////////////////////////////////////////////////
//              Assassination.cs                //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using TreeSharp;

namespace RogueRaidBT.Composites.Context
{
    static class Assassination
    {
        static public Composite BuildCombatBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                new Switch<Helpers.Enum.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Raid,
                        Raid.Assassination.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Arena,
                        Arena.Assassination.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.HeroicDungeon,
                        Raid.Assassination.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Dungeon,
                        Level.Assassination.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Battleground,
                        Battleground.Assassination.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.World,
                        Level.Assassination.BuildCombatBehavior()
                    )
                )
            );
        }

        static public Composite BuildPullBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                new Switch<Helpers.Enum.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Raid,
                        Raid.Assassination.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Arena,
                        Arena.Assassination.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.HeroicDungeon,
                        Raid.Assassination.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Dungeon,
                        Level.Assassination.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Battleground,
                        Battleground.Assassination.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.World,
                        Level.Assassination.BuildPullBehavior()
                    )
                )
            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Switch<Helpers.Enum.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Raid,
                        Raid.Assassination.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Arena,
                        Arena.Assassination.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.HeroicDungeon,
                        Raid.Assassination.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Dungeon,
                        Level.Assassination.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Battleground,
                        Battleground.Assassination.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.World,
                        Level.Assassination.BuildBuffBehavior()
                    )
            );
        }
    }
}
