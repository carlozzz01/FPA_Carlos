using UnityEngine;

[CreateAssetMenu(fileName = "ContextMessageSO", menuName = "Scriptable Objects/ContextMessageSO")]
public class ContextMessageSO : ScriptableObject
{
    [field: SerializeField] public string Message {get; private set;} = "This is what appears when getting near an interactable";
}
