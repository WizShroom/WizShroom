using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInputAction : MonoBehaviour
{
    public LayerMask enemyLayer;
    public LayerMask interactableLayer;

    PlayerMovement playerMovement;
    PlayerController playerController;

    public GameObject mouseGroundHighlight;
    Animator mouseGroundHighlightAnimator;

    bool paused;

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
        playerMovement = GetComponent<PlayerMovement>();
        playerController = GetComponent<PlayerController>();
        mouseGroundHighlightAnimator = mouseGroundHighlight.GetComponent<Animator>();
    }

    void Update()
    {
        if (paused)
        {
            return;
        }
        Vector3 groundPosition = MouseGroundPositionSingleton.Instance.returnGroundPosition;

        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        Vector3 highlightPosition = groundPosition;
        highlightPosition.y += 0.25f;
        if (Input.GetMouseButton(0) && !isOverUI && groundPosition != default(Vector3))
        {
            mouseGroundHighlightAnimator.StopPlayback();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerMovement.MoveToPosition(highlightPosition);
            }

            else
            {
                playerMovement.MoveToPosition(highlightPosition, false);
            }
            mouseGroundHighlight.transform.position = new Vector3(highlightPosition.x, highlightPosition.y + 0.25f, highlightPosition.z);
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseGroundHighlightAnimator.Play("GroundHighlight");
        }

        if (Input.GetMouseButtonDown(1) && !isOverUI)
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, Mathf.Infinity);

            if (hit.collider && 1 << hit.collider.gameObject.layer == enemyLayer.value)
            {
                EnemyController enemyController = hit.collider.gameObject.GetComponent<EnemyController>();
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Attack(enemyController, true);
                }
                else
                {
                    Attack(enemyController);
                }

            }
            else if (hit.collider && 1 << hit.collider.gameObject.layer == interactableLayer.value)
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable.clickable && !interactable.disabled)
                {
                    playerMovement.PrepareInteract(interactable);
                }
            }
        }
    }

    void Attack(EnemyController enemyToAttack, bool followEnemy = false)
    {
        playerMovement.EngageEnemy(enemyToAttack, followEnemy);
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
    }

    public void OnPaused()
    {
        paused = true;
    }

    public void OnResumed()
    {
        paused = false;
    }
}
