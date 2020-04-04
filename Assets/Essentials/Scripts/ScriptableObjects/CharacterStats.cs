using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterStats", order = 1)]
public class CharacterStats : ScriptableObject
{
    [Header("Looks")]
    [SerializeField] private Sprite _playerPortret;
    public Sprite CharacterPortret => _playerPortret;
    [SerializeField] private string _characterName;
    public string CharacterName => _characterName;

    [Header("Stats")]
    [Range(150, 400)] [SerializeField] private float _health;
    public float Health => _health;
    [Range(15, 50)] [SerializeField] private float _normalAttackDamage;
    public float NormalAttackDamage => _normalAttackDamage; 
    [Range(30, 100)] [SerializeField] private float _heavyAttackDamage;
    public float HeavyAttackDamage => _heavyAttackDamage;
    [Range(0.5f, 2.5f)] [SerializeField] private float _characterSpeed = 2f;
    public float CharacterSpeed => _characterSpeed;
    [Range(1, 8)] [SerializeField] private int _timeUntilNextBlock = 2;
    public int TimeUntilNextBlock => _timeUntilNextBlock;
    [Range(1, 50)] [SerializeField] private int _defence = 1;
    public int Defence => _defence;

    [Header("Sound")]
    [SerializeField] private AudioClip _getHitNormal;
    public AudioClip GetHitNormal => _getHitNormal;
    [SerializeField] private AudioClip _getHitHeavy;
    public AudioClip GetHitHeavy => _getHitHeavy;
    [SerializeField] private AudioClip _useHeavy;
    public AudioClip UseHeavy => _useHeavy;
    [SerializeField] private AudioClip _narrator;
    public AudioClip Narrator => _narrator;
    [SerializeField] private AudioClip _victory;
    public AudioClip Victory => _victory;
    [SerializeField] private AudioClip _punchSwoosh;
    public AudioClip PunchSwoosh => _punchSwoosh;
    [SerializeField] private AudioClip _punchImpact;
    public AudioClip PunchImpact => _punchImpact;

    private float _actualDamageTaken;
    public float ActualDamageTaken => _actualDamageTaken;

    private System.Random _random;

    public void Initialize()
    {
        _random = new System.Random();
    }

    public float TakeDamage(float dmgTaken)
    {
        int resultDamage = 0;
        int minDmg = (int) (dmgTaken * (100 / (100 + dmgTaken)));
        int maxDmg = (int) (dmgTaken / (100 / (100 + dmgTaken)));
        dmgTaken = _random.Next(minDmg, maxDmg);

        if (dmgTaken < 25) resultDamage = (int)(dmgTaken * (100f / (100f + _defence)));
        else if (dmgTaken < 40) resultDamage = (int)(dmgTaken * (100f / (100f + (_defence / 2))));
        else resultDamage = (int)(dmgTaken * (100f / (100f + (_defence / 3))));

        _actualDamageTaken = resultDamage;
        return resultDamage;
    }
}
