using UnityEngine;
using System.Collections;
using Ocean;

public class CShipEntityView : AOceanEntityView 
{
    public Transform[] m_portCannonPoints;
    public Transform[] m_portUpperCannonPoints;
    public Transform[] m_starboardCannonPoints;
    public Transform[] m_starboardUpperCannonPoints;

    public CShipEntity mu_shipEntity;

    bool mi_wantLerp;
    Vector3 mi_targetPos;
    Vector3 mi_startPos;
    Quaternion mi_startQuaternion;
    Quaternion mi_targetQuaternion;
    float mi_lerpTimer;
    
    // Use this for initialization
    void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
        

        if (!mi_wantLerp)
        {
            mi_targetPos = new Vector3(mu_shipEntity.pu_x, 0f, mu_shipEntity.pu_y) * CGameController.Get.mu_ocean.mu_cellSize;
            if (Vector3.Distance(mi_targetPos, transform.position) > 2.0f)
            {
                mi_wantLerp = true;
                mi_lerpTimer = 0.0f;
                mi_startPos = transform.position;
                mi_startQuaternion = transform.rotation;
                mi_targetQuaternion = Quaternion.identity;
                mi_targetQuaternion.eulerAngles = new Vector3(0, 90 * (int)(mu_shipEntity.pu_orientation), 0);
            }
        }
        else
        {
            mi_lerpTimer += Time.deltaTime;
            if (mi_lerpTimer >= 1.0)
            {
                mi_wantLerp = false;
            }
            else
            {
                transform.position = Vector3.Lerp(mi_startPos, mi_targetPos, mi_lerpTimer);
                transform.rotation = Quaternion.Slerp(mi_startQuaternion, mi_targetQuaternion, mi_lerpTimer);
            }
        }

//         if (Input.GetKeyDown(KeyCode.W))
//         {
//             CPlayer player = CGameController.Get.mu_PlayerDict[mu_shipEntity.mu_deviceId];
//             if (player != null)
//             {
//                 player.pu_movement1Mode = 1;
//                 mu_shipEntity.fu_ProcessMove(0);
//             }
//         }
//         else if (Input.GetKeyDown(KeyCode.Q))
//         {
//             CPlayer player = CGameController.Get.mu_PlayerDict[mu_shipEntity.mu_deviceId];
//             if (player != null)
//             {
//                 player.pu_movement1Mode = 2;
//                 mu_shipEntity.fu_ProcessMove(0);
//             }
//         }
//         else if (Input.GetKeyDown(KeyCode.E))
//         {
//             CPlayer player = CGameController.Get.mu_PlayerDict[mu_shipEntity.mu_deviceId];
//             if (player != null)
//             {
//                 player.pu_movement1Mode = 3;
//                 mu_shipEntity.fu_ProcessMove(0);
//             }
//         }
    }
}
