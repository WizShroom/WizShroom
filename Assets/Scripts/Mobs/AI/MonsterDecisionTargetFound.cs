using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decision/TargetFound")]
public class MonsterDecisionTargetFound : MonsterDecision
{
    public override bool Decide(MobAIController controller)
    {
        Vector3 direction = (controller.target.transform.position - controller.transform.position).normalized;
        RaycastHit hit;
        bool targetFound = true;
        Physics.Raycast(controller.transform.position, direction, out hit, 5);
        if (hit.collider == null || hit.collider.gameObject.tag != "Player")
        {
            targetFound = false;
        }
        if (targetFound && Vector3.Distance(controller.target.transform.position, controller.transform.position) <= 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}