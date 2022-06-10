using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AugmentationButton))]
public class AugmentationButtonEditor : UnityEditor.UI.ButtonEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AugmentationButton targetUIButton = (AugmentationButton)target;

        targetUIButton.augmentation = EditorGUILayout.ObjectField
        (
            "Augmentation",
            targetUIButton.augmentation,
            typeof(Augmentation),
            false
        ) as Augmentation;

        targetUIButton.index = EditorGUILayout.IntField
        (
            "Index",
            targetUIButton.index
        );
    }
}
