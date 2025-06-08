using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interactable_Rigidbody)), CanEditMultipleObjects]
public class Interactable_RigidbodyEditor : Editor
{
    SerializedProperty breakEvent;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Interactable_Rigidbody interactable = (Interactable_Rigidbody)target;

        // Display conditional for trigger
        if (interactable.IsBreakable)
        {
            serializedObject.Update();
            breakEvent = serializedObject.FindProperty("OnBreak");

            // interactable.SetTriggerKey(EditorGUILayout.TextField("Trigger ID", interactable.TriggerKey));
            interactable.SetBreakVelocity(EditorGUILayout.FloatField("Break Velocity", interactable.BreakVelocity));

            interactable.SetBreakEffect((ParticleSystem)EditorGUILayout.ObjectField("Break Effect", interactable.BreakEffect, typeof(ParticleSystem), true));
            
            EditorGUILayout.PropertyField(breakEvent, true);

            serializedObject.ApplyModifiedProperties();
        }

        // Display conditional for boolean
        // if (interactable.ReactionType == ReactionAnimation.AnimationReactionType.Bool)
        // {
        //     interactable.SetBoolKey(EditorGUILayout.TextField("Bool ID", interactable.BoolKey));
        //     interactable.SetBoolValue(EditorGUILayout.Toggle("Bool Value", interactable.BoolValue));
        // }

        // Display conditional for float
        // if (interactable.ReactionType == ReactionAnimation.AnimationReactionType.Float)
        // {
        //     interactable.SetFloatKey(EditorGUILayout.TextField("Float ID", interactable.FloatKey));
        //     interactable.SetFloatValue(EditorGUILayout.FloatField("Float Value", interactable.FloatValue));
        // }

        // // Display always
        // client.IndependentData = EditorGUILayout.TextField("String", client.IndependentData);
    }
}
