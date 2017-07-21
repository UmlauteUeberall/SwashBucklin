using UnityEngine;
using System.Collections;
using plib.Util;
using Ocean;
using System.Collections.Generic;

public class CGameController : SingletonBehaviour<CGameController>
{
    public GameObject mu_OceanPlane;
    public COceanData mu_ocean;

    public CRockEntityView mu_RockPrefab;
    public CShipEntityView mu_ShipPrefab;
    public CSwirlEntityView mu_SwirlPrefab;
    public CStormEntityView mu_StormPrefab;
    public CStreamEntityView mu_SteamPrefab;

    public int pu_Round { get { return mi_round; } }
    public EGameStage pu_GameStage { get { return mi_gameStage; } }

    private int mi_round = 0;
    private EGameStage mi_gameStage = EGameStage.SELECTION_PHASE;

    void Awake()
    {
        L.APP_NAME = "SwashBucklin";
        UnityHelper.LoadUnityLogger();
    }

    private void Start()
    {
        mu_ocean = new COceanData();
        mu_ocean.fu_CreateOcean(2, 20, 4, 8);
        fu_CreateViews();
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
                    prefab = mu_RockPrefab.gameObject;
                    break;
                case EOceanEntityType.Ship:
                    prefab = mu_ShipPrefab.gameObject;
                    break;
                case EOceanEntityType.Storm:
                    prefab = mu_StormPrefab.gameObject;
                    break;
                case EOceanEntityType.SwirlCenter:
                    prefab = mu_SwirlPrefab.gameObject;
                    break;
                case EOceanEntityType.Stream:
                    prefab = mu_SteamPrefab.gameObject;
                    break;
                default:
                    break;
            }

            if (prefab == null)
            {
                continue;
            }

            pos = new Vector3(e.pu_x, 0f, e.pu_y) * mu_ocean.mu_cellSize;
            tmp = Instantiate(prefab, pos, Quaternion.identity);       // TODO: Rotation
            tmp.transform.parent = mu_OceanPlane.transform;

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

        var ets = mu_ocean.fu_GetAllEntities();
        foreach (AOceanEntity e in ets)
        {
            if (e.pu_EntityType == EOceanEntityType.Ship || e.pu_EntityType == EOceanEntityType.Storm)
            {
                e.fu_processNextStage(mi_gameStage);
            }
        }
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
