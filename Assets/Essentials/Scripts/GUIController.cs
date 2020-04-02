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

    [SerializeField] private Slider _DefencebarPlayer1;
    [SerializeField] private Slider _DefencebarPlayer2;
    [SerializeField] private Image _defenceImageP1, _defenceImageP2;

    [Range(1, 10)] [SerializeField] private float _timeDevider = 5;

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

        _DefencebarPlayer1.maxValue = _player1PB.PlayerStats.TimeUntilNextBlock;
        _DefencebarPlayer2.maxValue = _player2PB.PlayerStats.TimeUntilNextBlock;

        _DefencebarPlayer1.value = _DefencebarPlayer1.maxValue;
        _DefencebarPlayer2.value = _DefencebarPlayer2.maxValue;

        _player1PB.OnChangeCurrentHealth += ChangePlayerHealthbar;
        _player2PB.OnChangeCurrentHealth += ChangePlayerHealthbar;

        _player1PB.OnDefenceCooldown += Cooldown;
        _player2PB.OnDefenceCooldown += Cooldown2;

        //looks
        PortretPlayer1.sprite = _player1PB.PlayerStats.CharacterPortret;
        PortretPlayer2.sprite = _player2PB.PlayerStats.CharacterPortret;

        NamePlayer1.text = _player1PB.PlayerStats.CharacterName;
        NamePlayer2.text = _player2PB.PlayerStats.CharacterName;
    }

    private void Cooldown2(object sender, EventArgs e)
    {
        StartCoroutine(ChangeDefenceBar(_DefencebarPlayer2, _defenceImageP2));
    }

    private IEnumerator ChangeDefenceBar(Slider defencebar, Image image)
    {
        defencebar.value = 0;
        image.fillAmount = 0;
        for (int i = 0; i < (defencebar.maxValue * _timeDevider); i++)
        {
            yield return new WaitForSeconds(1f / _timeDevider);
            defencebar.value += 1f / _timeDevider;
            image.fillAmount += 1f / (_timeDevider * defencebar.maxValue);
        }
    }

    private void Cooldown(object sender, EventArgs e)
    {
        StartCoroutine(ChangeDefenceBar(_DefencebarPlayer1, _defenceImageP1));
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
