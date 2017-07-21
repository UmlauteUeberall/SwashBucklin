using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using plib.Util;
using System.Linq;

namespace Ocean
{

    public class COceanData
    {
        public uint mu_xSize = 20;
        public uint mu_ySize = 20;

        public float mu_cellSize = 20.0f;

        private List<AOceanEntity> mi_entityList;

        public List<AOceanEntity> fu_GetAllEntities()
        {
            return mi_entityList.ToList();
        }

        public List<AOceanEntityView> fu_GetAllViews()
        {
            return mi_entityList.Select(_o => _o.mu_view).ToList();
        }

        public List<AOceanEntity> fu_GetListOfType(EOceanEntityType _entityType)
        {
            return mi_entityList.Where(_o => _o.pu_EntityType == _entityType).ToList();
            //List<AOceanEntity> list = new List<AOceanEntity>();
            //foreach (var e in mi_entityList)
            //{
            //    if (e.EntityType == _entityType)
            //    {
            //        list.Add(e);
            //    }
            //}
            //
            //return list;
        }

        public List<AOceanEntityView> fu_GetViewsOfType(EOceanEntityType _entityType)
        {
            return mi_entityList.Where(_o => _o.pu_EntityType == _entityType).Select(_o => _o.mu_view).ToList();
        }

        public void fu_CreateOcean(int _numPlayers, int _numRocks, int _numSwirls, int _numDrifts)
        {
            // first, distribute some rocks
            UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
            for (int i = 0; i < _numRocks; i++)
            {
                do
                {

                } while (true);
            }   

        }

        public bool fu_IsPlaceOccupied(EOceanEntityType _type)
        {
            return false;
        }
    }

}