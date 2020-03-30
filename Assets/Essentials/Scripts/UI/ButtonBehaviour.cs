using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    private GameObject _characterObject;
    public GameObject CharacterObject { get => _characterObject; set { _characterObject = value; } }

    [SerializeField] private CharacterButtonSetup _characterSetup;
    public CharacterButtonSetup CharacterSetup { get => _characterSetup; set { _characterSetup = value; } }

    private int _characterNumber;
    public int CharacterNumber 
    { 
        get => _characterNumber; 
        set 
        { _characterNumber = value;
            _text.text = "Character " + _characterNumber;
        } 
    }

    [SerializeField] private Text _text;
    [SerializeField] private bool _isCharacterSelectButton, _isSceneChangingButton;
    [SerializeField] private string _sceneNameToLoad;

    public ButtonBehaviour(GameObject characterObject, CharacterButtonSetup characterSetup, int characterNumber)
    {
        _characterObject = characterObject;
        _characterSetup = characterSetup;
        _characterNumber = characterNumber;
    }

    public void ChangeCharacter(int playerNumber)
    {
        _characterSetup.ChangePlayerCharacter(playerNumber, _characterObject);

        if (playerNumber == 1) _characterSetup.IsPlayer1LockedIn = true;
        else if (playerNumber == 2) _characterSetup.IsPlayer2LockedIn = true;

        if (_characterSetup.IsPlayer1LockedIn && _characterSetup.IsPlayer2LockedIn) _characterSetup.IsLockedIn = true;
    }

    public void TaskOnClick(int playerNumber)
    {
        if (_isSceneChangingButton) SetSceneInSelectScreen(_sceneNameToLoad);
        else if (_isCharacterSelectButton) ChangeCharacter(playerNumber);
    }

    public void SetSceneInSelectScreen(string sceneName)
    {
        if (_characterSetup.IsLockedIn)
        {
            GameController.ChangeGameState(true);
        }
    }
}
