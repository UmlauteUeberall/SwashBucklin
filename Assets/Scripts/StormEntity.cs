using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ocean
{
    public class CStormEntity : AOceanEntity
    {
        public override EOceanEntityType pu_EntityType
        {
            get
            {
                return EOceanEntityType.Rock;
            }
        }

        public CStormEntity(int _x, int _y, EOrientation _orientation) : base(_x, _y, _orientation)
        {
        }

        public void fu_processNextStage(EGameStage _newStage)
        {

        }
    }

}
