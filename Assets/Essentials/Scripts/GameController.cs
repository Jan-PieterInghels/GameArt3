using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SoundController))]
public class GameController : MonoBehaviour
{

    [SerializeField] private float _timeTillEndRound;
    private static float TIMETILLEND;
    [Range(1, 2)] [SerializeField] private int _playerAmount = 2;
    private static int PLAYERAMOUNT;
    public static int PlayerAmount { get => PLAYERAMOUNT; set { PLAYERAMOUNT = value; } }

    private static int _characterAmount;
    public static int CharacterAmount { get => _characterAmount; set { _characterAmount = value; } }

    private static Dictionary<int, GameObject> _playerCharacter;
    public static Dictionary<int, GameObject> PlayerCharacter { get => _playerCharacter; set { _playerCharacter = value; } }

    private static bool _isGamePlaying = true;
    public static bool IsGamePlaying { get => _isGamePlaying; set { _isGamePlaying = value;} }

    private static SoundController _soundControl;
    private static LinkedList<GameObject> _gameControllers = new LinkedList<GameObject>();

    private static GameController INSTANCE;

    [SerializeField] private AudioSource _winSource;
    private static AudioSource WINSOURCE;

    private void Awake()
    {
        CheckDontDestroyOnLoad();

        Setup();
    }

    private void CheckDontDestroyOnLoad()
    {
        _gameControllers.AddLast(this.gameObject);
        if (_gameControllers.Count > 1)
        {
            Destroy(_gameControllers.Last.Value);
        }
        else
            DontDestroyOnLoad(this.gameObject);
    }

    private void Setup()
    {
        INSTANCE = _gameControllers.First.Value.GetComponent<GameController>();
        _playerCharacter = new Dictionary<int, GameObject>();
        _soundControl = _gameControllers.First.Value.GetComponent<SoundController>();
        _winSource = _gameControllers.First.Value.GetComponentInChildren<AudioSource>();
        WINSOURCE = _winSource;
        WINSOURCE.loop = false;

        for (int i = 1; i <= _playerAmount; i++)
        {
            _playerCharacter.Add(i, null);
        }

        PLAYERAMOUNT = _playerAmount;
        TIMETILLEND = _timeTillEndRound;
    }    
    
    public static void PlayVictory(int playerNumber)
    {
        WINSOURCE.clip = _playerCharacter[playerNumber].GetComponent<PlayerBehaviour>().PlayerStats.Victory;
        WINSOURCE.Play();
    }

    public static void ChangeGameState(bool gamePlaying, PlayerBehaviour beh)
    {
        if (beh.PlayerNumber == 1) PlayVictory(2);
        else PlayVictory(1);

        _isGamePlaying = gamePlaying;
        INSTANCE.StartCoroutine(GoToCharacterSelectScreen());
    }

    public static void ChangeGameState(bool gamePlaying)
    {
        _isGamePlaying = gamePlaying;
        if (gamePlaying)
        {
            _soundControl.FadeTrack("Arena_Scene");
            SceneManager.LoadScene("Arena_Scene");
        }
    }

    public static IEnumerator GoToCharacterSelectScreen()
    {
        yield return new WaitForSeconds(TIMETILLEND - 1f);
        _soundControl.FadeTrack("CharacterSelection_Scene");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("CharacterSelection_Scene");
    }

    public static void ChangeLockedInCharacter(int playerNumber, GameObject character)
    {
        _playerCharacter[playerNumber] = character;
    }
}
