using UnityEditor;
using UnityEditor.EventSystems;

[CustomEditor(typeof(SpellSlot))]
public class SpellSlotEditor : EventTriggerEditor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        base.OnInspectorGUI();
    }
}
