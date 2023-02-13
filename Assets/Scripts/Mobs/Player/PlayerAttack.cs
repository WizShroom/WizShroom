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

    PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    int comboCount = 0;
    public float downtime = 1f;
    private float elapsedShotTime = 0f;
    public float elapsedDowntime = 0f;

    public void ResetAttack()
    {
        comboCount = 0;
        elapsedDowntime = 0;
        elapsedShotTime = 0;
    }

    public void Attack(Transform pivotPoint, Transform shootPoint, EnemyController engagedEnemy)
    {
        if (!CanAttack(pivotPoint, shootPoint, engagedEnemy))
        {
            return;
        }

        if (comboCount < 3)
        {
            normalAttackSpell.Cast(controller, engagedEnemy);
            comboCount++;
            elapsedShotTime = 0f;
        }
    }

    public bool CanAttack(Transform pivotPoint, Transform shootPoint, EnemyController engagedEnemy)
    {
        elapsedShotTime += Time.deltaTime;

        if (elapsedDowntime >= downtime)
        {
            comboCount = 0;
            elapsedDowntime = 0f;
        }

        if (comboCount >= 3)
        {
            elapsedDowntime += Time.deltaTime;
            return false;
        }

        if (elapsedShotTime < timeBetweenShots)
        {
            return false;
        }

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