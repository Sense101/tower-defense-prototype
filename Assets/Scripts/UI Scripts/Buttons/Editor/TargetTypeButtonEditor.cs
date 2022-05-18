using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TargetTypeButton))]
public class TargetTypeButtonEditor : UnityEditor.UI.ButtonEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TargetTypeButton targetUIButton = (TargetTypeButton)target;

        targetUIButton.targetType = (Turret.TargetType)EditorGUILayout.EnumPopup
        (
            "Target Type",
            targetUIButton.targetType
        );
    }
}