using System.Collections;
using System.Collections.Generic;

using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : Singleton<UIManager>
{
    [Header("Screens")]
    [SerializeField] private RectTransform inGameScreen;
    [SerializeField] private RectTransform startScreen;
    [SerializeField] private RectTransform winScreen;
    [SerializeField] private RectTransform loseScreen;
    [SerializeField] private RectTransform shopScreen;

    [Space(5)]

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI inGameScreen_CollectibleText;
    [SerializeField] private TextMeshProUGUI inGameScreen_LevelText;
    [SerializeField] private TextMeshProUGUI inGameScreen_incomePerSecondText;
    [SerializeField] private TextMeshProUGUI startScreen_LevelText;
    [SerializeField] private TextMeshProUGUI startScreen_CollectibleText;
    [SerializeField] private TextMeshProUGUI startScreen_incomePerSecondText;
    [SerializeField] private TextMeshProUGUI winScreen_LevelEndMoneyText;
    [SerializeField] private TextMeshProUGUI winScreen_CollectibleText;

    [Space(5)]

    [Header("Colors")]
    [SerializeField] private Color levelUpTextColor;
    [SerializeField] private Color levelDownTextColor;
    [SerializeField] private Color wealthUpTextColor;
    [SerializeField] private Color wealthDownTextColor;

    [Space(5)]

    [Header("Images")]
    [SerializeField] private Transform tapToStartImage;
    [SerializeField] private List<GameObject> lights;
    [SerializeField] private RectTransform winScreen_moneyImageRef;

    [Space(5)]
    [Header("Lights")]
    [SerializeField] private RectTransform winScreenLight;

    [Space(5)]
    [Header("LevelEnd")]
    [SerializeField] private RectTransform winScreen_incomeBG;
    [SerializeField] private Button nextButton;

    [Space(5)]

    [Header("Event Channels")]
    [SerializeField] private PlayerEventChannel playerEventChannel;
    [SerializeField] private InGameEventChannel inGameEventChannel;

    //protected override void Awake()
    //{
    //    base.Awake();

    //    inGameEventChannel.GameStartedEvent += OnGameStarted;
    //    inGameEventChannel.LevelStartedEvent += OnLevelStarted;
    //    inGameEventChannel.LevelAccomplishedEvent += OnLevelAccomplished;

    //    playerEventChannel.CollectibleCollectedEvent += OnCollectibleCollected;
    //    playerEventChannel.PlayerManagerStartedEvent += OnPlayerManagerStarted;
    //}

    //void OnDestroy()
    //{
    //    inGameEventChannel.GameStartedEvent -= OnGameStarted;
    //    inGameEventChannel.LevelStartedEvent -= OnLevelStarted;
    //    inGameEventChannel.LevelAccomplishedEvent -= OnLevelAccomplished;

    //    playerEventChannel.CollectibleCollectedEvent -= OnCollectibleCollected;
    //    playerEventChannel.PlayerManagerStartedEvent -= OnPlayerManagerStarted;
    //}

    private void Start()
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

        //StartCoroutine(TapToStartAnimationCoroutine());
        StartCoroutine(BoostsAnimationCoroutine(/*buraya boostlar gelecek*/));
    }

    private void OnGameStarted()
    {
        //ElephantSDK.Elephant.LevelStarted(currentLevel);

        startScreen_LevelText.text = "Level " + GameManager.Instance.level.ToString();
        startScreen_CollectibleText.text = Mathf.RoundToInt(GameManager.Instance.currentMoney).ToString();
    }


    private void OnLevelStarted(float currentMoney)
    {
        inGameScreen_CollectibleText.text = Mathf.RoundToInt(currentMoney).ToString();
        inGameScreen_LevelText.text = "Level " + GameManager.Instance.level.ToString();

        startScreen.gameObject.SetActive(false);
        inGameScreen.gameObject.SetActive(true);
    }

    private void OnLevelAccomplished()
    {
        winScreen_CollectibleText.text = Mathf.RoundToInt(GameManager.Instance.currentMoney).ToString();
        winScreenLight.DORotate(new Vector3(0, 0, 360), 10f, RotateMode.FastBeyond360)
            .SetLoops(-1)
            .SetEase(Ease.Linear);

        inGameScreen.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(true);
    }

    private void OnCollectibleCollected(Collectible collectible)
    {
        
    }

    IEnumerator BoostsAnimationCoroutine(params Transform[] boosts)
    {
        yield return new WaitForSeconds(2.0f);

        Sequence sequence = DOTween.Sequence();

        Ease ease = Ease.InOutBounce;
        Vector3 punchScale = Vector3.one * .1f;
        float duration = .5f;
        int vibrato = 1;


        foreach (Transform boost in boosts)
        {
            sequence.Join(boost.DOPunchScale(punchScale, duration, vibrato).SetEase(ease));
        }

        sequence.SetLoops(-1, LoopType.Yoyo);
        sequence.SetDelay(1.5f);
    }

    IEnumerator TapToStartAnimationCoroutine()
    {
        while (true)
        {
            tapToStartImage.DOMove(new Vector3(tapToStartImage.position.x, tapToStartImage.position.y * 4 / 3, tapToStartImage.position.z), 1f)
                .OnComplete(() =>
                {
                    tapToStartImage.DOMove(new Vector3(tapToStartImage.position.x, tapToStartImage.position.y * 3 / 4, tapToStartImage.position.z), 1f);
                });

            tapToStartImage.DOScale(tapToStartImage.localScale.x * 1.2f, 1f)
                .OnComplete(() => {
                    tapToStartImage.DOScale(tapToStartImage.localScale.x / 1.2f, 1f);
                });

            yield return new WaitForSeconds(3f);
        }
    }

    //public void OnTapToStartButtonClicked()
    //{
    //    Taptic.Light();

    //    inGameEventChannel.RaiseTapToStartClickedEvent();
    //}

    public void DoneButtonClick()
    {
        //Taptic.Light();

        winScreen_LevelEndMoneyText.text = PlayerManager.Instance.level.ToString();

        inGameScreen.gameObject.SetActive(false);
        shopScreen.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(true);

        //Sequence sequence = DOTween.Sequence();
        //float totalDuration = 26 * .05f;

        //for (int i = 1; i < 26; i++)
        //{
        //    GameObject moneyImageGO = ObjectPooler.Instance
        //        .SpawnFromPool("levelEndMoneyImage", winScreen_incomeBG.Find("MoneyImage").position, Quaternion.identity, false);
        //    RectTransform moneyImageTransform = moneyImageGO.GetComponent<RectTransform>();

        //    Tween tween = moneyImageTransform.DOMove(winScreen_moneyImageRef.position, i * .05f)
        //        .SetEase(Ease.InBack)
        //        .OnStart(() =>
        //        {
        //            moneyImageTransform.localScale = Vector3.one * 1.5f;
        //            moneyImageGO.SetActive(true);
        //        })
        //        .OnComplete(() =>
        //        {
        //            Taptic.Light();
        //            moneyImageTransform.gameObject.SetActive(false);
        //        });

        //    sequence.Join(tween);
        //}

        //sequence.SetDelay(1.5f)
        //    .SetEase(Ease.Linear)
        //    .OnComplete(() => nextButton.gameObject.SetActive(true));

        //StartCoroutine(UpdateMoneyDelayed(PlayerManager.Instance.level, totalDuration / (PlayerManager.Instance.level)));

        GameManager.Instance.EndLevel();
    }

    public void NextLevelButtonClick()
    {
        Taptic.Light();

        GameManager.Instance.NextLevel();
    }

    IEnumerator UpdateMoneyDelayed(int counter, float delay)
    {
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < counter; i++)
        {
            GameManager.Instance.UpdateMoney(1);
            yield return new WaitForSeconds(delay);
        }
    }
}
