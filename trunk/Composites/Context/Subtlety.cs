//////////////////////////////////////////////////
//                Subtlety.cs                   //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx.TreeSharp;

namespace RogueRaidBT.Composites.Context
{
    static class Subtlety
    {
        static public Composite BuildCombatBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                new Switch<Helpers.Enum.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Raid,
                        Raid.Subtlety.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Arena,
                        Arena.Subtlety.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.HeroicDungeon,
                        Raid.Subtlety.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Dungeon,
                        Level.Subtlety.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Battleground,
                        Battleground.Subtlety.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.World,
                        Level.Subtlety.BuildCombatBehavior()
                    )
                )
            );
        }

        static public Composite BuildPullBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                new Switch<Helpers.Enum.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Raid,
                        Raid.Subtlety.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Arena,
                        Arena.Subtlety.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.HeroicDungeon,
                        Raid.Subtlety.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Dungeon,
                        Level.Subtlety.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Battleground,
                        Battleground.Subtlety.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.World,
                        Level.Subtlety.BuildPullBehavior()
                    )
                )
            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Switch<Helpers.Enum.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Raid,
                        Raid.Subtlety.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Arena,
                        Arena.Subtlety.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.HeroicDungeon,
                        Raid.Subtlety.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Dungeon,
                        Level.Subtlety.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Battleground,
                        Battleground.Subtlety.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.World,
                        Level.Subtlety.BuildBuffBehavior()
                    )
            );
        }
    }
}
