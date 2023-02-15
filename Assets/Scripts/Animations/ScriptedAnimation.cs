using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ScriptedAnimation", menuName = "ScriptedAnimation")]
public class ScriptedAnimation : ScriptableObject
{

    public List<Animation> animations = new List<Animation>();

    public virtual IEnumerator AnimateAll()
    {
        List<Task> animationsRoutines = new List<Task>();
        foreach (Animation animation in animations)
        {
            List<AnimationActor> animationActors = animation.animationActors;

            foreach (AnimationActor animationActor in animationActors)
            {
                Task animationTask = new Task(AnimateActor(animationActor));
                animationsRoutines.Add(animationTask);
            }

            List<AnimationEffect> animationEffects = animation.animationEffects;
            foreach (AnimationEffect animationEffect in animationEffects)
            {
                Task animationTask = new Task(AnimateEffect(animationEffect));
                animationsRoutines.Add(animationTask);
            }
        }

        bool finishAnimation = false;
        while (!finishAnimation)
        {
            foreach (Task task in animationsRoutines)
            {
                if (task.Running)
                {
                    break;
                }

                finishAnimation = true;
            }
            yield return null;
        }

        yield return null;
    }

    public virtual IEnumerator AnimateActor(AnimationActor actor)
    {
        NavMeshAgent actorAgent = GameController.Instance.GetGameObjectFromID(actor.ActorID).GetComponent<NavMeshAgent>();
        actor.startingSpeed = actorAgent.speed;
        actor.startingPosition = actorAgent.transform.position;

        actorAgent.speed = actor.speedToDestination;

        actorAgent.isStopped = false;

        if (actor.onlyDestination)
        {
            actorAgent.SetDestination(actor.destination);
            while (actorAgent.pathPending || actorAgent.remainingDistance > 0.5f)
            {
                yield return null;
            }

            actorAgent.speed = actor.startingSpeed;
            if (actor.resetToInitialPosition)
            {
                actorAgent.Warp(actor.startingPosition);
            }
        }
        actorAgent.SetDestination(actorAgent.transform.position);
        actorAgent.isStopped = true;
    }

    public virtual IEnumerator AnimateEffect(AnimationEffect animationEffect)
    {
        yield return null;
        Instantiate(animationEffect.effectToPlay, animationEffect.positionToPlayAt, Quaternion.identity);
    }


}

[System.Serializable]
public struct Animation
{
    public List<AnimationActor> animationActors;
    public List<AnimationEffect> animationEffects;
}

[System.Serializable]
public struct AnimationActor
{
    public string ActorID;
    public NavMeshAgent navMeshAgent;
    public Vector3 startingPosition;
    public float startingSpeed;
    public bool onlyDestination;
    public Vector3 destination;
    public List<Vector3> pathToFollow;
    public float speedToDestination;
    public bool resetToInitialPosition;
    public bool animateOnlyThis;
}

[System.Serializable]
public struct AnimationEffect
{
    public GameObject effectToPlay;
    public Vector3 positionToPlayAt;
    public bool animateOnlyThis;
}