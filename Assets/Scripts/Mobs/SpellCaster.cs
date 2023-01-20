using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCaster : MonoBehaviour
{
    public List<AdvancedSpell> spells;
    public int maxSpellAllowed = 5;

    MobController mobController;
    MobController player;

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
        mobController = GetComponent<MobController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        HashSet<AdvancedSpell> uniqueSpells = new HashSet<AdvancedSpell>();
        foreach (AdvancedSpell spell in spells)
        {
            if (!uniqueSpells.Contains(spell))
            {
                uniqueSpells.Add(spell);
            }
        }

        spells = new List<AdvancedSpell>(uniqueSpells);

        if (spells.Count <= maxSpellAllowed)
        {
            return;
        }

        int indexToStartRemoving = spells.Count - maxSpellAllowed;
        spells.RemoveRange(indexToStartRemoving, maxSpellAllowed);
    }

    private void Update()
    {
        if (paused)
        {
            return;
        }
        for (int i = 0; i < spells.Count; i++)
        {
            if (Input.GetKeyDown(spells[i].key))
            {
                if (spells[i].cooldownRemaining <= 0f)
                {
                    // Cast spell
                    spells[i].Cast(mobController, player);

                    // Start cooldown
                    spells[i].cooldownRemaining = spells[i].cooldown;
                }
            }
            else
            {
                // Decrement cooldown
                spells[i].cooldownRemaining = Mathf.Max(spells[i].cooldownRemaining - Time.deltaTime, 0f);
            }
        }
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