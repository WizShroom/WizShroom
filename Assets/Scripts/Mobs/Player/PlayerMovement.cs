using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;

    [HideInInspector] public NavMeshAgent agent;

    public Vector3 destination;

    [SerializeField] Animator animator;
    PlayerAttack playerAttack;
    PlayerController playerController;

    [HideInInspector] public EnemyController engagedEnemy;
    GameObject enemyHighlight;

    public Interactable toInteract;

    bool paused;

    public float timeBetweenSteps = 1f;
    float stepTimePassed = 0;

    bool levelIsLoading = false;

    private void Awake()
    {
        GameEventHandler.Instance.OnEventReceived += OnEventReceived;
    }

    private void OnDestroy()
    {
        GameEventHandler.Instance.OnEventReceived -= OnEventReceived;
    }

    private void Start()
    {
        destination = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        playerAttack = GetComponent<PlayerAttack>();
        playerController = GetComponent<PlayerController>();
        enemyHighlight = UIHandler.Instance.mobHighlight;
    }

    private void Update()
    {
        if (levelIsLoading)
        {
            return;
        }

        if (Vector3.Distance(transform.position, destination) > 1 && !agent.isStopped)
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

            if (stepTimePassed > timeBetweenSteps * (1 / playerController.navMeshAgent.speed) * 3)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 3))
                {
                    StepSound soundToTry = hit.transform.GetComponent<StepSound>();
                    if (soundToTry)
                    {
                        AudioClip pickedSound = soundToTry.stepSounds[Random.Range(0, soundToTry.stepSounds.Count)];
                        SoundManager.Instance.PlaySoundOneShot(pickedSound, playerController.audioSource);
                    }
                }
                stepTimePassed = 0;
            }

            stepTimePassed += Time.deltaTime;

        }

        if (paused)
        {
            return;
        }

        if (engagedEnemy && !engagedEnemy.disabled)
        {
            AttackEnemy();
            if (!enemyHighlight.activeSelf)
            {
                enemyHighlight.SetActive(true);
            }
            enemyHighlight.transform.position = engagedEnemy.transform.position + new Vector3(0, 0.5f, 0);
        }
        else if (engagedEnemy && engagedEnemy.disabled)
        {
            enemyHighlight.SetActive(false);
            DisengageEnemy();
        }

        if (toInteract && Vector3.Distance(toInteract.transform.position, transform.position) <= toInteract.distanceForInteraction * 1.2f)
        {
            Interact();
        }

        if (engagedEnemy)
        {
            Vector3 aimDirection = (engagedEnemy.transform.position - transform.position).normalized;

            float angle = Mathf.Atan2(aimDirection.z, aimDirection.x) * Mathf.Rad2Deg;

            playerController.pivotPoint.eulerAngles = new Vector3(0, -angle, 0);
        }
        else
        {
            Vector3 groundPosition = MouseGroundPositionSingleton.Instance.returnGroundPosition;
            Vector3 aimDirection = (groundPosition - transform.position).normalized;

            float angle = Mathf.Atan2(aimDirection.z, aimDirection.x) * Mathf.Rad2Deg;

            playerController.pivotPoint.eulerAngles = new Vector3(0, -angle, 0);
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
        playerAttack.ResetAttack();
    }

    public void AttackEnemy()
    {
        playerAttack.Attack(playerController.pivotPoint, playerController.shootPoint, engagedEnemy);
    }

    public void PrepareInteract(Interactable toInteract)
    {
        agent.stoppingDistance = toInteract.distanceForInteraction;
        this.toInteract = toInteract;
        SetDestination(toInteract.transform.position);
    }

    public void Interact()
    {
        toInteract.Interact(playerController);
        agent.SetDestination(transform.position);
        agent.stoppingDistance = 0;
        toInteract = null;
    }

    public void OnEventReceived(GameObject source, EVENT receivedEvent)
    {
        if (receivedEvent == EVENT.PAUSED)
        {
            OnPaused();
        }
        if (receivedEvent == EVENT.RESUMED)
        {
            OnResumed();
        }
        if (receivedEvent == EVENT.LOADINGLEVEL)
        {
            OnLoadingLevel();
        }
        if (receivedEvent == EVENT.LOADEDLEVEL)
        {
            OnLoadedLevel();
        }
    }

    public void OnPaused()
    {
        paused = true;
    }

    public void OnResumed()
    {
        paused = false;
    }

    public void OnLoadingLevel()
    {
        levelIsLoading = true;
    }

    public void OnLoadedLevel()
    {
        levelIsLoading = false;
    }
}
