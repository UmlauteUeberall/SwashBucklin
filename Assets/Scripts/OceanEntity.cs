using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EOceanEntityType {  Empty, Rock, Ship, Storm }

public class COceanEntity
{
    public EOceanEntityType EntityType { get; set; }
    public int OwnerId { get; set;  }
}
