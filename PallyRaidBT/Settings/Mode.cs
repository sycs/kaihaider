//////////////////////////////////////////////////
//                 Mode.cs                      //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Linq;
using Styx;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace PallyRaidBT.Settings
{
    class Mode
    {
        public enum Modes
        {
            Auto,
            Raid,
            Arena,
            Dungeon,
            PvPMoveOff,
            PvPMoveOn,
            Level
        }

        public enum CooldownUse
        {
            Always = 0,
            ByFocus,
            OnlyOnBosses,
            Never
        }

        static private LocalPlayer Me { get { return StyxWoW.Me; } }

        static public Modes mCurMode { get; set; }
        static public CooldownUse mCooldownUse { get; set; }

        static public bool mUseCooldowns { get; set; }
        static public bool mUseAoe { get; set; }
        static public bool mUseCombat { get; set; }
        static public bool mForceBehind { get; set; }

        static public void Init()
        {
            mCurMode = Modes.Auto;
            mCooldownUse = CooldownUse.OnlyOnBosses;

            mUseCooldowns = true;
            mUseAoe = false;
            mUseCombat = true;
            mForceBehind = false;
        }

        static public bool IsCurTargetSpecial()
        {
            bool isTargetBoss = Me.CurrentTarget.CreatureRank == WoWUnitClassificationType.WorldBoss;

            if (!Me.IsInRaid)
            {
                if (isTargetBoss || (Me.CurrentTarget.Level == 87 && Me.CurrentTarget.Elite) ||
                   (Me.CurrentTarget.Level == 88))
                {
                    return true;
                }
            }
            else
            {
                if (isTargetBoss || (Me.CurrentTarget.Level == 88 && Me.CurrentTarget.Elite))
                {
                    return true;
                }
            }

            return false;
        }

        static public bool ShouldUseCooldowns()
        {
            if (mUseCooldowns)
            {
                switch (mCooldownUse)
                {
                    case CooldownUse.Always:

                        return true;

                    case CooldownUse.ByFocus:

                        return Helpers.Focus.mFocusTarget != null && Helpers.Focus.mFocusTarget.Guid == Me.CurrentTarget.Guid &&
                               !Helpers.Focus.mFocusTarget.IsFriendly;

                    case CooldownUse.OnlyOnBosses:

                        return IsCurTargetSpecial();

                }
            }

            return false;
        }

        static public bool ShouldAoe(int num)
        {
            if (mUseAoe)
            {
                if (ObjectManager.GetObjectsOfType<WoWUnit>(true, false).Count(unit => unit.Distance2D <= 8 && !unit.Dead && !unit.IsFriendly && !unit.IsNonCombatPet) >= num)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
