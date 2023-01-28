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
        foreach (Animation animation in animations)
        {
            List<AnimationActor> animationActors = animation.animationActors;

            foreach (AnimationActor animationActor in animationActors)
            {
                yield return GameController.Instance.StartCoroutine(AnimateActor(animationActor));
            }

            List<AnimationEffect> animationEffects = animation.animationEffects;
            foreach (AnimationEffect animationEffect in animationEffects)
            {
                yield return GameController.Instance.StartCoroutine(AnimateEffect(animationEffect));
            }
        }
    }

    public virtual IEnumerator AnimateActor(AnimationActor actor)
    {
        actor.navMeshAgent = GameController.Instance.GetGameObjectFromID(actor.ActorID).GetComponent<NavMeshAgent>();
        actor.startingSpeed = actor.navMeshAgent.speed;
        actor.startingPosition = actor.navMeshAgent.transform.position;

        actor.navMeshAgent.speed = actor.speedToDestination;

        actor.navMeshAgent.isStopped = false;

        if (actor.onlyDestination)
        {
            actor.navMeshAgent.SetDestination(actor.destination);
            while (actor.navMeshAgent.remainingDistance > 0.5f)
            {
                yield return null;
            }

            actor.navMeshAgent.speed = actor.startingSpeed;
            if (actor.resetToInitialPosition)
            {
                actor.navMeshAgent.Warp(actor.startingPosition);
            }
        }
        actor.navMeshAgent.SetDestination(actor.navMeshAgent.transform.position);
        actor.navMeshAgent.isStopped = true;
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
}

[System.Serializable]
public struct AnimationEffect
{
    public GameObject effectToPlay;
    public Vector3 positionToPlayAt;
}