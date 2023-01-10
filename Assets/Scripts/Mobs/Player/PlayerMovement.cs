using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;

    [HideInInspector] public NavMeshAgent agent;
    public Transform pivotPoint;
    public Transform shootPoint;

    public Vector3 destination;

    [SerializeField] Animator animator;
    PlayerAttack playerAttack;

    [HideInInspector] public EnemyController engagedEnemy;
    public GameObject enemyHighlight;

    private void Start()
    {
        destination = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        if (engagedEnemy)
        {
            AttackEnemy();
            enemyHighlight.SetActive(true);
            enemyHighlight.transform.position = engagedEnemy.transform.position + new Vector3(0, 0.5f, 0);
        }
        else
        {
            enemyHighlight.SetActive(false);
            DisengageEnemy();
        }

        if (Vector3.Distance(destination, transform.position) > 0.3f)
        {
            Vector3 direction = (destination - transform.position).normalized;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
            {
                if (direction.x >= 0)
                {
                    animator.CrossFade("MushIdleRight", 0, 0);
                }
                else
                {
                    animator.CrossFade("MushIdleLeft", 0, 0);
                }
            }
            else
            {
                if (direction.z >= 0)
                {
                    animator.CrossFade("MushIdleBack", 0, 0);
                }
                else
                {
                    animator.CrossFade("MushIdleFront", 0, 0);
                }
            }
        }
        if (engagedEnemy)
        {
            Vector3 aimDirection = (engagedEnemy.transform.position - transform.position).normalized;

            float angle = Mathf.Atan2(aimDirection.z, aimDirection.x) * Mathf.Rad2Deg;

            pivotPoint.eulerAngles = new Vector3(0, -angle, 0);
        }
        else
        {
            Vector3 groundPosition = MouseGroundPositionSingleton.Instance.returnGroundPosition;
            Vector3 aimDirection = (groundPosition - transform.position).normalized;

            float angle = Mathf.Atan2(aimDirection.z, aimDirection.x) * Mathf.Rad2Deg;

            pivotPoint.eulerAngles = new Vector3(0, -angle, 0);
        }
    }

    public void MoveToPosition(Vector3 movePosition, bool disengageEnemy = true)
    {
        if (disengageEnemy)
        {
            DisengageEnemy();
        }
        agent.stoppingDistance = 0;
        SetDestination(movePosition);
    }

    public void FollowEnemy()
    {
        agent.stoppingDistance = 5;
        Vector3 movePosition = engagedEnemy.transform.position;
        SetDestination(movePosition);
    }

    public void SetDestination(Vector3 movePosition)
    {
        destination = movePosition;
        agent.SetDestination(destination);
    }

    public void EngageEnemy(EnemyController enemyToEngage, bool follow = false)
    {
        engagedEnemy = enemyToEngage;
        agent.stoppingDistance = 5;
        if (!follow)
        {
            return;
        }
        FollowEnemy();
    }

    public void DisengageEnemy()
    {
        engagedEnemy = null;
        agent.stoppingDistance = 0;
    }

    public void AttackEnemy()
    {
        playerAttack.Attack(pivotPoint, shootPoint, engagedEnemy);
    }
}
