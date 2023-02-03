using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Basic")]
public class Spell : ScriptableObject
{
    public string spellName;
    public Sprite spellSprite;
    public SpellType spellType;
    public GameObject bulletPrefab;
    public int castAmount = 1;
    public List<SpellEffect> spellEffects;

    public AudioClip spellAudioShoot;

    public float chargeTime = 0;

    public string key;
    public float cooldown;
    public float cooldownRemaining;
    public Sprite UIImage;
    public bool requireEnemy = false;

    public int spellLevel;

    public virtual void Cast(MobController caster, MobController target)
    {
        caster.StartCoroutine(CastCoroutine(caster, target.transform.position, target));
    }

    public virtual void Cast(MobController caster, Vector3 targetPosition)
    {
        caster.StartCoroutine(CastCoroutine(caster, targetPosition));
    }

    public virtual IEnumerator CastCoroutine(MobController caster, Vector3 targetPosition, MobController target = null)
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
        targetPosition.y = caster.shootPoint.position.y;
        Vector3 castDirection = (targetPosition - caster.transform.position).normalized;
        for (int i = 0; i < Mathf.Max(castAmount, 1); i++)
        {
            switch (spellType)
            {
                case SpellType.SELF:

                    foreach (SpellEffect spellEffect in spellEffects)
                    {
                        spellEffect.OnMobEffect(caster, caster, castDirection);
                    }
                    break;

                case SpellType.MELEE:
                    foreach (SpellEffect spellEffect in spellEffects)
                    {
                        spellEffect.OnMobEffect(caster, target, castDirection);
                    }
                    break;

                case SpellType.WORLD:
                    foreach (SpellEffect spellEffect in spellEffects)
                    {
                        spellEffect.OnWorldEffect(targetPosition);
                    }
                    break;

                case SpellType.PROJECTILE:
                    GameObject bulletEntity = Instantiate(bulletPrefab, caster.shootPoint.position, Quaternion.identity);
                    BulletController bulletController = bulletEntity.GetComponent<BulletController>();

                    foreach (SpellEffect spellEffect in spellEffects)
                    {
                        bulletController.spellEffects.Add(spellEffect);
                    }
                    bulletController.InitializeEffects();
                    bulletController.FireBullet(castDirection, caster, target);
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
                            spellEffect.OnMobEffect(caster, selectedMob);
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
    WORLD,
    MELEE,
}

[System.Flags]
public enum SpellEffectType
{
    ON_CREATION = 1 << 0,
    ON_SHOT = 1 << 1,
    ON_FLIGHT = 1 << 2,
    ON_HIT = 1 << 3,
}