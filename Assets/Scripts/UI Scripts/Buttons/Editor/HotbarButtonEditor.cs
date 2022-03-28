using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HotbarButton))]
public class HotbarButtonEditor : UnityEditor.UI.ButtonEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HotbarButton targetUIButton = (HotbarButton)target;

        targetUIButton.TurretPrefab = EditorGUILayout.ObjectField
        (
            "Turret Prefab",
            targetUIButton.TurretPrefab,
            typeof(GameObject),
            false
        ) as GameObject;
    }
}
