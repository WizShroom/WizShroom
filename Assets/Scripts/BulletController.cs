using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float bulletDamage = 1;
    public float bulletSpeed = 10;
    public float destroyTime = 10;
    public float destroyDistance = 10;

    public MobController shooter;


    public List<SpellEffect> spellEffects;

    public bool bulletShot = false;

    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Vector3 currentVelocity;
    public MobController target;

    Vector3 pauseVelocity;
    float trailTime = 0.1f;
    float pauseTime;
    float resumeTime;
    bool paused = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GameEventHandler.Instance.OnEventReceived += OnEventReceived;
    }

    private void OnDestroy()
    {
        GameEventHandler.Instance.OnEventReceived -= OnEventReceived;
    }

    public void InitializeEffects()
    {
        foreach (SpellEffect spellEffect in spellEffects)
        {
            if ((spellEffect.spellEffectType & SpellEffectType.ON_CREATION) == 0)
            {
                continue;
            }

            spellEffect.OnCreationEffect(this);
        }

        Destroy(gameObject, destroyTime);
    }

    public void AfterFireEffects()
    {
        foreach (SpellEffect spellEffect in spellEffects)
        {
            if ((spellEffect.spellEffectType & SpellEffectType.ON_SHOT) == 0)
            {
                continue;
            }

            spellEffect.OnShotEffect(this);
        }
    }

    private void Update()
    {
        if (!bulletShot)
        {
            return;
        }

        currentVelocity = rb.velocity;

        if (Vector3.Distance(transform.position, shooter.transform.position) > destroyDistance)
        {
            Destroy(gameObject);
            return;
        }

        if (paused)
        {
            return;
        }

        foreach (SpellEffect spellEffect in spellEffects)
        {
            if ((spellEffect.spellEffectType & SpellEffectType.ON_FLIGHT) == 0)
            {
                continue;
            }

            spellEffect.OnFlightEffect(this);
        }
    }

    public void FireBullet(Vector3 shootDirection, MobController shooter, MobController target = null)
    {
        this.target = target;
        this.shooter = shooter;
        bulletDamage = bulletDamage *
            (1 + (shooter.GetStatValueByType(StatType.Intelligence) / 200f)) *
            (1 + (shooter.GetStatValueByType(StatType.Agility) / 500f)) *
            (1 + (shooter.GetStatValueByType(StatType.Luck) / (1000f + Random.Range(-500f, 500f))));

        float finalBulletSpeed = bulletSpeed *
            (1 + (shooter.GetStatValueByType(StatType.Intelligence) / 200f)) *
            (1 + (shooter.GetStatValueByType(StatType.Wisdom) / 200f));

        bulletDamage = Mathf.Round(bulletDamage * 100f) / 100f;
        finalBulletSpeed = Mathf.Round(finalBulletSpeed * 100f) / 100f;

        rb.AddForce(shootDirection * finalBulletSpeed, ForceMode.Impulse);
        bulletShot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            MobController monsterController = other.GetComponent<MobController>();

            Vector3 hitDirection = (monsterController.transform.position - transform.position).normalized;

            foreach (SpellEffect spellEffect in spellEffects)
            {
                if ((spellEffect.spellEffectType & SpellEffectType.ON_HIT) == 0)
                {
                    continue;
                }

                spellEffect.OnCollisionEffect(monsterController, this, hitDirection);
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            return;
        }
        foreach (SpellEffect spellEffect in spellEffects)
        {
            if ((spellEffect.spellEffectType & SpellEffectType.ON_HIT) == 0)
            {
                continue;
            }

            spellEffect.OnCollisionEffect(other.gameObject, this);
        }
        Destroy(gameObject);
    }

    public void OnEventReceived(GameObject source, EVENT eventReceived)
    {
        if (eventReceived == EVENT.PAUSED)
        {
            OnPauseGame();
        }
        if (eventReceived == EVENT.RESUMED)
        {
            OnResumeGame();
        }
    }

    public void OnPauseGame()
    {
        paused = true;
        pauseVelocity = rb.velocity;
        rb.velocity = Vector3.zero;

        if (TryGetComponent<TrailRenderer>(out TrailRenderer trail))
        {
            pauseTime = Time.time;
            trail.time = Mathf.Infinity;
        }
    }

    public void OnResumeGame()
    {
        paused = false;
        rb.velocity = pauseVelocity;

        if (TryGetComponent<TrailRenderer>(out TrailRenderer trail))
        {
            resumeTime = Time.time;
            trail.time = (resumeTime - pauseTime) + trailTime;
            Invoke("SetTrailTime", trailTime);
        }
    }

}
