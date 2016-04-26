using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS_alpha1
{
    public class oPathStep :oStep
    {
        public int Distance;

        public oPathStep(int X, int Y) : base(X, Y)
        {
            Distance = 0;
        }
    }
}
