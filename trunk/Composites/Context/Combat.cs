//////////////////////////////////////////////////
//                 Combat.cs                    //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx.TreeSharp;

namespace RogueRaidBT.Composites.Context
{
    static class Combat
    {
        static public Composite BuildCombatBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                new Switch<Helpers.Enum.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Raid,
                        Raid.Combat.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Arena,
                        Arena.Combat.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.HeroicDungeon,
                        Raid.Combat.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Dungeon,
                        Level.Combat.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Battleground,
                        Battleground.Combat.BuildCombatBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.World,
                        Level.Combat.BuildCombatBehavior()
                    )
                )
            );
        }

        static public Composite BuildPullBehavior()
        {
            return new Decorator(ret => Settings.Mode.mUseCombat,
                new Switch<Helpers.Enum.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Raid,
                        Raid.Combat.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Arena,
                        Arena.Combat.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.HeroicDungeon,
                        Raid.Combat.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Dungeon,
                        Level.Combat.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Battleground,
                        Battleground.Combat.BuildPullBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.World,
                        Level.Combat.BuildPullBehavior()
                    )
                )
            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Switch<Helpers.Enum.LocationContext>(ret => Helpers.Area.mLocation,

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Raid,
                        Raid.Combat.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Arena,
                        Arena.Combat.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.HeroicDungeon,
                        Raid.Combat.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Dungeon,
                        Level.Combat.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.Battleground,
                        Battleground.Combat.BuildBuffBehavior()
                    ),

                    new SwitchArgument<Helpers.Enum.LocationContext>(Helpers.Enum.LocationContext.World,
                        Level.Combat.BuildBuffBehavior()
                    )
            );
        }
    }
}
