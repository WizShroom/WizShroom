using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "CreateBarrierEffect", menuName = "SpellEffects/CreateBarrierEffect", order = 0)]
public class CreateBarrierEffect : SpellEffect
{
    public GameObject barrierPrefab;
    public float distance;
    public int numObjects;
    public float deleteTime = 5;
    public float maxAngle = 180f;

    public override void OnMobEffect(MobController casterMob, MobController affectedMob, Vector3 castDirection = default)
    {
        CreateBarrier(affectedMob, castDirection);
    }

    public override void OnCollisionEffect(MobController mobHit, BulletController bulletController, Vector3 hitDirection)
    {
        CreateBarrier(mobHit, Vector3.zero);
    }

    public void CreateBarrier(MobController affectedMob, Vector3 castDirection = default)
    {
        BarrierDebuff barrierDebuff = new BarrierDebuff("BarrierDebuff", deleteTime);
        if (affectedMob.HasBuff(barrierDebuff))
        {
            return;
        }
        affectedMob.ApplyBuff(barrierDebuff);

        Vector3 centerPosition = affectedMob.transform.position + castDirection * distance;

        // set radius and angle between objects
        float radius = 1.0f;
        float angleBetweenObjects = maxAngle / numObjects;

        // calculate starting angle based on the desired direction
        Vector3 desiredDirection = castDirection;
        float startAngle = Mathf.Atan2(desiredDirection.z, desiredDirection.x) * Mathf.Rad2Deg - (numObjects - 1) * angleBetweenObjects / 2.0f;

        // spawn objects in an arch around the center position
        for (int i = 0; i < numObjects; i++)
        {
            // calculate angle for this object
            float angle = startAngle + i * angleBetweenObjects;

            // calculate position for this object using polar coordinates
            float xPos = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float zPos = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 spawnPosition = centerPosition + new Vector3(xPos, 1, zPos);

            Quaternion spawnRotation = Quaternion.Euler(0, 0, 90);
            GameObject newObject = Instantiate(barrierPrefab, spawnPosition, spawnRotation);
            newObject.GetComponent<DeleteAfterTime>().CallDestroy(deleteTime);
        }
    }
}