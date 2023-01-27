using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class MonsterActionAttack : MonsterAction
{
    public override void Act(MobAIController controller)
    {
        if (!CanAttack(controller))
        {
            return;
        }
        controller.mobController.Attack();
    }

    public bool CanAttack(MobAIController controller)
    {
        controller.mobController.elapsedAttackTime += Time.deltaTime;
        if (controller.mobController.elapsedAttackTime < controller.mobAIStats.attackDelay)
        {
            return false;
        }
        controller.mobController.elapsedAttackTime = 0;

        return true;
    }
}
