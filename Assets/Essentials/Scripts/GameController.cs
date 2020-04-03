using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SoundController))]
public class GameController : MonoBehaviour
{
    [Range(1, 2)] [SerializeField] private static int _playerAmount = 2;
    public static int PlayerAmount { get => _playerAmount; set { _playerAmount = value; } }

    private static int _characterAmount;
    public static int CharacterAmount { get => _characterAmount; set { _characterAmount = value; } }

    private static Dictionary<int, GameObject> _playerCharacter;
    public static Dictionary<int, GameObject> PlayerCharacter { get => _playerCharacter; set { _playerCharacter = value; } }

    private static bool _isGamePlaying = true;
    public static bool IsGamePlaying { get => _isGamePlaying; set { _isGamePlaying = value; } }

    private static SoundController _soundControl;
    private static LinkedList<GameObject> _gameControllers = new LinkedList<GameObject>();

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
        _playerCharacter = new Dictionary<int, GameObject>();
        _soundControl = _gameControllers.Last.Value.GetComponent<SoundController>();

        for (int i = 1; i <= _playerAmount; i++)
        {
            _playerCharacter.Add(i, null);
        }
    }     
    
    public static void ChangeGameState(bool gamePlaying)
    {
        if (gamePlaying) 
        {
            _soundControl.FadeTrack("Arena_Scene");
            SceneManager.LoadScene("Arena_Scene");
        }
        _isGamePlaying = gamePlaying;
    }

    public static IEnumerator GoToCharacterSelectScreen()
    {
        yield return new WaitForSeconds(2f);
        _soundControl.FadeTrack("CharacterSelection_Scene");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("CharacterSelection_Scene");
    }

    public static void ChangeLockedInCharacter(int playerNumber, GameObject character)
    {
        _playerCharacter[playerNumber] = character;
    }
}
