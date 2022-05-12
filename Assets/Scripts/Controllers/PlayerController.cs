using System.Collections;
using System.Collections.Generic;

using PathCreation;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[SerializeField] private Transform playerParent;
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private Vector3 platformSensorRayOffset;

    [SerializeField] private float forwardSpeed;
    [SerializeField] private float swerveSpeed;
    [SerializeField] private float swerveMoveRange;

    [SerializeField] private InGameEventChannel inGameEventChannel;
    [SerializeField] private PlayerEventChannel playerEventChannel;

    private PathCreator pathCreator;
    private float distanceTravelled;
    private float currentSpeed;

    private Vector3 currentDirection;
    private Vector3 swerveDirection => Vector3.Cross(PlayerManager.Instance.playerParent.up, currentDirection).normalized;

    private void Awake()
    {
        inGameEventChannel.LevelStartedEvent += OnLevelStarted;
        inGameEventChannel.SwerveEvent += OnSwerve;

        playerEventChannel.StateChangedEvent += OnPlayerStateChanged;
        playerEventChannel.PlayerManagerStartedEvent += OnPlayerManagerStarted;
    }

    private void OnDestroy()
    {
        inGameEventChannel.LevelStartedEvent -= OnLevelStarted;
        inGameEventChannel.SwerveEvent -= OnSwerve;

        playerEventChannel.StateChangedEvent -= OnPlayerStateChanged;
        playerEventChannel.PlayerManagerStartedEvent -= OnPlayerManagerStarted;
    }

    private void Start()
    {
        currentDirection = PlayerManager.Instance.playerParent.forward;
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        SensePlatform();
    }

    private void OnSwerve(float xSwerveValue, float ySwerveValue)
    {
        Transform playerTarget = PlayerManager.Instance.playerTarget;

        if ((xSwerveValue < 0 && playerTarget.localPosition.x >= swerveMoveRange * -1) ||
            (xSwerveValue > 0 && playerTarget.localPosition.x <= swerveMoveRange))
        {
            playerTarget.Translate(swerveSpeed * xSwerveValue * swerveDirection / 10 * Time.maximumDeltaTime, Space.World);
        }
    }

    private void OnPlayerStateChanged(PlayerState state)
    {
        switch (state)
        {
            default:
                break;
        }
    }

    private void OnPlayerManagerStarted()
    {

    }

    private void OnLevelStarted()
    {
        enabled = true;
        currentSpeed = forwardSpeed;
    }

    private void Move()
    {
        if (pathCreator != null)
        {
            distanceTravelled += currentSpeed * Time.deltaTime;

            Quaternion rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
            PlayerManager.Instance.playerParent.rotation = rotation;
        }

        PlayerManager.Instance.playerParent.Translate(currentSpeed * Time.deltaTime * currentDirection, Space.World);
    }

    private void SensePlatform()
    {
        Vector3 initialRayPosition = PlayerManager.Instance.playerTarget.position + platformSensorRayOffset;

        if (Physics.Raycast(initialRayPosition, Vector3.down, out RaycastHit hit, 5f, platformLayerMask))
        {
            if (hit.transform.TryGetComponent(out PathCreator pathCreator))
            {
                this.pathCreator = pathCreator;
                currentDirection = pathCreator.path.GetDirectionAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
            }
            else if (hit.transform.TryGetComponent(out Platform platform))
            {
                if (this.pathCreator != null)
                {
                    this.pathCreator = null;
                }

                currentDirection = platform.direction;
            }
            else
            {
                if (this.pathCreator != null)
                {
                    this.pathCreator = null;
                }
            }
        }
    }

}
