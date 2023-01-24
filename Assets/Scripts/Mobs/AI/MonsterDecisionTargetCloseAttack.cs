using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decision/TargetCloseAttack")]
public class MonsterDecisionTargetCloseAttack : MonsterDecision
{
    public override bool Decide(MobAIController controller)
    {
        if (controller.targetFound && Vector3.Distance(controller.target.transform.position, controller.transform.position) < 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}