using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptedAnimation))]
public class ScriptedAnimationEditor : Editor
{
    private ScriptedAnimation script;

    private void OnEnable()
    {
        script = (ScriptedAnimation)target;
    }


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        for (int i = 0; i < script.animations.Count; i++)
        {
            Animation animation = script.animations[i];
            for (int j = 0; j < animation.animationActors.Count; j++)
            {
                AnimationActor actor = animation.animationActors[j];
                if (actor.destination == Vector3.zero)
                {
                    GameObject obj = (GameObject)EditorGUILayout.ObjectField("Destination" + " " + actor.ActorID, null, typeof(GameObject), true);
                    if (obj != null)
                    {
                        actor.destination = obj.transform.position;
                        animation.animationActors[j] = actor;
                    }
                }

                int pathCount = actor.pathToFollow == null ? 0 : actor.pathToFollow.Count;
                for (int k = 0; k < pathCount; k++)
                {
                    if (actor.pathToFollow[k] == Vector3.zero)
                    {
                        GameObject pathPoint = (GameObject)EditorGUILayout.ObjectField("Path Point #" + k.ToString(), null, typeof(GameObject), true);
                        if (pathPoint != null)
                        {
                            actor.pathToFollow[k] = pathPoint.transform.position;
                        }
                    }
                }
            }
        }
    }
}