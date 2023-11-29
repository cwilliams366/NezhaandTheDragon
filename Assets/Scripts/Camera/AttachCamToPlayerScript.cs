using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttachCamToPlayerScript : MonoBehaviour
{
    private Camera mainCam;
    private ICinemachineCamera cinemachineCamera;
    private UnityAction<Transform> setCameraAction;
    private void Awake()
    {
        cinemachineCamera = GetComponent<ICinemachineCamera>();
        mainCam = GetComponent<Camera>();
        setCameraAction = new UnityAction<Transform>(SetCameraTarget);
    }

    private void SetCameraTarget(Transform camTarget)
    {
        cinemachineCamera.Follow = camTarget;
        CameraScript.target = camTarget;
        //cinemachineCamera.VirtualCameraGameObject.transform.parent = camTarget;
    }

    private void OnEnable()
    {
        Debug.Log("Cam active");
        PlayerEventsScript.PlayerSpawned += setCameraAction;
    }

    private void OnDisable()
    {
        PlayerEventsScript.PlayerSpawned -= setCameraAction;
    }
}
