using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Basic")]
public class Spell : ScriptableObject
{
    public SpellType spellType;
    public GameObject bulletPrefab;
    public int castAmount = 1;
    public List<SpellEffect> spellEffects;

    public AudioClip spellAudioShoot;

    public float chargeTime = 0;

    public virtual void Cast(MobController caster, MobController target)
    {
        Vector3 castDirection = (target.transform.position - caster.transform.position).normalized;
        caster.StartCoroutine(CastCoroutine(caster, castDirection, target));
    }

    public virtual void Cast(MobController caster, Vector3 castDirection)
    {
        caster.StartCoroutine(CastCoroutine(caster, castDirection));
    }

    public virtual IEnumerator CastCoroutine(MobController caster, Vector3 castDirection, MobController target = null)
    {

        if (chargeTime > 0)
        {
            float resetTime = chargeTime;
            while (chargeTime > 0)
            {
                chargeTime -= Time.deltaTime;
                yield return null;
            }
            chargeTime = resetTime;
        }

        for (int i = 0; i < Mathf.Max(castAmount, 1); i++)
        {
            switch (spellType)
            {
                case SpellType.SELF:
                    foreach (SpellEffect spellEffect in spellEffects)
                    {
                        spellEffect.OnMobEffect(caster, castDirection);
                    }
                    break;

                case SpellType.PROJECTILE:
                    GameObject bulletEntity = Instantiate(bulletPrefab, caster.castPosition.position, Quaternion.identity);
                    BulletController bulletController = bulletEntity.GetComponent<BulletController>();

                    foreach (SpellEffect spellEffect in spellEffects)
                    {
                        bulletController.spellEffects.Add(spellEffect);
                    }
                    bulletController.InitializeEffects();
                    bulletController.FireBullet(castDirection.normalized, caster, target);
                    bulletController.AfterFireEffects();
                    break;

                case SpellType.RANDOM:
                    Collider[] colliders = Physics.OverlapCapsule(caster.transform.position + Vector3.up, caster.transform.position + Vector3.down, 15f, LayerMask.NameToLayer("Enemy"));
                    List<MobController> nearMobs = new List<MobController>();
                    foreach (Collider collider in colliders)
                    {
                        Vector3 direction = (collider.transform.position - caster.transform.position).normalized;
                        if (Physics.Raycast(caster.transform.position, direction, Mathf.Infinity, LayerMask.NameToLayer("Wall")))
                        {
                            continue;
                        }
                        nearMobs.Add(collider.GetComponent<MobController>());
                    }
                    List<MobController> selectedMobs = new List<MobController>();
                    for (int j = 0; j < Mathf.Min(Random.Range(1, 4), nearMobs.Count); j++)
                    {
                        selectedMobs.Add(nearMobs[Random.Range(0, nearMobs.Count)]);
                    }

                    foreach (MobController selectedMob in selectedMobs)
                    {
                        foreach (SpellEffect spellEffect in spellEffects)
                        {
                            spellEffect.OnMobEffect(selectedMob);
                        }
                    }
                    break;
            }
            SoundManager.Instance.PlaySoundOneShot(spellAudioShoot, caster.audioSource);
            yield return new WaitForSeconds(0.1f);
        }
    }
}

public enum SpellType
{
    SELF,
    PROJECTILE,
    RANDOM,
}

[System.Flags]
public enum SpellEffectType
{
    ON_CREATION = 1 << 0,
    ON_SHOT = 1 << 1,
    ON_FLIGHT = 1 << 2,
    ON_HIT = 1 << 3,
}