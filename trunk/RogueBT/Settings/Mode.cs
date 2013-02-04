//////////////////////////////////////////////////
//                 Mode.cs                      //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////


namespace RogueBT.Settings
{
    static class Mode
    {
        static public Helpers.Enum.CooldownUse mCooldownUse { get; set; }
        static public Helpers.Enum.LocationContext mLocationSettings { get; set; }
        static public Helpers.Enum.Saps mSap { get; set; }

        static public Helpers.Enum.LeathalPoisonSpellId[] mPoisonsMain { get; set; }
        static public Helpers.Enum.NonLeathalPoisonSpellId[] mPoisonsOff { get; set; }

        static public bool[] mUsePoisons { get; set; }

        static public bool mOverrideContext { get; set; }

        static public bool mUseCooldowns { get; set; }
        static public bool mUseCombat { get; set; }
        static public bool mForceBehind { get; set; }
        static public bool mTargeting { get; set; }
        static public bool mUseMovement { get; set; }
        static public bool mMoveBehind { get; set; }
        static public bool mMoveBackwards { get; set; }
        static public bool mAlwaysStealth { get; set; }
        static public bool mNeverStealth { get; set; }
        static public bool mUseAoe { get; set; }
        static public bool mCrowdControl { get; set; }
        static public bool mPickPocket { get; set; }
        static public bool mSWPick { get; set; }
        static public bool mFeint { get; set; }
        static public bool mFoKPull { get; set; }

        static Mode()
        {
            mUsePoisons  = new bool[6];
            mPoisonsMain = new Helpers.Enum.LeathalPoisonSpellId[6];
            mPoisonsOff = new Helpers.Enum.NonLeathalPoisonSpellId[6];

            mUsePoisons[(int)Helpers.Enum.LocationContext.Raid] = true;
            mUsePoisons[(int)Helpers.Enum.LocationContext.Arena] = true;
            mUsePoisons[(int)Helpers.Enum.LocationContext.Dungeon] = true;
            mUsePoisons[(int)Helpers.Enum.LocationContext.Battleground] = true;
            mUsePoisons[(int)Helpers.Enum.LocationContext.World] = true;

            for (int i = 1; i < 6; i++)
            {
                mPoisonsMain[i] = Helpers.Enum.LeathalPoisonSpellId.Deadly;
                mPoisonsOff[i] = Helpers.Enum.NonLeathalPoisonSpellId.Leeching;
            }

            mCooldownUse = Helpers.Enum.CooldownUse.Always;
            mSap = Helpers.Enum.Saps.Adds;

            mOverrideContext = false;

            mUseCooldowns = true;
            mUseCombat = true;
            mForceBehind = false;
            mTargeting = true;
            mUseMovement = true;
            mMoveBehind = true;
            mMoveBackwards = true;
            mAlwaysStealth = false;
            mNeverStealth = false;
            mUseAoe = true;
            mCrowdControl = true;
            mPickPocket = true;
            mFoKPull = false;
        }
    }
}
