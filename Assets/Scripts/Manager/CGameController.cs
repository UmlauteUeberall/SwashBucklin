using UnityEngine;
using System.Collections;
using plib.Util;
using Ocean;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

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
    private EGameSubStage mi_subStage;
    private float mi_gameStageTimer;

    public Dictionary<int, CPlayer> mu_PlayerDict = new Dictionary<int, CPlayer>();

    void Awake()
    {
        //L.APP_NAME = "SwashBucklin";
        UnityHelper.LoadUnityLogger();
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    private void Start()
    {
        mu_ocean = new COceanData();
    }

    private void Update()
    {
        if (mi_gameStage != EGameStage.SELECTION_PHASE)
        {
            mi_gameStageTimer -= Time.deltaTime;
            switch(mi_subStage)
            {
                case EGameSubStage.MOVE_STREAM:
                break;
                case EGameSubStage.MOVE_SHIP:
                break;
                case EGameSubStage.HOOK_ENEMY:
                break;
                case EGameSubStage.SHOOT_ENEMY:
                break;

            }

            if (mi_gameStageTimer <= 0.0f)
            {
                mi_gameStageTimer = 1.0f;
            }
        }
    }

    void OnConnect(int device_id)
    {
        if (AirConsole.instance.GetControllerDeviceIds().Count < 8)
        {
            mu_PlayerDict.Add(device_id, new CPlayer(device_id));
        }

        if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0)
        {
            if (AirConsole.instance.GetControllerDeviceIds().Count >= 1) // TODO: set to min players
            {
                AirConsole.instance.SetActivePlayers(8);
                mu_ocean.fu_CreateOcean(mu_PlayerDict.Count, 20, 4, 8);
                fu_CreateViews();
            }
        }
    }

    void OnDisconnect(int device_id)
    {
        if (mu_PlayerDict.ContainsKey(device_id))
        {

            mu_PlayerDict.Remove(device_id);
        }


        int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
        if (active_player != -1)
        {

            if (AirConsole.instance.GetControllerDeviceIds().Count >= 1)
            {
            }
            else
            {
            }
        }
    }

    /// <summary>
    /// We check which one of the active players has moved the paddle.
    /// </summary>
    /// <param name="from">From.</param>
    /// <param name="data">Data.</param>
    void OnMessage(int device_id, JToken data)
    {
        CPlayer player = mu_PlayerDict[device_id];

        Debug.Log(data.ToString());
        player.fu_UpdateStatus(data.ToString());
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
                Debug.Log("Ship created");
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
            prefab = null;
            tmp.transform.parent = mu_OceanPlane.transform;

            e.mu_view = tmp.GetComponent<AOceanEntityView>();
            if (e.pu_EntityType == EOceanEntityType.Ship)
            {
                ((CShipEntityView)e.mu_view).mu_shipEntity = (CShipEntity)e;
            }
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
            mi_gameStageTimer = 1.0f;
        }

        mi_gameStage = (EGameStage) gs;

        var ets = mu_ocean.fu_GetListOfType(EOceanEntityType.Ship);
        foreach (CShipEntity e in ets)
        {
            e.fu_ProcessStreamsAndSwirls();
        }
        foreach (CShipEntity e in ets)
        {
            e.fu_ProcessMove(mi_gameStage);
        }
        foreach (CShipEntity e in ets)
        {
            e.fu_ProcessHooks(mi_gameStage);
        }
        foreach (CShipEntity e in ets)
        {
            e.fu_ProcessCannons(mi_gameStage);
        }
    }
}

public enum EMoveCommand {  STAY, FORWARD, LEFT, RIGHT }

public enum EGameStage
{
    SELECTION_PHASE = -1,
    MOVE_1 = 0,
    MOVE_2 = 1,
    MOVE_3 = 2,
    MOVE_4 = 3,
}

public enum EGameSubStage
{
    MOVE_STREAM,
    MOVE_SHIP,
    HOOK_ENEMY,
    SHOOT_ENEMY
}