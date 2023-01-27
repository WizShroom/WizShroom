using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decision/Attacked")]
public class MonsterDecisionAttacked : MonsterDecision
{
    public override bool Decide(MobAIController controller)
    {

        if (controller.lastAttackPosition != default(Vector3))
        {
            return true;
        }
        return false;
    }
}