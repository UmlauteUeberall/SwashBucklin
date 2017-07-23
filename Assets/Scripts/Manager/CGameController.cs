using UnityEngine;
using System.Collections;
using plib.Util;
using Ocean;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System.Linq;

public class CGameController : SingletonBehaviour<CGameController>
{
    public GameObject mu_OceanPlane;
    public COceanData mu_ocean;

    public CRockEntityView mu_RockPrefab;
    public CShipEntityView mu_ShipPrefab;
    public CSwirlEntityView mu_SwirlPrefab;
    public CStormEntityView mu_StormPrefab;
    public CStreamEntityView mu_StreamPrefab;
    public AOceanEntityView mu_OceanPrefab;

    public int pu_Round { get { return mi_round; } }
    public EGameStage pu_GameStage { get { return mi_gameStage; } }
    public Dictionary<int, CPlayer> mu_PlayerDict = new Dictionary<int, CPlayer>();

    private int mi_round = 0;
    private EGameStage mi_gameStage = EGameStage.SELECTION_PHASE;
    //private EGameSubStage mi_subStage;
    private float mi_gameStageTimer;
    UIManager mu_uiManager;

    void Awake()
    {
        L.APP_NAME = "SwashBucklin";
        UnityHelper.LoadUnityLogger();

        if (AirConsole.instance)
        {
            AirConsole.instance.onMessage += OnMessage;
            AirConsole.instance.onConnect += OnConnect;
            AirConsole.instance.onDisconnect += OnDisconnect;
        }

        Canvas[] cArray = FindObjectsOfType<Canvas>();
        foreach (var c in cArray)
        {
            mu_uiManager = c.GetComponent<UIManager>();
            if (mu_uiManager != null)
                break;
        }
    }

    private void Start()
    {
        mu_ocean = new COceanData();


        /// HACK ///
        //mu_PlayerDict.Add(1, new CPlayer(1));
        //mu_PlayerDict.Add(2, new CPlayer(2));
        //mu_ocean.fu_CreateOcean(20, 4, 8);
        //fu_CreateViews();
        /// HACK ///
    }

    private void Update()
    {
        mi_gameStageTimer -= Time.deltaTime;
        if (mi_gameStage == EGameStage.SELECTION_PHASE)
            mu_uiManager.SetTimerDisplay(mi_gameStageTimer);
        else
            mu_uiManager.SetTimerDisplay(0);

        if (mi_gameStageTimer <= 0.0f ||
                            (mi_gameStage == EGameStage.SELECTION_PHASE && mu_PlayerDict.Count > 0
                            && mu_PlayerDict.Values.All(_o => _o.pu_isReady != 0)))
        {
            fu_ProcessNextStage();

            if (mi_gameStage == EGameStage.SELECTION_PHASE)
            {
                foreach (CPlayer p in mu_PlayerDict.Values)
                {
                    p.pu_movement1LeftMode = 0;
                    p.pu_movement1Mode = 0;
                    p.pu_movement1RightMode = 0;
                    p.pu_movement2LeftMode = 0;
                    p.pu_movement2Mode = 0;
                    p.pu_movement2RightMode = 0;
                    p.pu_movement3LeftMode = 0;
                    p.pu_movement3Mode = 0;
                    p.pu_movement3RightMode = 0;
                    p.pu_movement4LeftMode = 0;
                    p.pu_movement4Mode = 0;
                    p.pu_movement4RightMode = 0;
                    p.fu_UpdateClient();
                }
            }
        }
    }

    void OnGUI()
    {
        GUILayout.Box(mi_gameStageTimer.ToString());
    }

