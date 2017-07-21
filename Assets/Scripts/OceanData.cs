using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using plib.Util;

public class COceanData
{
    private List<COceanEntity> mi_entityList;

    public List<COceanEntity> fu_GetListOfType(EOceanEntityType _entityType)
    {
        List<COceanEntity> list = new List<COceanEntity>();
        foreach(var e in mi_entityList)
        {
            if (e.EntityType == _entityType)
            {
                list.Add(e);
            }
        }

        return list;
    }

    public void fu_CreateOcean(int _numPlayers, int _numRocks, int _numSwirls, int _numDrifts)
    {
        // first, distribute some rocks
        UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        for (int i = 0; i < _numRocks; i++)
        {
            do
            {

            }
        }


    }

    public bool fu_IsPlaceOccupied(EOceanEntityType _type)
    {
        return false;
    }
}
