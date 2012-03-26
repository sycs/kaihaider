//////////////////////////////////////////////////
//              Helpers/Trinkets.cs              //
//////////////////////////////////////////////////
//                 Helpers/Spells.cs            //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////
// Credit where credit is due:                  //
//  Thanks to Apoc and Singular for an example  //
//  of automagically determining if a trinket   //
//  is usable or not.                           //
//////////////////////////////////////////////////

using System.Drawing;
using Styx;
using Styx.Helpers;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using TreeSharp;

namespace PallyRaidBT.Helpers
{
    public delegate WoWItem WoWItemDelegate(object context);

    class Trinkets
    {
        static public WoWItem mTrinket1 { get; private set; }
        static public bool mTrinket1Usable { get; private set; }

        static public WoWItem mTrinket2 { get; private set; }
        static public bool mTrinket2Usable { get; private set; }

        static public void Pulse()
        {
            if (StyxWoW.Me.Inventory.Equipped.Trinket1 != null && (mTrinket1 == null ||
                mTrinket1.Guid != StyxWoW.Me.Inventory.Equipped.Trinket1.Guid))
            {
                mTrinket1 = StyxWoW.Me.Inventory.Equipped.Trinket1;
                mTrinket1Usable = ItemHasUseEffectLua(mTrinket1);

                Logging.Write(Color.Orange, "");
                Logging.Write(Color.Orange, "" + mTrinket1.Name + " detected in Trinket Slot 1.");
                Logging.Write(Color.Orange, " Usable spell: " + mTrinket1Usable);
                Logging.Write(Color.Orange, "");
            }

            if (StyxWoW.Me.Inventory.Equipped.Trinket2 != null && (mTrinket2 == null ||
                mTrinket2.Guid != StyxWoW.Me.Inventory.Equipped.Trinket2.Guid))
            {
                mTrinket2 = StyxWoW.Me.Inventory.Equipped.Trinket2;
                mTrinket2Usable = ItemHasUseEffectLua(mTrinket2);

                Logging.Write(Color.Orange, "" + mTrinket2.Name + " detected in Trinket Slot 2.");
                Logging.Write(Color.Orange, " Usable spell: " + mTrinket2Usable);
                Logging.Write(Color.Orange, "");
            }
        }

        static public Composite UseTrinkets(CanRunDecoratorDelegate cond)
        {
            return new PrioritySelector(
                UseItem(ret => GetFirstTrinket(), ret => cond(ret) && mTrinket1Usable),
                UseItem(ret => GetSecondTrinket(), ret => cond(ret) && mTrinket2Usable)
            );
        }

        static public Composite UseTrinkets()
        {
            return UseTrinkets(ret => true);
        }

        static public Composite UseItem(WoWItemDelegate item, CanRunDecoratorDelegate cond)
        {
            return new Decorator(ret => item(ret) != null && cond(ret) && ItemUsable(item(ret)),
                new Action(ret =>
                    {
                        item(ret).Use();
                        Logging.Write(Color.Red, item(ret).Name);
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
            return StyxWoW.Me.Inventory.Equipped.Trinket1;
        }

        static private WoWItem GetSecondTrinket()
        {
            return StyxWoW.Me.Inventory.Equipped.Trinket2;
        }
    }
}