    void OnConnect(int device_id)
    {
        mu_PlayerDict.Add(device_id, new CPlayer(device_id));

        int connected = AirConsole.instance.GetControllerDeviceIds().Count;
        if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0)
        {
            if (connected >= 2) // TODO: set to min players
            {
                AirConsole.instance.SetActivePlayers(8);

                mu_ocean.fu_CreateOcean(7, 2, 0);
                fu_CreateViews();

                mu_uiManager.SetStage(UIStage.HUD);
            }
        }
        mu_uiManager.UpdateLobbyScreen(connected, 2);
    }

    void OnDisconnect(int device_id)
    {
        if (mu_PlayerDict.ContainsKey(device_id))
        {
            mu_ocean.fu_GetListOfType(EOceanEntityType.Ship).Cast<CShipEntity>().
                FirstOrDefault(_o => _o.mu_deviceId == device_id).
                fu_Kill();
            mu_PlayerDict.Remove(device_id);
        }

        int connected = AirConsole.instance.GetControllerDeviceIds().Count;
        if (connected < 2)
        {
            mu_uiManager.SetStage(UIStage.Lobby);
        }
        mu_uiManager.UpdateLobbyScreen(connected, 2);

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

        L.Log(data.ToString());
        player.fu_UpdateStatus(data.ToString());
    }

    public void fu_CreateViews()
    {
        List<AOceanEntity> ets = mu_ocean.fu_GetAllEntities();

        GameObject tmp = null;
        GameObject prefab = null;
        Vector3 pos = Vector3.zero;
        for (int y = 0; y < mu_ocean.mu_ySize; y++)
        {
            for (int x = 0; x < mu_ocean.mu_xSize; x++)
            {
                List<AOceanEntity> entityList = mu_ocean.fu_GetListAt(x, y);
                bool wantOcean = true;

                foreach (var e in entityList)
                {
                    switch (e.pu_EntityType)
                    {
                        case EOceanEntityType.Rock:
                        {
                            tmp = Instantiate(mu_RockPrefab.gameObject);
                        }
                        break;
                        case EOceanEntityType.Ship:
                        {
                            tmp = Instantiate(mu_ShipPrefab.gameObject);
                            L.Log("instantiate ship model at (" + x + ", " + y + ")");
                        }
                        break;
                        case EOceanEntityType.Storm:
                        {
                            tmp = Instantiate(mu_StormPrefab.gameObject);
                        }
                        break;
                        case EOceanEntityType.SwirlCenter:
                        {
                            wantOcean = false;
                            tmp = Instantiate(mu_SwirlPrefab.gameObject);
                        }
                        break;
                        case EOceanEntityType.Stream:
                        {
                            wantOcean = false;
                            tmp = Instantiate(mu_StreamPrefab.gameObject);
                        }
                        break;

                        case EOceanEntityType.SwirlClock:
                            wantOcean = false;
                            tmp = null;
                        break;

                        default:
                            tmp = null;
                        break;
                    }

                    if (tmp != null)
                    {
                        pos = new Vector3(e.pu_x, 0f, e.pu_y) * mu_ocean.mu_cellSize;
                        tmp.transform.position = pos;
                        tmp.transform.eulerAngles = new Vector3(0, ((int)e.pu_orientation) * 90, 0);
                        tmp.transform.parent = mu_OceanPlane.transform;

                        e.mu_view = tmp.GetComponent<AOceanEntityView>();
                        if (e.pu_EntityType == EOceanEntityType.Ship)
                        {
                            ((CShipEntityView)e.mu_view).mu_shipEntity = (CShipEntity)e;
                            ((CShipEntityView)e.mu_view).mu_sailRenderer.material.color = fi_FetchColor();
                        }
                    }
                }

                if (wantOcean)
                {
                    pos = new Vector3(x, 0f, y) * mu_ocean.mu_cellSize;

                    tmp = Instantiate(mu_OceanPrefab.gameObject); // standard ocean
                    tmp.name += "_" + x + "_" + y;
                    tmp.transform.position = pos;
                    tmp.transform.parent = mu_OceanPlane.transform;
                }
            }
        }
        return;

        // now remove all water around swirl center
        //         var list = mu_OceanPlane.transform.GetOnlyChildren().Where(_o => _o.name.StartsWith(mu_OceanPrefab.name)).ToLinkedList();
        // 
        //         List<AOceanEntity> entities = mu_ocean.fu_GetListOfType(EOceanEntityType.SwirlCenter);
        //         string goName;
        //         Transform t;
        //         foreach (var e in entities)
        //         {
        //             for (int y = -1; y < 2; y++)
        //             {
        //                 for (int x = -1; x < 2; x++)
        //                 {
        //                     if (x == 0 && y == 0)
        //                         continue;
        // 
        //                     goName = "_" + (e.pu_x+x) + "_" + (e.pu_y + y);
        // 
        //                     t = list.FirstOrDefault(_o => _o.name.EndsWith(goName));
        //                     if (t != null)
        //                     {
        //                         list.Remove(t);
        //                         Destroy(t.gameObject);
        //                     }
        //                 }
        //             }
        //         }
    }

    public void fu_ProcessNextStage()
    {
        int gs = (int)mi_gameStage;
        gs++;
        if (gs > 3)
        {
            gs = -1;
            mi_round++;
            mi_gameStageTimer = 60.0f;

            int rnd;
            foreach (var p in mu_PlayerDict.Values)
            {
                rnd = Random.Range(4, 7);
                for (int i = rnd; i > 0; i--)
                {
                    rnd = Random.Range(0, 5);
                    switch (rnd)
                    {
                        case 0:
                        p.pu_tokenGunAmount = Mathf.Min(1 + p.pu_tokenGunAmount, CPlayer.pu_tokenGunMax);
                        break;
                        case 1:
                        p.pu_tokenHookAmount = Mathf.Min(1 + p.pu_tokenHookAmount, CPlayer.pu_tokenHookMax);
                        break;
                        case 2:
                        p.pu_tokenLeftAmount = Mathf.Min(1 + p.pu_tokenLeftAmount, CPlayer.pu_tokenLeftMax);
                        break;
                        case 3:
                        p.pu_tokenRightAmount = Mathf.Min(1 + p.pu_tokenRightAmount, CPlayer.pu_tokenRightMax);
                        break;
                        case 4:
                        p.pu_tokenStraightAmount = Mathf.Min(1 + p.pu_tokenStraightAmount, CPlayer.pu_tokenStraightMax);
                        break;
                    }
                }
            }
        }
        else
        {
            L.Log("Set");
            mi_gameStageTimer = 1.0f;
            mu_PlayerDict.Values.ForEach(_o => _o.pu_gameState = 1);
        }
        mu_PlayerDict.Values.ForEach(_o =>
        {
            _o.pu_isReady = 0;
            _o.fu_UpdateClient();
        });



        mi_gameStage = (EGameStage)gs;

        var ets = mu_ocean.fu_GetListOfType(EOceanEntityType.Ship);
        if (mi_gameStage != EGameStage.SELECTION_PHASE)
        {
            foreach (CShipEntity e in ets.Randomize())      // Lazy as fuck
            {
                e.fu_ProcessStreamsAndSwirls();
            }
            foreach (CShipEntity e in ets.Randomize())
            {
                e.fu_ProcessMove(mi_gameStage);
            }
            foreach (CShipEntity e in ets.Randomize())
            {
                e.fu_ProcessHooks(mi_gameStage);
            }
            foreach (CShipEntity e in ets.Randomize())
            {
                e.fu_ProcessCannons(mi_gameStage);
            }
            foreach (CShipEntity e in ets.Randomize())
            {
    //             if (e.fu_KillCheck())
    //             {
    //                 e.fu_Kill();
    //             }
            }
        }
    }

    private int cc = 0;

    Color fi_FetchColor()
    {
        Color cr;
        if (cc == 0)
            cr = Color.white;
        else if (cc == 1)
            cr = Color.red;
        else if (cc == 2)
            cr = Color.black;
        else if (cc == 3)
            cr = Color.green;
        else if (cc == 4)
            cr = Color.blue;
        else
            cr = Color.gray;
        cc++;
        return cr;
    }
}

public enum EMoveCommand { STAY, FORWARD, LEFT, RIGHT }

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