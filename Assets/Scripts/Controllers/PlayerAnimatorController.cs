using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerEventChannel playerEventChannel;

    private string activeTrigger;

    private const string IDLE = "_idle";
    private const string WALK_SAD = "walk_sad";

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
        animator = animator == null ? GetComponent<Animator>() : animator;
    }

    private void TriggerAnim(string trigger, bool reset = true)
    {
        if (reset && activeTrigger != null && activeTrigger != trigger)
        {
            animator.ResetTrigger(activeTrigger);
        }

        animator.SetTrigger(trigger);
        activeTrigger = trigger;
    }

    private void OnPlayerStateChanged(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Walk:
                TriggerAnim(WALK_SAD);
                break;
            case PlayerState.Idle:
                TriggerAnim(IDLE);
                break;
        }
    }
}
