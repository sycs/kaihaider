//////////////////////////////////////////////////
//                 Rogue.cs                     //
//      Part of RogueBT by kaihaider        //
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

namespace RogueBT.Helpers
{
    static class Rogue
    {
        static public int mCurrentEnergy { get; private set; }
        static public int mComboPoints { get; private set; }
        static public int mRawComboPoints { get; private set; }
        static public WoWUnit mTarget { get; set; }
        static public LocalPlayer me { get; set; }
        static public double mTargetHP { get; private set; }
        static public double mHP { get; private set; }
        
        static public bool spamming { get; private set; }
        static public bool haveSapped { get; private set; }
        static public int pickCount { get; set; }
        static public int running { get; set; }

        public static WoWSpec mCurrentSpec { get; private set; }


        static Rogue()
        {
            pickCount = 3;
            if (Helpers.Rogue.me == null && StyxWoW.Me != null)
                Helpers.Rogue.me = StyxWoW.Me;
            if (Helpers.Rogue.me != null)
            {
               

                mCurrentSpec = Helpers.Rogue.me.Specialization;

                Lua.Events.AttachEvent("CHARACTER_POINTS_CHANGED", delegate
                {
                    Logging.Write(LogLevel.Normal, "Your spec has been updated. Rebuilding behaviors...");
                    Helpers.Rogue.CreateWaitForLagDuration();
                    Helpers.Rogue.CreateWaitForLagDuration();
                    Helpers.Rogue.CreateWaitForLagDuration();
                    Helpers.Rogue.CreateWaitForLagDuration();
                    mCurrentSpec = Helpers.Rogue.me.Specialization;
                }
                );

                Lua.Events.AttachEvent("ACTIVE_TALENT_GROUP_CHANGED", delegate
                {
                    Logging.Write(LogLevel.Normal, "Your spec has changed. Rebuilding behaviors...");
                    Helpers.Rogue.CreateWaitForLagDuration();
                    Helpers.Rogue.CreateWaitForLagDuration();
                    Helpers.Rogue.CreateWaitForLagDuration();
                    Helpers.Rogue.CreateWaitForLagDuration();
                    mCurrentSpec = Helpers.Rogue.me.Specialization;

                    if (Helpers.Rogue.me.Inventory.Equipped.MainHand != null && !Helpers.Rogue.me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass.Equals(WoWItemWeaponClass.Dagger) && !Helpers.Rogue.me.Specialization.Equals(Styx.WoWSpec.RogueCombat)) Logging.Write(LogLevel.Normal, "No dagger in MainHand!!! Only Combat supports none dagger weapons!");
                }
                );


                if (Helpers.Rogue.me.Inventory.Equipped.MainHand != null && !Helpers.Rogue.me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass.Equals(WoWItemWeaponClass.Dagger) && !Helpers.Rogue.me.Specialization.Equals(Styx.WoWSpec.RogueCombat)) Logging.Write(LogLevel.Normal, "No dagger in MainHand!!! Only Combat supports none dagger weapons!");
            }
        }

        static public void Pulse()
        {
            

            mCurrentEnergy = GetCurrentEnergyLua();
            if (Helpers.Rogue.me == null)
            {
                mRawComboPoints = 0;
                mComboPoints = 0;
            }
            else
            {
                mComboPoints = Helpers.Rogue.me.ComboPoints;

                if (Helpers.Rogue.me.Combat)
                    mRawComboPoints = Helpers.Rogue.me.RawComboPoints;

                mTarget = Helpers.Rogue.me.CurrentTarget;
                if (mTarget != null && !mTarget.IsAlive) mTarget = null;

                mHP = Helpers.Rogue.me.HealthPercent;
            }
            if (mTarget != null)
                mTargetHP = mTarget.HealthPercent;


        }

