using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButtonSetup : MonoBehaviour
{
    [SerializeField] private ChangeStats _player1Stats, _player2Stats;
    [SerializeField] private GameObject[] _characterObjects;
    [SerializeField] private ButtonBehaviour _characterButtonBehaviour;
    [SerializeField] private GetRefCharacter[] _playerRefs;
    [SerializeField] private GameObject _player1RefSpawnpoint;
    public GameObject Player1RefSpawnpoint => _player1RefSpawnpoint;
    [SerializeField] private GameObject _player2RefSpawnpoint;
    public GameObject Player2RefSpawnpoint => _player2RefSpawnpoint;
    [SerializeField] private Text _text;

    private int _characterAmount;

    public bool IsLockedIn;
    public bool IsPlayer1LockedIn, IsPlayer2LockedIn;

    private int _playerAmount;
    public int PlayerAmount => _playerAmount;

    float maxHealthValue = 0;
    float maxDefenceValue = 0;
    float maxNormalAttValue = 0;
    float maxHeavyAttValue = 0;
    float maxSpeedValue = 0;

    private List<ButtonBehaviour> _buttonList = new List<ButtonBehaviour>();

    // Start is called before the first frame update
    void Start()
    {
        GameController.CharacterAmount = _characterObjects.Length;
        _characterAmount = GameController.CharacterAmount;
        _playerAmount = GameController.PlayerAmount;
        _text.text = "Lock in your characters";

        _player1Stats.gameObject.SetActive(false);
        _player2Stats.gameObject.SetActive(false);

        for (int i = 1; i <= _characterAmount; i++)
        {
            ButtonBehaviour beh = Instantiate(_characterButtonBehaviour, this.transform);
            beh.CharacterSprite = _characterObjects[i - 1].GetComponent<PlayerBehaviour>().PlayerStats.CharacterPortret;
            beh.CharacterSetup = this;
            beh.CharacterObject = _characterObjects[i - 1];

            _buttonList.Add(beh);
        }

        foreach (var item in _characterObjects)
        {
            PlayerBehaviour beh = item.GetComponent<PlayerBehaviour>();
            CheckValues(beh.PlayerStats);
        }

        SetValuesSliders(_player1Stats);
        SetValuesSliders(_player2Stats);
    }

    private void SetValuesSliders(ChangeStats stats)
    {
        stats.HealthSlider.maxValue = maxHealthValue;
        stats.HeavySlider.maxValue = maxHeavyAttValue;
        stats.DefenceSlider.maxValue = maxDefenceValue;
        stats.SpeedSlider.maxValue = maxSpeedValue;
        stats.NormalSlider.maxValue = maxNormalAttValue;
    }

    private void CheckValues(CharacterStats playerStats)
    {
        if (playerStats.Health > maxHealthValue) maxHealthValue = playerStats.Health;
        if (playerStats.Defence > maxDefenceValue) maxDefenceValue = playerStats.Defence;
        if (playerStats.NormalAttackDamage > maxNormalAttValue) maxNormalAttValue = playerStats.NormalAttackDamage;
        if (playerStats.HeavyAttackDamage > maxHeavyAttValue) maxHeavyAttValue = playerStats.HeavyAttackDamage;
        if (playerStats.CharacterSpeed > maxSpeedValue) maxSpeedValue = playerStats.CharacterSpeed;
    }

    public void ChangePlayerCharacter(int i, GameObject go)
    {
        GameController.ChangeLockedInCharacter(i, go);

        foreach (var playerRef in _playerRefs)
        {
            if (i == 1 && IsPlayer1LockedIn) break;
            if (i == 2 && IsPlayer2LockedIn) break;

            playerRef.ChangeCharacter();
        }

        PlayerBehaviour beh = GameController.PlayerCharacter[i]?.GetComponent<PlayerBehaviour>();
        if(beh != null)
        {
            if(i == 1 && !IsPlayer1LockedIn)
            {
                ChangeStats(_player1Stats, beh.PlayerStats.Health, beh.PlayerStats.Defence, beh.PlayerStats.NormalAttackDamage, beh.PlayerStats.HeavyAttackDamage,
                    beh.PlayerStats.CharacterSpeed, beh.PlayerStats.CharacterName);
            }
            else if(i == 2 && !IsPlayer2LockedIn)
            {
                ChangeStats(_player2Stats, beh.PlayerStats.Health, beh.PlayerStats.Defence, beh.PlayerStats.NormalAttackDamage, beh.PlayerStats.HeavyAttackDamage,
                    beh.PlayerStats.CharacterSpeed, beh.PlayerStats.CharacterName);
            }
        }
    }

    private void ChangeStats(ChangeStats _stats, float health, float Defence, float normalAtt, float heavyAtt, float speed, string name)
    {
        _stats.HealthStats = health;
        _stats.DefenceStats = Defence;
        _stats.NormalAttackStats = normalAtt;
        _stats.HeavyAttackStats = heavyAtt;
        _stats.SpeedStats = speed;
        _stats.NameText.text = name;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Player1_BButton"))
        {
            Deselect(1);
        }
        if (Input.GetButtonDown("Player2_BButton"))
        {
            Deselect(2);
        }

        if (IsLockedIn && Input.GetButtonDown("Start_Button"))
        {
            GameController.ChangeGameState(true);
        }

        if (Input.GetButtonDown("Player1_BackButton"))
        {
            _player1Stats.gameObject.SetActive(!_player1Stats.gameObject.activeSelf);
        }
        if (Input.GetButtonDown("Player2_BackButton"))
        {
            _player2Stats.gameObject.SetActive(!_player2Stats.gameObject.activeSelf);
        }
    }

    private void Deselect(int playerNumber)
    {
        foreach (var button in _buttonList)
        {
            button.Deselect(playerNumber);
        }

        if (playerNumber == 1)
        {
            ChangeStats(_player1Stats, 0,0,0,0,0, "Select character");
        }
        else if (playerNumber == 2)
        {
            ChangeStats(_player2Stats, 0, 0, 0, 0, 0, "Select character");
        }
    }
       
    public void ChangeText(string text)
    {
        _text.text = text;
    }
}
