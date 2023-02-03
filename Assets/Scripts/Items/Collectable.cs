using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Collectable : MonoBehaviour
{
    public int collectionRadious = 1;

    public bool autoCollect = false;

    public Item itemToCollect;
    public int containedItemAmount = 1;

    public Spell spellToCollect;

    public Interactable ourInteract;

    public float originalY;
    public float floatStrength;
    public MobController target;

    private void Awake()
    {
        Interactable tryConnect;
        if (TryGetComponent<Interactable>(out tryConnect))
        {
            ourInteract = tryConnect;
            ourInteract.OnInteracted += OnInteracted;
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
        originalY = transform.position.y;
        GetComponent<SphereCollider>().radius = collectionRadious;
        target = GameController.Instance.GetGameObjectFromID("MushPlayer").GetComponent<MobController>();
    }

    private void Update()
    {
        if (!autoCollect)
        {
            transform.position = new Vector3(transform.position.x,
                    originalY + ((float)Mathf.Sin(Time.time) * floatStrength),
                    transform.position.z);
        }
        else
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.Translate(direction * 0.1f * (1 / Vector3.Distance(target.transform.position, transform.position)));
        }
    }

    public void Collect()
    {
        if (itemToCollect)
        {
            Inventory.Instance.AddItem(itemToCollect, containedItemAmount);
        }
        if (spellToCollect)
        {
            SpellsInventory.Instance.AddSpellToFreeSlot(spellToCollect);
        }
        Destroy(gameObject);
    }

    public void OnInteracted()
    {
        Collect();
    }
}