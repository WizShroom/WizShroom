using UnityEngine;

[CreateAssetMenu(fileName = "GenerateSoundOnHitEffect", menuName = "SpellEffects/GenerateSoundOnHitEffect", order = 0)]
public class GenerateSoundOnHitEffect : SpellEffect
{

    public AudioClip soundToPlayOnHit;

    public override void OnCollisionEffect(GameObject objectHit, BulletController bulletController)
    {
        GenerateSound(objectHit.transform.position);
    }

    public override void OnCollisionEffect(MobController mobHit, BulletController bulletController, Vector3 hitDirection)
    {
        GenerateSound(mobHit.transform.position);
    }

    public void GenerateSound(Vector3 position)
    {
        GameObject soundEmitter = new GameObject("SoundEmitter");
        AudioSource soundEmitterSource = soundEmitter.AddComponent<AudioSource>();
        soundEmitterSource.PlayOneShot(soundToPlayOnHit);
        Destroy(soundEmitter, 5f);
    }

}