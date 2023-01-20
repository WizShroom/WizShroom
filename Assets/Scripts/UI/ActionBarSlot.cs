using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionBarSlot : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public string key;
    public AdvancedSpell equippedSpell;
    public Image slotSprite;
    public Image cooldownIndicator;

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
        UpdateSlot(true);
    }

    private void Update()
    {
        if (paused)
        {
            return;
        }

        if (equippedSpell && equippedSpell.cooldownRemaining > 0)
        {
            equippedSpell.cooldownRemaining = Mathf.Max(equippedSpell.cooldownRemaining - Time.deltaTime, 0f);
            cooldownIndicator.fillAmount = equippedSpell.cooldownRemaining / equippedSpell.cooldown;
        }
    }

    public void CastSpell(MobController target)
    {
        PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        CastSpell(playerController, target);
    }

    public void CastSpell(MobController caster, MobController target)
    {
        if (equippedSpell.cooldownRemaining <= 0f)
        {
            if (!equippedSpell.requireEnemy)
            {
                Vector3 groundPosition = MouseGroundPositionSingleton.Instance.returnGroundPosition;
                Vector3 castDirection = (groundPosition - caster.transform.position).normalized;
                CastSpell(caster, castDirection);
                return;
            }
            equippedSpell.Cast(caster, target);
            equippedSpell.cooldownRemaining = equippedSpell.cooldown;
        }
    }

    public void CastSpell(MobController caster, Vector3 castDirection)
    {
        if (equippedSpell.cooldownRemaining <= 0f)
        {
            if (equippedSpell.requireEnemy)
            {
                return;
            }
            equippedSpell.Cast(caster, castDirection);
            equippedSpell.cooldownRemaining = equippedSpell.cooldown;
        }
    }

    public void UpdateSlot(bool initial = false)
    {
        cooldownIndicator.fillAmount = 0;
        if (!equippedSpell)
        {
            slotSprite.sprite = null;
            return;
        }
        slotSprite.sprite = equippedSpell.UIImage;
        if (!initial)
        {
            return;
        }
        equippedSpell.cooldownRemaining = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!equippedSpell)
        {
            return;
        }
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject != gameObject)
        {
            GameObject otherSlot = eventData.pointerCurrentRaycast.gameObject;
            if (otherSlot != null && otherSlot != gameObject && otherSlot.CompareTag("ActionBarSlot"))
            {
                SwapSpells(otherSlot.GetComponent<ActionBarSlot>());
            }
            return;
        }

        PlayerMovement playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        PlayerController playerController = playerMovement.GetComponent<PlayerController>();
        EnemyController enemyController = playerMovement.engagedEnemy;
        if (enemyController)
        {
            CastSpell(playerController, enemyController);
        }
        else
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 castDirection = (playerController.transform.position - (Vector3)mousePosition).normalized;
            CastSpell(playerController, castDirection);
        }
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {

    }

    public void AddSpell(AdvancedSpell spellToAdd)
    {
        if (!spellToAdd)
        {
            return;
        }
        equippedSpell = spellToAdd;
        UpdateSlot();
    }

    public void RemoveSpell()
    {
        equippedSpell = null;
        UpdateSlot();
    }

    public void SwapSpells(ActionBarSlot otherSlot)
    {
        AdvancedSpell ourSpell = equippedSpell;
        AdvancedSpell otherSpell = otherSlot.equippedSpell;
        RemoveSpell();
        otherSlot.RemoveSpell();
        AddSpell(otherSpell);
        otherSlot.AddSpell(ourSpell);
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