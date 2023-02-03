using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuestsResetButton))]
public class QuestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        QuestsResetButton script = (QuestsResetButton)target;

        if (GUILayout.Button("ResetQuests"))
        {
            script.ResetQuests();
        }
    }
}