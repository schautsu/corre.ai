using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ArenaController : MonoBehaviour
{
    public static ArenaController Instance { get; private set; }

    [SerializeField] float startMoveSpeed;
    [SerializeField] float speedBoost;
    [Header("Panels")]
    [SerializeField] GameObject caughtPanel;
    [SerializeField] GameObject timeOutPanel;
    [Header("Players Points UI")]
    [SerializeField] TextMeshProUGUI playerWASDPointsText;
    [SerializeField] TextMeshProUGUI playerArrowsPointsText;
    [Header("Players Speed UI")]
    [SerializeField] TextMeshProUGUI playersSpeedIndicator;
    [Header("Players Sprites")]
    [SerializeField] Sprite hideSprite;
    [SerializeField] Sprite seekSprite;
    [Header("Grid Button Sprites")]
    [SerializeField] Sprite gridOnSprite;
    [SerializeField] Sprite gridOffSprite;
    [Header("Options Button")]
    [SerializeField] Button optionsButton;

    GameObject playerWASD, playerArrows;
    Vector2 startPosPlayerWASD, startPosPlayerArrows;
    Timer timer;
    Collectable collectable;

    bool isGridOn = true;

    int currentSeeker;
    int[] playerPoints = new int[2];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        timer = GetComponent<Timer>();
        playerWASD = FindAnyObjectByType<PlayerWASD>().gameObject;
        playerArrows = FindAnyObjectByType<PlayerArrows>().gameObject;
        collectable = FindAnyObjectByType<Collectable>();
    }

    private void Start()
    {
        RenewCollectable();

        startPosPlayerWASD = playerWASD.transform.position;
        startPosPlayerArrows = playerArrows.transform.position;

        currentSeeker = Random.Range(0,2);

        UpdateSeekHidePlayers();

        playerWASD.GetComponent<PlayerWASD>().moveSpeed = startMoveSpeed;
        playerArrows.GetComponent<PlayerArrows>().moveSpeed = startMoveSpeed;

        playerWASD.GetComponent<PlayerWASD>().speedBoost = speedBoost;
        playerArrows.GetComponent<PlayerArrows>().speedBoost = speedBoost;

        playerWASD.GetComponent<PlayerWASD>().boostLimitTime = 1.0f;
        playerArrows.GetComponent<PlayerArrows>().boostLimitTime = 1.0f;

        playersSpeedIndicator.text = playerWASD.GetComponent<PlayerWASD>().moveSpeed.ToString();
    }

    void UpdateSeekHidePlayers()
    {
        if (currentSeeker == 0)
        {
            playerWASD.GetComponent<SpriteRenderer>().sprite = seekSprite;
            playerArrows.GetComponent<SpriteRenderer>().sprite = hideSprite;
        }
        else
        {
            playerWASD.GetComponent<SpriteRenderer>().sprite = hideSprite;
            playerArrows.GetComponent<SpriteRenderer>().sprite = seekSprite;
        }
    }

    void RenewCollectable()
    {
        collectable.gameObject.SetActive(false);
        collectable.Invoke(nameof(collectable.RandomisePosition), Random.Range(collectable.minSpawnRate, collectable.maxSpawnRate));
    }

    public void Caught()
    {
        playerPoints[currentSeeker]++;

        caughtPanel.SetActive(true);

        StartCoroutine(nameof(ReloadScenario), caughtPanel);
    }

    public void TimeOut()
    {
        playerPoints[(currentSeeker + 1) % playerPoints.Length]++;

        timeOutPanel.SetActive(true);

        StartCoroutine(nameof(ReloadScenario), timeOutPanel);
    }

    IEnumerator ReloadScenario(GameObject obj)
    {
        timer.TimerRunning(false);
        optionsButton.interactable = false;

        playerWASDPointsText.text = playerPoints[0].ToString();
        playerArrowsPointsText.text = playerPoints[1].ToString();

        Rigidbody2D rbWASD = playerWASD.GetComponent<Rigidbody2D>();
        Rigidbody2D rbArrows = playerArrows.GetComponent<Rigidbody2D>();

        rbWASD.simulated = rbArrows.simulated = false;
        rbWASD.linearVelocity = rbArrows.linearVelocity = new Vector2(0f, 0f);

        yield return new WaitForSeconds(2f);

        currentSeeker = (currentSeeker + 1) % 2;

        UpdateSeekHidePlayers();
        RenewCollectable();

        playerWASD.transform.position = startPosPlayerWASD;
        playerArrows.transform.position = startPosPlayerArrows;

        obj.SetActive(false);

        timer.RestartTimer();
        timer.TimerRunning(true);
        optionsButton.interactable = true;

        rbWASD.simulated = rbArrows.simulated = true;
    }

    public void IncreasePlayersSpeed()
    {
        PlayerWASD pWASD = playerWASD.GetComponent<PlayerWASD>();
        PlayerArrows pArrows = playerArrows.GetComponent<PlayerArrows>();

        if (pWASD.moveSpeed < 32)
        {
            pWASD.moveSpeed++;
            pArrows.moveSpeed++;
        }
        playersSpeedIndicator.text = pWASD.moveSpeed.ToString();
    }

    public void DecreasePlayersSpeed()
    {
        PlayerWASD pWASD = playerWASD.GetComponent<PlayerWASD>();
        PlayerArrows pArrows = playerArrows.GetComponent<PlayerArrows>();

        if (pWASD.moveSpeed > 6)
        {
            pWASD.moveSpeed--;
            pArrows.moveSpeed--;
        }
        playersSpeedIndicator.text = pWASD.moveSpeed.ToString();
    }

    public void ChangeGridState(GameObject caller)
    {
        isGridOn = !isGridOn;

        TilemapRenderer[] rends = FindObjectsByType<TilemapRenderer>(FindObjectsSortMode.None);

        foreach (var rend in rends)
        {
            rend.enabled = isGridOn;
        }
        caller.GetComponentInChildren<TextMeshProUGUI>().text = isGridOn ? "ATIVADO" : "DESATIVADO";
        caller.GetComponent<Image>().sprite = isGridOn ? gridOnSprite : gridOffSprite;
    }

    public void ReturnToMainMenu()
    {
        SceneController.Instance.LoadNewScene(0);
    }
}
