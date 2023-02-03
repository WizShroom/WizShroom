using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEffectObject : MonoBehaviour
{

    public float destroyInTime;
    public bool hasSound;
    public AudioSource ourSource;
    public AudioClip soundToPlay;

    private void Start()
    {
        if (hasSound)
        {
            destroyInTime = Mathf.Max(0, destroyInTime - soundToPlay.length);
        }

        StartCoroutine(AnimateEffect());
    }

    private IEnumerator AnimateEffect()
    {
        if (hasSound)
        {
            SoundManager.Instance.PlaySoundOneShot(soundToPlay, ourSource);

            while (ourSource.isPlaying)
            {
                yield return null;
            }
        }
        Destroy(gameObject, destroyInTime);
    }

}