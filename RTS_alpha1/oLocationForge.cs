using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS_alpha1
{
    public class oLocationForge: oLocation
    {
        public int Supply;
        public float ProcessTime;
        public float ProcessRate;
        public int ProcessAmount;

        private float _processTimer;

        public oLocationForge() : base()
        {
            Supply = 0;
            LocationType = LocationType.IronForge;
            ProcessTime = 1 + (float)(Global.rnd.NextDouble() * 3.0);
            ProcessRate = 0.1f;
            ProcessAmount = 4;
            _processTimer = 0;
        }

        public void Update()
        {
            if (Supply >= ProcessAmount)
            {
                _processTimer += ProcessRate;
                if (_processTimer >= ProcessTime)
                {
                    Supply -= ProcessAmount;
                    Engine.AddMoney(1);
                    _processTimer = 0;
                }
            }
        }

    }
}
