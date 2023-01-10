using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerInputAction : MonoBehaviour
{
    public LayerMask enemyLayer;
    public LayerMask interactableLayer;

    PlayerMovement playerMovement;

    public GameObject mouseGroundHighlight;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Vector3 groundPosition = MouseGroundPositionSingleton.Instance.returnGroundPosition;

        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        Vector3 highlightPosition = groundPosition;
        highlightPosition.y += 0.25f;
        if (Input.GetMouseButtonDown(0) && !isOverUI && groundPosition != default(Vector3))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerMovement.MoveToPosition(highlightPosition);
            }

            else
            {
                playerMovement.MoveToPosition(highlightPosition, false);
            }
            mouseGroundHighlight.transform.position = new Vector3(highlightPosition.x, highlightPosition.y + 0.25f, highlightPosition.z);
            Animator groundHighlightAnimator = mouseGroundHighlight.transform.GetChild(0).GetComponent<Animator>();
            groundHighlightAnimator.Play("GroundHighlight");
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
                Debug.Log("Hit interactable");
            }
        }
    }

    void Attack(EnemyController enemyToAttack, bool followEnemy = false)
    {
        playerMovement.EngageEnemy(enemyToAttack, followEnemy);
    }
}
