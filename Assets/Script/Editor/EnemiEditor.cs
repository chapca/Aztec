using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnnemiAttack))]
[CanEditMultipleObjects]
public class EnemiEditor : Editor
{
    /*float setUpStartRalentiAnim;
    //float setUpRalentiDuration;
    float setUpEndActionPlayer;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(10);
        GUILayout.Label("SetUpAnimation");
        setUpStartRalentiAnim = EditorGUILayout.Slider("Start Ralenti", setUpStartRalentiAnim, 0, 1);
        setUpEndActionPlayer = EditorGUILayout.Slider("End Ralenti", setUpEndActionPlayer, 0, 1);
        //setUpRalentiDuration = EditorGUILayout.Slider("Durée Ralenti", setUpRalentiDuration, 0, 1- setUpStartRalentiAnim);

        serializedObject.FindProperty("setUpStartActionPlayer").floatValue = setUpStartRalentiAnim;
        serializedObject.ApplyModifiedProperties();

        serializedObject.FindProperty("setUpEndActionPlayer").floatValue = setUpEndActionPlayer;
        serializedObject.ApplyModifiedProperties();

    }*/
}