using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ocean
{
    public class CSwirlCenterEntity : AOceanEntity
    {
        public override EOceanEntityType pu_EntityType
        {
            get
            {
                return EOceanEntityType.SwirlCenter;
            }
        }

        public CSwirlCenterEntity(int _x, int _y, EOrientation _orientation) : base(_x, _y, _orientation)
        {
        }

        public override void fu_processNextStage(EGameStage _newStage)
        {
        }
    }
}
