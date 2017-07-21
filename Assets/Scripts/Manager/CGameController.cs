using UnityEngine;
using System.Collections;
using plib.Util;
using Ocean;

public class CGameController : SingletonBehaviour<CGameController>
{
    public COceanData mu_ocean;

    private int mi_round = 0;
    private EGameStage mi_gameStage = EGameStage.SELECTION_PHASE;

    void Awake()
    {
        L.APP_NAME = "SwashBucklin";
        UnityHelper.LoadUnityLogger();
    }

    public void fu_ProcessNextStage()
    {
        int gs = (int)mi_gameStage;
        gs++;
        if (gs > 3)
        {
            gs = -1;
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
