using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterAction : ScriptableObject
{
    public abstract void Act(MobAIController controller);
    public virtual void AdjustAnimation(MobAIController controller)
    {
        Animator animator = controller.animator;
        Vector3 direction = (controller.target.transform.position - controller.transform.position).normalized;

        string prefix = controller.animationPrefix;

        string state = prefix + "WalkRight";
        if (direction.x > 0)
        {
            state = prefix + "WalkRight";
        }
        else if (direction.x < 0)
        {
            state = prefix + "WalkLeft";
        }
        animator.CrossFade(state, 0, 0);
    }
}
