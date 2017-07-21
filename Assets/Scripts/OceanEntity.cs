using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ocean
{
    public enum EOceanEntityType {  Empty, Rock, Ship, Storm }

    public abstract class AOceanEntity
    {
        public int pu_x;
        public int pu_y;

        public AOceanEntity(int _x, int _y)
        {
            pu_x = _x;
            pu_y = _y;
        }

        public abstract EOceanEntityType EntityType { get; }
        public int OwnerId { get; set; }
    }
}