using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ocean
{
    public enum EOceanEntityType {  Empty, Rock, Ship, Storm }

    public abstract class AOceanEntity
    {
		public AOceanEntityView mu_view;
        public int pu_x;
        public int pu_y;

        public AOceanEntity(int _x, int _y)
        {
            pu_x = _x;
            pu_y = _y;
        }

        public abstract EOceanEntityType pu_EntityType { get; }
        public int pu_OwnerId { get; set; }

		public virtual void fu_processNextStage()
	    {

    	}
    }
}