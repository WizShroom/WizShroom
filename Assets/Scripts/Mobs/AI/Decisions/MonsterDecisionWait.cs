using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decision/Wait")]
public class MonsterDecisionWait : MonsterDecision
{
    public override bool Decide(MobAIController controller)
    {
        bool nextWait = controller.CheckIfCountDownElapsed(1);
        if (controller.navMeshAgent.remainingDistance <= 0.2f && nextWait)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}