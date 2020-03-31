using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    private GameObject _characterObject;
    public GameObject CharacterObject { get => _characterObject; set { _characterObject = value; } }

    [SerializeField] private CharacterButtonSetup _characterSetup;
    public CharacterButtonSetup CharacterSetup { get => _characterSetup; set { _characterSetup = value; } }

    private Sprite _characterSprite;
    public Sprite CharacterSprite 
    { 
        get => _characterSprite; 
        set 
        {
            _characterSprite = value;
            _image.sprite = value;
        } 
    }

    [SerializeField] private Image _image;

    public void ChangeCharacter(int playerNumber)
    {

        if (playerNumber == 1 && !_characterSetup.IsPlayer1LockedIn)
        {
            _characterSetup.ChangePlayerCharacter(playerNumber, _characterObject);
            CharacterSetup.ChangeText("Player 2 you need to lock in");
            _characterSetup.IsPlayer1LockedIn = true;
        }
        else if (playerNumber == 2 && !_characterSetup.IsPlayer2LockedIn)
        {
            _characterSetup.ChangePlayerCharacter(playerNumber, _characterObject);
            CharacterSetup.ChangeText("Player 1 you need to lock in");
            _characterSetup.IsPlayer2LockedIn = true;
        }        

        if (_characterSetup.IsPlayer1LockedIn && _characterSetup.IsPlayer2LockedIn)
        {
            CharacterSetup.ChangeText("Press Start to start game");
            _characterSetup.IsLockedIn = true; 
        }
    }

    public void Deselect(int playerNumber)
    {
        _characterSetup.ChangePlayerCharacter(playerNumber, null);

        if (playerNumber == 1)
        {
            CharacterSetup.ChangeText("Player 1 you need to lock in");
            _characterSetup.IsPlayer1LockedIn = false;
        }
        else if (playerNumber == 2)
        {
            CharacterSetup.ChangeText("Player 2 you need to lock in");
            _characterSetup.IsPlayer2LockedIn = false;
        }

        if (_characterSetup.IsPlayer1LockedIn && _characterSetup.IsPlayer2LockedIn)
        {
            _characterSetup.IsLockedIn = false;
        }
        if (!_characterSetup.IsPlayer1LockedIn && !_characterSetup.IsPlayer2LockedIn) CharacterSetup.ChangeText("Lock in your characters");
    }

    public void TaskOnClick(int playerNumber)
    {
        ChangeCharacter(playerNumber);
    }

    public void SetSceneInSelectScreen()
    {
        if (_characterSetup.IsLockedIn)
        {
            GameController.ChangeGameState(true);
        }
    }
}
