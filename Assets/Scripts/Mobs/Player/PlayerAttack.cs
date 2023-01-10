using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class PlayerAttack : MonoBehaviour
{

    public float attackRange = 5;

    public Spell normalAttackSpell;

    public float timeBetweenShots = 1f;
    public float elapsedShotTime;

    PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    public void Attack(Transform pivotPoint, Transform shootPoint, EnemyController engagedEnemy)
    {
        if (!CanAttack(pivotPoint, shootPoint, engagedEnemy))
        {
            return;
        }
        normalAttackSpell.Cast(controller, engagedEnemy);
    }

    public bool CanAttack(Transform pivotPoint, Transform shootPoint, EnemyController engagedEnemy)
    {

        elapsedShotTime += Time.deltaTime;
        if (elapsedShotTime < timeBetweenShots)
        {
            return false;
        }
        elapsedShotTime = 0;

        if (Vector3.Distance(engagedEnemy.transform.position, shootPoint.position) > attackRange)
        {
            return false;
        }

        Physics.Raycast(shootPoint.position, pivotPoint.right, attackRange);

        RaycastHit hit;
        Physics.Raycast(shootPoint.position, pivotPoint.right, out hit, attackRange);
        if (hit.collider && hit.collider.CompareTag("Wall"))
        {
            return false;
        }

        return true;
    }
}