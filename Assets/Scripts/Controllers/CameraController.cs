using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraParent;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform target;
    [SerializeField, Range(.0f, 100.0f)] private float distance;
    [SerializeField, Range(1.0f, 20.0f)] private float lerpTime;

    [SerializeField] private PlayerEventChannel playerEventChannel;

    private Vector3 positionOffset;
    private Quaternion rotationOffset;

    private void Awake()
    {
        playerEventChannel.StateChangedEvent += OnPlayerStateChanged;
    }

    private void OnDestroy()
    {
        playerEventChannel.StateChangedEvent -= OnPlayerStateChanged;
    }

    private void Start()
    {
        cameraParent = cameraParent == null ? transform.parent : cameraParent;
        _camera = _camera == null ? transform : _camera;

        positionOffset = _camera.position;
        rotationOffset = cameraParent.rotation;
    }

    void LateUpdate()
    {
        rotationOffset.y = target.parent.rotation.y;

        cameraParent.position = Vector3.Lerp(cameraParent.position, target.position, Time.deltaTime * lerpTime);
        cameraParent.rotation = Quaternion.Euler(Vector3.Lerp(cameraParent.rotation.eulerAngles, target.rotation.eulerAngles, Time.deltaTime * 1000 *lerpTime));
    }

    private void OnPlayerStateChanged(PlayerState state)
    {
    }
}