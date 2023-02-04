using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public float healthPoints = 1;

    public AudioClip breakClip;
    AudioSource ourSource;

    private void Start()
    {
        ourSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Bullet"))
        {
            BulletController otherBullet = other.transform.GetComponent<BulletController>();
            TakeDamage(otherBullet.bulletDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            StartBreaking();
        }
    }

    public void StartBreaking()
    {
        SoundManager.Instance.PlaySoundOneShot(breakClip, ourSource);
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 1f);
    }
}
