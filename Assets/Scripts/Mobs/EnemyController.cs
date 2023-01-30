using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MobController
{
    public Spell attackSpell;

    public override void Attack()
    {
        attackSpell.Cast(this, mobAIController.target.GetComponent<MobController>());
    }
}
