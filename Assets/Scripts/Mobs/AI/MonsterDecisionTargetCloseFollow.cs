using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decision/TargetCloseFollow")]
public class MonsterDecisionTargetCloseFollow : MonsterDecision
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