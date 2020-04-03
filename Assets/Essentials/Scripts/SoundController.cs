using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{
    private static AudioSource _audio;

    private static bool IsFadingIn = false;
    private static bool IsFadingOut = false;

    [Header("Background soundtracks")] [SerializeField] private AudioClip[] _clips;
    private static AudioClip[] CLIPS;

    public static SoundController INSTANCE;

    private void Awake()
    {
        INSTANCE = this;
    }

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        CLIPS = _clips;

        INSTANCE.StartCoroutine(FadeIn(CLIPS[0], 0.05f, 0.5f));
    }

    public static void FadeTrack(string sceneName)
    {
        switch (sceneName)
        {
            case "Arena_Scene": INSTANCE.StartCoroutine(FadeOut(CLIPS[1], 0.08f, 0.5f));
                break;
            case "CharacterSelection_Scene": INSTANCE.StartCoroutine(FadeOut(CLIPS[0], 0.05f, 0.5f));
                break;
        }
    }

    static IEnumerator FadeIn(AudioClip clip, float speed, float maxVolume)
    {
        IsFadingIn = true;
        IsFadingOut = false;

        _audio.volume = 0;
        float audioVolume = _audio.volume;

        _audio.clip = clip;
        _audio.Play();

        while(_audio.volume < maxVolume && IsFadingIn)
        {
            audioVolume += speed;
            _audio.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }
    }

    static IEnumerator FadeOut(AudioClip clip, float speed, float maxVolume)
    {
        IsFadingIn = false;
        IsFadingOut = true;

        float audioVolume = _audio.volume;

        while (_audio.volume >= speed && IsFadingOut)
        {
            audioVolume -= speed;
            _audio.volume = audioVolume;
            yield return new WaitForSeconds(0.1f);
        }

        INSTANCE.StartCoroutine(FadeIn(clip, speed, maxVolume));
    }
}
