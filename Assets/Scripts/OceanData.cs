using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using plib.Util;
using Ocean;

namespace Ocean
{

    public class COceanData
    {
        private List<AOceanEntity> mi_entityList;

        public List<AOceanEntity> fu_GetListOfType(EOceanEntityType _entityType)
        {
            List<AOceanEntity> list = new List<AOceanEntity>();
            foreach (var e in mi_entityList)
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
            int x;
            int y;
                do
                {
                x = UnityEngine.Random.Range(0, 100);
                y = UnityEngine.Random.Range(0, 100);
            } while (fu_IsPlaceOccupied(x, y));

                } while (true);
            mi_entityList.Add(entity);
            }   

        }

        public bool fu_IsPlaceOccupied(EOceanEntityType _type)
        {
            return false;
        }
    }

}