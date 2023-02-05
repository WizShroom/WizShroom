using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class MonsterActionPatrol : MonsterAction
{
    public override void Act(MobAIController controller)
    {
        if (controller.patrolPoints.Count == 0)
        {
            controller.navMeshAgent.stoppingDistance = 0;

            if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
            {
                Vector3 newDestination = Random.insideUnitSphere * 5 + controller.homeBase;
                newDestination.y = 0;
                controller.navMeshAgent.SetDestination(newDestination);
                if (controller.navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial)
                {
                    controller.navMeshAgent.SetDestination(controller.transform.position);
                }
            }

            //AdjustAnimation(controller);
            return;
        }

        if (controller.patrolPoints[controller.wayPointListIndex] == null)
        {
            Debug.LogError("Patrol point is null");
            return;
        }

        controller.navMeshAgent.SetDestination(controller.patrolPoints[controller.wayPointListIndex].transform.position);

        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
        {
            controller.wayPointListIndex = (controller.wayPointListIndex + 1) % controller.patrolPoints.Count;
        }

        //AdjustAnimation(controller);
    }
}