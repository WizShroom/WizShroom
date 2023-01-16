using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MobController
{
    public string playerName;

    public int availableStatPoints;

    public int statPointPerLevel = 5;

    public StatUIHolder statUIHolder;
    public ActionBarHolder actionBarHolder;

    public override void Initialize()
    {
        base.Initialize();
        statUIHolder.ConnectStats(this);
        actionBarHolder.playerMovement = GetComponent<PlayerMovement>();
        actionBarHolder.playerController = this;
    }

    public override void LevelUp()
    {
        base.LevelUp();
        availableStatPoints += statPointPerLevel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (!interactable.clickable && !interactable.disabled)
            {
                interactable.Interact(this);
            }
        }
    }
}