        static public Composite Distract()
        {
            return new Decorator(ret => Helpers.Rogue.mTarget.Distance < 10 && Helpers.Spells.CanCast("Distract") && CheckRunning(),
                    new Sequence(
                //Pick Pocket's Window - Is Avoiding Wait Possible?
                //Styx.CommonBot.LootTargeting.LootFrameIsOpen,
                //new Action(ret => Styx.CommonBot.Frames.LootFrame.Instance.LootAll())),
                        new Action(ret =>
                        {
                            
                            Styx.CommonBot.SpellManager.Cast("Distract", Helpers.Rogue.mTarget);
                            ///Styx.CommonBot.SpellManager.ClickRemoteLocation(Helpers.Rogue.mTarget.Location);
                            Styx.Common.Logging.Write(Styx.Common.LogLevel.Diagnostic, "Distract");
                        }),
                        new Action(ret =>Styx.CommonBot.SpellManager.ClickRemoteLocation(Helpers.Rogue.mTarget.Location))
                    )
                );

        }

        static public Composite PickPocket()
        {
            return new Decorator(ret => Settings.Mode.mPickPocket
                    && Helpers.Aura.Stealth && Helpers.Movement.IsInAttemptMeleeRange  //Helpers.Rogue.mTarget.Distance < 10 
                    && Styx.CommonBot.SpellManager.HasSpell("Pick Pocket"),
                    new Sequence(
                        new DecoratorContinue(ret => Settings.Mode.mSWPick && Helpers.Rogue.mTarget.Distance < 10 && !Helpers.Rogue.me.MovementInfo.IsStrafing,
                            new Action(ret => Styx.Pathing.Navigator.PlayerMover.MoveStop())),
                //Pick Pocket's Window - Is Avoiding Wait Possible?
                //Styx.CommonBot.LootTargeting.LootFrameIsOpen,
                //new Action(ret => Styx.CommonBot.Frames.LootFrame.Instance.LootAll())),
                        new Action(ret =>
                        {
                            Helpers.Rogue.pickCount--;
                            Styx.CommonBot.SpellManager.Cast("Pick Pocket", Helpers.Rogue.mTarget);
                            Styx.Common.Logging.Write(Styx.Common.LogLevel.Diagnostic, "Pick Pocket attempted");
                            return RunStatus.Success;
                        }),
                        new Decorator(ret => !Helpers.Rogue.me.Combat && Settings.Mode.mSWPick && Helpers.Rogue.pickCount > 0,
                            new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret => false, new ActionAlwaysSucceed())),
                        new Decorator(ret => !Helpers.Rogue.me.Combat && Settings.Mode.mSWPick && Helpers.Rogue.pickCount > 0,
                            new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret => false, new ActionAlwaysSucceed())),
                        new Decorator(ret => !Helpers.Rogue.me.Combat && Settings.Mode.mSWPick && Helpers.Rogue.pickCount > 0,
                            new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret => false, new ActionAlwaysSucceed())),
                        new Decorator(ret => !Helpers.Rogue.me.Combat && Settings.Mode.mSWPick && Helpers.Rogue.pickCount > 0,
                            new WaitContinue(System.TimeSpan.FromMilliseconds(500), ret => false, new ActionAlwaysSucceed()))
                    )
                );

        }

        static public bool CheckSpamLock()
        {
            if(!spamming)
            {
                mRawComboPoints = 0;
                spamming = true;
                return true;
            }
            return false;
            
        }

        static public bool SapLock()
        {
            if (!haveSapped)
            {
                haveSapped = true;
                return true;
            }
            return false;

        }

        static public bool CheckRunning()
        {
            if (running > 0)
            {
                running--;
                return false;
            }
            return true;

        }

        static public bool ReleaseSpamLock()
        {
            running = 30;
            pickCount = 3;
            haveSapped = false;
            spamming = false;
            return true;
        }


        static public bool IsInterruptUsable()
        {
            if (Helpers.Rogue.me.CurrentTarget != null)
                return Helpers.Rogue.me.CurrentTarget.IsCasting &&
                    Helpers.Rogue.me.CurrentTarget.CurrentCastTimeLeft.TotalSeconds <= 0.6 &&
                    Helpers.Rogue.me.CurrentTarget.CurrentCastTimeLeft.TotalSeconds >= 0.1 &&
                    Helpers.Rogue.me.CurrentTarget.CanInterruptCurrentSpellCast;//&& 
                    //((Helpers.Rogue.me.CurrentTarget.CastingSpell.School == WoWSpellSchool.Holy ||
                    //Helpers.Rogue.me.CurrentTarget.CastingSpell.School == WoWSpellSchool.Nature) ||
                    //(Helpers.Rogue.me.CurrentTarget.CastingSpellId ==  ||
                    //Helpers.Rogue.me.CurrentTarget.CastingSpellId == ));
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
                                Helpers.Spells.CastCooldown("Kidney Shot", ret => Helpers.Rogue.me.ComboPoints > 1 &&
                                    !Helpers.Aura.IsTargetImmuneStun)

                                )),

                    Helpers.Spells.CastCooldown("Gouge", ret => 
                                Helpers.Rogue.mTarget.IsSafelyFacing(Helpers.Rogue.me)),
                                
                    Helpers.Spells.CastCooldown("Deadly Throw", ret => Helpers.Rogue.mTarget.CanInterruptCurrentSpellCast
                               && Helpers.Rogue.mComboPoints > 4 && Helpers.Rogue.mTarget.Distance > 5)
                                )
            );
        }

        static public Composite TryToInterruptFocus(CanRunDecoratorDelegate cond)
        {
            return new Decorator(cond,
                 new PrioritySelector(
                         Helpers.Spells.CastFocusRaw("Kick", ret => !Helpers.Aura.ShadowDance && Helpers.Rogue.mTarget.CanInterruptCurrentSpellCast),

                         Helpers.Spells.CastFocusRaw("Gouge", ret => !Helpers.Aura.ShadowDance && Helpers.Focus.rawFocusTarget.IsSafelyFacing(Helpers.Rogue.me)),

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

        static public bool IsAoeUsable()
        {
            return Settings.Mode.mUseAoe;
        }

        static public bool IsCloakUsable()
        {
            return Target.mNearbyEnemyUnits.Any(unit => unit.CurrentTarget != null &&
                                                        unit.CurrentTargetGuid == Helpers.Rogue.me.Guid &&
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

                        return Helpers.Rogue.me.FocusedUnit != null && Helpers.Rogue.me.FocusedUnit.Guid == Helpers.Rogue.me.CurrentTarget.Guid &&
                               !Helpers.Rogue.me.FocusedUnit.IsFriendly;

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
        public static Composite ApplyPoisons
        {
            get
            {
                return new PrioritySelector
                        (new Decorator
                             (ret => Helpers.Rogue.me != null && !Helpers.Rogue.me.IsMoving && !Helpers.Rogue.me.Mounted
                                 
                                 && Aura.NeedsPoison && !(Aura.Wound || Aura.Deadly) && Helpers.Rogue.mHP > 40
                                     && (bool)Settings.Mode.mUsePoisons[(int)Area.mLocation] && 
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
                                   new WaitContinue(2, ret => Helpers.Rogue.me.IsCasting, new ActionAlwaysSucceed()),
                                   new WaitContinue(10, ret => !Helpers.Rogue.me.IsCasting, new ActionAlwaysSucceed()),
                                   new WaitContinue(1, ret => false, new ActionAlwaysSucceed()))),
                         new Decorator
                             (ret => Helpers.Rogue.me != null && !Helpers.Rogue.me.IsMoving && !Helpers.Rogue.me.Mounted && Helpers.Rogue.mHP > 30
                                 && Aura.NeedsPoison && !(Aura.MindNumbing || Aura.Crippling || Aura.Paralytic || Aura.Leeching)
                                 && (bool)Settings.Mode.mUsePoisons[(int)Area.mLocation] && Helpers.Rogue.me != null &&
                              SpellManager.HasSpell((int)Settings.Mode.mPoisonsOff[(int)Area.mLocation]),
                              new Sequence
                                  (new Action
                                       (ret =>
                                       Logging.Write("Applying {0} to off hand", Settings.Mode.mPoisonsOff[(int)Area.mLocation])),
                                   new Action(ret => Navigator.PlayerMover.MoveStop()),
                                   CreateWaitForLagDuration(),
                                   new Action(ret => SpellManager.CastSpellById((uint)Settings.Mode.mPoisonsOff[(int)Area.mLocation])),
                                   CreateWaitForLagDuration(),
                                   new WaitContinue(2, ret => Helpers.Rogue.me.IsCasting, new ActionAlwaysSucceed()),
                                   new WaitContinue(10, ret => !Helpers.Rogue.me.IsCasting, new ActionAlwaysSucceed()),
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
