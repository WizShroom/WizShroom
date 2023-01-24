using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/FindOut")]
public class MonsterActionFindOut : MonsterAction
{
    public override void Act(MobAIController controller)
    {
        if (controller.lastAttackPosition != default(Vector3) && controller.navMeshAgent.remainingDistance <= 0.2f)
        {
            controller.lastAttackPosition = default(Vector3);
        }
        if (controller.lastAttackPosition == default(Vector3))
        {
            return;
        }
        controller.navMeshAgent.stoppingDistance = 0;
        controller.navMeshAgent.SetDestination(controller.lastAttackPosition);
        controller.navMeshAgent.isStopped = false;
        //AdjustAnimation(controller);
    }
}
