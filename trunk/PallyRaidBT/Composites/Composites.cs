//////////////////////////////////////////////////
//                 Composites.cs                //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using TreeSharp;

namespace PallyRaidBT.Composites
{
    class Composites
    {
        static public Composite BuildCombatBehavior()
        {
            return new Switch<Helpers.Enumeration.TalentTrees>(ret => Helpers.Pally.mCurrentSpec,

                new SwitchArgument<Helpers.Enumeration.TalentTrees>(Helpers.Enumeration.TalentTrees.None,
                    Context.None.BuildCombatBehavior()
                ),

                new SwitchArgument<Helpers.Enumeration.TalentTrees>(Helpers.Enumeration.TalentTrees.Retribution,
                    Context.Retribution.BuildCombatBehavior()
                ),

                new SwitchArgument<Helpers.Enumeration.TalentTrees>(Helpers.Enumeration.TalentTrees.Holy,
                    Context.Holy.BuildCombatBehavior()
                ),

                new SwitchArgument<Helpers.Enumeration.TalentTrees>(Helpers.Enumeration.TalentTrees.Protection,
                    Context.Protection.BuildCombatBehavior()
                )

            );
        }

        static public Composite BuildPullBehavior()
        {
            return new Switch<Helpers.Enumeration.TalentTrees>(ret => Helpers.Pally.mCurrentSpec,

                new SwitchArgument<Helpers.Enumeration.TalentTrees>(Helpers.Enumeration.TalentTrees.None,
                    Context.None.BuildPullBehavior()
                ),

                new SwitchArgument<Helpers.Enumeration.TalentTrees>(Helpers.Enumeration.TalentTrees.Retribution,
                    Context.Retribution.BuildPullBehavior()
                ),

                new SwitchArgument<Helpers.Enumeration.TalentTrees>(Helpers.Enumeration.TalentTrees.Holy,
                    Context.Holy.BuildPullBehavior()
                ),

                new SwitchArgument<Helpers.Enumeration.TalentTrees>(Helpers.Enumeration.TalentTrees.Protection,
                    Context.Protection.BuildPullBehavior()
                )

            );
        }

        static public Composite BuildBuffBehavior()
        {
            return new Switch<Helpers.Enumeration.TalentTrees>(ret => Helpers.Pally.mCurrentSpec,

                new SwitchArgument<Helpers.Enumeration.TalentTrees>(Helpers.Enumeration.TalentTrees.None,
                    new Action(ret => RunStatus.Failure)
                ),

                new SwitchArgument<Helpers.Enumeration.TalentTrees>(Helpers.Enumeration.TalentTrees.Retribution,
                    new PrioritySelector(
                        //Helpers.Pally.ApplyPosions(),
                        Context.Retribution.BuildBuffBehavior()
                    )
                ),

                new SwitchArgument<Helpers.Enumeration.TalentTrees>(Helpers.Enumeration.TalentTrees.Holy,
                    new PrioritySelector(
                        //Helpers.Rogue.ApplyPosions(),
                        Context.Holy.BuildBuffBehavior()
                    )
                ),

                new SwitchArgument<Helpers.Enumeration.TalentTrees>(Helpers.Enumeration.TalentTrees.Protection,
                    new PrioritySelector(
                        //Helpers.Rogue.ApplyPosions(),
                        Context.Protection.BuildBuffBehavior()
                    )
                )

            );
        }
    }
}
