using UnityEngine;

public class GameModes : MonoBehaviour
{
    [SerializeField] private GameObject EasyMode;
    [SerializeField] private GameObject MediumMode;
    [SerializeField] private GameObject HardMode;
    [SerializeField] private GameObject ExtremeMode;

    [SerializeField] private Vector3 StartPoz = new Vector3(-1.62f,-1.6f,14.8599997f);

    [SerializeField] private bool Test_Easy;
    [SerializeField] private bool Test_Medium;
    [SerializeField] private bool Test_Hard;
    [SerializeField] private bool Test_Extreme;

    [SerializeField] private int StartAudioDelay_Easy = 5;
    [SerializeField] private int StartAudioDelay_Medium = 4;
    [SerializeField] private int StartAudioDelay_Hard = 3;
    [SerializeField] private int StartAudioDelay_Extreme = 2;

    private GameObject tutorial;

    [SerializeField] private bool Nosound_Test_All;
    private static GameModes instance;

    private bool GameStart;
    private Animator animator;
    private static readonly int _gamestart = Animator.StringToHash("Game Started");


    private void Start()
    {
        animator = GetComponent<Animator>();

        tutorial = GameObject.FindGameObjectWithTag("Tutorial");
    }

    public static GameModes Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameModes>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("GameModes");
                    instance = singletonObject.AddComponent<GameModes>();
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnselectEasyMode()
    {
        GameObject inst = Instantiate(EasyMode,StartPoz,Quaternion.identity);
        inst.GetComponent<Spawner>().StartGame();
        HideButtons();
        FindObjectOfType<AudioManager>().PlaySound("Easy_GameMode", StartAudioDelay_Easy);
    }

    public void OnselectMediumMode()
    {
        GameObject inst = Instantiate(MediumMode, StartPoz, Quaternion.identity);
        inst.GetComponent<Spawner>().StartGame();
        HideButtons();
        FindObjectOfType<AudioManager>().PlaySound("Medium_GameMode", StartAudioDelay_Medium);
    }

    public void OnselectHardMode()
    {
        GameObject inst = Instantiate(HardMode, StartPoz, Quaternion.identity);
        inst.GetComponent<Spawner>().StartGame();
        HideButtons();
        FindObjectOfType<AudioManager>().PlaySound("Hard_GameMode", StartAudioDelay_Hard);
    }

    public void OnselectExtremeMode()
    {
        GameObject inst = Instantiate(ExtremeMode, StartPoz, Quaternion.identity);
        inst.GetComponent<Spawner>().StartGame();
        HideButtons();
        FindObjectOfType<AudioManager>().PlaySound("Extreme_GameMode", StartAudioDelay_Extreme);
    }
    void Test_InstantiateAndStartGame(GameObject mode, string soundname)
    {
        GameObject inst = Instantiate(mode, StartPoz, Quaternion.identity);
        inst.GetComponent<Spawner>().StartGame();
        HideButtons();
        FindObjectOfType<AudioManager>().PlaySound(soundname);
    }

    void Test_InstantiateAndStartGameNosound(GameObject mode)
    {
        GameObject inst = Instantiate(mode, StartPoz, Quaternion.identity);
        inst.GetComponent<Spawner>().StartGame();
        HideButtons();
    }

    public void HideButtons()
    {
        GameStart = true;
        animator.SetBool(_gamestart, GameStart);
        tutorial.SetActive(false);
    }

    public void ShowButtons()
    {
        GameStart = false;
        animator.SetBool(_gamestart, GameStart);
        tutorial.SetActive(true);
    }

    private void Update()
    {
        if (this.gameObject.activeSelf)
        {
            if (Test_Easy)
            {
                Test_InstantiateAndStartGame(EasyMode, "Easy_GameMode");
            }
            else if (Test_Medium)
            {
                Test_InstantiateAndStartGame(MediumMode, "Medium_GameMode");
            }
            else if (Test_Hard)
            {
                Test_InstantiateAndStartGame(HardMode, "Hard_GameMode");
            }
            else if (Test_Extreme)
            {
                Test_InstantiateAndStartGame(ExtremeMode, "Extreme_GameMode");
            }
            else if (Nosound_Test_All)
            {
                Test_InstantiateAndStartGameNosound(EasyMode);
                Test_InstantiateAndStartGameNosound(MediumMode);
                Test_InstantiateAndStartGameNosound(HardMode);
                Test_InstantiateAndStartGameNosound(ExtremeMode);
            }
        }
    }

}
