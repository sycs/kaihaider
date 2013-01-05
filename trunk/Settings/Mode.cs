//////////////////////////////////////////////////
//                 Mode.cs                      //
//      Part of RogueRaidBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////


namespace RogueRaidBT.Settings
{
    static class Mode
    {
        static public Helpers.Enum.CooldownUse mCooldownUse { get; set; }
        static public Helpers.Enum.LocationContext mLocationSettings { get; set; }

        static public Helpers.Enum.PoisonSpellId[] mPoisonsMain { get; set; }
        static public Helpers.Enum.PoisonSpellId[] mPoisonsOff { get; set; }

        static public bool[] mUsePoisons { get; set; }

        static public bool mOverrideContext { get; set; }

        static public bool mUseMovement { get; set; }
        static public bool mUseCooldowns { get; set; }
        static public bool mUseAoe { get; set; }
        static public bool mUseCombat { get; set; }
        static public bool mForceBehind { get; set; }

        static Mode()
        {
            mUsePoisons  = new bool[6];
            mPoisonsMain = new Helpers.Enum.PoisonSpellId[6];
            mPoisonsOff  = new Helpers.Enum.PoisonSpellId[6];

            mUsePoisons[(int)Helpers.Enum.LocationContext.Raid] = false;
            mUsePoisons[(int)Helpers.Enum.LocationContext.Arena] = false;
            mUsePoisons[(int)Helpers.Enum.LocationContext.Dungeon] = true;
            mUsePoisons[(int)Helpers.Enum.LocationContext.Battleground] = true;
            mUsePoisons[(int)Helpers.Enum.LocationContext.World] = true;

            for (int i = 1; i < 6; i++)
            {
                mPoisonsMain[i] = Helpers.Enum.PoisonSpellId.Deadly;
                mPoisonsOff[i] = Helpers.Enum.PoisonSpellId.Leeching;
            }

            mCooldownUse = Helpers.Enum.CooldownUse.Always;

            mOverrideContext = false;

            mUseMovement = true;
            mUseCooldowns = true;
            mUseAoe = true;
            mUseCombat = true;
            mForceBehind = false;
        }
    }
}
