using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS_alpha1
{
    public class oLocationMine : oLocation
    {
        public int Supply;
        public int Distance;

        public oLocationMine() : base()
        {
            Distance = 9999;
            Supply = Global.rnd.Next(1, 11) * 5;
            LocationType = LocationType.IronMine;
        }

        public int Mine()
        {
            if (Supply > 0)
            {
                Supply -= 1;
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
