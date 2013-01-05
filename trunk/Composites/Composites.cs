//////////////////////////////////////////////////
//               Composites.cs                  //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx.TreeSharp;

namespace RogueRaidBT.Composites
{
    class Composites
    {
        static public Composite BuildCombatBehavior()
        {
            return new Switch<Styx.WoWSpec>(ret => Helpers.Rogue.mCurrentSpec,

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.None,
                 Context.Subtlety.BuildCombatBehavior()  //     Context.None.BuildCombatBehavior()
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueAssassination,
                  Context.Subtlety.BuildCombatBehavior()  //    Context.Assassination.BuildCombatBehavior()
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueCombat,
                  Context.Subtlety.BuildCombatBehavior()  //    Context.Combat.BuildCombatBehavior()
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueSubtlety,
                    Context.Subtlety.BuildCombatBehavior()  //  
                )

            );
        }
        


        static public Composite BuildPullBehavior()
        {
            return new Switch<Styx.WoWSpec>(ret => Helpers.Rogue.mCurrentSpec,

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.None,
                    Context.Subtlety.BuildPullBehavior() //Context.None.BuildPullBehavior()
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueAssassination,
                    Context.Subtlety.BuildPullBehavior() //Context.Assassination.BuildPullBehavior()
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueCombat,
                    Context.Subtlety.BuildPullBehavior() //Context.Combat.BuildPullBehavior()
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueSubtlety,
                    Context.Subtlety.BuildPullBehavior() //
                )

            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Switch<Styx.WoWSpec>(ret => Helpers.Rogue.mCurrentSpec,

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.None,
                    new Action(ret => RunStatus.Failure)
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueAssassination,
                    new PrioritySelector(
                        Helpers.Rogue.ApplyPosions,
                        Context.Subtlety.BuildBuffBehavior()  //Context.Assassination.BuildBuffBehavior()
                    )
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueCombat,
                    new PrioritySelector(
                        Helpers.Rogue.ApplyPosions,
                        Context.Subtlety.BuildBuffBehavior()  //Context.Combat.BuildBuffBehavior()
                    )
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueSubtlety,
                    new PrioritySelector(
                        Helpers.Rogue.ApplyPosions,
                        Context.Subtlety.BuildBuffBehavior()  //
                    )
                )

            );
        }
    }
}
