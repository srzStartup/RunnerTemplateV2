using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-5)]
public class GameManager : Singleton<GameManager>
{
    public int level { get; private set; }
    public bool started { get; private set; }
    public bool ended { get; private set; }
    public float currentMoney { get; private set; } = 0;
    public int multiplier = 1;

    [SerializeField] private InGameEventChannel inGameEventChannel;
    [SerializeField] private PlayerEventChannel playerEventChannel;

    void Start()
    {
        inGameEventChannel.RaiseGameStartedEvent();
    }

    public void EndLevel(bool success = true)
    {
        ended = true;
    }

    public void NextLevel()
    {
        //DOTween.KillAll();
        //SavePlayerPrefs();

        //PlayerPrefs.SetInt("currentLevel", level + 1);
        //hasLevel++;
        //if (hasLevel > 15)
        //{
        //    hasLevel = 7;
        //}
        //PlayerPrefs.SetInt("HASLEVEL", hasLevel);
        //SceneManager.LoadScene(hasLevel);
    }

    public void UpdateMoney(float amount)
    {
        currentMoney += amount;
    }
}
