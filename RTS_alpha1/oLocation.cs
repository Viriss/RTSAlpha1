using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS_alpha1
{
   public enum LocationType { IronMine, IronForge }

    public abstract class oLocation
    {
        public Guid LocationGuid;
        public LocationType LocationType;
        
        public oLocation()
        {
            LocationGuid = Guid.NewGuid();
        }
    }
}
