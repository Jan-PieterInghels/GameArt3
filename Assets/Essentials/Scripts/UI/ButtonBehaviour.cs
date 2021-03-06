﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class ButtonBehaviour : MonoBehaviour
{
    public string ChooseCharacters = "Choose character's";
    public string P1ChooseCharacter = "P1 choose character";
    public string P2ChooseCharacter = "P2 choose character";
    public string StartToPlay = "Press start to play";

    private GameObject _characterObject;
    public GameObject CharacterObject { get => _characterObject; set { _characterObject = value; } }

    private CharacterButtonSetup _characterSetup;
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
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
    }

    public void ChangeCharacter(int playerNumber)
    {
        _audioSource.clip = _characterObject.GetComponent<PlayerBehaviour>().PlayerStats.Narrator;

        if(_audioSource.clip != null)
            _audioSource.Play();

        if (playerNumber == 1 && !_characterSetup.IsPlayer1LockedIn)
        {
            _characterSetup.ChangePlayerCharacter(playerNumber, _characterObject);
            CharacterSetup.ChangeText(P2ChooseCharacter);
            _characterSetup.IsPlayer1LockedIn = true;
        }
        else if (playerNumber == 2 && !_characterSetup.IsPlayer2LockedIn)
        {
            _characterSetup.ChangePlayerCharacter(playerNumber, _characterObject);
            CharacterSetup.ChangeText(P1ChooseCharacter);
            _characterSetup.IsPlayer2LockedIn = true;
        }        

        if (_characterSetup.IsPlayer1LockedIn && _characterSetup.IsPlayer2LockedIn)
        {
            CharacterSetup.ChangeText(StartToPlay);
            _characterSetup.IsLockedIn = true; 
        }
    }

    public void Deselect(int playerNumber)
    {
        _characterSetup.ChangePlayerCharacter(playerNumber, null);

        if (playerNumber == 1)
        {
            CharacterSetup.ChangeText(P1ChooseCharacter);
            _characterSetup.IsPlayer1LockedIn = false;
        }
        else if (playerNumber == 2)
        {
            CharacterSetup.ChangeText(P2ChooseCharacter);
            _characterSetup.IsPlayer2LockedIn = false;
        }

        if (_characterSetup.IsPlayer1LockedIn && _characterSetup.IsPlayer2LockedIn)
        {
            _characterSetup.IsLockedIn = false;
        }
        if (!_characterSetup.IsPlayer1LockedIn && !_characterSetup.IsPlayer2LockedIn) CharacterSetup.ChangeText(ChooseCharacters);
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
