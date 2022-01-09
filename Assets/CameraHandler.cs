using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MoreMountains.Feedbacks;
public class CameraHandler : MonoBehaviour
{
    public static CameraHandler instance;

    [SerializeField] GlobalHelper.Perspective defaultPerspective;
    [SerializeField] Camera playerCam;
    [SerializeField] CameraPoints cameraPoints;
    [SerializeField] Feels feelsDetails;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        CamStateSwitch(defaultPerspective);
    }


    public void CamStateSwitch(GlobalHelper.Perspective perspective)
    {
        Transform newPos = null;
        switch (perspective)
        {
            case GlobalHelper.Perspective.firstPerson:
                newPos = cameraPoints.firstPerson;
                break;
            case GlobalHelper.Perspective.thirdPerson:
                newPos = cameraPoints.thirdPerson;
                break;
            case GlobalHelper.Perspective.sideScroller:
                newPos = cameraPoints.sideScroller;
                break;
            case GlobalHelper.Perspective.topDown:
                newPos = cameraPoints.topDown;
                break;
            default:
                break;
        }
        feelsDetails.MoveCamera(newPos);
    }
}

[System.Serializable]
public class CameraPoints
{
    public Transform firstPerson;
    public Transform thirdPerson;
    public Transform sideScroller;
    public Transform topDown;
}

[System.Serializable]
public class Feels
{
    [HideInInspector]
    public Transform targetPosition;
    public MMFeedbacks moveCameraFeels;
    public UnityEvent onCameraAnimationBegin;
    public UnityEvent onCameraAnimationComplete;

    public void MoveCamera(Transform newTarget)
    {
        if(newTarget != targetPosition)
        {
            SetupListener();
            targetPosition = newTarget;

            if(moveCameraFeels.TryGetComponent(out MMFeedbackDestinationTransform destination))
            {
                destination.TargetTransform = targetPosition;
            }
            moveCameraFeels.Initialization();
            moveCameraFeels.PlayFeedbacks();
        }
    }

    void SetupListener()
    {
        UnityAction startAction = null;
        startAction = new UnityAction(delegate () { onCameraAnimationBegin.Invoke(); moveCameraFeels.Events.OnPlay.RemoveListener(startAction); });
        moveCameraFeels.Events.OnPlay.AddListener(startAction);

        UnityAction endAction = null;
        endAction = new UnityAction(delegate () { onCameraAnimationComplete.Invoke(); moveCameraFeels.Events.OnComplete.RemoveListener(endAction); });
        moveCameraFeels.Events.OnPlay.AddListener(endAction);
    }
}

[System.Serializable]
public class CameraScripts
{
   /* List<camScript> scripts;

    public void EnableOnlyThisScript(camScript wantedScript)
    {
        foreach (camScript item in scripts)
        {
            if(item != wantedScript)
            {
                item.enabled = false;
            }

            else
            {
                item.enabled = true;
            }
        }
    }
   */
}
