using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ocean;

public class CameraController : MonoBehaviour
{
    public AnimationCurve mu_cameraMoveCurve;
    public float mu_rotateAngleSpeed;
    public float height;
    public float distance;

    bool mi_positionModeAuto;
    bool mi_returnToAuto;
    float mi_AnimTimer;
    Transform mi_grabberTransform;
    Vector3 mi_lookatPos;
    Vector3 mi_startPos;
    Quaternion mi_startQuaternion;
    float mi_slowRotateAngle = 0.0f;

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
        float maxDistance = 0.0f;
        Vector3 center = fi_CalcCenterBetweenShips(out maxDistance);
        transform.RotateAround(center, Vector3.up, Time.deltaTime * mu_rotateAngleSpeed);
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        transform.LookAt(center);

        transform.position = center - transform.forward * maxDistance * 20;
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
        transform.LookAt(center);
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

    Vector3 fi_CalcCenterBetweenShips(out float _maxDistance)
    {
        List<AOceanEntityView>  views = CGameController.Get.mu_ocean.fu_GetViewsOfType(EOceanEntityType.Ship);

        Vector3 center = new Vector3(0,0,0);
        if (views.Count == 0)
        {
            _maxDistance = 0;
            return center;
        }

        float xMin = 100000;
        float xMax = -100000;
        float yMin = 100000;
        float yMax = -100000;
        foreach(var v in views)
        {
            float lx = v.transform.position.x;
            float ly = v.transform.position.z;
            if (lx > xMax)
                xMax = lx;
            else if (lx < xMin)
                xMin = lx;
            if (ly > yMax)
                yMax = ly;
            else if (ly < yMin)
                yMin = ly;

            center += v.transform.position;
        }
        center.x = center.x / views.Count;
        center.y = center.y / views.Count;
        center.z = center.z / views.Count;

        if (xMax - xMin > yMax - yMin)
            _maxDistance = xMax - xMin;
        else
            _maxDistance = yMax - yMin;

        return center;
    }
}
