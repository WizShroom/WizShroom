using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class MonsterActionChase : MonsterAction
{
    public override void Act(MobAIController controller)
    {
        controller.navMeshAgent.stoppingDistance = controller.mobAIStats.attackDistance;
        controller.navMeshAgent.destination = controller.target.transform.position;
        //AdjustAnimation(controller);
    }
}
