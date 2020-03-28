using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterStats", order = 1)]
public class CharacterStats : ScriptableObject
{
    [Range(150, 400)] [SerializeField] private float _health;
    public float Health => _health;
    [Range(15, 50)] [SerializeField] private float _normalAttackDamage;
    public float NormalAttackDamage => _normalAttackDamage; 
    [Range(30, 100)] [SerializeField] private float _heavyAttackDamage;
    public float HeavyAttackDamage => _heavyAttackDamage;
    [Range(0.5f, 2.5f)] [SerializeField] private float _characterSpeed = 2f;
    public float CharacterSpeed => _characterSpeed;
    [Range(1f, 7.5f)] [SerializeField] private float _timeUntilNextBlock = 2f;
    public float TimeUntilNextBlock => _timeUntilNextBlock;

    [Range(1, 50)] [SerializeField] private int _defence = 1;
    public int Defence => _defence; 

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
