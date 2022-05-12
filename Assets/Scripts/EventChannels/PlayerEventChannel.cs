using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event Channels/Player Event Channel")]
public class PlayerEventChannel : ScriptableObject
{
    public UnityAction PlayerManagerStartedEvent;
    public UnityAction<PlayerState> StateChangedEvent;

    public void RaiseStateChangedEvent(PlayerState state)
    {
        StateChangedEvent?.Invoke(state);
    }

    public void RaisePlayerManagerStartedEvent()
    {
        PlayerManagerStartedEvent?.Invoke();
    }
}
