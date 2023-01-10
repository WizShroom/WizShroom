using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decision/TargetAttack")]
public class MonsterDecisionTargetAttack : MonsterDecision
{
    public override bool Decide(MobAIController controller)
    {
        if (Vector3.Distance(controller.target.transform.position, controller.transform.position) <= 6)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}