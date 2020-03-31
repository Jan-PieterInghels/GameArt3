using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIController : MonoBehaviour
{
    //stats
    [SerializeField] private Slider _healthbarPlayer1;
    [SerializeField] private Slider _healthbarPlayer2;
    public Image PortretPlayer1;
    public Image PortretPlayer2;
    public TextMeshProUGUI NamePlayer1;
    public TextMeshProUGUI NamePlayer2;

    //looks
    private PlayerBehaviour _player1PB;
    private PlayerBehaviour _player2PB;
    
    void Start()
    {
        StartCoroutine(StartRoutine());
    }

    private void Initialize()
    {
        _player1PB = GameObject.FindGameObjectWithTag("Player1").GetComponentInChildren<PlayerBehaviour>();
        _player2PB = GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<PlayerBehaviour>();

        //stats
        _healthbarPlayer1.maxValue = _player1PB.CurrentHP;
        _healthbarPlayer2.maxValue = _player2PB.CurrentHP;

        _healthbarPlayer1.value = _healthbarPlayer1.maxValue;
        _healthbarPlayer2.value = _healthbarPlayer2.maxValue;

        _player1PB.OnChangeCurrentHealth += ChangePlayerHealthbar;
        _player2PB.OnChangeCurrentHealth += ChangePlayerHealthbar;

        //looks
        PortretPlayer1.sprite = _player1PB.PlayerStats.PlayerPortret;
        PortretPlayer2.sprite = _player2PB.PlayerStats.PlayerPortret;

        NamePlayer1.text = _player1PB.PlayerStats.CharacterName;
        NamePlayer2.text = _player2PB.PlayerStats.CharacterName;
    }

    private void ChangePlayerHealthbar(object sender, EventArgs e)
    {
        _healthbarPlayer2.value = _player2PB.CurrentHP;
        _healthbarPlayer1.value = _player1PB.CurrentHP;
    }
    IEnumerator StartRoutine()
    {
        yield return new WaitForEndOfFrame();
        Initialize();
    }
}
