

using System.Linq;
using System.Windows.Media;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.Common;

namespace RogueBT.Helpers
{
    class Aura
    {
        static public bool Deadly { get; private set; }
        static public bool Wound { get; private set; }
        static public bool MindNumbing { get; private set; }
        static public bool Crippling { get; private set; }
        static public bool Paralytic { get; private set; }
        static public bool Leeching { get; private set; }
        //Rogue
        static public bool Feint { get; private set; }
        static public bool Stealth { get; private set; }
        static public bool Vanish { get; private set; }
        static public bool Recuperate { get; private set; }
        static public double TimeRecuperate { get; private set; }
        static public bool SliceandDice { get; private set; }
        static public double TimeSliceandDice { get; private set; }
        static public bool Tricks { get; private set; }
        static public bool DeadlyThrow { get; private set; }
        static public bool ShouldShiv { get; private set; }

        //Legendary
        static public bool FuryoftheDestroyer { get; private set; }

        //Ass
        static public bool Vendetta { get; private set; }
        static public double TimeVendetta { get; private set; }
        static public bool ColdBlood { get; private set; }
        static public bool Envenom { get; private set; }
        static public bool Blindside { get; private set; }

        //static public bool MasterOfSubtlety { get; private set; }
        static public bool ShadowDance { get; private set; }
        static public double TimeHemorrhage { get; private set; }
        static public bool FindWeakness { get; private set; }
        static public bool Rupture { get; private set; }
        static public double TimeRupture { get; private set; }
        static public bool DeadlyPoison { get; private set; }
        static public bool CripplingPoison { get; private set; }
        static public bool LeechingPoison { get; private set; }

        //Combat
        static public bool RevealingStrike { get; private set; }
        static public bool ModerateInsight { get; private set; }
        static public bool DeepInsight { get; private set; }
        static public bool AdrenalineRush { get; private set; }
        static public bool BladeFlurry { get; private set; }

        //Target
        static public bool IsInSafeMeleeRange { get; private set; }
        static public bool IsTargetDisoriented { get; private set; }
        static public bool IsTargetInvulnerable { get; private set; }
        static public bool IsTargetInterrupted { get; private set; }
        static public bool IsTargetSapped { get; private set; }
        static public bool IsTargetImmuneStun { get; private set; }
        static public bool IsTargetImmuneSilence { get; private set; }
        static public int IsTargetCasting { get; private set; }

        static public bool IsBehind { get; private set; }
        static public bool LastDirection { get;  set; }
        static public bool FaerieFire { get; private set; }
        static public bool HealingGhost { get; private set; }
        
        static public float LastRenderFacing { get; set; }

        static public double KidneyTime { get; private set; }

        //stolen from clu
        public static bool NeedsPoison
        {
            get
            {
                return Helpers.Rogue.me.Inventory.Equipped.MainHand != null &&
                       Helpers.Rogue.me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id == 0 &&
                       Helpers.Rogue.me.Inventory.Equipped.MainHand.ItemInfo.WeaponClass != WoWItemWeaponClass.FishingPole;
            }
        }

