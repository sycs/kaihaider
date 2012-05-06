//////////////////////////////////////////////////
//                 Rogue.cs                     //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Drawing;
using System.Linq;
using System;
using System.Collections.Generic;
using CommonBehaviors.Actions;
using Styx;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.Logic.Pathing;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace RogueRaidBT.Helpers
{
    static class Rogue
    {
        static public int mCurrentEnergy { get; private set; }
        static public int mComboPoints { get; private set; }
        static public int mRawComboPoints { get; private set; }
        static public WoWUnit mTarget { get; private set; }
        static public double mTargetHP { get; private set; }
        static public double mHP { get; private set; }

        static public Enum.TalentTrees mCurrentSpec { get; private set; }

        static Rogue()
        {
            Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", delegate
                {
                    Logging.Write(Color.Orange, "Your spec has been updated. Rebuilding behaviors...");
                    mCurrentSpec = GetCurrentSpecLua();
                }
            );

            Lua.Events.AttachEvent("ACTIVE_TALENT_GROUP_CHANGED", delegate
                {
                    Logging.Write(Color.Orange, "Your spec has changed. Rebuilding behaviors...");
                    mCurrentSpec = GetCurrentSpecLua();
                }
            );

            mCurrentSpec = GetCurrentSpecLua();
        }

        static public void Pulse()
        {
            mCurrentEnergy = GetCurrentEnergyLua();


            mComboPoints = StyxWoW.Me.ComboPoints;
            mRawComboPoints = StyxWoW.Me.RawComboPoints;
            mTarget = StyxWoW.Me.CurrentTarget;
            if (mTarget != null)
                mTargetHP = mTarget.HealthPercent;
            mHP = StyxWoW.Me.HealthPercent;
        }

        static public bool ResetRawComboPoints()
        {
            mRawComboPoints = 0;
            return true;
        }

        static public bool IsInterruptUsable()
        {
            if (StyxWoW.Me.CurrentTarget != null)
                return StyxWoW.Me.CurrentTarget.IsCasting &&
                    StyxWoW.Me.CurrentTarget.CurrentCastTimeLeft.TotalSeconds <= 0.6 &&
                    StyxWoW.Me.CurrentTarget.CurrentCastTimeLeft.TotalSeconds >= 0.1 &&
                    StyxWoW.Me.CurrentTarget.CanInterruptCurrentSpellCast;//&& 
                    //((StyxWoW.Me.CurrentTarget.CastingSpell.School == WoWSpellSchool.Holy ||
                    //StyxWoW.Me.CurrentTarget.CastingSpell.School == WoWSpellSchool.Nature) ||
                    //(StyxWoW.Me.CurrentTarget.CastingSpellId ==  ||
                    //StyxWoW.Me.CurrentTarget.CastingSpellId == ));
            return false;
        }

        static public Composite TryToInterrupt(CanRunDecoratorDelegate cond)
        {
            return new Decorator(cond,
                new PrioritySelector(
                    new Decorator(ret => Helpers.Rogue.mTarget.CanInterruptCurrentSpellCast,
                        new PrioritySelector(
                            Helpers.Spells.CastCooldown("Kick", ret => true)

                                )),

                    new Decorator(ret => !Helpers.Rogue.mTarget.CanInterruptCurrentSpellCast,
                            new PrioritySelector(
                                Helpers.Spells.CastCooldown("Kidney Shot", ret => Helpers.Focus.rawFocusTarget != null &&
                                Helpers.Focus.rawFocusTarget == Helpers.Rogue.mTarget && StyxWoW.Me.ComboPoints > 1 &&
                                    !Helpers.Aura.IsTargetImmuneStun)

                                )),

                    Helpers.Spells.CastCooldown("Gouge", ret => Helpers.Focus.rawFocusTarget != null &&
                                Helpers.Focus.rawFocusTarget == Helpers.Rogue.mTarget &&
                                Helpers.Rogue.mCurrentEnergy > 40 && Helpers.Rogue.IsHolyOrNat() &&
                                Helpers.Rogue.mTarget.IsSafelyFacing(StyxWoW.Me)))
            );
        }

        static public Composite TryToInterruptFocus(CanRunDecoratorDelegate cond)
        {
            return new Decorator(cond,
                 new PrioritySelector(
                         Helpers.Spells.CastFocusRaw("Kick", ret => !Helpers.Aura.ShadowDance && Helpers.Rogue.mTarget.CanInterruptCurrentSpellCast),

                         Helpers.Spells.CastFocusRaw("Gouge", ret => !Helpers.Aura.ShadowDance && Helpers.Focus.rawFocusTarget.IsSafelyFacing(StyxWoW.Me)),

                         Helpers.Spells.CastFocusRaw("Cheap Shot", ret => !Helpers.Aura.IsTargetImmuneStun && (Helpers.Aura.Stealth || Helpers.Aura.Vanish || Helpers.Aura.ShadowDance))
                 )
             );
        }
        static public bool IsHolyOrNat()
        {
            return (StyxWoW.Me.CurrentTarget.CastingSpell.School == WoWSpellSchool.Holy ||
                    StyxWoW.Me.CurrentTarget.CastingSpell.School == WoWSpellSchool.Nature);
        }


        static public bool IsBehindUnit(WoWUnit unit)
        {
            return Settings.Mode.mForceBehind || unit.MeIsBehind;
        }

        static public bool IsAoeUsable()
        {
            return Settings.Mode.mUseAoe && IsThrowingItemEquipped();
        }

        static public bool IsCloakUsable()
        {
            return Target.mNearbyEnemyUnits.Any(unit => unit.CurrentTarget != null &&
                                                        unit.CurrentTarget.Guid == StyxWoW.Me.Guid &&
                                                        unit.IsCasting &&
                                                        unit.CurrentCastTimeLeft.TotalSeconds <= 0.5 &&
                                                        (!unit.IsWithinMeleeRange ||
                                                        (Spells.GetSpellCooldown("Kick") > 0 && 
                                                        Spells.GetSpellCooldown("Kidney Shot") > 0)));
        }

        static public bool IsCooldownsUsable()
        {
            if (Settings.Mode.mUseCooldowns)
            {
                switch (Settings.Mode.mCooldownUse)
                {
                    case Enum.CooldownUse.Always:

                        return true;

                    case Enum.CooldownUse.ByFocus:

                        return StyxWoW.Me.FocusedUnit != null && StyxWoW.Me.FocusedUnit.Guid == StyxWoW.Me.CurrentTarget.Guid &&
                               !StyxWoW.Me.FocusedUnit.IsFriendly;

                    case Enum.CooldownUse.OnlyOnBosses:

                        return Area.IsCurTargetSpecial();
                }
            }

            return false;
        }

        static public Composite ApplyPosions()
        {
            return new Decorator(ret => Settings.Mode.mUsePoisons[(int) Area.mLocation],
                new PrioritySelector(
                    ApplyPoisonToItem(StyxWoW.Me.Inventory.Equipped.MainHand, ret => (uint) Settings.Mode.mPoisonsMain[(int) Area.mLocation]),
                    ApplyPoisonToItem(StyxWoW.Me.Inventory.Equipped.OffHand,  ret => (uint) Settings.Mode.mPoisonsOff[(int) Area.mLocation])
                )
            );
        }

        static private Composite ApplyPoisonToItem(WoWItem item, PoisonDelegate poison)
        {
            return new Decorator(ret => !IsPoisonApplied(item) && IsPoisonInInventory(poison(ret)),
                new Sequence(
                    new Action(ret =>
                        {
                            Logging.Write(Color.Orange, "Applying " + (Enum.PoisonSpellId) poison(ret) + " to " + item.Name);
                            Navigator.PlayerMover.MoveStop();
                        }
                    ),

                    new WaitContinue(TimeSpan.FromSeconds(0.5), ret => false, new ActionAlwaysSucceed()),

                    new Action(ret =>
                        {
                            var thePoison = StyxWoW.Me.BagItems.First(inventoryItem => inventoryItem.ItemInfo.Id == poison(ret));

                            thePoison.Interact();
                            item.Interact();          
                        }),

                    new WaitContinue(TimeSpan.FromSeconds(5), ret => false, new ActionAlwaysSucceed())
                )
            );

        }

        static private bool IsPoisonApplied(WoWItem item)
        {
            return item != null && item.TemporaryEnchantment.Id != 0;
        }

        static private bool IsPoisonInInventory (uint poisonId)
        {
            return StyxWoW.Me.BagItems.Any(inventoryItem => inventoryItem != null && inventoryItem.ItemInfo.Id == poisonId);
        }

        static private bool IsThrowingItemEquipped()
        {
            return true; //StyxWoW.Me.Inventory.Equipped.Ranged.IsThrownWeapon;
        }
        
        // Used the fix the slow resource updating in Honorbuddy.
        // Still required as of HB build 5842
        static private int GetCurrentEnergyLua()
        {
            return Lua.GetReturnVal<int>("return UnitMana(\"player\");", 0);
        }

        // Returns the index of the current active dual spec -- first or second.
        static private int GetSpecGroupLua()
        {
            return Lua.GetReturnVal<int>("return GetActiveTalentGroup(false, false)", 0);
        }

        static private Enum.TalentTrees GetCurrentSpecLua()
        {
            int group = GetSpecGroupLua();

            var pointsSpent = new int[3];
           
            for (int tab = 1; tab <= 3; tab++)
            {
                List<string> talentTabInfo = Lua.GetReturnValues("return GetTalentTabInfo(" + tab + ", false, false, " + group + ")");
                pointsSpent[tab - 1] = Convert.ToInt32(talentTabInfo[4]);
            }

            if (pointsSpent[0] > (pointsSpent[1] + pointsSpent[2]))
            {
                return Enum.TalentTrees.Assassination;
            }

            if (pointsSpent[1] > (pointsSpent[0] + pointsSpent[2])) 
            {
                return Enum.TalentTrees.Combat;
            }

            if (pointsSpent[2] > (pointsSpent[0] + pointsSpent[1]))
            {
                return Enum.TalentTrees.Subtlety;
            }

            return Enum.TalentTrees.None;
        }
    }
}
