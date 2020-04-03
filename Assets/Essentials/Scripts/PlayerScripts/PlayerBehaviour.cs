using System;
using System.Collections;
using UnityEngine;
using EZCameraShake;

[RequireComponent(typeof(CharacterController))] [RequireComponent(typeof(AudioSource))]
public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private CharacterStats _characterStats;
    public CharacterStats PlayerStats { get => _characterStats; }

    [SerializeField] private Animator _animController;
    public Animator AnimController { get => _animController; set { _animController = value; } }

    [SerializeField] private GameObject[] _attackTriggers;
    public GameObject[] AttackTriggers { get => _attackTriggers; set { _attackTriggers = value; } }

    [SerializeField] private PunchHitDetection[] _hitDetection;
    [SerializeField] private GameObject _floatingPoints;
    [SerializeField] private Transform _instantiationPointFloatingPoints;

    private int _playerNumber = 1;
    public int PlayerNumber { get => _playerNumber; set { _playerNumber = value; } }    
    private float _currentHP = 0;
    public float CurrentHP => _currentHP;
    private bool _isDamageDone = false;
    public bool IsDamageDone { get => _isDamageDone; set { _isDamageDone = value; } }
    private bool _isAttacking = false;
    public bool IsAttacking { get => _isAttacking; set { _isAttacking = value; } }
    private bool _isBlocking = false;
    public bool IsBlocking { get => _isBlocking; set { _isBlocking = value; } }
    private bool _cantMove = false;
    public bool CantMove { get => _cantMove; set { _cantMove = value; } }
    private bool _canBlock = true;
    public bool CanBlock => _canBlock;
    private bool _isHeavy = false; public bool IsHeavy => _isHeavy;

    public event EventHandler OnChangeCurrentHealth;
    public event EventHandler OnDefenceCooldown;

    private CharacterController _characterController;
    private Vector3 _direction;

    private string _horizontalAxis, _normalAttackButton, _heavyAttackButton, _blockButton;
    private bool _hasInitialized = false;
    private float _doDamageValue = 0;

    private Vector2 _screenBounds;
    private float _objectWidth;

    private Vector3 impact;
    private AudioSource _audioSource;

    private bool _defeated = false;

    // Start is called before the first frame update
    public void Initialize()
    {
        transform.parent = null;
        this.tag = "Player" + _playerNumber;
        _characterStats.Initialize();
        _currentHP = _characterStats.Health;
        
        _horizontalAxis = "Player" + _playerNumber + "_HorizontalAxis";
        _normalAttackButton = "Player" + _playerNumber + "_AButton";
        _heavyAttackButton = "Player" + _playerNumber + "_BButton";
        _blockButton = "Player" + _playerNumber + "_XButton";

        foreach (var trigger in _attackTriggers)
        {
            trigger.SetActive(false);
        }

        _characterController = GetComponent<CharacterController>();

        _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        _objectWidth = GetComponent<Collider>().bounds.size.x;

        _hasInitialized = true;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1;
    }

    void Update()
    {
        if (_hasInitialized && GameController.IsGamePlaying)
        {
            if (Input.GetButtonDown(_normalAttackButton) && !_cantMove)
            {
                _isHeavy = false;
                _animController.SetTrigger("FastAttack");
                _doDamageValue = _characterStats.NormalAttackDamage;
                _isAttacking = true;
                _cantMove = true;
            }
            if (Input.GetButtonDown(_heavyAttackButton) && !_cantMove)
            {
                _isHeavy = true;
                PlaySound(_characterStats.UseHeavy);

                _animController.SetTrigger("HeavyAttack");
                _doDamageValue = _characterStats.HeavyAttackDamage;
                _isAttacking = true;
                _cantMove = true;
            }
            if(Input.GetButtonDown(_blockButton) && !_cantMove && _canBlock)
            {
                _animController.SetTrigger("IsBlocking");
                _cantMove = true;
                _canBlock = false;
            }
            _direction = new Vector3(Input.GetAxis(_horizontalAxis), 0, 0);

            foreach (var hit in _hitDetection)
            {
                hit.OnHit += DoDamage;
            }

            if (impact.magnitude > 0.2) _characterController.Move(impact * Time.deltaTime);
            impact = Vector3.Lerp(impact, Vector3.zero, 2 * Time.deltaTime);
        }
        else if (!GameController.IsGamePlaying && !_defeated)
        {
            PlaySound(_characterStats.Victory);
        }
    }

    private void TriggerFightEnd()
    {
        _defeated = true;
        PlaySound(_characterStats.Defeat);

        _animController.SetTrigger("HasFainted");
        GameController.ChangeGameState(false);
        StartCoroutine(GameController.GoToCharacterSelectScreen());
    }

    public IEnumerator TimeTillBlock()
    {
        OnDefenceCooldown?.Invoke(this, EventArgs.Empty);
        _canBlock = false;
        yield return new WaitForSeconds(_characterStats.TimeUntilNextBlock);
        _canBlock = true;
    }

    public void TakeDamage(float damage, float force, Vector3 direction, bool isHeavy)
    {
        if (!_isBlocking)
        {
            var resultDamage = _characterStats.TakeDamage(damage);
            _currentHP -= resultDamage;

            CheckCameraShakeAmount(resultDamage);

            if (_currentHP > 0)
                _animController.SetTrigger("Hit");
            else
                TriggerFightEnd();

            InstantiateDamage();

            if (_currentHP < 0) _currentHP = 0;
            OnChangeCurrentHealth?.Invoke(this, EventArgs.Empty);

            impact += direction * force / _characterStats.Defence;

            if (isHeavy) PlaySound(_characterStats.GetHitHeavy);
            else PlaySound(_characterStats.GetHitNormal);
        }
    }

    private void CheckCameraShakeAmount(float resultDamage)
    {
        if (resultDamage < 25) CameraShake(5, 10, 0.25f, .25f);
        else if(resultDamage < 50f) CameraShake(7f, 12, 0.15f, .35f);
        else if (resultDamage < 75f) CameraShake(8f, 12.5f, 0.15f, .45f);
        else CameraShake(10f, 15, 0.10f, .50f);
    }

    private static void CameraShake(float impact, float roughness, float fadeInTime, float fadeOutTime)
    {
        CameraShaker.Instance.ShakeOnce(impact, roughness, fadeInTime, fadeOutTime);
    }

    private void InstantiateDamage()
    {
        GameObject go = Instantiate(_floatingPoints, transform.position, Quaternion.identity);
        go.transform.position = _instantiationPointFloatingPoints.position;
        go.transform.parent = null;
        FloatingDamageBehaviour fdb = go.GetComponentInChildren<FloatingDamageBehaviour>();
        fdb.DamageAmount = _characterStats.ActualDamageTaken;
        fdb.Initialize();
    }

    private void DoDamage(GameObject go, Vector3 direction, bool isHeavy)
    {
        PlaySound(_characterStats.PunchImpact);

        if (!_isDamageDone)
        {
            go?.GetComponent<PlayerBehaviour>()?.TakeDamage(_doDamageValue, 40, direction, isHeavy);
            _isDamageDone = true;
        }
    }

    private void FixedUpdate()
    {
        if (_hasInitialized && GameController.IsGamePlaying)
        {
            if (!_cantMove)
            {
                WalkAndIdle();
            }
        }
        if(_hasInitialized && !GameController.IsGamePlaying) _animController.SetFloat("WalkSpeed", 0);
    }

    private void WalkAndIdle()
    {
        Vector3 speed = _direction * _characterStats.CharacterSpeed * Time.deltaTime;
        if (!_characterController.isGrounded)
            speed -= transform.up * 9.81f * Time.deltaTime;
                
        _characterController.Move(speed);

        if (_playerNumber % 2 == 0)
            _animController.SetFloat("WalkSpeed", -_direction.x);
        else
            _animController.SetFloat("WalkSpeed", _direction.x);
        
        Clamp();
    }

    private void Clamp()
    {
        Vector3 viewPos = transform.position;
        viewPos = new Vector3
        (
            FloatClamp(viewPos.x, _screenBounds.x + _objectWidth, -(_screenBounds.x + _objectWidth)),
            viewPos.y,
            viewPos.z
        );

        transform.position = viewPos;
    }

    private float FloatClamp(float value, float min, float max)
    {
        return (value <= min) ? min : (value >= max) ? max : value;
    }

    private void PlaySound(AudioClip clip)
    {
        _audioSource.Stop();
        _audioSource.clip = clip;

        if (_audioSource.clip != null)
            _audioSource.Play();
    }
}