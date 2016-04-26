using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS_alpha1
{
    public class oNode
    {
        public int Index;
        public int Width;
        public Guid LocationGuid;

        public int X { get { return Index % Width; } }
        public int Y { get { return (Index - X) / Width; } }

        public oNode(int Index, int Width)
        {
            this.Index = Index;
            this.Width = Width;
            LocationGuid = Guid.Empty;
        }

        public void SetLocationGuid(Guid LocationGuid)
        {
            this.LocationGuid = LocationGuid;
        }

    }
}
