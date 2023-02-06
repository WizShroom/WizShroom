using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Collectable : MonoBehaviour
{
    public int collectionRadious = 1;

    public bool autoCollect = false;
    public bool flyToTarget = false;

    public Item itemToCollect;
    public int containedItemAmount = 1;

    public Spell spellToCollect;

    public Interactable ourInteract;

    public float originalY;
    public float floatStrength;
    public MobController target;

    public GameObject collectableEffect;
    GameObject spawnedEffect;

    public bool isQuestItem = false;

    private void Awake()
    {
        Interactable tryConnect;
        if (TryGetComponent<Interactable>(out tryConnect))
        {
            ourInteract = tryConnect;
            ourInteract.OnInteracted += OnInteracted;
        }

        if (!isQuestItem)
        {
            GetComponent<ParticleSystem>().Stop();
        }
    }

    private void OnDestroy()
    {
        if (ourInteract)
        {
            ourInteract.OnInteracted -= OnInteracted;
        }
    }

    private void Start()
    {
        if (collectableEffect)
        {
            spawnedEffect = Instantiate(collectableEffect, transform);
        }
        originalY = transform.position.y;
        GetComponent<SphereCollider>().radius = collectionRadious;
        target = GameController.Instance.GetGameObjectFromID("MushPlayer").GetComponent<MobController>();
        if (autoCollect && ourInteract)
        {
            ourInteract.clickable = false;
        }
        else if (!autoCollect && ourInteract)
        {
            ourInteract.clickable = true;
        }
        else if (!autoCollect && !ourInteract)
        {
            autoCollect = true;
        }
    }

    private void Update()
    {
        if (!autoCollect || !flyToTarget)
        {
            transform.position = new Vector3(transform.position.x,
                    originalY + ((float)Mathf.Sin(Time.time) * floatStrength),
                    transform.position.z);
        }
        else if (autoCollect && flyToTarget)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.Translate(direction * 0.1f * (1 / Vector3.Distance(target.transform.position, transform.position)));
        }
    }

    public bool Collect()
    {
        if (itemToCollect)
        {
            if (Inventory.Instance.IsInventoryFull(itemToCollect, containedItemAmount))
            {
                return false;
            }
            Inventory.Instance.AddItem(itemToCollect, containedItemAmount);
        }
        if (spellToCollect)
        {
            SpellsInventory.Instance.AddSpellToFreeSlot(spellToCollect);
        }

        return true;
    }

    public void OnInteracted()
    {
        if (Collect())
        {
            Destroy(gameObject);
        }
    }
}