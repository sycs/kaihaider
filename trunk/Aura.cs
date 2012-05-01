using System;
using System.Drawing;
using System.Linq;
using Styx;
using Styx.Helpers;
using Styx.Logic.Combat;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;
using Action = TreeSharp.Action;

namespace PvP_RaidBT.Helpers
{
    class Aura
    {
        static public Boolean Stealth { get; private set; }
        static public Boolean Vanish { get; private set; }
        static public Boolean Recuperate { get; private set; }
        static public double TimeRecuperate { get; private set; }
        static public Boolean SliceandDice { get; private set; }
        static public double TimeSliceandDice { get; private set; }
        static public Boolean FuryoftheDestroyer { get; private set; }
        static public Boolean ColdBlood { get; private set; }
        static public Boolean Envenom { get; private set; }
        static public Boolean Tricks { get; private set; }
        static public Boolean ShadowDance { get; private set; }
        static public Boolean FindWeakness { get; private set; }
        static public Boolean Rupture { get; private set; }
        static public double TimeRupture { get; private set; }
        static public Boolean ShouldShiv { get; private set; }
        static public double TimeHemorrhage { get; private set; }

        static public void Pulse() //Slice and Dice 
        {
            Stealth = false; Vanish = false; Recuperate = false; SliceandDice = false;
            FuryoftheDestroyer = false; ColdBlood = false; Envenom = false; Tricks = false;
            ShadowDance = false; FindWeakness = false; Rupture = false; ShouldShiv = false;
            TimeRecuperate = 0; TimeSliceandDice = 0; TimeRupture = 0; TimeHemorrhage = 0;

            foreach (WoWAura aura in StyxWoW.Me.GetAllAuras())
                {
                    if (aura.SpellId == 89775) TimeHemorrhage = aura.TimeLeft.TotalSeconds;
                    switch (aura.Name) //goto case ; 
                    {
                        case "Stealth":
                            {
                                Stealth = true;
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
                        case "Cold Blood":
                            {
                                ColdBlood = true;
                                break;
                            }
                        case "Envenom":
                            {
                                Envenom = true;
                                break;
                            }
                        case "Tricks":
                            {
                                Tricks = true;
                                break;
                            }
                        case "Shadow Dance":
                            {
                                ShadowDance = true;
                                break;
                            }
                    }
                }

            if (StyxWoW.Me.CurrentTarget != null)
            {
                foreach (WoWAura aura in StyxWoW.Me.CurrentTarget.GetAllAuras())
                {
                    switch (aura.Name) //goto case ; 
                    {
                        case "Rupture":
                            {
                                if (aura.CreatorGuid == StyxWoW.Me.Guid)
                                {
                                    Rupture = true;
                                    TimeRupture = aura.TimeLeft.TotalSeconds;
                                }
                                break;
                            }
                        case "Find Weakness":
                            {
                                FindWeakness = true;
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
                    }
                }
            }

            
                    

        }
    }
}
