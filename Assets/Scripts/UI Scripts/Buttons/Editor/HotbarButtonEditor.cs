using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HotbarButton))]
public class HotbarButtonEditor : UnityEditor.UI.ButtonEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HotbarButton targetUIButton = (HotbarButton)target;

        targetUIButton.turretPrefab = EditorGUILayout.ObjectField
        (
            "Turret Prefab",
            targetUIButton.turretPrefab,
            typeof(Turret),
            false
        ) as Turret;

        targetUIButton.turretPreviewSprite = EditorGUILayout.ObjectField
        (
            "Preview Sprite",
            targetUIButton.turretPreviewSprite,
            typeof(Sprite),
            false
        ) as Sprite;
    }
}
