using UnityEngine;
using System.Collections;
using Ocean;

public class CShipEntityView : AOceanEntityView 
{
    public Transform[] m_portCannonPoints;
    public Transform[] m_portUpperCannonPoints;
    public Transform[] m_starboardCannonPoints;
    public Transform[] m_starboardUpperCannonPoints;
    public ParticleSystem m_cannonEffect;

    public CShipEntity mu_shipEntity;

    private Animator mi_Animator;

    bool mi_wantLerp;
    Vector3 mi_targetPos;
    Vector3 mi_startPos;
    Quaternion mi_startQuaternion;
    Quaternion mi_targetQuaternion;
    float mi_lerpTimer;

    float mi_cannonTimer;
    int mi_CannonIndex;
    bool mi_firePortCannons;
    bool mi_fireStarCannons;
    int mi_IsMovingAnimHash;

    // Use this for initialization
    void Awake () 
	{
        mi_Animator = GetComponent<Animator>();
        mi_IsMovingAnimHash = Animator.StringToHash("IsMoving");
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (!mi_wantLerp)
        {
            mi_targetPos = new Vector3(mu_shipEntity.pu_x, 0f, mu_shipEntity.pu_y) * CGameController.Get.mu_ocean.mu_cellSize;
            if (Vector3.Distance(mi_targetPos, transform.position) > 2.0f)
            {
                mi_Animator.SetBool(mi_IsMovingAnimHash, true);
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
                mi_Animator.SetBool(mi_IsMovingAnimHash, false);
            }
            else
            {
                transform.position = Vector3.Lerp(mi_startPos, mi_targetPos, mi_lerpTimer);
                transform.rotation = Quaternion.Slerp(mi_startQuaternion, mi_targetQuaternion, mi_lerpTimer);
            }
        }
        if (mi_firePortCannons)
        {
        }
    }

    public void fu_FireCannons(bool _firePort, bool _fireStar)
    {
        mi_firePortCannons = _firePort ? true : mi_firePortCannons;
        mi_fireStarCannons = _fireStar ? true : mi_fireStarCannons;
        if (_fireStar || _firePort)
        {
            mi_cannonTimer = 0.0f;
            mi_CannonIndex = 0;

            for (int i = 0; _firePort && i < m_portCannonPoints.Length; i++)
            {
                ParticleSystem effect = Instantiate(m_cannonEffect, m_portCannonPoints[i]);
                effect.startDelay = Random.Range(0.0f, 0.3f) + i * 0.02f;
                effect.Play(true);
                if (i == 2 || i == 3)
                {
                    effect = Instantiate(m_cannonEffect, m_portUpperCannonPoints[i == 1 ? 0 : 1]);
                    effect.startDelay = Random.Range(0.0f, 0.3f) + i * 0.02f;
                    effect.Play(true);
                }
            }

            for (int i = 0; _fireStar && i < m_starboardCannonPoints.Length; i++)
            {
                ParticleSystem effect = Instantiate(m_cannonEffect, m_starboardCannonPoints[i]);
                effect.startDelay = Random.Range(0.0f, 0.3f) + i * 0.02f;
                effect.Play(true);
                if (i==1 || i==4)
                {
                    effect = Instantiate(m_cannonEffect, m_starboardUpperCannonPoints[i==1 ? 0 : 1]);
                    effect.startDelay = Random.Range(0.0f, 0.3f) + i * 0.02f;
                    effect.Play(true);
                }
            }
        }
    }
}
