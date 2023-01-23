using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decision/TargetClose")]
public class MonsterDecisionTargetClose : MonsterDecision
{
    public override bool Decide(MobAIController controller)
    {
        if (controller.targetFound && Vector3.Distance(controller.target.transform.position, controller.transform.position) > 5)
        {
            controller.targetFound = false;
        }
        return controller.targetFound;
    }
}