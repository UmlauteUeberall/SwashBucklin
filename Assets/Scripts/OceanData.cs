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

        private List<AOceanEntity> mi_entityList = new List<AOceanEntity>();

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
        }

        public List<AOceanEntity> fu_GetListAt(int _x, int _y)
        {
            return mi_entityList.Where(_o => _o.pu_x  ==_x && _o.pu_y == _y).ToList();
        }

        public List<AOceanEntityView> fu_GetViewsOfType(EOceanEntityType _entityType)
        {
            return mi_entityList.Where(_o => _o.pu_EntityType == _entityType).Select(_o => _o.mu_view).ToList();
        }

        public void fu_CreateOcean(int _numRocks, int _numSwirls, int _numStreams)
        {
            // first, distribute some rocks
            UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
            AOceanEntity entity = null;
            int x;
            int y;

            // make rocks
            for (int i = 0; i < _numRocks; i++)
            {
                do
                {
                    x = UnityEngine.Random.Range((int)-mu_xSize, (int)mu_xSize);
                    y = UnityEngine.Random.Range((int)-mu_ySize, (int)mu_ySize);
                } while (fu_IsPlaceOccupied(x, y));
                entity = new CRockEntity(x, y, EOrientation.North);
                mi_entityList.Add(entity);
            }

            // make ships
            foreach(var kvp in CGameController.Get.mu_PlayerDict)
            {
                do
                {
                    x = UnityEngine.Random.Range((int)mu_xSize, (int)mu_xSize);
                    y = UnityEngine.Random.Range((int)mu_ySize, (int)mu_ySize);
                } while (fu_IsPlaceOccupied(x, y));

                EOrientation or = (EOrientation)UnityEngine.Random.Range(0, (int)EOrientation.MAX_ORIENTATION);

                int deviceId = kvp.Key;
                entity = new CShipEntity(x, y, or, deviceId);
                mi_entityList.Add(entity);
            }

            // make swirls
            for (int i = 0; i < _numSwirls; i++)
            {
                do
                {
                    x = UnityEngine.Random.Range((int)-mu_xSize, (int)mu_xSize);
                    y = UnityEngine.Random.Range((int)-mu_ySize, (int)mu_ySize);
                } while (fi_IsPlaceOccupiedForSwirl(x, y));

                // top row
                entity = new CSwirlClockEntity(x - 1, y - 1, EOrientation.East);
                mi_entityList.Add(entity);
                entity = new CStreamEntity(x, y - 1, EOrientation.East);
                mi_entityList.Add(entity);
                entity = new CSwirlClockEntity(x + 1, y - 1, EOrientation.South);
                mi_entityList.Add(entity);

                // center row
                entity = new CStreamEntity(x - 1, y, EOrientation.North);
                mi_entityList.Add(entity);
                entity = new CSwirlCenterEntity(x, y, EOrientation.MAX_ORIENTATION);
                mi_entityList.Add(entity);
                entity = new CStreamEntity(x + 1, y, EOrientation.South);
                mi_entityList.Add(entity);

                // bottom row
                entity = new CSwirlClockEntity(x - 1, y + 1, EOrientation.North);
                mi_entityList.Add(entity);
                entity = new CStreamEntity(x, y + 1, EOrientation.West);
                mi_entityList.Add(entity);
                entity = new CSwirlClockEntity(x + 1, y + 1, EOrientation.West);
                mi_entityList.Add(entity);
            }

            // make streams
            for (int i = 0; i < _numStreams; i++)
            {
                do
                {
                    x = UnityEngine.Random.Range((int)-mu_xSize, (int)mu_xSize);
                    y = UnityEngine.Random.Range((int)-mu_ySize, (int)mu_ySize);
                } while (fu_IsPlaceOccupied(x, y));

                EOrientation or = (EOrientation)UnityEngine.Random.Range(0, (int)EOrientation.MAX_ORIENTATION);
                entity = new CRockEntity(x, y, or);
                mi_entityList.Add(entity);
            }
        }

        public bool fu_IsPlaceOccupied(int _x, int _y)
        {
            return mi_entityList.Any(_o => _o.pu_x == _x && _o.pu_y == _y);
        }

        private bool fi_IsPlaceOccupiedForSwirl(int _x, int _y)
        {

            return mi_entityList.Any(_o => _o.pu_x >= _x - 1 && _o.pu_x <= _x + 1 &&
                                           _o.pu_y >= _y - 1 && _o.pu_y <= _y + 1);
        }
    }

}
