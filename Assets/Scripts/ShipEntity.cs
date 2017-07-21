using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ocean
{

    public class CShipEntity : AOceanEntity
    {
        public CShipEntity(int _x, int _y, EOrientation _orientation) : base(_x, _y, _orientation)
        {
        }

        public override EOceanEntityType EntityType
        {
            get
            {
                return EOceanEntityType.Ship;
            }
        }

        public override void fu_processNextStage()
        {

        }
    }
}