using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MobAIController : MonoBehaviour
{
    public string animationPrefix = "";
    public NavMeshAgent navMeshAgent;

    public MonsterState currentState;
    public MonsterState remainState;

    public List<GameObject> patrolPoints = new List<GameObject>();
    public int wayPointListIndex = 0;
    public float stateTimeElapsed;
    [HideInInspector] public GameObject target;
    [HideInInspector] public MobController mobController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 homeBase;
    [HideInInspector] public Vector3 lastAttackPosition;

    public bool aiActive = true;

    public bool targetFound = false;

    public MobAIStats mobAIStats;

    private void Awake()
    {
        mobController = GetComponent<MobController>();
    }

    private void Start()
    {
        target = GameController.Instance.GetGameObjectFromID("MushPlayer");
        animator = GetComponent<Animator>();
        navMeshAgent = mobController.navMeshAgent;
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        navMeshAgent.speed = mobAIStats.baseSpeed;
        if (homeBase == default(Vector3))
        {
            homeBase = transform.position;
        }
    }

    public void CallUpdateState()
    {
        if (!aiActive || !target)
        {
            transform.rotation = Quaternion.identity;
            transform.position = new Vector3(navMeshAgent.nextPosition.x, 0, navMeshAgent.nextPosition.z);
            return;
        }
        if (Vector3.Distance(target.transform.position, transform.position) > 20)
        {
            return;
        }
        if (target)
        {
            Vector3 aimDirection = (target.transform.position - transform.position).normalized;

            float angle = Mathf.Atan2(aimDirection.z, aimDirection.x) * Mathf.Rad2Deg;

            mobController.pivotPoint.eulerAngles = new Vector3(0, -angle, 0);
        }
        currentState.UpdateState(this);
    }

    public void TransitionToState(MonsterState nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }

    public void ChangeAIState(bool isActive)
    {
        aiActive = isActive;
        navMeshAgent.isStopped = !isActive;
    }

    public void ResetAIState()
    {
        aiActive = true;
        navMeshAgent.isStopped = false;
    }

    public void SetPatrolPoints(List<GameObject> patrolPoints)
    {
        this.patrolPoints = patrolPoints;
    }

}

[System.Serializable]
public struct MobAIStats
{
    public float attackDistance;
    public float followDistance;
    public float attackDelay;
    public float waitDelay;
    public float baseSpeed;
}