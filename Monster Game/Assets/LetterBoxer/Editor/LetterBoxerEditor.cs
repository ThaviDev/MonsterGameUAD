using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LetterBoxer))]
public class LetterBoxerEditor : Editor
{
    SerializedProperty matteColorProp;
    SerializedProperty referenceModeProp;
    SerializedProperty xProp;
    SerializedProperty yProp;
    SerializedProperty widthProp;
    SerializedProperty heightProp;
    SerializedProperty onAwakeProp;
    SerializedProperty onUpdateProp;

    // new props
    SerializedProperty useTextureProp;
    SerializedProperty matteTextureProp;
    SerializedProperty backgroundLayerProp;

    void OnEnable()
    {
        matteColorProp = serializedObject.FindProperty("matteColor");
        referenceModeProp = serializedObject.FindProperty("referenceMode");
        xProp = serializedObject.FindProperty("x");
        yProp = serializedObject.FindProperty("y");
        widthProp = serializedObject.FindProperty("width");
        heightProp = serializedObject.FindProperty("height");
        onAwakeProp = serializedObject.FindProperty("onAwake");
        onUpdateProp = serializedObject.FindProperty("onUpdate");

        useTextureProp = serializedObject.FindProperty("useTexture");
        matteTextureProp = serializedObject.FindProperty("matteTexture");
        backgroundLayerProp = serializedObject.FindProperty("backgroundLayer");
    }

    override public void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(matteColorProp, new GUIContent("Matte Color", "The color to use for the matte bars"));
        EditorGUILayout.PropertyField(referenceModeProp, new GUIContent("Reference Mode", "Toggles whether you want to use an aspect ratio or a resolution to calculate the letterboxing"));

        LetterBoxer.ReferenceMode currentReferenceMode = (LetterBoxer.ReferenceMode)referenceModeProp.enumValueIndex;

        if (currentReferenceMode == LetterBoxer.ReferenceMode.DesignedAspectRatio)
        {
            EditorGUI.indentLevel++;            
            EditorGUILayout.PropertyField(xProp, new GUIContent("X", "The X portion of the aspect ratio"));
            EditorGUILayout.PropertyField(yProp, new GUIContent("Y", "The Y portion of the aspect ratio"));            
            EditorGUI.indentLevel--;
        }
        else
        { 
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(widthProp, new GUIContent("Width", "The width in pixels"));
            EditorGUILayout.PropertyField(heightProp, new GUIContent("Height", "The height in pixels"));
            EditorGUI.indentLevel--;            
        }
                
        EditorGUILayout.PrefixLabel("Calculate");
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(onAwakeProp, new GUIContent("On Awake", "Calculate the letterboxing during OnAwake()"));
        EditorGUILayout.PropertyField(onUpdateProp, new GUIContent("On Update", "Calculate the letterboxing during OnUpdate()"));
        EditorGUI.indentLevel--;

        // New UI for texture background
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Background", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(useTextureProp, new GUIContent("Use Texture", "If true, use the assigned texture for the matte bars instead of a flat color."));
        if (useTextureProp.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(matteTextureProp, new GUIContent("Matte Texture", "Texture to use as background for the matte areas"));
            EditorGUILayout.PropertyField(backgroundLayerProp, new GUIContent("Background Layer", "Select the layer the background sprite will be placed on. The letterbox camera will render only this layer."));
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}

