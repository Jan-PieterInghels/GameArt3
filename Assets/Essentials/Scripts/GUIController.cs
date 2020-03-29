using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    [SerializeField] private Slider _healthbarPlayer1;
    [SerializeField] private Slider _healthbarPlayer2;

    private PlayerBehaviour _player1PB;
    private PlayerBehaviour _player2PB;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartRoutine());
    }

    private void Initialize()
    {
        _player1PB = GameObject.FindGameObjectWithTag("Player1").GetComponentInChildren<PlayerBehaviour>();
        _player2PB = GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<PlayerBehaviour>();

        _healthbarPlayer1.maxValue = _player1PB.CurrentHP;
        _healthbarPlayer2.maxValue = _player2PB.CurrentHP;

        _healthbarPlayer1.value = _healthbarPlayer1.maxValue;
        _healthbarPlayer2.value = _healthbarPlayer2.maxValue;

        _player1PB.OnChangeCurrentHealth += ChangePlayerHealthbar;
        _player2PB.OnChangeCurrentHealth += ChangePlayerHealthbar;
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
