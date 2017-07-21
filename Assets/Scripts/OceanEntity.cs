using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ocean
{
    public enum EOceanEntityType {  Water, Rock, Ship, Storm, SwirlCClock, SwirlClock, SwirlCenter, Stream }
    public enum EOrientation { North = 0, East = 1, South = 2, West = 3 }

    public abstract class AOceanEntity
    {
		public AOceanEntityView mu_view;
        public int pu_x;
        public int pu_y;

        public AOceanEntity(int _x, int _y, EOrientation _orientation)
        {
            pu_x = _x;
            pu_y = _y;
        }

        public abstract EOceanEntityType EntityType { get; }
        public int OwnerId { get; set; }

		public virtual void fu_processNextStage()
	    {

    	}
    }
}