//////////////////////////////////////////////////
//               Composites.cs                  //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx.TreeSharp;

namespace RogueBT.Composites
{
    class Composites
    {
        static public Composite BuildCombatBehavior()
        {
            return new Switch<Styx.WoWSpec>(ret => Helpers.Rogue.mCurrentSpec,

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.None,
                 Context.None.BuildCombatBehavior()
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueAssassination,
                    new PrioritySelector(
                        Helpers.Rogue.ApplyPoisons,
                  Context.Assassination.BuildCombatBehavior())
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueCombat,
                    new PrioritySelector(
                        Helpers.Rogue.ApplyPoisons,
                  Context.Combat.BuildCombatBehavior())
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueSubtlety,
                    new PrioritySelector(
                        Helpers.Rogue.ApplyPoisons,
                    Context.Subtlety.BuildCombatBehavior())
                )

            );
        }
        


        static public Composite BuildPullBehavior()
        {
            return new Switch<Styx.WoWSpec>(ret => Helpers.Rogue.mCurrentSpec,

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.None,
                    new Sequence(
                    new Action(ret => { Helpers.General.UpdateHelpers(); }),
                    Context.None.BuildPullBehavior())
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueAssassination,
                    new Sequence(
                    new Action(ret => { Helpers.General.UpdateHelpers(); }),
                    Context.Assassination.BuildPullBehavior())
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueCombat,
                    new Sequence(
                    new Action(ret => { Helpers.General.UpdateHelpers(); }),
                    Context.Combat.BuildPullBehavior())
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueSubtlety,
                    new Sequence(
                    new Action(ret => { Helpers.General.UpdateHelpers(); }),
                    Context.Subtlety.BuildPullBehavior() )
                )

            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Switch<Styx.WoWSpec>(ret => Helpers.Rogue.mCurrentSpec,

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.None,
                    Context.None.BuildBuffBehavior()
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueAssassination,
                    new PrioritySelector(
                        Helpers.Rogue.ApplyPoisons,
                        Context.Assassination.BuildBuffBehavior()
                    )
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueCombat,
                    new PrioritySelector(
                        Helpers.Rogue.ApplyPoisons,
                        Context.Combat.BuildBuffBehavior()
                    )
                ),

                new SwitchArgument<Styx.WoWSpec>(Styx.WoWSpec.RogueSubtlety,
                    new PrioritySelector(
                        Helpers.Rogue.ApplyPoisons,
                        Context.Subtlety.BuildBuffBehavior()  
                    )
                )

            );
        }
    }
}
