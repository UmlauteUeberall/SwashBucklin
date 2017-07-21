﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ocean
{
    public class CRockEntity : AOceanEntity
    {
        public override EOceanEntityType pu_EntityType
        public CRockEntity(int _x, int _y, EOrientation _orientation) : base(_x, _y, _orientation)
        {
        }

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
        public override void fu_processNextStage()
        {

        }
    }

}
