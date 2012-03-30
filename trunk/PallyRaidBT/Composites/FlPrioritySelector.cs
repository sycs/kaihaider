//////////////////////////////////////////////////
//           FlPrioritySelector.cs              //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using Styx;
using TreeSharp;

namespace PallyRaidBT.Composites
{
    class FlPrioritySelector : PrioritySelector
    {
        public FlPrioritySelector(params Composite[] children) : base(children)
        {
        }

        public override RunStatus Tick(object context)
        {
            using (new FrameLock())
            {
                return base.Tick(context);
            }
        }
    }
}
