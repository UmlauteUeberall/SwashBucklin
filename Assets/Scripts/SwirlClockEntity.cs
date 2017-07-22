using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ocean
{
    public class CSwirlClockEntity : AOceanEntity
    {
        public override EOceanEntityType pu_EntityType
        {
            get
            {
                return EOceanEntityType.SwirlClock;
            }
        }

        public CSwirlClockEntity(int _x, int _y, EOrientation _orientation) : base(_x, _y, _orientation)
        {
        }
    }
}
