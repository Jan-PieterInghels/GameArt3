using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    private static AudioSource _audio;

    private static bool IsFadingIn = false;
    private static bool IsFadingOut = false;

    [Header("Background soundtracks")] 
    [SerializeField] private AudioClip[] _clips;

    [Header("FadeIn variables")]
    [SerializeField] private float _fadeInTime;
    [SerializeField] private float _maxVolume;
    [SerializeField] private float _fadeInSteps;

    [Header("FadeOut variables")]
    [SerializeField] private float _fadeOutTime;
    [SerializeField] private float _fadeOutSteps;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();

        StartCoroutine(FadeIn(_clips[0]));
    }

    public void FadeTrack(string sceneName)
    {
        switch (sceneName)
        {
            case "Arena_Scene": 
                if(_audio.clip != _clips[1])
                    StartCoroutine(FadeOut(_clips[1]));
                break;
            case "CharacterSelection_Scene":
                if (_audio.clip != _clips[0])
                    StartCoroutine(FadeOut(_clips[0]));
                break;
        }
    }

    private IEnumerator FadeIn(AudioClip clip)
    {
        IsFadingIn = true;
        IsFadingOut = false;

        _audio.volume = 0;
        float audioVolume = _audio.volume;

        _audio.clip = clip;
        _audio.Play();

        while(_audio.volume < _maxVolume && IsFadingIn)
        {
            audioVolume += _fadeInTime/_fadeInSteps;
            _audio.volume = audioVolume;
            yield return new WaitForSeconds(_fadeInTime / _fadeInSteps);
        }
    }

    private IEnumerator FadeOut(AudioClip clip)
    {
        IsFadingIn = false;
        IsFadingOut = true;

        float audioVolume = _audio.volume;

        while (_audio.volume >= 0.1f && IsFadingOut)
        {
            audioVolume -= _fadeOutTime / _fadeOutSteps;
            _audio.volume = audioVolume;
            yield return new WaitForSeconds(_fadeOutTime / _fadeOutSteps);
        }

        StartCoroutine(FadeIn(clip));
    }
}
