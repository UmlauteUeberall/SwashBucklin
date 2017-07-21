using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ocean
{
    public class CRockEntity : AOceanEntity
    {
        public override EOceanEntityType EntityType
        {
            get
            {
                return EOceanEntityType.Rock;
            }
        }

        public CRockEntity(int _x, int _y) 
            : base(_x, _y)
        { }
    }
}
