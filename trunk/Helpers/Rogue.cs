//////////////////////////////////////////////////
//                 Rogue.cs                     //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Drawing;
using System.Windows.Media;
using System.Linq;
using System;
using System.Collections.Generic;
using CommonBehaviors.Actions;
using Styx;
using Styx.Helpers;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;
using Styx.Common;
using Styx.Pathing;

namespace RogueRaidBT.Helpers
{
    static class Rogue
    {
        static public int mCurrentEnergy { get; private set; }
        static public int mComboPoints { get; private set; }
        static public int mRawComboPoints { get; private set; }
        static public WoWUnit mTarget { get; set; }
        static public double mTargetHP { get; private set; }
        static public double mHP { get; private set; }

        static public bool spamming { get; private set; }

        public static WoWSpec mCurrentSpec { get; private set; }


        static Rogue()
        {
            mCurrentSpec = StyxWoW.Me.Specialization;

        }

        static public void Pulse()
        {
            mCurrentEnergy = GetCurrentEnergyLua();

            mComboPoints = StyxWoW.Me.ComboPoints;
            if(StyxWoW.Me.Combat)
            mRawComboPoints = StyxWoW.Me.RawComboPoints;
            mTarget = StyxWoW.Me.CurrentTarget;
            if (mTarget != null)
                mTargetHP = mTarget.HealthPercent;
            mHP = StyxWoW.Me.HealthPercent;


        }
        
        static public bool CheckSpamLock()
        {
            if(!spamming)
            {
                spamming = true;
                return true;
            }
            return false;
            
        }

        static public bool ReleaseSpamLock()
        {
            spamming = false;
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
                    new Decorator(ret => Helpers.Rogue.mTarget.CanInterruptCurrentSpellCast ,
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
            return (Aura.IsTargetCasting != 0 &&
                    (WoWSpell.FromId(Aura.IsTargetCasting).School == WoWSpellSchool.Holy) ||
                    WoWSpell.FromId(Aura.IsTargetCasting).School == WoWSpellSchool.Nature);
        }


        static public bool IsBehindUnit(WoWUnit unit)
        {
            return Settings.Mode.mForceBehind || unit.MeIsSafelyBehind;
        }

        static public bool IsAoeUsable()
        {
            return Settings.Mode.mUseAoe;
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

        //Stolen from CLU
        /// <summary>
        /// This is meant to replace the 'SleepForLagDuration()' method. Should only be used in a Sequence
        /// </summary>
        public static Composite CreateWaitForLagDuration()
        {
            return new WaitContinue(TimeSpan.FromMilliseconds((StyxWoW.WoWClient.Latency * 2) + 150), ret => false, new ActionAlwaysSucceed());
        }

        // stolen from clu
        public static Composite ApplyPosions
        {
            get
            {
                return new PrioritySelector
                        (new Decorator
                             (ret => Aura.NeedsPoison && !(Aura.Wound || Aura.Deadly) && (bool)Settings.Mode.mUsePoisons[(int)Area.mLocation] && StyxWoW.Me != null &&
                           SpellManager.HasSpell((int)Settings.Mode.mPoisonsMain[(int)Area.mLocation]),
                              new Sequence
                                  (new Action
                                       (ret =>
                                        Logging.Write
                                            ("Applying {0} to main hand", Settings.Mode.mPoisonsMain[(int)Area.mLocation])),
                                   new Action(ret => Navigator.PlayerMover.MoveStop()),
                                   CreateWaitForLagDuration(),
                                   new Action(ret => SpellManager.CastSpellById((uint)Settings.Mode.mPoisonsMain[(int)Area.mLocation])),
                                   CreateWaitForLagDuration(),
                                   new WaitContinue(2, ret => StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                                   new WaitContinue(10, ret => !StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                                   new WaitContinue(1, ret => false, new ActionAlwaysSucceed()))),
                         new Decorator
                             (ret => Aura.NeedsPoison && !(Aura.MindNumbing || Aura.Crippling || Aura.Paralytic || Aura.Leeching) && (bool)Settings.Mode.mUsePoisons[(int)Area.mLocation] && StyxWoW.Me != null &&
                              SpellManager.HasSpell((int)Settings.Mode.mPoisonsOff[(int)Area.mLocation]),
                              new Sequence
                                  (new Action
                                       (ret =>
                                       Logging.Write("Applying {0} to off hand", Settings.Mode.mPoisonsOff[(int)Area.mLocation])),
                                   new Action(ret => Navigator.PlayerMover.MoveStop()),
                                   CreateWaitForLagDuration(),
                                   new Action(ret => SpellManager.CastSpellById((uint)Settings.Mode.mPoisonsOff[(int)Area.mLocation])),
                                   CreateWaitForLagDuration(),
                                   new WaitContinue(2, ret => StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                                   new WaitContinue(10, ret => !StyxWoW.Me.IsCasting, new ActionAlwaysSucceed()),
                                   new WaitContinue(1, ret => false, new ActionAlwaysSucceed()))));
            }
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

        
        
    }
}
