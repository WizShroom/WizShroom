using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : SingletonMono<SoundManager>
{
    public List<AudioClip> allSounds;

    public void PlaySoundOneShot(string soundToPlay, AudioSource source)
    {
        AudioClip clipToPlay = GetAudioClip(soundToPlay);

        if (clipToPlay == null || source == null)
        {
            return;
        }

        source.PlayOneShot(clipToPlay);
    }

    public void PlaySoundOneShot(AudioClip clipToPlay, AudioSource source)
    {
        if (clipToPlay == null || source == null)
        {
            return;
        }
        source.PlayOneShot(clipToPlay);
    }

    public AudioClip GetAudioClip(string soundToGet)
    {
        AudioClip returnClip = null;

        foreach (AudioClip audioClip in allSounds)
        {
            if (audioClip.name != soundToGet)
            {
                continue;
            }

            returnClip = audioClip;
            break;
        }

        return returnClip;
    }

}