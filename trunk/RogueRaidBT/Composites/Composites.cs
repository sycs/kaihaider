//////////////////////////////////////////////////
//               Composites.cs                  //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using TreeSharp;

namespace RogueRaidBT.Composites
{
    class Composites
    {
        static public Composite BuildCombatBehavior()
        {
            return new Switch<Helpers.Enum.TalentTrees>(ret => Helpers.Rogue.mCurrentSpec,

                new SwitchArgument<Helpers.Enum.TalentTrees>(Helpers.Enum.TalentTrees.None,
                    Context.None.BuildCombatBehavior()
                ),

                new SwitchArgument<Helpers.Enum.TalentTrees>(Helpers.Enum.TalentTrees.Assassination,
                    Context.Assassination.BuildCombatBehavior()
                ),

                new SwitchArgument<Helpers.Enum.TalentTrees>(Helpers.Enum.TalentTrees.Combat,
                    Context.Combat.BuildCombatBehavior()
                ),

                new SwitchArgument<Helpers.Enum.TalentTrees>(Helpers.Enum.TalentTrees.Subtlety,
                    Context.Subtlety.BuildCombatBehavior()
                )

            );
        }

        static public Composite BuildPullBehavior()
        {
            return new Switch<Helpers.Enum.TalentTrees>(ret => Helpers.Rogue.mCurrentSpec,

                new SwitchArgument<Helpers.Enum.TalentTrees>(Helpers.Enum.TalentTrees.None,
                    Context.None.BuildPullBehavior()
                ),

                new SwitchArgument<Helpers.Enum.TalentTrees>(Helpers.Enum.TalentTrees.Assassination,
                    Context.Assassination.BuildPullBehavior()
                ),

                new SwitchArgument<Helpers.Enum.TalentTrees>(Helpers.Enum.TalentTrees.Combat,
                    Context.Combat.BuildPullBehavior()
                ),

                new SwitchArgument<Helpers.Enum.TalentTrees>(Helpers.Enum.TalentTrees.Subtlety,
                    Context.Subtlety.BuildPullBehavior()
                )

            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Switch<Helpers.Enum.TalentTrees>(ret => Helpers.Rogue.mCurrentSpec,

                new SwitchArgument<Helpers.Enum.TalentTrees>(Helpers.Enum.TalentTrees.None,
                    new Action(ret => RunStatus.Failure)
                ),

                new SwitchArgument<Helpers.Enum.TalentTrees>(Helpers.Enum.TalentTrees.Assassination,
                    new PrioritySelector(
                        Helpers.Rogue.ApplyPosions(),
                        Context.Assassination.BuildBuffBehavior()
                    )
                ),

                new SwitchArgument<Helpers.Enum.TalentTrees>(Helpers.Enum.TalentTrees.Combat,
                    new PrioritySelector(
                        Helpers.Rogue.ApplyPosions(),
                        Context.Combat.BuildBuffBehavior()
                    )
                ),

                new SwitchArgument<Helpers.Enum.TalentTrees>(Helpers.Enum.TalentTrees.Subtlety,
                    new PrioritySelector(
                        Helpers.Rogue.ApplyPosions(),
                        Context.Subtlety.BuildBuffBehavior()
                    )
                )

            );
        }
    }
}
