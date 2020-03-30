using System;
using UnityEngine;

public class CharacterButtonSetup : MonoBehaviour
{
    [SerializeField] private ChangeStats _player1Stats, _player2Stats;
    [SerializeField] private GameObject[] _characterObjects;
    [SerializeField] private GameController _gameController;
    [SerializeField] private ButtonBehaviour _characterButtonBehaviour;
    [SerializeField] private GetRefCharacter[] _playerRefs;
    [SerializeField] private GameObject _player1RefSpawnpoint;
    public GameObject Player1RefSpawnpoint => _player1RefSpawnpoint;
    [SerializeField] private GameObject _player2RefSpawnpoint;
    public GameObject Player2RefSpawnpoint => _player2RefSpawnpoint;

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

    // Start is called before the first frame update
    void Start()
    {
        _gameController.CharacterAmount = _characterObjects.Length;
        _characterAmount = _gameController.CharacterAmount;
        _playerAmount = _gameController.PlayerAmount;

        for (int i = 1; i <= _characterAmount; i++)
        {
            ButtonBehaviour beh = Instantiate(_characterButtonBehaviour, this.transform);
            beh.CharacterNumber = i;
            beh.CharacterSetup = this;
            beh.CharacterObject = _characterObjects[i - 1];
        }

        //_player1Stats.ResetTexts();
        //_player2Stats.ResetTexts();

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
            playerRef.ChangeCharacter();
        }

        PlayerBehaviour beh = GameController.PlayerCharacter[i]?.GetComponent<PlayerBehaviour>();
        if(beh != null)
        {
            if(i == 1)
            {
                _player1Stats.HealthStats = beh.PlayerStats.Health;
                _player1Stats.DefenceStats = beh.PlayerStats.Defence;
                _player1Stats.NormalAttackStats = beh.PlayerStats.NormalAttackDamage;
                _player1Stats.HeavyAttackStats = beh.PlayerStats.HeavyAttackDamage;
                _player1Stats.SpeedStats = beh.PlayerStats.CharacterSpeed;
            }
            else if(i == 2)
            {
                _player2Stats.HealthStats = beh.PlayerStats.Health;
                _player2Stats.DefenceStats = beh.PlayerStats.Defence;
                _player2Stats.NormalAttackStats = beh.PlayerStats.NormalAttackDamage;
                _player2Stats.HeavyAttackStats = beh.PlayerStats.HeavyAttackDamage;
                _player2Stats.SpeedStats = beh.PlayerStats.CharacterSpeed;
            }
        }
    }
}
