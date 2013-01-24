//////////////////////////////////////////////////
//               Trinkets.cs                    //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
// Credit where credit is due:                  //
//  Thanks to Apoc and Singular for an example  //
//  of automagically determining if a trinket   //
//  is usable or not.                           //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System.Drawing;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Styx.TreeSharp;
using Styx.Common;

namespace RogueBT.Helpers
{
    static class Specials
    {
        static public WoWItem mTrinket1 { get; private set; }
        static public bool mTrinket1Usable { get; private set; }

        static public WoWItem mTrinket2 { get; private set; }
        static public bool mTrinket2Usable { get; private set; }

        static public WoWItem mGloves { get; private set; }
        static public bool mGlovesUsable { get; private set; }

        public static string mRacialName { get; private set; }

        static Specials()
        {
            switch (Helpers.Rogue.me.Race)
            {
                case WoWRace.Orc:
                    mRacialName = "Blood Fury";
                    break;

                case WoWRace.Troll:
                    mRacialName = "Berserking";
                    break;

                case WoWRace.BloodElf:
                    mRacialName = "Arcane Torrent";
                    break;

                //case WoWRace.Pandaren:
                //    mRacialName = "Quaking Palm";
                //    break;

                //case WoWRace.Goblin:
                //    mRacialName = "";
                //    break;

                //case WoWRace.Undead:
                //    mRacialName = "";
                //    break;

                //case WoWRace.Human:
                //    mRacialName = "";
                //    break;

                //case WoWRace.Dwarf:
                //    mRacialName = "";
                //    break;

                //case WoWRace.NightElf:
                //    mRacialName = "";
                //    break;

                //case WoWRace.Gnome:
                //    mRacialName = "";
                //    break;

                //case WoWRace.Worgen:
                //    mRacialName = "";
                //    break;
            }
        }

        static public void Pulse()
        {
            var trinket1 = GetFirstTrinket();
            var trinket2 = GetSecondTrinket();
            var gloves = GetGloves();

            if (trinket1 != null && (mTrinket1 == null ||
                mTrinket1.Guid != trinket1.Guid))
            {
                mTrinket1 = trinket1;
                mTrinket1Usable = ItemHasUseEffectLua(mTrinket1);

                foreach (WoWItem.WoWItemSpell itemSpell in mTrinket1.ItemSpells)
                {
                    if (itemSpell.ActualSpell.Id == 42292) mTrinket1Usable = false;
                }
                

                Logging.Write(LogLevel.Normal, "");
                Logging.Write(LogLevel.Normal, "" + mTrinket1.Name + " detected in Trinket Slot 1.");
                Logging.Write(LogLevel.Normal, " Usable spell: " + mTrinket1Usable);
                Logging.Write(LogLevel.Normal, "");
            }

            if (trinket2 != null && (mTrinket2 == null ||
                mTrinket2.Guid != trinket2.Guid))
            {
                mTrinket2 = trinket2;
                mTrinket2Usable = ItemHasUseEffectLua(mTrinket2);
                foreach (WoWItem.WoWItemSpell itemSpell in mTrinket2.ItemSpells)
                {
                    if (itemSpell.ActualSpell.Id == 42292) mTrinket2Usable = false;
                }
                

                Logging.Write(LogLevel.Normal, "" + mTrinket2.Name + " detected in Trinket Slot 2.");
                Logging.Write(LogLevel.Normal, " Usable spell: " + mTrinket2Usable);
                Logging.Write(LogLevel.Normal, "");
            }

            if (gloves != null && (mGloves == null ||
                mGloves.Guid != gloves.Guid))
            {
                mGloves = gloves;
                mGlovesUsable = ItemHasUseEffectLua(mGloves);

                Logging.Write(LogLevel.Normal, "" + mGloves.Name + " detected in Gloves Slot.");
                Logging.Write(LogLevel.Normal, " Usable spell: " + mGlovesUsable);
                Logging.Write(LogLevel.Normal, "");
            }
        }

        static public Composite UseSpecialAbilities(CanRunDecoratorDelegate cond)
        {
            return new PrioritySelector(
                UseItem(ret => mTrinket1, ret => cond(ret) && mTrinket1Usable),
                UseItem(ret => mTrinket2, ret => cond(ret) && mTrinket2Usable)
                //UseItem(ret => mGloves, ret => cond(ret) && mGlovesUsable)//,

                //new Decorator(ret => mRacialName != null,
                //    Spells.Cast(mRacialName))
            );
        }

        static public Composite UseSpecialAbilities()
        {
            return UseSpecialAbilities(ret => true);
        }

        static public Composite UseItem(WoWItemDelegate item, CanRunDecoratorDelegate cond)
        {
            return new Decorator(ret => item(ret) != null && cond(ret) && ItemUsable(item(ret)),
                new Action(ret =>
                    {
                        item(ret).Use();
                        Logging.Write(LogLevel.Normal, item(ret).Name);
                    }
                )
            );
        }

        static private bool ItemUsable(WoWItem item)
        {
            return item.Usable && item.Cooldown == 0;
        }

        static private bool ItemHasUseEffectLua(WoWItem item)
        {
            var itemSpell = Lua.GetReturnVal<string>("return GetItemSpell(" + item.Entry + ")", 0);

            if (itemSpell != null)
            {
                return true;
            }

            return false;
        }

        static private WoWItem GetFirstTrinket()
        {
            return Helpers.Rogue.me.Inventory.Equipped.Trinket1;
        }

        static private WoWItem GetSecondTrinket()
        {
            return Helpers.Rogue.me.Inventory.Equipped.Trinket2;
        }

        static private WoWItem GetGloves()
        {
            return Helpers.Rogue.me.Inventory.Equipped.Hands;
        }
    }
}
