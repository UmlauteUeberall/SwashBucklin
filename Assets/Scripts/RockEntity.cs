using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ocean
{
    public class CRockEntity : AOceanEntity
    {
        public override EOceanEntityType pu_EntityType
        {
            get
            {
                return EOceanEntityType.Rock;
            }
        }

        public CRockEntity(int _x, int _y, EOrientation _orientation) : base(_x, _y, _orientation)
        {
        }
    }
}
