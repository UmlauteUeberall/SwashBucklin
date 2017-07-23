using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public AnimationCurve mu_cameraMoveCurve;

    bool mi_positionModeAuto;
    bool mi_returnToAuto;
    float mi_AnimTimer;
    Transform mi_grabberTransform;
    Vector3 mi_lookatPos;
    Vector3 mi_startPos;
    Quaternion mi_startQuaternion;

	// Use this for initialization
	void Start ()
    {
        mi_positionModeAuto = true;	
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (mi_positionModeAuto)
        {
            fi_AutoPositionCamera();
        }
        else
        {
            fi_ControlPositionCamera();
        }
	}

    void fi_AutoPositionCamera()
    {
        transform.position = new Vector3(10 * 20.0f, 100, 0.0f);
        transform.LookAt(Vector3.zero);
    }

    void fi_ControlPositionCamera()
    {
        mi_AnimTimer += Time.deltaTime;
        if (mi_AnimTimer > 1.0f)
            mi_AnimTimer = 1.0f;

        float lerpVal = mu_cameraMoveCurve.Evaluate(mi_AnimTimer);
        transform.position = Vector3.Lerp(mi_startPos, mi_grabberTransform.transform.position, lerpVal);
        if (mi_AnimTimer > 0.8f)
        {
            float slerpTimer = (mi_AnimTimer - 0.8f) * 5.0f;
            float slerpValue = mu_cameraMoveCurve.Evaluate(slerpTimer);
            transform.rotation = Quaternion.Slerp(mi_startQuaternion, mi_grabberTransform.rotation, slerpValue);
            if (mi_AnimTimer >= 1.0f)
            {
                transform.LookAt(mi_lookatPos);
            }
        }
    }

    public void fu_GrabCamera(Transform _grabberTransform, Vector3 _lookatPos)
    {
        mi_positionModeAuto = false;
        mi_returnToAuto = false;
        mi_AnimTimer = 0.0f;
        mi_grabberTransform = _grabberTransform;
        mi_startQuaternion = transform.rotation;
        mi_startPos = transform.position;
        mi_lookatPos = _lookatPos;
        SetCamMode(false);
    }

    public void fu_unGrabCamera()
    {
        mi_positionModeAuto = true;
        SetCamMode(true);
    }

    void SetCamMode(bool _ortho)
    {
        Camera.main.orthographic = _ortho;
        if (_ortho)
        {
            Camera.main.orthographicSize = 90;
        }
    }
}
