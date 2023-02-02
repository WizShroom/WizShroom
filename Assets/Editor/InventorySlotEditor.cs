using UnityEditor;
using UnityEditor.EventSystems;

[CustomEditor(typeof(InventorySlot))]
public class InventorySlotEditor : EventTriggerEditor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        base.OnInspectorGUI();
    }
}
