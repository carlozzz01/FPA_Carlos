using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ReactionAnimation)), CanEditMultipleObjects]
public class ReactionAnimationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ReactionAnimation reaction = (ReactionAnimation)target;

        // Display conditional for trigger
        if (reaction.ReactionType == ReactionAnimation.AnimationReactionType.Trigger)
        {
            reaction.SetTriggerKey(EditorGUILayout.TextField("Trigger ID", reaction.TriggerKey));
        }

        // Display conditional for boolean
        if (reaction.ReactionType == ReactionAnimation.AnimationReactionType.Bool)
        {
            reaction.SetBoolKey(EditorGUILayout.TextField("Bool ID", reaction.BoolKey));
            reaction.SetBoolValue(EditorGUILayout.Toggle("Bool Value", reaction.BoolValue));
        }

        // Display conditional for float
        if (reaction.ReactionType == ReactionAnimation.AnimationReactionType.Float)
        {
            reaction.SetFloatKey(EditorGUILayout.TextField("Float ID", reaction.FloatKey));
            reaction.SetFloatValue(EditorGUILayout.FloatField("Float Value", reaction.FloatValue));
        }

        // // Display always
        // client.IndependentData = EditorGUILayout.TextField("String", client.IndependentData);
    }
}
