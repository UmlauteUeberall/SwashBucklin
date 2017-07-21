using UnityEngine;
using System.Collections;
using plib.Util;
using Ocean;
using System.Collections.Generic;

public class CGameController : SingletonBehaviour<CGameController>
{
    public GameObject mu_OceanPlane;
    public COceanData mu_ocean;

    public int pu_Round { get { return mi_round; } }
    public EGameStage pu_GameStage { get { return mi_gameStage; } }

    private int mi_round = 0;
    private EGameStage mi_gameStage = EGameStage.SELECTION_PHASE;

    void Awake()
    {
        L.APP_NAME = "SwashBucklin";
        UnityHelper.LoadUnityLogger();
    }

    public void fu_CreateViews()
    {
        List<AOceanEntity> ets = mu_ocean.fu_GetAllEntities();

        GameObject tmp;
        GameObject prefab = null;
        Vector3 pos = Vector3.zero;
        foreach (AOceanEntity e in ets)
        {
            switch (e.pu_EntityType)
            {
                case EOceanEntityType.Rock:
                    prefab = null;          // Todo: Models anlegen
                    break;
                case EOceanEntityType.Ship:
                    prefab = null;
                    break;
                case EOceanEntityType.Storm:
                    prefab = null;
                    break;
                default:
                    break;
            }

            if (prefab == null)
            {
                continue;
            }

            pos = new Vector3(e.pu_x, 0f, e.pu_y) * mu_ocean.mu_cellSize;
            tmp = Instantiate(prefab, pos, Quaternion.identity, mu_OceanPlane.transform);       // TODO: Rotation

            e.mu_view = tmp.GetComponent<AOceanEntityView>();
        }
    }

    public void fu_ProcessNextStage()
    {
        int gs = (int)mi_gameStage;
        gs++;
        if (gs > 3)
        {
            gs = -1;
            mi_round++;
        }

        mi_gameStage = (EGameStage) gs;

        mu_ocean.fu_GetListOfType(EOceanEntityType.Ship).ForEach(_o => _o.fu_processNextStage());
        // TODO: Bewegliche Stürme
    }
}

public enum EGameStage
{
    SELECTION_PHASE = -1,
    MOVE_1 = 0,
    MOVE_2 = 1,
    MOVE_3 = 2,
    MOVE_4 = 3,
}
