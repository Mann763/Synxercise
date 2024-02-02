using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;

        }
    }

    void Start()
    {
        PlaySound("Theme");
    }

    public void PlaySound(string name)
    {
        StopActiveSound();

        Sound s = Array.Find(sounds, sounds => sounds.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not Found!!");
            return;
        }

        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;

        s.source.Play();
    }

    public void PlaySound(string name,int delay)
    {
        StopActiveSound();

        Sound s = Array.Find(sounds, sounds => sounds.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not Found!!");
            return;
        }

        s.source.clip = s.clip;
        s.source.volume = s.volume;
        s.source.pitch = s.pitch;
        s.source.loop = s.loop;

        s.source.PlayDelayed(delay);
    }

    public void StopActiveSound(float fadeOutDuration = 1.0f)
    {
        StartCoroutine(FadeOutActiveSound(fadeOutDuration));
    }

    IEnumerator FadeOutActiveSound(float duration)
    {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying)
            {
                float startVolume = s.source.volume;

                while (s.source.volume > 0)
                {
                    s.source.volume -= startVolume * Time.deltaTime / duration;
                    yield return null;
                }

                s.source.Stop();
                s.source.volume = startVolume;
                yield break;
            }
        }
    }
}
