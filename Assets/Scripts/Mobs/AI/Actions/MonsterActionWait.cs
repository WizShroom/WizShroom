using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Wait")]
public class MonsterActionWait : MonsterAction
{
    public override void Act(MobAIController controller)
    {
        controller.navMeshAgent.isStopped = true;
        GameController.Instance.StartCoroutine(ResetStoppedAgent(controller, controller.mobAIStats.waitDelay));
        //AdjustAnimation(controller);
    }

    public IEnumerator ResetStoppedAgent(MobAIController mobToReset, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        if (!mobToReset.mobController.disabled)
        {
            mobToReset.navMeshAgent.isStopped = false;
        }
    }
}