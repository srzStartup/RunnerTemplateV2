using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event Channels/In Game Event Channel")]
public class InGameEventChannel : ScriptableObject
{
    // Game states
    public UnityAction GameStartedEvent;
    public UnityAction LevelStartedEvent;
    public UnityAction LevelAccomplishedEvent;
    public UnityAction LevelFailedEvent;

    // screen input events
    public UnityAction<float, float> SwerveEvent;
    public UnityAction<Vector3> ScreenTouchedEvent;
    public UnityAction ScreenTouchEndedEvent;

    public void RaiseGameStartedEvent()
    {
        GameStartedEvent?.Invoke();
    }

    public void RaiseLevelStartedEvent()
    {
        LevelStartedEvent?.Invoke();
    }

    public void RaiseLevelAccomplishedEvent()
    {
        LevelAccomplishedEvent?.Invoke();
    }

    public void RaiseLevelFailedEvent()
    {
        LevelFailedEvent?.Invoke();
    }

    public void RaiseScreenTouchedEvent(Vector3 touchPosition)
    {
        ScreenTouchedEvent?.Invoke(touchPosition);
    }

    public void RaiseScreenTouchEndedEvent()
    {
        ScreenTouchEndedEvent?.Invoke();
    }

    public void RaiseSwerveEvent(float xPerstentage, float yPersentage)
    {
        SwerveEvent?.Invoke(xPerstentage, yPersentage);
    }
}
