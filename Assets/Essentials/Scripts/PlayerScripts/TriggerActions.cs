using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class TriggerActions : MonoBehaviour
{
    [SerializeField] private PlayerBehaviour _playerBeh;
    [SerializeField] private GameObject _blockParticle;
    [SerializeField] private Transform _blockTrans;

    private AudioSource _audioSource;
    private GameObject instObj;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
        _audioSource.playOnAwake = false;
        _audioSource.spatialBlend = 1;
    }

    public void ResetAnimTrigger()
    {
        _playerBeh.AnimController.ResetTrigger("FastAttack");
        _playerBeh.AnimController.ResetTrigger("HeavyAttack");
        _playerBeh.AnimController.ResetTrigger("Hit");
        _playerBeh.AnimController.ResetTrigger("IsBlocking");
        _playerBeh.IsDamageDone = false;
        _playerBeh.IsAttacking = false;
        _playerBeh.IsBlocking = false;
        _playerBeh.CantMove = false;
        
        if(!_playerBeh.CanBlock)
            StartCoroutine(_playerBeh.TimeTillBlock());
    }

    public void SetBlocking()
    {
        _playerBeh.IsBlocking = true;
        instObj = Instantiate(_blockParticle, _blockTrans);
        instObj.transform.position = _blockTrans.position;
    }

    public void ResetBlocking()
    {
        _playerBeh.IsBlocking = false;
        Destroy(instObj);
    }

    public void ResetTriggers()
    {
        foreach (var trigger in _playerBeh.AttackTriggers)
        {
            trigger.SetActive(false);
        }
    }

    public void SetTriggers()
    {
        foreach (var trigger in _playerBeh.AttackTriggers)
        {
            trigger.SetActive(true);
        }        
    }

    public void PlaySwoosh()
    {
        _audioSource.clip = _playerBeh.PlayerStats.PunchSwoosh;
        _audioSource.Play();
    }
}
