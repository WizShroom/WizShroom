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
        bool targetFound = false;

        for (int i = -1; i <= 1; i++)
        {
            Vector3 rayDirection = Quaternion.AngleAxis(i * 15, Vector3.up) * direction;
            if (Physics.Raycast(controller.transform.position, rayDirection, out hit, controller.mobAIStats.followDistance))
            {
                if (hit.collider != null && hit.collider.gameObject.tag == "Player")
                {
                    targetFound = true;
                    break;
                }
            }
        }
        if (targetFound && Vector3.Distance(controller.target.transform.position, controller.transform.position) <= controller.mobAIStats.followDistance)
        {
            if (!controller.targetFound)
            {
                controller.targetFound = true;
                SoundManager.Instance.PlaySoundOneShot("enemyNotice", controller.mobController.audioSource);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}