        static public void Pulse()
        {
            

            Stealth = false; Vanish = false; Recuperate = false; SliceandDice = false;
            FuryoftheDestroyer = false; ColdBlood = false; Envenom = false; Tricks = false;
            ShadowDance = false; FindWeakness = false; Rupture = false; ShouldShiv = false;
            IsTargetDisoriented = false; IsTargetInvulnerable = false; IsTargetSapped = false;
            IsTargetImmuneStun = false; IsTargetImmuneSilence = false; IsBehind = false;
            DeadlyPoison = false; Vendetta = false; Blindside = false; RevealingStrike = false;//MasterOfSubtlety = false;  
            ModerateInsight = false; DeepInsight = false; BladeFlurry = false; AdrenalineRush = false;
            CripplingPoison = false; LeechingPoison = false; Feint = false;
            DeadlyThrow = false;
            FaerieFire = false;

            HealingGhost = false;

            TimeRecuperate = 0; TimeSliceandDice = 0; TimeRupture = 0; TimeHemorrhage = 0; IsTargetCasting = 0;
            TimeVendetta = 0;

            Deadly = false; Wound = false; MindNumbing = false; Crippling = false; Paralytic = false; Leeching = false;

            foreach (WoWAura aura in Helpers.Rogue.me.GetAllAuras())
                {
                    if (aura.IsActive) { 
                    switch (aura.SpellId)
                    {

                        case 2823:
                            {
                                Deadly = true;
                                break;
                            }
                        case 8679:
                            {
                                Wound = true;
                                break;
                            }
                        case 5761:
                            {
                                MindNumbing = true;
                                break;
                            }
                        case 3408:
                            {
                                Crippling = true;
                                break;
                            }
                        case 108215:
                            {
                                Paralytic = true;
                                break;
                            }
                        case 108211:
                            {
                                Leeching = true;
                                break;
                            }
                        case 57934:
                            {
                                Tricks = true;
                                break;
                            }// MasterOfSubtlety 31223\
                        
                    }
                    switch (aura.Name) //goto case ; 
                    {

                        case "Feint": 
                            {
                                Feint = true;
                                break;
                            }
                        case "Blindside": //121152 
                            {
                                Blindside = true;
                                break;
                            }
                        case "Vanish":
                            {
                                Vanish = true;
                                break;
                            }
                        case "Recuperate":
                            {
                                TimeRecuperate = aura.TimeLeft.TotalSeconds;
                                Recuperate = true;
                                break;
                            }
                        case "Slice and Dice":
                            {
                                TimeSliceandDice = aura.TimeLeft.TotalSeconds;
                                SliceandDice = true;
                                break;
                            }
                        case "Fury of the Destroyer":
                            {
                                FuryoftheDestroyer = true;
                                break;
                            }
                        case "Envenom":
                            {
                                Envenom = true;
                                break;
                            }
                        case "Shadow Dance":
                            {
                                ShadowDance = true;
                                break;
                            }
                        case "Adrenaline Rush":
                            {
                                //if (aura.TimeLeft.TotalSeconds < 16 && aura.TimeLeft.TotalSeconds > 1)
                                AdrenalineRush = true;
                                break;
                            }
                        case "Faerie Fire":
                            {
                                FaerieFire = true;
                                break;
                            }
                    }

                  }
                    switch (aura.Name) //goto case ; 
                    {

                        case "Stealth":
                            {
                                Stealth = true;
                                break;
                            }

                        case "Moderate Insight":
                            {
                                ModerateInsight = true;
                                break;
                            }
                        case "Deep Insight":
                            {
                                DeepInsight = true;
                                break;
                            }
                        case "Blade Flurry":
                            {
                                BladeFlurry = true;
                                break;
                            }
                    }

                }

            if (Rogue.mTarget != null)
            {
                IsInSafeMeleeRange = Helpers.Movement.IsInSafeMeleeRange;
                IsBehind = Rogue.mTarget.IsPlayerBehind || Settings.Mode.mForceBehind || Rogue.mTarget.MeIsSafelyBehind;
                //if (IsBehind) Logging.Write(Colors.White, "IsPlayerBehind:" +Rogue.mTarget.IsPlayerBehind + " MeIsSafelyBehind:"+ Rogue.mTarget.MeIsSafelyBehind);

                if (Rogue.mTarget.IsCasting)
                    IsTargetCasting = Rogue.mTarget.CastingSpellId;
                foreach (WoWAura aura in Rogue.mTarget.GetAllAuras())
                {
                    if (aura.IsActive) { 
                    if (aura.SpellId == 89775 && aura.CreatorGuid == Helpers.Rogue.me.Guid) TimeHemorrhage = aura.TimeLeft.TotalSeconds;
                    //if (aura.SpellId == 91023 && aura.CreatorGuid == Helpers.Rogue.me.Guid) TimeHemorrhage = aura.TimeLeft.TotalSeconds;
                     
                    if(aura.Spell.Mechanic == WoWSpellMechanic.Disoriented ||
                        aura.Spell.Mechanic == WoWSpellMechanic.Incapacitated)
                    {
                        Logging.Write(LogLevel.Diagnostic, Colors.White, "Disoriented!!!");
                        IsTargetDisoriented = true;
                    }/**
                    else if (aura.Spell.Mechanic == WoWSpellMechanic.Invulnerable ||
                        aura.Spell.Mechanic == WoWSpellMechanic.Invulnerable2)
                    {
                        Logging.WriteDebug(Colors.White, "Invulnerable!!!");
                        IsTargetInvulnerable = true;
                    }*/
                    else if (aura.Spell.Mechanic == WoWSpellMechanic.Sapped)
                    {

                        Logging.Write(LogLevel.Diagnostic, Colors.White, "Sapped!!!!");
                        IsTargetSapped = true;
                    }
                    else if (aura.Spell.Mechanic == WoWSpellMechanic.Interrupted)
                    {
                        Logging.Write(LogLevel.Diagnostic, Colors.White, "Interrupted!!!");
                        IsTargetInterrupted = true;
                    }
                        switch (aura.Name) //goto case ; 
                        { 

                            case "Kidney Shot":
                                {
                                    KidneyTime = aura.TimeLeft.TotalSeconds;
                                    break;
                                }
                            case "Vendetta":
                                {
                                    if (aura.CreatorGuid == Helpers.Rogue.me.Guid) 
                                    {
                                        Vendetta = true;
                                        TimeVendetta = aura.TimeLeft.TotalSeconds;
                                        
                                    }
                                    break;
                                }
                            case "Rupture":
                                {
                                    if (aura.CreatorGuid == Helpers.Rogue.me.Guid)
                                    {
                                        Rupture = true;
                                        TimeRupture = aura.TimeLeft.TotalSeconds;
                                    }
                                    break;
                                }
                            case "Enrage":
                                {
                                    ShouldShiv = true;
                                    break;
                                }
                            case "Wrecking Crew":
                                {
                                    ShouldShiv = true;
                                    break;
                                }
                            case "Savage Roar":
                                {
                                    ShouldShiv = true;
                                    break;
                                }
                            case "Unholy Frenzy":
                                {
                                    ShouldShiv = true;
                                    break;
                                }
                            case "Berserker Rage":
                                {
                                    ShouldShiv = true;
                                    //immune sap
                                    break;
                                }
                            case "Death Wish":
                                {
                                    ShouldShiv = true;
                                    break;
                                }
                            case "Owlkin Frenzy":
                                {
                                    ShouldShiv = true;
                                    break;
                                }
                            case "Bastion of Defense":
                                {
                                    ShouldShiv = true;
                                    break;
                                }
                            case "Cyclone":
                                {
                                    IsTargetInvulnerable = true;
                                    break;
                                }
                            case "Ice Block":
                                {
                                    IsTargetInvulnerable = true;
                                    break;
                                }
                            case "Divine Shield":
                                {
                                    IsTargetInvulnerable = true;
                                    break;
                                }
                            case "Hand of Protection":
                                {
                                    IsTargetInvulnerable = true;
                                    break;
                                }
                            case "Icebound Fortitude":
                                {
                                    IsTargetImmuneStun = true;
                                    break;
                                }
                            case "Beast Within":
                                {
                                    IsTargetImmuneStun = true;
                                    break;
                                }
                            case "Deadly Poison":
                                {
                                    DeadlyPoison = true;
                                    break;
                                }
                            case "Crippling Poison":
                                {
                                    CripplingPoison = true;
                                    break;
                                }
                            case "Leeching Poison":
                                {
                                    LeechingPoison = true;
                                    break;
                                }
                            case "Revealing Strike":
                                {
                                    if (aura.CreatorGuid == Helpers.Rogue.me.Guid)
                                        RevealingStrike = true;
                                    break;
                                }
                            case "Spirit of Redemption":
                                {
                                    HealingGhost = true;
                                    break;
                                }
                            case "Deadly Throw":
                                {
                                    DeadlyThrow = true;
                                    break;
                                }
                            case "Find Weakness":
                                {
                                    if (aura.CreatorGuid == Helpers.Rogue.me.Guid)
                                      FindWeakness = true;
                                    break;
                                }
                        


                        }


                    }
                    //switch (aura.Name) //goto case ; 
                    //{
                    
                    //        case "Find Weakness":
                    //            {
                    //                if (aura.CreatorGuid == Helpers.Rogue.me.Guid)
                    //                    FindWeakness = true;
                    //                break;
                    //            }
                    //}
                }
                if (IsTargetInvulnerable) Logging.Write(LogLevel.Diagnostic, Colors.White, "Invulnerable!!!");

                
                //Logging.Write(LogLevel.Normal, Movement.IsInSafeMeleeRange + " " + IsBehind);
            }
            ShouldShiv = false;

            if (!Helpers.Rogue.me.Combat)
                Styx.Common.Logging.Write(Styx.Common.LogLevel.Diagnostic, "Thinking");
            //if (Styx.CommonBot.InactivityDetector.TimeUntilLogout!=null)
            //Logging.Write(LogLevel.Normal, Styx.CommonBot.InactivityDetector.TimeUntilLogout.TotalMinutes + " " );
            //if (Helpers.Target.mNearbyEnemyUnits != null && Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance < Helpers.Rogue.me.CombatReach + 0.3333334f + unit.CombatReach && unit.IsBehind(Helpers.Rogue.me)) >0)
              //  Logging.Write(LogLevel.Normal, Helpers.Target.mNearbyEnemyUnits.Count(unit => unit.Distance < Helpers.Rogue.me.CombatReach + 0.3333334f + unit.CombatReach && unit.IsBehind(Helpers.Rogue.me)) + " ");
        }
    }
}
