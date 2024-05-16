using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 音频管理脚本单例，直接使用即可
/// 类似于这样：AudioManager.Instance.FadeIn("YourSoundName", 2f);
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private List<Sound> sounds = new List<Sound>();

    private Dictionary<string, AudioSource> soundDictionary = new Dictionary<string, AudioSource>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound sound in sounds)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
            audioSource.loop = sound.loop;

            soundDictionary.Add(sound.name, audioSource);
        }
    }

    public void PlaySound(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            soundDictionary[soundName].Play();
            //Debug.Log("sound play correctly");
        }
        else
        {
            Debug.LogWarning("Sound with name " + soundName + " not found!");
        }
    }

    public void StopSound(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            soundDictionary[soundName].Stop();

        }
        else
        {
            Debug.LogWarning("Sound with name " + soundName + " not found!");
        }
    }

    public void FadeIn(string soundName, float duration)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            StartCoroutine(Fade(soundDictionary[soundName], 0f, soundDictionary[soundName].volume, duration));
        }
        else
        {
            Debug.LogWarning("Sound with name " + soundName + " not found!");
        }
    }

    public void FadeOut(string soundName, float duration)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            StartCoroutine(Fade(soundDictionary[soundName], soundDictionary[soundName].volume, 0f, duration));
        }
        else
        {
            Debug.LogWarning("Sound with name " + soundName + " not found!");
        }
    }

    private IEnumerator Fade(AudioSource audioSource, float startVolume, float endVolume, float duration)
    {
        float startTime = Time.time;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed = Time.time - startTime;
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, elapsed / duration);
            yield return null;
        }

        audioSource.volume = endVolume;
    }

    // Additional methods for controlling volume, pitch, etc. can be added as needed.
    // 可以根据需要添加其他控制音量、音调等的方法。
}

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop;
}
