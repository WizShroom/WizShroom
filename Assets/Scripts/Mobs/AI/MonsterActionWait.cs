using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Wait")]
public class MonsterActionWait : MonsterAction
{
    public override void Act(MobAIController controller)
    {
        controller.navMeshAgent.isStopped = true;
        //AdjustAnimation(controller);
    }
}