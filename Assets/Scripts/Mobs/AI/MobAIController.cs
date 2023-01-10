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
    public GameObject target;
    public MobController mobController;
    public Animator animator;
    public Vector3 homeBase;
    public Vector3 lastAttackPosition;

    public bool aiActive = true;

    private void Awake()
    {
        mobController = GetComponent<MobController>();
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        if (homeBase == default(Vector3))
        {
            homeBase = transform.position;
        }
    }

    public void CallUpdateState()
    {
        if (!aiActive)
        {
            transform.rotation = Quaternion.identity;
            transform.position = new Vector3(navMeshAgent.nextPosition.x, 0, navMeshAgent.nextPosition.z);
            return;
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
