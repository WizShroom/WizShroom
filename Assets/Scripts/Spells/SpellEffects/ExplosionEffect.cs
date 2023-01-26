using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionEffect", menuName = "SpellEffects/ExplosionEffect", order = 0)]
public class ExplosionEffect : SpellEffect
{
    public float explosionRadious = 5;
    public GameObject explosionEffect;
    public override void OnCollisionEffect(GameObject objectHit, BulletController bulletController)
    {
        Explode(objectHit.transform.position, bulletController);
    }

    public override void OnCollisionEffect(MobController mobHit, BulletController bulletController, Vector3 hitDirection)
    {
        Explode(mobHit.transform.position, bulletController);
    }

    public void Explode(Vector3 explosionPosition, BulletController bulletController)
    {
        Collider[] explodedColliders = Physics.OverlapSphere(explosionPosition, explosionRadious);
        foreach (Collider explodedCollider in explodedColliders)
        {
            if (explodedCollider.CompareTag("Mob") || explodedCollider.CompareTag("Enemy") || explodedCollider.CompareTag("Player"))
            {
                float distanceFromCenter = Vector3.Distance(explosionPosition, explodedCollider.transform.position);
                MobController explodedMob = explodedCollider.GetComponent<MobController>();
                explodedMob.TakeDamage(bulletController.shooter, (bulletController.bulletDamage * bulletController.explosiveMultiplier) - distanceFromCenter);
            }
        }

        GameObject explosion = Instantiate(explosionEffect, explosionPosition + Vector3.up, Quaternion.identity);
        explosion.AddComponent<DeleteAfterTime>().CallDestroy(1f);
    }
}