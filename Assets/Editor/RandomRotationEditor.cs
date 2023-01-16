using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RandomRotation))]
public class RandomRotationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RandomRotation script = (RandomRotation)target;

        if (GUILayout.Button("Random Rotation"))
        {
            script.RandomRotate();
        }
    }
}