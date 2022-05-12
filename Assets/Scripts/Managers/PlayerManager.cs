using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerController controller;
    [SerializeField] private LayerMask platformLayerMask;

    [SerializeField] private PlayerEventChannel playerEventChannel;
    [SerializeField] private InGameEventChannel inGameEventChannel;

    //represents the player's level.
    public int level { get; private set; }

    public PlayerState state { get; private set; }

    public Transform Player => player;
    public Transform playerTarget => player.parent;
    public Transform playerParent => playerTarget.parent;

    protected override void Awake()
    {
        base.Awake();

        inGameEventChannel.GameStartedEvent += OnGameStarted;
    }

    private void OnDestroy()
    {
        inGameEventChannel.GameStartedEvent -= OnGameStarted;
    }

    private void Start()
    {
        player = player == null ? transform : player;
        controller = controller == null ? GetComponent<PlayerController>() : controller;

        playerEventChannel.RaisePlayerManagerStartedEvent();
    }

    private void Update()
    {

    }

    private void OnGameStarted()
    {
        this.ChangeState(PlayerState.Idle);
    }

    private void ChangeState(PlayerState newState)
    {
        if (newState == state)
            return;

        state = newState;
        playerEventChannel.RaiseStateChangedEvent(state);
    }
}

public enum PlayerState
{
    Idle,
    Walk
}
